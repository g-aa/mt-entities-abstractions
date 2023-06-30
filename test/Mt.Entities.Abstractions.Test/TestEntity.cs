using Mt.Entities.Abstractions.Interfaces;
using System.Linq.Expressions;

namespace Mt.Entities.Abstractions.Test;

/// <summary>
/// Сущность для тестов.
/// </summary>
public sealed class TestEntity : IEntity, IDefaultable, IEqualityPredicate<TestEntity>
{
    /// <inheritdoc />
    public Guid Id { get; set; }

    /// <inheritdoc />
    public bool Default { get; set; }

    /// <inheritdoc />

    public string Title { get; set; }

    /// <summary>
    /// Инициализация нового экземпляра класса <see cref="TestEntity"/>.
    /// </summary>
    public TestEntity()
    {
        this.Id = Guid.NewGuid();
        this.Title = string.Empty;
    }

    /// <inheritdoc />
    public Expression<Func<TestEntity, bool>> GetEqualityPredicate()
    {
        return entity => this.Id == entity.Id || this.Title == entity.Title;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"ID = {this.Id}; title = {this.Title}";
    }
}
