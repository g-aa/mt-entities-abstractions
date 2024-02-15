using System.Linq.Expressions;

using Mt.Entities.Abstractions.Interfaces;

namespace Mt.Entities.Abstractions.Test;

/// <summary>
/// Сущность для тестов.
/// </summary>
public sealed class TestEntity : IEntity, IDefaultable, IEqualityPredicate<TestEntity>
{
    /// <summary>
    /// Инициализация нового экземпляра класса <see cref="TestEntity"/>.
    /// </summary>
    public TestEntity()
    {
        Id = Guid.NewGuid();
        Title = string.Empty;
    }

    /// <inheritdoc />
    public Guid Id { get; set; }

    /// <inheritdoc />
    public bool Default { get; set; }

    /// <summary>
    /// Заголовок.
    /// </summary>
    public string Title { get; set; }

    /// <inheritdoc />
    public Expression<Func<TestEntity, bool>> GetEqualityPredicate()
    {
        return entity => Id == entity.Id || Title == entity.Title;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"ID = {Id}; title = {Title}";
    }
}