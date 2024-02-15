using Mt.Entities.Abstractions.Extensions;

namespace Mt.Entities.Abstractions.Test;

/// <summary>
/// Набор тестов для <see cref="QueryableExtensions"/>.
/// </summary>
public sealed class QueryableExtensionsTests
{
    private static readonly IQueryable<TestEntity>? NullSequence = null!;

    private static readonly IQueryable<TestEntity> EmptySequence = Array.Empty<TestEntity>().AsQueryable();

    private static readonly IQueryable<TestEntity> Sequence = new[]
    {
        new TestEntity() { Id = Guid.Parse("C5BBF5EB-38FD-4394-A0A5-24912BCC5A63"), Title = "Entity 0" },
        new TestEntity() { Id = Guid.Parse("F354633E-2744-4F73-9B44-5D1D401283D7"), Title = "Entity 1" },
        new TestEntity() { Id = Guid.Parse("71D69566-C5C8-4B56-8268-BAC368F98CF4"), Title = "Entity 2" },
        new TestEntity() { Id = Guid.Parse("E342C5E4-66A6-4105-A050-DA0DF10D8200"), Title = "Entity 3" },
        new TestEntity() { Id = Guid.Parse("F29AB748-08DF-4CA0-92B3-666A7A7C88E2"), Title = "Entity 4" },
    }.AsQueryable();

    private static readonly IQueryable<TestEntity> DefaultSequence = new[]
    {
        new TestEntity() { Id = Guid.Parse("6A6F10FD-36E6-4AB9-A2DD-7CBE064D9A49"), Title = "Entity 5" },
        new TestEntity() { Id = Guid.Parse("36D5AF43-9EA8-45EF-8A77-A484439CE20C"), Title = "Default 1", Default = true },
        new TestEntity() { Id = Guid.Parse("4C11E861-8E9B-48BD-A029-46D8320142D7"), Title = "Entity 7" },
        new TestEntity() { Id = Guid.Parse("58F891F7-52A5-4AEC-9A3A-AD36621A14AF"), Title = "Entity 9" },
    }.AsQueryable();

    private static readonly IQueryable<TestEntity> DefaultManySequence = new[]
    {
        new TestEntity() { Id = Guid.Parse("6A6F10FD-36E6-4AB9-A2DD-7CBE064D9A49"), Title = "Entity 5" },
        new TestEntity() { Id = Guid.Parse("36D5AF43-9EA8-45EF-8A77-A484439CE20C"), Title = "Default 1", Default = true },
        new TestEntity() { Id = Guid.Parse("4C11E861-8E9B-48BD-A029-46D8320142D7"), Title = "Entity 7" },
        new TestEntity() { Id = Guid.Parse("F3DD228E-F266-4251-87D0-3F433A8384E9"), Title = "Default 2", Default = true },
        new TestEntity() { Id = Guid.Parse("58F891F7-52A5-4AEC-9A3A-AD36621A14AF"), Title = "Entity 9" },
    }.AsQueryable();

    private static readonly Func<TestEntity>? NullFactory = null!;

    private static readonly Func<TestEntity> EntityFactory = () =>
    {
        return new TestEntity()
        {
            Id = Guid.NewGuid(),
            Title = "Entity 6",
            Default = false,
        };
    };

    /// <summary>
    /// Положительный тест для <see cref="QueryableExtensions.Search{TEntity}(IQueryable{TEntity}, Guid)"/>.
    /// </summary>
    /// <param name="seq">Наименование последовательности.</param>
    /// <param name="guid">UUID.</param>
    /// <param name="expected">Ожидаемый результат.</param>
    [TestCase(nameof(Sequence), "C5BBF5EB-38FD-4394-A0A5-24912BCC5A63", "Entity 0")]
    [TestCase(nameof(DefaultManySequence), "4C11E861-8E9B-48BD-A029-46D8320142D7", "Entity 7")]
    public void SearchPositiveTest(string seq, Guid guid, string expected)
    {
        // act
        var result = GetQueryable(seq)!.Search(guid);

        // assert
        result.Title.Should().Be(expected);
    }

