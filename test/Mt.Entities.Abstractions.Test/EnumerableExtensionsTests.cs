using Mt.Entities.Abstractions.Extensions;

namespace Mt.Entities.Abstractions.Test;

/// <summary>
/// Набор тестов для <see cref="EnumerableExtensions"/>.
/// </summary>
public sealed class EnumerableExtensionsTests
{
    private IEnumerable<TestEntity> entitySeq;

    private IEnumerable<TestEntity>? entitySeqNull;

    private IEnumerable<TestEntity> entitySeqEmpty;

    private IEnumerable<TestEntity> entitySeqDefault;

    private IEnumerable<TestEntity> entitySeqDefaultMany;

    private Func<TestEntity>? factoryNull;

    private Func<TestEntity> factoryEntity;

    /// <summary>
    /// Настройки.
    /// </summary>
    [OneTimeSetUp]
    public void SetUp()
    {
        this.entitySeqNull = null;

        this.entitySeqEmpty = Array.Empty<TestEntity>();

        this.entitySeq = new TestEntity[]
        {
            new TestEntity() { Id = Guid.Parse("C5BBF5EB-38FD-4394-A0A5-24912BCC5A63"), Title = "Entity 0" },
            new TestEntity() { Id = Guid.Parse("F354633E-2744-4F73-9B44-5D1D401283D7"), Title = "Entity 1" },
            new TestEntity() { Id = Guid.Parse("71D69566-C5C8-4B56-8268-BAC368F98CF4"), Title = "Entity 2" },
            new TestEntity() { Id = Guid.Parse("E342C5E4-66A6-4105-A050-DA0DF10D8200"), Title = "Entity 3" },
            new TestEntity() { Id = Guid.Parse("F29AB748-08DF-4CA0-92B3-666A7A7C88E2"), Title = "Entity 4" },
        };

        this.entitySeqDefault = new TestEntity[]
        {
            new TestEntity() { Id = Guid.Parse("6A6F10FD-36E6-4AB9-A2DD-7CBE064D9A49"), Title = "Entity 5" },
            new TestEntity() { Id = Guid.Parse("36D5AF43-9EA8-45EF-8A77-A484439CE20C"), Title = "Default 1", Default = true },
            new TestEntity() { Id = Guid.Parse("4C11E861-8E9B-48BD-A029-46D8320142D7"), Title = "Entity 7" },
            new TestEntity() { Id = Guid.Parse("58F891F7-52A5-4AEC-9A3A-AD36621A14AF"), Title = "Entity 9" },
        };

        this.entitySeqDefaultMany = new TestEntity[]
        {
            new TestEntity() { Id = Guid.Parse("6A6F10FD-36E6-4AB9-A2DD-7CBE064D9A49"), Title = "Entity 5" },
            new TestEntity() { Id = Guid.Parse("36D5AF43-9EA8-45EF-8A77-A484439CE20C"), Title = "Default 1", Default = true },
            new TestEntity() { Id = Guid.Parse("4C11E861-8E9B-48BD-A029-46D8320142D7"), Title = "Entity 7" },
            new TestEntity() { Id = Guid.Parse("F3DD228E-F266-4251-87D0-3F433A8384E9"), Title = "Default 2", Default = true },
            new TestEntity() { Id = Guid.Parse("58F891F7-52A5-4AEC-9A3A-AD36621A14AF"), Title = "Entity 9" },
        };

        this.factoryNull = null;

        this.factoryEntity = () =>
        {
            return new TestEntity()
            {
                Id = Guid.NewGuid(),
                Title = "Entity 6",
                Default = false,
            };
        };
    }

    /// <summary>
    /// Положительный тест для <see cref="EnumerableExtensions.Search{TEntity}(IEnumerable{TEntity}, Guid)"/>.
    /// </summary>
    /// <param name="seq">Наименование последовательности.</param>
    /// <param name="guid">UUID.</param>
    /// <param name="expected">Ожидаемый результат.</param>
    [TestCase(nameof(this.entitySeq), "C5BBF5EB-38FD-4394-A0A5-24912BCC5A63", "Entity 0")]
    [TestCase(nameof(this.entitySeqDefaultMany), "4C11E861-8E9B-48BD-A029-46D8320142D7", "Entity 7")]
    public void SearchPositiveTest(string seq, Guid guid, string expected)
    {
        // act
        var result = this.GetEnumerable(seq)!.Search(guid);

        // assert
        Assert.That(result.Title, Is.EqualTo(expected));
    }

