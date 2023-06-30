namespace Mt.Entities.Abstractions.Interfaces;

/// <summary>
/// Сущность МТ по умолчанию.
/// </summary>
public interface IDefaultable
{
    /// <summary>
    /// Значение по умолчанию.
    /// </summary>
    bool Default { get; set; }
}