namespace Agora.Core.Commands;

/// <summary>
/// Command to update details of an existing <c>PostCategory</c>.
/// Only provided properties will be updated; null values will be ignored.
/// </summary>
public class UpdatePostCategoryDetailsCommand
{
    /// <summary>
    /// New name of the post category.
    /// </summary>
    public string Name { get; set; } = String.Empty;
}