    /// <summary>
    /// Положительный тест для <see cref="EnumerableExtensions.Search{TEntity}(IEnumerable{TEntity}, TEntity)"/>.
    /// </summary>
    /// <param name="seq">Наименование последовательности.</param>
    /// <param name="guid">UUID.</param>
    /// <param name="title">Заголовок.</param>
    /// <param name="expected">Ожидаемый результат.</param>
    [TestCase(nameof(this.entitySeq), "C5BBF5EB-38FD-4394-A0A5-24912BCC5A63", "Entity 0", "Entity 0")]
    [TestCase(nameof(this.entitySeqDefaultMany), "4C11E861-8E9B-48BD-A029-46D8320142D7", "Entity 7", "Entity 7")]
    public void SearchPositiveTest(string seq, Guid guid, string title, string expected)
    {
        // arrange
        var entity = new TestEntity()
        {
            Id = guid,
            Title = title,
        };

        // act
        var result = this.GetEnumerable(seq)!.Search(entity);

        // assert
        Assert.That(result.Title, Is.EqualTo(expected));
    }

    /// <summary>
    /// Отрицательный тест для <see cref="EnumerableExtensions.Search{TEntity}(IEnumerable{TEntity}, Guid)"/>.
    /// </summary>
    /// <param name="seq">Наименование последовательности.</param>
    /// <param name="guid">UUID.</param>
    /// <param name="expected">Ожидаемый результат.</param>
    [TestCase(nameof(this.entitySeqNull), "00000000-0000-0000-0000-000000000000", "Checked parameter is null. (Parameter 'enumerable')")]
    [TestCase(nameof(this.entitySeqEmpty), "00000000-0000-0000-0000-000000000000", "MT-E0011: Entity not found in sequence. ('Mt.Entities.Abstractions.Test.TestEntity'; ID = '00000000-0000-0000-0000-000000000000')")]
    [TestCase(nameof(this.entitySeq), "00000000-0000-0000-0000-000000000000", "MT-E0011: Entity not found in sequence. ('Mt.Entities.Abstractions.Test.TestEntity'; ID = '00000000-0000-0000-0000-000000000000')")]
    [TestCase(nameof(this.entitySeqDefaultMany), "00000000-0000-0000-0000-000000000000", "MT-E0011: Entity not found in sequence. ('Mt.Entities.Abstractions.Test.TestEntity'; ID = '00000000-0000-0000-0000-000000000000')")]
    public void SearchNegotiveTest(string seq, Guid guid, string expected)
    {
        // act
        var ex = Assert.Catch(() => this.GetEnumerable(seq)!.Search(guid));

        // assert
        Assert.That(ex.Message, Is.EqualTo(expected));
    }

    /// <summary>
    /// Отрицательный тест для <see cref="EnumerableExtensions.Search{TEntity}(IEnumerable{TEntity}, TEntity)"/>.
    /// </summary>
    /// <param name="seq">Наименование последовательности.</param>
    /// <param name="guid">UUID.</param>
    /// <param name="title">Заголовок.</param>
    /// <param name="expected">Ожидаемый результат.</param>
    [TestCase(nameof(this.entitySeqNull), "00000000-0000-0000-0000-000000000000", "Entity", "Checked parameter is null. (Parameter 'enumerable')")]
    [TestCase(nameof(this.entitySeqEmpty), "00000000-0000-0000-0000-000000000000", "Entity", "MT-E0011: Entity not found in sequence. ('ID = 00000000-0000-0000-0000-000000000000; title = Entity')")]
    [TestCase(nameof(this.entitySeq), "00000000-0000-0000-0000-000000000000", "Entity", "MT-E0011: Entity not found in sequence. ('ID = 00000000-0000-0000-0000-000000000000; title = Entity')")]
    [TestCase(nameof(this.entitySeqDefaultMany), "00000000-0000-0000-0000-000000000000", "Entity", "MT-E0011: Entity not found in sequence. ('ID = 00000000-0000-0000-0000-000000000000; title = Entity')")]
    public void SearchNegotiveTest(string seq, Guid guid, string title, string expected)
    {
        // arrange
        var entity = new TestEntity()
        {
            Id = guid,
            Title = title,
        };

        // act
        var ex = Assert.Catch(() => this.GetEnumerable(seq)!.Search(entity));

        // assert
        Assert.That(ex.Message, Is.EqualTo(expected));
    }

