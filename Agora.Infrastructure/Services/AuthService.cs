using Agora.API.Settings;
using Agora.Core.Common;
using Agora.Core.Enums;
using Agora.Core.Interfaces;
using Agora.Core.Interfaces.Repositories;
using Agora.Core.Models;
using Agora.Core.Models.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Agora.Infrastructure.Services;

/// <inheritdoc />
public class AuthService(
    SignInManager<User> signInManager,
    UserManager<User> userManager,
    IUserRepository userRepo,
    IOptions<UserSettings> userSettings
    ) : IAuthService
{
    private const string EntityName = "user";

    /// <inheritdoc />
    public async Task<Result<User>> RegisterAsync(UserRegistrationInfo registrationInfo)
    {

        User user = new User()
        {
            UserName = registrationInfo.UserName,
            Email = registrationInfo.Email,
            Credit = userSettings.Value.InitialCredit,
            CreatedAt = DateTime.UtcNow
        };

        // Registration
        IdentityResult registrationResult = await userRepo.AddUserAsync(user, registrationInfo.Password);
        if (!registrationResult.Succeeded)
        {
            return Result<User>.Failure(ErrorType.Persistence, ErrorMessages.ErrorWhenSavingToDb(EntityName));
        }

        User? createdUser = await userRepo.GetUserByEmailAsync(registrationInfo.Email);
        if (createdUser == null)
        {
            return Result<User>.Failure(ErrorType.Persistence, ErrorMessages.SavedButNotRetrieved(EntityName));
        }

        // Login
        Result loginResult =
            await LoginAsync(new UserSignInInfo(){
                Email = registrationInfo.Email, 
                Password = registrationInfo.Password
            });
        return loginResult.IsFailure
                ? Result<User>.Failure(loginResult.Errors!)
                : Result<User>.Success(createdUser);
    }

    /// <inheritdoc />
    public async Task<Result> LoginAsync(UserSignInInfo signInInfo)
    {
        // Retrieve user
        User? user = await userRepo.GetUserByEmailAsync(signInInfo.Email);
        if (user == null)
        {
            return Result.Failure(ErrorType.Unauthorized, ErrorMessages.User.InvalidCredentials);
        }
        
        // SignIn with ASP.NET Core Identity
        SignInResult signInResult = await signInManager.PasswordSignInAsync(user, signInInfo.Password, false, false);
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