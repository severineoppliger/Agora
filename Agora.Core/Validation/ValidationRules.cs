namespace Agora.Core.Validation;

public class ValidationRules
{
    public static class AppUser
    {
        public const int UsernameMinLength = 5;
        public const int UsernameMaxLength = 50;
        public const int EmailMinLength = 5;
        public const int EmailMaxLength = 255;
        public const int PasswordMinLength = 8;
        public const int PasswordMaxLength = 255;
    }
    
    public static class PostCategory
    {
        public const int NameMinLength = 3;
        public const int NameMaxLength = 100;
    }
    
    public static class Post
    {
        public const int TitleMinLength = 3;
        public const int TitleMaxLength = 100;
        public const int DescriptionMinLength = 5;
        public const int DescriptionMaxLength = 2000;
        public const int PriceMin = 0;
        public const int PriceMax = 100000;
    }

    
    public static class TransactionStatus
    {
        public const int NameMinLength = 3;
        public const int NameMaxLength = 50;
        public const int DescriptionMinLength = 3;
        public const int DescriptionMaxLength = 500;
    }

    
    public static class Transaction
    {
        public const int PriceMin = Post.PriceMin;
        public const int PriceMax = Post.PriceMax;
    }
}