    /// <summary>
    /// Положительный тест для <see cref="EnumerableExtensions.SearchOrDefault{TEntity}(IEnumerable{TEntity}, Guid)"/>.
    /// </summary>
    /// <param name="seq">Наименование последовательности.</param>
    /// <param name="guid">UUID.</param>
    /// <param name="expected">Ожидаемый результат.</param>
    [TestCase(nameof(this.entitySeqDefault), "4C11E861-8E9B-48BD-A029-46D8320142D7", "Entity 7")]
    [TestCase(nameof(this.entitySeqDefaultMany), "4C11E861-8E9B-48BD-A029-46D8320142D7", "Entity 7")]
    [TestCase(nameof(this.entitySeqDefault), "00000000-0000-0000-0000-000000000000", "Default 1")]
    public void SearchOrDefaultPositiveTest(string seq, Guid guid, string expected)
    {
        // act
        var result = this.GetEnumerable(seq)!.SearchOrDefault(guid);

        // assert
        Assert.That(result.Title, Is.EqualTo(expected));
    }

    /// <summary>
    /// Положительный тест для <see cref="EnumerableExtensions.SearchOrDefault{TEntity}(IEnumerable{TEntity}, TEntity)"/>.
    /// </summary>
    /// <param name="seq">Наименование последовательности.</param>
    /// <param name="guid">UUID.</param>
    /// <param name="title">Заголовок.</param>
    /// <param name="expected">Ожидаемый результат.</param>
    [TestCase(nameof(this.entitySeqDefault), "4C11E861-8E9B-48BD-A029-46D8320142D7", "Entity 7", "Entity 7")]
    [TestCase(nameof(this.entitySeqDefaultMany), "4C11E861-8E9B-48BD-A029-46D8320142D7", "Entity 7", "Entity 7")]
    [TestCase(nameof(this.entitySeqDefault), "00000000-0000-0000-0000-000000000000", "Entity", "Default 1")]
    public void SearchOrDefaultPositiveTest(string seq, Guid guid, string title, string expected)
    {
        // arrange
        var entity = new TestEntity()
        {
            Id = guid,
            Title = title,
        };

        // act
        var result = this.GetEnumerable(seq)!.SearchOrDefault(entity);

        // assert
        Assert.That(result.Title, Is.EqualTo(expected));
    }

    /// <summary>
    /// Отрицательный тест для <see cref="EnumerableExtensions.SearchOrDefault{TEntity}(IEnumerable{TEntity}, Guid)"/>.
    /// </summary>
    /// <param name="seq">Наименование последовательности.</param>
    /// <param name="guid">UUID.</param>
    /// <param name="expected">Ожидаемый результат.</param>
    [TestCase(nameof(this.entitySeqNull), "00000000-0000-0000-0000-000000000000", "Checked parameter is null. (Parameter 'enumerable')")]
    [TestCase(nameof(this.entitySeqDefaultMany), "00000000-0000-0000-0000-000000000000", "Sequence contains more than one matching element")]
    [TestCase(nameof(this.entitySeqEmpty), "00000000-0000-0000-0000-000000000000", "MT-E0011: Entity or default value not found in sequence. ('Mt.Entities.Abstractions.Test.TestEntity'; ID = '00000000-0000-0000-0000-000000000000')")]
    [TestCase(nameof(this.entitySeq), "00000000-0000-0000-0000-000000000000", "MT-E0011: Entity or default value not found in sequence. ('Mt.Entities.Abstractions.Test.TestEntity'; ID = '00000000-0000-0000-0000-000000000000')")]
    public void SearchOrDefaultNegotiveTest(string seq, Guid guid, string expected)
    {
        // act
        var ex = Assert.Catch(() => this.GetEnumerable(seq)!.SearchOrDefault(guid));

        // assert
        Assert.That(ex.Message, Is.EqualTo(expected));
    }

