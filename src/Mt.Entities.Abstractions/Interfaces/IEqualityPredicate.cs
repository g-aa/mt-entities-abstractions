using System.Linq.Expressions;

namespace Mt.Entities.Abstractions.Interfaces;

/// <summary>
/// Сущность МТ с предикатом сравнения.
/// </summary>
/// <typeparam name="TEntity">Тип сущности.</typeparam>
public interface IEqualityPredicate<TEntity>
{
    /// <summary>
    /// Получить предикат сравнения.
    /// </summary>
    /// <returns>Предикат сравнения.</returns>
    Expression<Func<TEntity, bool>> GetEqualityPredicate();
}