    /// <summary>
    /// Положительный тест для <see cref="QueryableExtensions.Search{TEntity}(IQueryable{TEntity}, TEntity)"/>.
    /// </summary>
    /// <param name="seq">Наименование последовательности.</param>
    /// <param name="guid">UUID.</param>
    /// <param name="title">Заголовок.</param>
    /// <param name="expected">Ожидаемый результат.</param>
    [TestCase(nameof(Sequence), "C5BBF5EB-38FD-4394-A0A5-24912BCC5A63", "Entity 0", "Entity 0")]
    [TestCase(nameof(DefaultManySequence), "4C11E861-8E9B-48BD-A029-46D8320142D7", "Entity 7", "Entity 7")]
    public void SearchPositiveTest(string seq, Guid guid, string title, string expected)
    {
        // arrange
        var entity = new TestEntity
        {
            Id = guid,
            Title = title,
        };

        // act
        var result = GetQueryable(seq)!.Search(entity);

        // assert
        result.Title.Should().Be(expected);
    }

    /// <summary>
    /// Отрицательный тест для <see cref="QueryableExtensions.Search{TEntity}(IQueryable{TEntity}, Guid)"/>.
    /// </summary>
    /// <param name="seq">Наименование последовательности.</param>
    /// <param name="guid">UUID.</param>
    /// <param name="expected">Ожидаемый результат.</param>
    [TestCase(nameof(NullSequence), "00000000-0000-0000-0000-000000000000", "Checked parameter is null. (Parameter 'queryable')")]
    [TestCase(nameof(EmptySequence), "00000000-0000-0000-0000-000000000000", "MT-E0011: Entity not found in sequence. ('Mt.Entities.Abstractions.Test.TestEntity'; ID = '00000000-0000-0000-0000-000000000000')")]
    [TestCase(nameof(Sequence), "00000000-0000-0000-0000-000000000000", "MT-E0011: Entity not found in sequence. ('Mt.Entities.Abstractions.Test.TestEntity'; ID = '00000000-0000-0000-0000-000000000000')")]
    [TestCase(nameof(DefaultManySequence), "00000000-0000-0000-0000-000000000000", "MT-E0011: Entity not found in sequence. ('Mt.Entities.Abstractions.Test.TestEntity'; ID = '00000000-0000-0000-0000-000000000000')")]
    public void SearchNegotiveTest(string seq, Guid guid, string expected)
    {
        // act
        var func = () => GetQueryable(seq)!.Search(guid);

        // assert
        func.Should().Throw<Exception>().WithMessage(expected);
    }

    /// <summary>
    /// Отрицательный тест для <see cref="QueryableExtensions.Search{TEntity}(IQueryable{TEntity}, TEntity)"/>.
    /// </summary>
    /// <param name="seq">Наименование последовательности.</param>
    /// <param name="guid">UUID.</param>
    /// <param name="title">Заголовок.</param>
    /// <param name="expected">Ожидаемый результат.</param>
    [TestCase(nameof(NullSequence), "00000000-0000-0000-0000-000000000000", "Entity", "Checked parameter is null. (Parameter 'queryable')")]
    [TestCase(nameof(EmptySequence), "00000000-0000-0000-0000-000000000000", "Entity", "MT-E0011: Entity not found in sequence. ('ID = 00000000-0000-0000-0000-000000000000; title = Entity')")]
    [TestCase(nameof(Sequence), "00000000-0000-0000-0000-000000000000", "Entity", "MT-E0011: Entity not found in sequence. ('ID = 00000000-0000-0000-0000-000000000000; title = Entity')")]
    [TestCase(nameof(DefaultManySequence), "00000000-0000-0000-0000-000000000000", "Entity", "MT-E0011: Entity not found in sequence. ('ID = 00000000-0000-0000-0000-000000000000; title = Entity')")]
    public void SearchNegotiveTest(string seq, Guid guid, string title, string expected)
    {
        // arrange
        var entity = new TestEntity
        {
            Id = guid,
            Title = title,
        };

        // act
        var func = () => GetQueryable(seq)!.Search(entity);

        // assert
        func.Should().Throw<Exception>().WithMessage(expected);
    }