    /// <summary>
    /// Отрицательный тест для <see cref="EnumerableExtensions.SearchOrDefault{TEntity}(IEnumerable{TEntity}, TEntity)"/>.
    /// </summary>
    /// <param name="seq">Наименование последовательности.</param>
    /// <param name="guid">UUID.</param>
    /// <param name="title">Заголовок.</param>
    /// <param name="expected">Ожидаемый результат.</param>
    [TestCase(nameof(this.entitySeqNull), "00000000-0000-0000-0000-000000000000", "Entity", "Checked parameter is null. (Parameter 'enumerable')")]
    [TestCase(nameof(this.entitySeqDefaultMany), "00000000-0000-0000-0000-000000000000", "Entity", "Sequence contains more than one matching element")]
    [TestCase(nameof(this.entitySeqEmpty), "00000000-0000-0000-0000-000000000000", "Entity", "MT-E0011: Entity or default value not found in sequence. ('ID = 00000000-0000-0000-0000-000000000000; title = Entity')")]
    [TestCase(nameof(this.entitySeq), "00000000-0000-0000-0000-000000000000", "Entity", "MT-E0011: Entity or default value not found in sequence. ('ID = 00000000-0000-0000-0000-000000000000; title = Entity')")]
    public void SearchOrDefaultNegotiveTest(string seq, Guid guid, string title, string expected)
    {
        // arrange
        var entity = new TestEntity()
        {
            Id = guid,
            Title = title,
        };

        // act
        var ex = Assert.Catch(() => this.GetEnumerable(seq)!.SearchOrDefault(entity));

        // assert
        Assert.That(ex.Message, Is.EqualTo(expected));
    }

    /// <summary>
    /// Положительный тест для <see cref="EnumerableExtensions.SearchOrCreate{TEntity}(IEnumerable{TEntity}, Guid, Func{TEntity})"/>.
    /// </summary>
    /// <param name="seq">Наименование последовательности.</param>
    /// <param name="factory">Наименование фабрики.</param>
    /// <param name="guid">UUID.</param>
    /// <param name="expected">Ожидаемый результат.</param>
    [TestCase(nameof(this.entitySeqDefault), nameof(this.factoryNull), "4C11E861-8E9B-48BD-A029-46D8320142D7", "Entity 7")]
    [TestCase(nameof(this.entitySeqDefault), nameof(this.factoryNull), "00000000-0000-0000-0000-000000000000", null)]
    [TestCase(nameof(this.entitySeqDefault), nameof(this.factoryEntity), "00000000-0000-0000-0000-000000000000", "Entity 6")]
    public void SearchOrCreatePositiveTest(string seq, string factory, Guid guid, string expected)
    {
        // act
        var result = this.GetEnumerable(seq)!.SearchOrCreate(guid, this.GetFactory(factory));

        // assert
        Assert.That(result?.Title, Is.EqualTo(expected));
    }

    /// <summary>
    /// Положительный тест для <see cref="EnumerableExtensions.SearchOrCreate{TEntity}(IEnumerable{TEntity}, TEntity, Func{TEntity})"/>.
    /// </summary>
    /// <param name="seq">Наименование последовательности.</param>
    /// <param name="factory"></param>
    /// <param name="title">Заголовок.</param>
    /// <param name="guid">UUID.</param>
    /// <param name="expected">Ожидаемый результат.</param>
    [TestCase(nameof(this.entitySeqDefault), nameof(this.factoryNull), "Entity 7", "4C11E861-8E9B-48BD-A029-46D8320142D7", "Entity 7")]
    [TestCase(nameof(this.entitySeqDefault), nameof(this.factoryNull), "Entity 1", "00000000-0000-0000-0000-000000000000", null)]
    [TestCase(nameof(this.entitySeqDefault), nameof(this.factoryEntity), "Entity 1", "00000000-0000-0000-0000-000000000000", "Entity 6")]
    public void SearchOrCreatePositiveTest(string seq, string factory, string title, Guid guid, string expected)
    {
        // arrange
        var entity = new TestEntity()
        {
            Id = guid,
            Title = title,
        };

        // act
        var result = this.GetEnumerable(seq)!.SearchOrCreate(entity, this.GetFactory(factory));

        // assert
        Assert.That(result?.Title, Is.EqualTo(expected));
    }

    /// <summary>
    /// Отрицательный тест для <see cref="EnumerableExtensions.SearchOrCreate{TEntity}(IEnumerable{TEntity}, Guid, Func{TEntity})"/>.
    /// </summary>
    /// <param name="seq">Наименование последовательности.</param>
    /// <param name="guid">UUID.</param>
    /// <param name="expected">Ожидаемый результат.</param>
    [TestCase(nameof(this.entitySeqNull), "00000000-0000-0000-0000-000000000000", "Checked parameter is null. (Parameter 'enumerable')")]
    public void SearchOrCreateNegotiveTest(string seq, Guid guid, string expected)
    {
        // act
        var ex = Assert.Catch(() => this.GetEnumerable(seq)!.SearchOrCreate(guid));

        // assert
        Assert.That(ex.Message, Is.EqualTo(expected));
    }

