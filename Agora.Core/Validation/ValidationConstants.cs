namespace Agora.Core.Validation;

/// <summary>
/// Contains constant values defining validation rules for properties of domain entities,
/// such as minimum and maximum lengths or allowed ranges.
/// </summary>
public class ValidationConstants
{
    /// <summary>
    /// Validation constants for the properties of the <c>User</c> entity.
    /// </summary>
    public static class User
    {
        public const int UsernameMinLength = 5;
        public const int UsernameMaxLength = 50;
        public const int EmailMinLength = 5;
        public const int EmailMaxLength = 255;
        public const int PasswordMinLength = 8;
        public const int PasswordMaxLength = 255;
    }
    
    /// <summary>
    /// Validation constants for the properties of the <c>PostCategory</c> entity.
    /// </summary>
    public static class PostCategory
    {
        public const int NameMinLength = 3;
        public const int NameMaxLength = 100;
    }
    
    /// <summary>
    /// Validation constants for the properties of the <c>Post</c> entity.
    /// </summary>
    public static class Post
    {
        public const int TitleMinLength = 3;
        public const int TitleMaxLength = 100;
        public const int DescriptionMinLength = 5;
        public const int DescriptionMaxLength = 2000;
        public const int PriceMin = 0;
        public const int PriceMax = 100000;
    }

    /// <summary>
    /// Validation constants for the properties of the <c>TransactionStatus</c> entity.
    /// </summary>
    public static class TransactionStatus
    {
        public const int NameMinLength = 3;
        public const int NameMaxLength = 50;
        public const int DescriptionMinLength = 5;
        public const int DescriptionMaxLength = 500;
    }

    /// <summary>
    /// Validation constants for the properties of the <c>Transaction</c> entity.
    /// </summary>
    public static class Transaction
    {
        public const int TitleMinLength = 10;
        public const int TitleMaxLength = 200;
        public const int PriceMin = Post.PriceMin;
        public const int PriceMax = Post.PriceMax;
    }
}