    /// <summary>
    /// Положительный тест для <see cref="QueryableExtensions.SearchOrDefault{TEntity}(IQueryable{TEntity}, Guid)"/>.
    /// </summary>
    /// <param name="seq">Наименование последовательности.</param>
    /// <param name="guid">UUID.</param>
    /// <param name="expected">Ожидаемый результат.</param>
    [TestCase(nameof(DefaultSequence), "4C11E861-8E9B-48BD-A029-46D8320142D7", "Entity 7")]
    [TestCase(nameof(DefaultManySequence), "4C11E861-8E9B-48BD-A029-46D8320142D7", "Entity 7")]
    [TestCase(nameof(DefaultSequence), "00000000-0000-0000-0000-000000000000", "Default 1")]
    public void SearchOrDefaultPositiveTest(string seq, Guid guid, string expected)
    {
        // act
        var result = GetQueryable(seq)!.SearchOrDefault(guid);

        // assert
        result.Title.Should().Be(expected);
    }

    /// <summary>
    /// Положительный тест для <see cref="QueryableExtensions.SearchOrDefault{TEntity}(IQueryable{TEntity}, TEntity)"/>.
    /// </summary>
    /// <param name="seq">Наименование последовательности.</param>
    /// <param name="guid">UUID.</param>
    /// <param name="title">Заголовок.</param>
    /// <param name="expected">Ожидаемый результат.</param>
    [TestCase(nameof(DefaultSequence), "4C11E861-8E9B-48BD-A029-46D8320142D7", "Entity 7", "Entity 7")]
    [TestCase(nameof(DefaultManySequence), "4C11E861-8E9B-48BD-A029-46D8320142D7", "Entity 7", "Entity 7")]
    [TestCase(nameof(DefaultSequence), "00000000-0000-0000-0000-000000000000", "Entity", "Default 1")]
    public void SearchOrDefaultPositiveTest(string seq, Guid guid, string title, string expected)
    {
        // arrange
        var entity = new TestEntity
        {
            Id = guid,
            Title = title,
        };

        // act
        var result = GetQueryable(seq)!.SearchOrDefault(entity);

        // assert
        result.Title.Should().Be(expected);
    }

    /// <summary>
    /// Отрицательный тест для <see cref="QueryableExtensions.SearchOrDefault{TEntity}(IQueryable{TEntity}, Guid)"/>.
    /// </summary>
    /// <param name="seq">Наименование последовательности.</param>
    /// <param name="guid">UUID.</param>
    /// <param name="expected">Ожидаемый результат.</param>
    [TestCase(nameof(NullSequence), "00000000-0000-0000-0000-000000000000", "Checked parameter is null. (Parameter 'queryable')")]
    [TestCase(nameof(DefaultManySequence), "00000000-0000-0000-0000-000000000000", "Sequence contains more than one matching element")]
    [TestCase(nameof(EmptySequence), "00000000-0000-0000-0000-000000000000", "MT-E0011: Entity or default value not found in sequence. ('Mt.Entities.Abstractions.Test.TestEntity'; ID = '00000000-0000-0000-0000-000000000000')")]
    [TestCase(nameof(Sequence), "00000000-0000-0000-0000-000000000000", "MT-E0011: Entity or default value not found in sequence. ('Mt.Entities.Abstractions.Test.TestEntity'; ID = '00000000-0000-0000-0000-000000000000')")]
    public void SearchOrDefaultNegotiveTest(string seq, Guid guid, string expected)
    {
        // act
        var func = () => GetQueryable(seq)!.SearchOrDefault(guid);

        // assert
        func.Should().Throw<Exception>().WithMessage(expected);
    }

