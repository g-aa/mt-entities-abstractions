using Mt.Entities.Abstractions.Interfaces;
using Mt.Utilities;
using Mt.Utilities.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mt.Entities.Abstractions.Extensions
{
    /// <summary>
    /// Методы расширения для типа Queryable.
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Найти сущность в последовательности.
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности.</typeparam>
        /// <param name="queryable">Запрашиваемый тип.</param>
        /// <param name="guid">Идентификатор.</param>
        /// <returns>Сущность.</returns>
        /// <exception cref="MtException">Если сущность не найдена.</exception>
        /// <exception cref="ArgumentNullException">Если входная последовательность равна null.</exception>
        public static TEntity Search<TEntity>(this IQueryable<TEntity> queryable, Guid guid)
            where TEntity : class, IEntity
        {
            var result = Check.NotNull(queryable, nameof(queryable)).SingleOrDefault(e => e.Id == guid);
            if (result is null)
            {
                throw new MtException(ErrorCode.EntityNotFoundError, $"Entity not found in sequence. ('{typeof(TEntity)}'; ID = '{guid}')");
            }
            return result;
        }

        /// <summary>
        /// Найти сущность в последовательности.
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности.</typeparam>
        /// <param name="queryable">Запрашиваемый тип.</param>
        /// <param name="entity">Исковая сущность.</param>
        /// <returns>Сущность.</returns>
        /// <exception cref="MtException">Если сущность не найдена.</exception>
        /// <exception cref="ArgumentNullException">Если входная последовательность равна null.</exception>
        public static TEntity Search<TEntity>(this IQueryable<TEntity> queryable, TEntity entity)
            where TEntity : class, IEqualityPredicate<TEntity>
        {
            var result = Check.NotNull(queryable, nameof(queryable)).SingleOrDefault(entity.GetEqualityPredicate());
            if (result is null)
            {
                throw new MtException(ErrorCode.EntityNotFoundError, $"Entity not found in sequence. ('{entity}')");
            }
            return result;
        }

        /// <summary>
        /// Найти сущность в последовательности или вернуть значение по умолчанию из последовательности.
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности.</typeparam>
        /// <param name="queryable">Запрашиваемый тип.</param>
        /// <param name="guid">Идентификатор.</param>
        /// <returns>Сущность.</returns>
        /// <exception cref="MtException">Если сущность не найдена.</exception>
        /// <exception cref="ArgumentNullException">Если входная последовательность равна null.</exception>
        public static TEntity SearchOrDefault<TEntity>(this IQueryable<TEntity> queryable, Guid guid)
            where TEntity : class, IEntity, IDefaultable
        {
            var result = Check.NotNull(queryable, nameof(queryable)).SingleOrDefault(e => e.Id == guid);
            if (result is not null)
            {
                return result;
            }
            result = queryable.SingleOrDefault(e => e.Default);
            if (result is null)
            {
                throw new MtException(ErrorCode.EntityNotFoundError, $"Entity or default value not found in sequence. ('{typeof(TEntity)}'; ID = '{guid}')");
            }
            return result;
        }

        /// <summary>
        /// Найти сущность в последовательности или вернуть значение по умолчанию из последовательности.
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности.</typeparam>
        /// <param name="queryable">Запрашиваемый тип.</param>
        /// <param name="entity">Исковая сущность.</param>
        /// <returns>Сущность.</returns>
        /// <exception cref="MtException">Если сущность не найдена.</exception>
        /// <exception cref="ArgumentNullException">Если входная последовательность равна null.</exception>
        public static TEntity SearchOrDefault<TEntity>(this IQueryable<TEntity> queryable, TEntity entity)
            where TEntity : class, IDefaultable, IEqualityPredicate<TEntity>
        {
            var result = Check.NotNull(queryable, nameof(queryable)).SingleOrDefault(entity.GetEqualityPredicate());
            if (result is not null)
            {
                return result;
            }
            result = queryable.SingleOrDefault(e => e.Default);
            if (result is null)
            {
                throw new MtException(ErrorCode.EntityNotFoundError, $"Entity or default value not found in sequence. ('{entity}')");
            }
            return result;
        }

        /// <summary>
        /// Найти сущность в последовательности или вернуть значение null.
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности.</typeparam>
        /// <param name="queryable">Запрашиваемый тип.</param>
        /// <param name="guid">Идентификатор.</param>
        /// <returns>Сущность.</returns>
        /// <exception cref="ArgumentNullException">Если входная последовательность равна null.</exception>
        public static TEntity SearchOrNull<TEntity>(this IQueryable<TEntity> queryable, Guid guid)
            where TEntity : class, IEntity
        {
            return Check.NotNull(queryable, nameof(queryable)).SingleOrDefault(e => e.Id == guid);
        }

        /// <summary>
        /// Найти несколько сущностей в последовательности или вернуть значение по умолчанию из последовательности.
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности.</typeparam>
        /// <param name="queryable">Запрашиваемый тип.</param>
        /// <param name="guids">Перечень идентификаторов.</param>
        /// <returns>Сущности.</returns>
        /// <exception cref="MtException">Если сущность не найдены.</exception>
        /// <exception cref="ArgumentNullException">Если входная последовательность равна null.</exception>
        public static IQueryable<TEntity> SearchManyOrDefault<TEntity>(this IQueryable<TEntity> queryable, IEnumerable<Guid> guids)
            where TEntity : class, IDefaultable, IEntity
        {
            var result = Check.NotNull(queryable, nameof(queryable)).Where(e => guids.Contains(e.Id));
            if (result.Any())
            {
                return result;
            }
            result = queryable.Where(e => e.Default);
            if (!result.Any())
            {
                throw new MtException(ErrorCode.EntityNotFoundError, $"The required entities not found in the sequence by keys. (IDs = '{string.Join(", ", guids)}')");
            }
            return result;
        }

        /// <summary>
        /// Проверить содержится ли сущность в последовательности.
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности.</typeparam>
        /// <param name="queryable">Запрашиваемый тип.</param>
        /// <param name="entity">Искомая сущность.</param>
        /// <returns>Результат поиска.</returns>
        /// <exception cref="ArgumentNullException">Если входная последовательность равна null.</exception>
        public static bool IsContained<TEntity>(this IQueryable<TEntity> queryable, TEntity entity)
            where TEntity : class, IEqualityPredicate<TEntity>
        {
            return Check.NotNull(queryable, nameof(queryable)).SingleOrDefault(entity.GetEqualityPredicate()) != null;
        }
    }
}