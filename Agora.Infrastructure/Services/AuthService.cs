using Agora.Core.Commands;
using Agora.Core.Enums;
using Agora.Core.Interfaces;
using Agora.Core.Interfaces.Repositories;
using Agora.Core.Models.Entities;
using Agora.Core.Settings;
using Agora.Core.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Agora.Infrastructure.Services;

/// <summary>
/// Default implementation of <see cref="IAuthService"/>.
/// </summary>
public class AuthService(
    SignInManager<User> signInManager,
    UserManager<User> userManager,
    IUserRepository userRepo,
    IOptions<UserSettings> userSettings
    ) : IAuthService
{
    private const string EntityName = "user";

    /// <inheritdoc />
    public async Task<Result<User>> RegisterAsync(RegisterUserCommand command)
    {

        User user = new User()
        {
            UserName = command.UserName,
            Email = command.Email,
            Credit = userSettings.Value.InitialCredit,
            CreatedAt = DateTime.UtcNow
        };

        // Registration
        IdentityResult registrationResult = await userRepo.AddUserAsync(user, command.Password);
        if (!registrationResult.Succeeded)
        {
            return Result<User>.Failure(ErrorType.Persistence, ErrorMessages.ErrorWhenSavingToDb(EntityName));
        }

        User? createdUser = await userRepo.GetUserByEmailAsync(command.Email);
        if (createdUser == null)
        {
            return Result<User>.Failure(ErrorType.Persistence, ErrorMessages.SavedButNotRetrieved(EntityName));
        }

        // Login
        Result loginResult =
            await LoginAsync(new SignInUserCommand(){
                Email = command.Email, 
                Password = command.Password
            });
        return loginResult.IsFailure
                ? Result<User>.Failure(loginResult.Errors!)
                : Result<User>.Success(createdUser);
    }

    /// <inheritdoc />
    public async Task<Result> LoginAsync(SignInUserCommand command)
    {
        // Retrieve user
        User? user = await userRepo.GetUserByEmailAsync(command.Email);
        if (user == null)
        {
            return Result.Failure(ErrorType.Unauthorized, ErrorMessages.User.InvalidCredentials);
        }
        
        // SignIn with ASP.NET Core Identity
        SignInResult signInResult = await signInManager.PasswordSignInAsync(user, command.Password, false, false);
        if (!signInResult.Succeeded)
        {
            return Result.Failure(ErrorType.Unauthorized, ErrorMessages.User.InvalidCredentials);
        }
        
        // Update LastLoginAt property
        user.LastLoginAt = DateTime.UtcNow;
        IdentityResult identityResult = await userManager.UpdateAsync(user);
        return identityResult.Succeeded
            ? Result.Success()
            : Result.Failure(ErrorType.Invalid, identityResult.Errors.ToString()!);
    }

    /// <inheritdoc />
    public async Task LogoutAsync()
    {
        await signInManager.SignOutAsync();
    }
}