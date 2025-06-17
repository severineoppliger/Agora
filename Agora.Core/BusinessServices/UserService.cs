using Agora.Core.BusinessRules.Interfaces;
using Agora.Core.Common;
using Agora.Core.Enums;
using Agora.Core.Interfaces.BusinessServices;
using Agora.Core.Interfaces.Filters;
using Agora.Core.Interfaces.Repositories;
using Agora.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Agora.Core.BusinessServices;

/// <inheritdoc />
public class UserService(
    IUserRepository userRepo,
    IAuthorizationBusinessRules authorizationBusinessRules) : IUserService
{
    private const string EntityName = "user";

    /// <inheritdoc />
    public async Task<Result<IReadOnlyList<User>>> GetAllUsersAsync(IUserFilter userQueryParameters)
    {
        IReadOnlyList<User> users = await userRepo.GetAllUsersAsync(userQueryParameters);
        return Result<IReadOnlyList<User>>.Success(users);
    }

    /// <inheritdoc />
    public async Task<Result<User>> GetUserByIdAsync(string userId, UserContext userContext)
    {
        // Control authorization to see some user details
        if (!authorizationBusinessRules.CanViewUser(userId, userContext))
        {
            return Result<User>.Failure(ErrorType.Forbidden, ErrorMessages.User.NotAuthorized);
        }
            
        // Retrieve entity
        User? user = await userRepo.GetUserByIdAsync(userId);
        if (user is null)
        {
            return Result<User>.Failure(ErrorType.NotFound,ErrorMessages.NotFound(EntityName));
        }
            
        user.Posts = user.Posts.Where(p => authorizationBusinessRules.CanViewPost(p, userContext)).ToList();
        
        return Result<User>.Success(user);
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