    /// <summary>
    /// Отрицательный тест для <see cref="QueryableExtensions.SearchOrDefault{TEntity}(IQueryable{TEntity}, TEntity)"/>.
    /// </summary>
    /// <param name="seq">Наименование последовательности.</param>
    /// <param name="guid">UUID.</param>
    /// <param name="title">Заголовок.</param>
    /// <param name="expected">Ожидаемый результат.</param>
    [TestCase(nameof(NullSequence), "00000000-0000-0000-0000-000000000000", "Entity", "Checked parameter is null. (Parameter 'queryable')")]
    [TestCase(nameof(DefaultManySequence), "00000000-0000-0000-0000-000000000000", "Entity", "Sequence contains more than one matching element")]
    [TestCase(nameof(EmptySequence), "00000000-0000-0000-0000-000000000000", "Entity", "MT-E0011: Entity or default value not found in sequence. ('ID = 00000000-0000-0000-0000-000000000000; title = Entity')")]
    [TestCase(nameof(Sequence), "00000000-0000-0000-0000-000000000000", "Entity", "MT-E0011: Entity or default value not found in sequence. ('ID = 00000000-0000-0000-0000-000000000000; title = Entity')")]
    public void SearchOrDefaultNegotiveTest(string seq, Guid guid, string title, string expected)
    {
        // arrange
        var entity = new TestEntity
        {
            Id = guid,
            Title = title,
        };

        // act
        var func = () => GetQueryable(seq)!.SearchOrDefault(entity);

        // assert
        func.Should().Throw<Exception>().WithMessage(expected);
    }

    /// <summary>
    /// Положительный тест для <see cref="QueryableExtensions.SearchOrCreate{TEntity}(IQueryable{TEntity}, Guid, Func{TEntity})"/>.
    /// </summary>
    /// <param name="seq">Наименование последовательности.</param>
    /// <param name="factory">Наименование фабрики.</param>
    /// <param name="guid">UUID.</param>
    /// <param name="expected">Ожидаемый результат.</param>
    [TestCase(nameof(DefaultSequence), nameof(NullFactory), "4C11E861-8E9B-48BD-A029-46D8320142D7", "Entity 7")]
    [TestCase(nameof(DefaultSequence), nameof(NullFactory), "00000000-0000-0000-0000-000000000000", null)]
    [TestCase(nameof(DefaultSequence), nameof(EntityFactory), "00000000-0000-0000-0000-000000000000", "Entity 6")]
    public void SearchOrCreatePositiveTest(string seq, string factory, Guid guid, string? expected)
    {
        // act
        var result = GetQueryable(seq)!.SearchOrCreate(guid, GetFactory(factory));

        // assert
        result?.Title.Should().Be(expected);
    }

    /// <summary>
    /// Положительный тест для <see cref="QueryableExtensions.SearchOrCreate{TEntity}(IQueryable{TEntity}, TEntity, Func{TEntity})"/>.
    /// </summary>
    /// <param name="seq">Наименование последовательности.</param>
    /// <param name="factory">Наименование фабрики.</param>
    /// <param name="title">Заголовок.</param>
    /// <param name="guid">UUID.</param>
    /// <param name="expected">Ожидаемый результат.</param>
    [TestCase(nameof(DefaultSequence), nameof(NullFactory), "Entity 7", "4C11E861-8E9B-48BD-A029-46D8320142D7", "Entity 7")]
    [TestCase(nameof(DefaultSequence), nameof(NullFactory), "Entity 1", "00000000-0000-0000-0000-000000000000", null)]
    [TestCase(nameof(DefaultSequence), nameof(EntityFactory), "Entity 1", "00000000-0000-0000-0000-000000000000", "Entity 6")]
    public void SearchOrCreatePositiveTest(string seq, string factory, string title, Guid guid, string? expected)
    {
        // arrange
        var entity = new TestEntity
        {
            Id = guid,
            Title = title,
        };

        // act
        var result = GetQueryable(seq)!.SearchOrCreate(entity, GetFactory(factory));

        // assert
        result?.Title.Should().Be(expected);
    }

    /// <summary>
    /// Отрицательный тест для <see cref="QueryableExtensions.SearchOrCreate{TEntity}(IQueryable{TEntity}, Guid, Func{TEntity})"/>.
    /// </summary>
    /// <param name="seq">Наименование последовательности.</param>
    /// <param name="guid">UUID.</param>
    /// <param name="expected">Ожидаемый результат.</param>
    [TestCase(nameof(NullSequence), "00000000-0000-0000-0000-000000000000", "Checked parameter is null. (Parameter 'queryable')")]
    public void SearchOrCreateNegotiveTest(string seq, Guid guid, string expected)
    {
        // act
        var func = () => GetQueryable(seq)!.SearchOrCreate(guid);

        // assert
        func.Should().Throw<Exception>().WithMessage(expected);
    }