    /// <summary>
    /// Отрицательный тест для <see cref="EnumerableExtensions.SearchOrCreate{TEntity}(IEnumerable{TEntity}, TEntity, Func{TEntity})"/>.
    /// </summary>
    /// <param name="seq">Наименование последовательности.</param>
    /// <param name="title">Заголовок.</param>
    /// <param name="guid">UUID.</param>
    /// <param name="expected">Ожидаемый результат.</param>
    [TestCase(nameof(this.entitySeqNull), "Entity 7", "00000000-0000-0000-0000-000000000000", "Checked parameter is null. (Parameter 'enumerable')")]
    public void SearchOrCreateNegotiveTest(string seq, string title, Guid guid, string expected)
    {
        // arrange
        var entity = new TestEntity()
        {
            Id = guid,
            Title = title,
        };

        // act
        var ex = Assert.Catch(() => this.GetEnumerable(seq)!.SearchOrCreate(entity));

        // assert
        Assert.That(ex.Message, Is.EqualTo(expected));
    }

    /// <summary>
    /// Положительный тест для <see cref="EnumerableExtensions.SearchOrNull{TEntity}(IEnumerable{TEntity}, Guid)"/>.
    /// </summary>
    /// <param name="seq">Наименование последовательности.</param>
    /// <param name="guid">UUID.</param>
    /// <param name="expected">Ожидаемый результат.</param>
    [TestCase(nameof(this.entitySeq), "C5BBF5EB-38FD-4394-A0A5-24912BCC5A63", "Entity 0")]
    [TestCase(nameof(this.entitySeq), "00000000-0000-0000-0000-000000000000", null)]
    [TestCase(nameof(this.entitySeqEmpty), "C5BBF5EB-38FD-4394-A0A5-24912BCC5A63", null)]
    public void SearchOrNullPositiveTest(string seq, Guid guid, string expected)
    {
        // act
        var result = this.GetEnumerable(seq)!.SearchOrNull(guid);

        // assert
        Assert.That(result?.Title, Is.EqualTo(expected));
    }

    /// <summary>
    /// Отрицательный тест для <see cref="EnumerableExtensions.SearchOrNull{TEntity}(IEnumerable{TEntity}, Guid)"/>.
    /// </summary>
    /// <param name="seq">Наименование последовательности.</param>
    /// <param name="guid">UUID.</param>
    /// <param name="expected">Ожидаемый результат.</param>
    [TestCase(nameof(this.entitySeqNull), "C5BBF5EB-38FD-4394-A0A5-24912BCC5A63", "Checked parameter is null. (Parameter 'enumerable')")]
    public void SearchOrNullNegotiveTest(string seq, Guid guid, string expected)
    {
        // act
        var ex = Assert.Catch(() => this.GetEnumerable(seq)!.SearchOrNull(guid));

        // assert
        Assert.That(ex.Message, Is.EqualTo(expected));
    }

    /// <summary>
    /// Положительный тест для <see cref="EnumerableExtensions.IsContained{TEntity}(IEnumerable{TEntity}, TEntity)"/>.
    /// </summary>
    /// <param name="seq">Наименование последовательности.</param>
    /// <param name="guid">UUID.</param>
    /// <param name="title">Заголовок.</param>
    /// <param name="expected">Ожидаемый результат.</param>
    [TestCase(nameof(this.entitySeq), "F354633E-2744-4F73-9B44-5D1D401283D7", "Entity 1", true)]
    [TestCase(nameof(this.entitySeq), "00000000-0000-0000-0000-000000000000", "Entity", false)]
    [TestCase(nameof(this.entitySeqEmpty), "C5BBF5EB-38FD-4394-A0A5-24912BCC5A63", "Entity", false)]
    public void IsContainedPositiveTest(string seq, Guid guid, string title, bool expected)
    {
        // arrange
        var entity = new TestEntity()
        {
            Id = guid,
            Title = title,
        };

        // act
        var result = this.GetEnumerable(seq)!.IsContained(entity);

        // assert
        Assert.That(result, Is.EqualTo(expected));
    }

