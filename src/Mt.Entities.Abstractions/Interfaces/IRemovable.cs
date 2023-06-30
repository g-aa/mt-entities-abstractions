namespace Mt.Entities.Abstractions.Interfaces;

/// <summary>
/// Сущность МТ, удаляемая.
/// </summary>
public interface IRemovable
{
    /// <summary>
    /// Возможность удалить.
    /// </summary>
    bool Removable { get; set; }
}