    /// <summary>
    /// Отрицательный тест для <see cref="QueryableExtensions.SearchOrCreate{TEntity}(IQueryable{TEntity}, TEntity, Func{TEntity})"/>.
    /// </summary>
    /// <param name="seq">Наименование последовательности.</param>
    /// <param name="title">Заголовок.</param>
    /// <param name="guid">UUID.</param>
    /// <param name="expected">Ожидаемый результат.</param>
    [TestCase(nameof(NullSequence), "Entity 7", "00000000-0000-0000-0000-000000000000", "Checked parameter is null. (Parameter 'queryable')")]
    public void SearchOrCreateNegotiveTest(string seq, string title, Guid guid, string expected)
    {
        // arrange
        var entity = new TestEntity
        {
            Id = guid,
            Title = title,
        };

        // act
        var func = () => GetQueryable(seq)!.SearchOrCreate(entity);

        // assert
        func.Should().Throw<Exception>().WithMessage(expected);
    }

    /// <summary>
    /// Положительный тест для <see cref="QueryableExtensions.SearchOrNull{TEntity}(IQueryable{TEntity}, Guid)"/>.
    /// </summary>
    /// <param name="seq">Наименование последовательности.</param>
    /// <param name="guid">UUID.</param>
    /// <param name="expected">Ожидаемый результат.</param>
    [TestCase(nameof(Sequence), "C5BBF5EB-38FD-4394-A0A5-24912BCC5A63", "Entity 0")]
    [TestCase(nameof(Sequence), "00000000-0000-0000-0000-000000000000", null)]
    [TestCase(nameof(EmptySequence), "C5BBF5EB-38FD-4394-A0A5-24912BCC5A63", null)]
    public void SearchOrNullPositiveTest(string seq, Guid guid, string? expected)
    {
        // act
        var result = GetQueryable(seq)!.SearchOrNull(guid);

        // assert
        result?.Title.Should().Be(expected);
    }

    /// <summary>
    /// Отрицательный тест для <see cref="QueryableExtensions.SearchOrNull{TEntity}(IQueryable{TEntity}, Guid)"/>.
    /// </summary>
    /// <param name="seq">Наименование последовательности.</param>
    /// <param name="guid">UUID.</param>
    /// <param name="expected">Ожидаемый результат.</param>
    [TestCase(nameof(NullSequence), "C5BBF5EB-38FD-4394-A0A5-24912BCC5A63", "Checked parameter is null. (Parameter 'queryable')")]
    public void SearchOrNullNegotiveTest(string seq, Guid guid, string expected)
    {
        // act
        var func = () => GetQueryable(seq)!.SearchOrNull(guid);

        // assert
        func.Should().Throw<Exception>().WithMessage(expected);
    }

    /// <summary>
    /// Положительный тест для <see cref="QueryableExtensions.IsContained{TEntity}(IQueryable{TEntity}, TEntity)"/>.
    /// </summary>
    /// <param name="seq">Наименование последовательности.</param>
    /// <param name="guid">UUID.</param>
    /// <param name="title">Заголовок.</param>
    /// <param name="expected">Ожидаемый результат.</param>
    [TestCase(nameof(Sequence), "F354633E-2744-4F73-9B44-5D1D401283D7", "Entity 1", true)]
    [TestCase(nameof(Sequence), "00000000-0000-0000-0000-000000000000", "Entity", false)]
    [TestCase(nameof(EmptySequence), "C5BBF5EB-38FD-4394-A0A5-24912BCC5A63", "Entity", false)]
    public void IsContainedPositiveTest(string seq, Guid guid, string title, bool expected)
    {
        // arrange
        var entity = new TestEntity
        {
            Id = guid,
            Title = title,
        };

        // act
        var result = GetQueryable(seq)!.IsContained(entity);

        // assert
        result.Should().Be(expected);
    }

