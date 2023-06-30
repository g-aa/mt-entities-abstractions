namespace Mt.Entities.Abstractions.Interfaces;

/// <summary>
/// Сущность МТ.
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    Guid Id { get; set; }
}