    /// <summary>
    /// Отрицательный тест для <see cref="EnumerableExtensions.IsContained{TEntity}(IEnumerable{TEntity}, TEntity)"/>.
    /// </summary>
    /// <param name="seq">Наименование последовательности.</param>
    /// <param name="guid">UUID.</param>
    /// <param name="title">Заголовок.</param>
    /// <param name="expected">Ожидаемый результат.</param>
    [TestCase(nameof(this.entitySeqNull), "C5BBF5EB-38FD-4394-A0A5-24912BCC5A63", "Entity", "Checked parameter is null. (Parameter 'enumerable')")]
    public void IsContainedNegotiveTest(string seq, Guid guid, string title, string expected)
    {
        // arrange
        var entity = new TestEntity()
        {
            Id = guid,
            Title = title,
        };

        // act
        var ex = Assert.Catch(() => this.GetEnumerable(seq)!.IsContained(entity));

        // assert
        Assert.That(ex.Message, Is.EqualTo(expected));
    }

    /// <summary>
    /// Положительный тест для <see cref="EnumerableExtensions.SearchManyOrDefault{TEntity}(IEnumerable{TEntity}, IEnumerable{Guid})"/>.
    /// </summary>
    /// <param name="seq">Наименование последовательности.</param>
    /// <param name="objects">UUIDs.</param>
    [TestCase(nameof(this.entitySeq), new object[] { "C5BBF5EB-38FD-4394-A0A5-24912BCC5A63", "E342C5E4-66A6-4105-A050-DA0DF10D8200" })]
    [TestCase(nameof(this.entitySeqDefault), new object[] { "00000000-0000-0000-0000-000000000000", "11111111-1111-1111-1111-111111111111" })]
    [TestCase(nameof(this.entitySeqDefaultMany), new object[] { "00000000-0000-0000-0000-000000000000", "11111111-1111-1111-1111-111111111111" })]
    public void SearchManyOrDefaultPositiveTest(string seq, object[] objects)
    {
        // arrange
        var ids = objects.Select(obj => Guid.Parse((string)obj));

        // act
        var resutl = this.GetEnumerable(seq)!.SearchManyOrDefault(ids);

        // assert
        Assert.That(resutl.Any());
    }

    /// <summary>
    /// Отрицательный тест для <see cref="EnumerableExtensions.SearchManyOrDefault{TEntity}(IEnumerable{TEntity}, IEnumerable{Guid})"/>.
    /// </summary>
    /// <param name="seq">Наименование последовательности.</param>
    /// <param name="objects">UUIDs.</param>
    /// <param name="expected">Ожидаемый результат.</param>
    [TestCase(nameof(this.entitySeqNull), new object[] { "C5BBF5EB-38FD-4394-A0A5-24912BCC5A63", "E342C5E4-66A6-4105-A050-DA0DF10D8200" }, "Checked parameter is null. (Parameter 'enumerable')")]
    [TestCase(nameof(this.entitySeq), new object[] { "00000000-0000-0000-0000-000000000000", "11111111-1111-1111-1111-111111111111" }, "MT-E0011: The required entities not found in the sequence by keys. (IDs = '00000000-0000-0000-0000-000000000000, 11111111-1111-1111-1111-111111111111')")]
    public void SearchManyOrDefaultNegotiveTest(string seq, object[] objects, string expected)
    {
        // arrange
        var ids = objects.Select(obj => Guid.Parse((string)obj));

        // act
        var ex = Assert.Catch(() => this.GetEnumerable(seq)!.SearchManyOrDefault(ids));

        // assert
        Assert.That(ex.Message, Is.EqualTo(expected));
    }

    /// <summary>
    /// Получить последовательность для проведения тестов.
    /// </summary>
    /// <param name="seqName">Наименование последовательности.</param>
    /// <returns>Последовательность сущностей.</returns>
    private IEnumerable<TestEntity>? GetEnumerable(string seqName)
    {
        return seqName switch
        {
            nameof(this.entitySeq) => this.entitySeq,
            nameof(this.entitySeqEmpty) => this.entitySeqEmpty,
            nameof(this.entitySeqDefaultMany) => this.entitySeqDefaultMany,
            nameof(this.entitySeqDefault) => this.entitySeqDefault,
            _ => this.entitySeqNull,
        };
    }

    /// <summary>
    /// Получить фабрику для создания сущности.
    /// </summary>
    /// <param name="factoryName">Наименование фабрики.</param>
    /// <returns>Фабричный метод.</returns>
    private Func<TestEntity>? GetFactory(string factoryName)
    {
        return factoryName switch
        {
            nameof(this.factoryEntity) => this.factoryEntity,
            _ => this.factoryNull,
        };
    }
}