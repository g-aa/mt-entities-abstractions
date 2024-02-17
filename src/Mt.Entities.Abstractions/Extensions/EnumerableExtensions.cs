using Mt.Entities.Abstractions.Interfaces;
using Mt.Utilities;
using Mt.Utilities.Exceptions;

namespace Mt.Entities.Abstractions.Extensions;

/// <summary>
/// Методы расширения для <see cref="IEnumerable{T}"/>.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Найти сущность в последовательности.
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности.</typeparam>
    /// <param name="enumerable">Перечисляемый тип.</param>
    /// <param name="guid">Идентификатор.</param>
    /// <returns>Сущность.</returns>
    /// <exception cref="MtException">Если сущность не найдена.</exception>
    /// <exception cref="ArgumentNullException">Если входная последовательность равна null.</exception>
    public static TEntity Search<TEntity>(this IEnumerable<TEntity> enumerable, Guid guid)
        where TEntity : class, IEntity
    {
        var result = Check.NotNull(enumerable, nameof(enumerable)).SingleOrDefault(e => e.Id == guid);
        if (result is null)
        {
            throw new MtException(ErrorCode.EntityNotFound, $"Entity not found in sequence. ('{typeof(TEntity)}'; ID = '{guid}')");
        }

        return result;
    }

    /// <summary>
    /// Найти сущность в последовательности.
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности.</typeparam>
    /// <param name="enumerable">Перечисляемый тип.</param>
    /// <param name="entity">Исковая сущность.</param>
    /// <returns>Сущность.</returns>
    /// <exception cref="MtException">Если сущность не найдена.</exception>
    /// <exception cref="ArgumentNullException">Если входная последовательность равна null.</exception>
    public static TEntity Search<TEntity>(this IEnumerable<TEntity> enumerable, TEntity entity)
        where TEntity : class, IEqualityPredicate<TEntity>
    {
        var result = Check.NotNull(enumerable, nameof(enumerable)).SingleOrDefault(entity.GetEqualityPredicate().Compile());
        if (result is null)
        {
            throw new MtException(ErrorCode.EntityNotFound, $"Entity not found in sequence. ('{entity}')");
        }

        return result;
    }

    /// <summary>
    /// Найти сущность в последовательности или вернуть значение по умолчанию из последовательности.
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности.</typeparam>
    /// <param name="enumerable">Перечисляемый тип.</param>
    /// <param name="guid">Идентификатор.</param>
    /// <returns>Сущность.</returns>
    /// <exception cref="MtException">Если сущность не найдена.</exception>
    /// <exception cref="ArgumentNullException">Если входная последовательность равна null.</exception>
    public static TEntity SearchOrDefault<TEntity>(this IEnumerable<TEntity> enumerable, Guid guid)
        where TEntity : class, IEntity, IDefaultable
    {
        var result = Check.NotNull(enumerable, nameof(enumerable)).SingleOrDefault(e => e.Id == guid);
        if (result is not null)
        {
            return result;
        }

        result = enumerable.SingleOrDefault(e => e.Default);
        if (result is null)
        {
            throw new MtException(ErrorCode.EntityNotFound, $"Entity or default value not found in sequence. ('{typeof(TEntity)}'; ID = '{guid}')");
        }

        return result;
    }

    /// <summary>
    /// Найти сущность в последовательности или вернуть значение по умолчанию из последовательности.
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности.</typeparam>
    /// <param name="enumerable">Перечисляемый тип.</param>
    /// <param name="entity">Исковая сущность.</param>
    /// <returns>Сущность.</returns>
    /// <exception cref="MtException">Если сущность не найдена.</exception>
    /// <exception cref="ArgumentNullException">Если входная последовательность равна null.</exception>
    public static TEntity SearchOrDefault<TEntity>(this IEnumerable<TEntity> enumerable, TEntity entity)
        where TEntity : class, IDefaultable, IEqualityPredicate<TEntity>
    {
        var result = Check.NotNull(enumerable, nameof(enumerable)).SingleOrDefault(entity.GetEqualityPredicate().Compile());
        if (result is not null)
        {
            return result;
        }

        result = enumerable.SingleOrDefault(e => e.Default);
        if (result is null)
        {
            throw new MtException(ErrorCode.EntityNotFound, $"Entity or default value not found in sequence. ('{entity}')");
        }

        return result;
    }

    /// <summary>
    /// Найти сущность в последовательности или создать новую сущность.
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности.</typeparam>
    /// <param name="enumerable">Перечисляемый тип.</param>
    /// <param name="guid">Идентификатор.</param>
    /// <param name="factory">Способ создания сущноти.</param>
    /// <returns>Сущность.</returns>
    /// <exception cref="ArgumentNullException">Если входная последовательность равна null.</exception>
    public static TEntity? SearchOrCreate<TEntity>(this IEnumerable<TEntity> enumerable, Guid guid, Func<TEntity?>? factory = null)
        where TEntity : class, IEntity
    {
        var result = Check.NotNull(enumerable, nameof(enumerable)).SingleOrDefault(e => e.Id == guid);
        return result ?? factory?.Invoke();
    }

    /// <summary>
    /// Найти сущность в последовательности или создать новую сущность.
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности.</typeparam>
    /// <param name="enumerable">Перечисляемый тип.</param>
    /// <param name="entity">Исковая сущность.</param>
    /// <param name="factory">Способ создания сущноти.</param>
    /// <returns>Сущность.</returns>
    /// <exception cref="ArgumentNullException">Если входная последовательность равна null.</exception>
    public static TEntity? SearchOrCreate<TEntity>(this IEnumerable<TEntity> enumerable, TEntity entity, Func<TEntity?>? factory = null)
        where TEntity : class, IEqualityPredicate<TEntity>
    {
        var result = Check.NotNull(enumerable, nameof(enumerable)).SingleOrDefault(entity.GetEqualityPredicate().Compile());
        return result ?? factory?.Invoke();
    }

    /// <summary>
    /// Найти сущность в последовательности или вернуть значение null.
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности.</typeparam>
    /// <param name="enumerable">Перечисляемый тип.</param>
    /// <param name="guid">Идентификатор.</param>
    /// <returns>Сущность.</returns>
    /// <exception cref="ArgumentNullException">Если входная последовательность равна null.</exception>
    public static TEntity? SearchOrNull<TEntity>(this IEnumerable<TEntity> enumerable, Guid guid)
        where TEntity : class, IEntity
    {
        return Check.NotNull(enumerable, nameof(enumerable)).SingleOrDefault(e => e.Id == guid);
    }

    /// <summary>
    /// Найти несколько сущностей в последовательности или вернуть значение по умолчанию из последовательности.
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности.</typeparam>
    /// <param name="enumerable">Перечисляемый тип.</param>
    /// <param name="guids">Перечень идентификаторов.</param>
    /// <returns>Сущности.</returns>
    /// <exception cref="MtException">Если сущность не найдены.</exception>
    /// <exception cref="ArgumentNullException">Если входная последовательность равна null.</exception>
    public static IEnumerable<TEntity> SearchManyOrDefault<TEntity>(this IEnumerable<TEntity> enumerable, IEnumerable<Guid> guids)
        where TEntity : class, IDefaultable, IEntity
    {
        var result = Check.NotNull(enumerable, nameof(enumerable)).Where(e => guids.Contains(e.Id));
        if (result.Any())
        {
            return result;
        }

        result = enumerable.Where(e => e.Default);
        if (!result.Any())
        {
            throw new MtException(ErrorCode.EntityNotFound, $"The required entities not found in the sequence by keys. (IDs = '{string.Join(", ", guids)}')");
        }

        return result;
    }

    /// <summary>
    /// Проверить содержится ли сущность в последовательности.
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности.</typeparam>
    /// <param name="enumerable">Перечисляемый тип.</param>
    /// <param name="entity">Искомая сущность.</param>
    /// <returns>Результат поиска.</returns>
    /// <exception cref="ArgumentNullException">Если входная последовательность равна null.</exception>
    public static bool IsContained<TEntity>(this IEnumerable<TEntity> enumerable, TEntity entity)
        where TEntity : class, IEqualityPredicate<TEntity>
    {
        return Check.NotNull(enumerable, nameof(enumerable)).SingleOrDefault(entity.GetEqualityPredicate().Compile()) != null;
    }
}