namespace Agora.Core.Models.Entities;

/// <summary>
/// Base entity class providing a unique identifier for all entities inheriting this class.
/// </summary>
public class BaseEntity
{
    /// <summary>
    /// Unique identifier of the entity.
    /// </summary>
    public long Id { get; set; }
}