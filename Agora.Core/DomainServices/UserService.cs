using Agora.Core.Constants;
using Agora.Core.Enums;
using Agora.Core.Interfaces.DomainServices;
using Agora.Core.Interfaces.QueryParameters;
using Agora.Core.Interfaces.Repositories;
using Agora.Core.Models;
using Agora.Core.Models.Entities;
using Agora.Core.Shared;
using Agora.Core.Validation.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Agora.Core.DomainServices;

/// <summary>
/// Default implementation of <see cref="IUserService"/>.
/// </summary>
public class UserService(
    IUserRepository userRepo,
    IAuthorizationValidator authorizationValidator,
    IBusinessRulesValidator businessRulesValidator) : IUserService
{
    private const string EntityName = "user";

    /// <inheritdoc />
    public async Task<Result<IReadOnlyList<User>>> GetAllUsersAsync(IUserQueryParameters queryParams)
    {
        // Validate business rules
        Result businessRulesValidationResult = businessRulesValidator.ValidateSortBy(queryParams.SortBy, SortByOptions.User);
        if (businessRulesValidationResult.IsFailure)
        {
            return Result<IReadOnlyList<User>>.Failure(businessRulesValidationResult.Errors!);
        }
        
        // Retrieve in database
        IReadOnlyList<User> users = await userRepo.GetAllUsersAsync(queryParams);
        return Result<IReadOnlyList<User>>.Success(users);
    }

    /// <inheritdoc />
    public async Task<Result<User>> GetUserByIdAsync(string userId, UserContext userContext)
    {
        // Control authorization to see some user details
        if (!authorizationValidator.CanViewUser(userId, userContext))
        {
            return Result<User>.Failure(ErrorType.Forbidden, ErrorMessages.User.NotAuthorized);
        }
            
        // Retrieve entity
        User? user = await userRepo.GetUserByIdAsync(userId);
        if (user is null)
        {
            return Result<User>.Failure(ErrorType.NotFound,ErrorMessages.NotFound(EntityName));
        }
            
        user.Posts = user.Posts.Where(p => authorizationValidator.CanViewPost(p, userContext)).ToList();
        
        return Result<User>.Success(user);
    }

    public async Task<Result<User>> GetUserEmailByUsernameAsync(string userName)
    {
        // Retrieve entity
        User? user = await userRepo.GetUserByUsernameAsync(userName);
        return user is null
            ? Result<User>.Failure(ErrorType.NotFound,ErrorMessages.NotFound(EntityName, userName))
            : Result<User>.Success(user);
    }

    public async Task<Result> TransferCreditAsync(User buyer, User seller, int price)
    {
        buyer.Credit -= price;
        seller.Credit += price;
        
        // Update buyer
        IdentityResult updateBuyerResult = await userRepo.UpdateUserAsync(buyer);
        if (!updateBuyerResult.Succeeded)
        {
            return Result.Failure(ErrorType.Persistence, ErrorMessages.ErrorWhenSavingToDb("new buyer credit"));
        }

        // Update seller
        IdentityResult updateSellerResult = await userRepo.UpdateUserAsync(seller);
        if (!updateSellerResult.Succeeded)
        {
            return Result.Failure(ErrorType.Persistence, ErrorMessages.ErrorWhenSavingToDb("new seller credit"));
        }

        return Result.Success();
    }
}