    /// <summary>
    /// Отрицательный тест для <see cref="QueryableExtensions.IsContained{TEntity}(IQueryable{TEntity}, TEntity)"/>.
    /// </summary>
    /// <param name="seq">Наименование последовательности.</param>
    /// <param name="guid">UUID.</param>
    /// <param name="title">Заголовок.</param>
    /// <param name="expected">Ожидаемый результат.</param>
    [TestCase(nameof(NullSequence), "C5BBF5EB-38FD-4394-A0A5-24912BCC5A63", "Entity", "Checked parameter is null. (Parameter 'queryable')")]
    public void IsContainedNegotiveTest(string seq, Guid guid, string title, string expected)
    {
        // arrange
        var entity = new TestEntity
        {
            Id = guid,
            Title = title,
        };

        // act
        var func = () => GetQueryable(seq)!.IsContained(entity);

        // assert
        func.Should().Throw<Exception>().WithMessage(expected);
    }

    /// <summary>
    /// Положительный тест для <see cref="QueryableExtensions.SearchManyOrDefault{TEntity}(IQueryable{TEntity}, System.Collections.Generic.IEnumerable{Guid})"/>.
    /// </summary>
    /// <param name="seq">Наименование последовательности.</param>
    /// <param name="objects">UUIDs.</param>
    [TestCase(nameof(Sequence), new object[] { "C5BBF5EB-38FD-4394-A0A5-24912BCC5A63", "E342C5E4-66A6-4105-A050-DA0DF10D8200" })]
    [TestCase(nameof(DefaultSequence), new object[] { "00000000-0000-0000-0000-000000000000", "11111111-1111-1111-1111-111111111111" })]
    [TestCase(nameof(DefaultManySequence), new object[] { "00000000-0000-0000-0000-000000000000", "11111111-1111-1111-1111-111111111111" })]
    public void SearchManyOrDefaultPositiveTest(string seq, object[] objects)
    {
        // arrange
        var ids = objects.Select(obj => Guid.Parse((string)obj));

        // act
        var resutl = GetQueryable(seq)!.SearchManyOrDefault(ids);

        // assert
        resutl.Should().NotBeNullOrEmpty();
    }

    /// <summary>
    /// Отрицательный тест для <see cref="QueryableExtensions.SearchManyOrDefault{TEntity}(IQueryable{TEntity}, System.Collections.Generic.IEnumerable{Guid})"/>.
    /// </summary>
    /// <param name="seq">Наименование последовательности.</param>
    /// <param name="objects">UUIDs.</param>
    /// <param name="expected">Ожидаемый результат.</param>
    [TestCase(nameof(NullSequence), new object[] { "C5BBF5EB-38FD-4394-A0A5-24912BCC5A63", "E342C5E4-66A6-4105-A050-DA0DF10D8200" }, "Checked parameter is null. (Parameter 'queryable')")]
    [TestCase(nameof(Sequence), new object[] { "00000000-0000-0000-0000-000000000000", "11111111-1111-1111-1111-111111111111" }, "MT-E0011: The required entities not found in the sequence by keys. (IDs = '00000000-0000-0000-0000-000000000000, 11111111-1111-1111-1111-111111111111')")]
    public void SearchManyOrDefaultNegotiveTest(string seq, object[] objects, string expected)
    {
        // arrange
        var ids = objects.Select(obj => Guid.Parse((string)obj));

        // act
        var func = () => GetQueryable(seq)!.SearchManyOrDefault(ids);

        // assert
        func.Should().Throw<Exception>().WithMessage(expected);
    }

    /// <summary>
    /// Получить последовательность для проведения тестов.
    /// </summary>
    /// <param name="seqName">Наименование последовательности.</param>
    /// <returns>Последовательность сущностей.</returns>
    private static IQueryable<TestEntity>? GetQueryable(string seqName)
    {
        return seqName switch
        {
            nameof(Sequence) => Sequence,
            nameof(EmptySequence) => EmptySequence,
            nameof(DefaultManySequence) => DefaultManySequence,
            nameof(DefaultSequence) => DefaultSequence,
            _ => NullSequence,
        };
    }

    /// <summary>
    /// Получить фабрику для создания сущности.
    /// </summary>
    /// <param name="factoryName">Наименование фабрики.</param>
    /// <returns>Фабричный метод.</returns>
    private static Func<TestEntity>? GetFactory(string factoryName)
    {
        return factoryName switch
        {
            nameof(EntityFactory) => EntityFactory,
            _ => NullFactory,
        };
    }
}