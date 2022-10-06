using Mt.Entities.Abstractions.Extensions;
using NUnit.Framework;
using System.Linq;
using System;

namespace Mt.Entities.Abstractions.Test
{
    /// <summary>
    /// Набор тестов для расширений класса Queryable.
    /// </summary>
    public sealed class QueryableExtensionsTests
    {
        private IQueryable<TestEntity> entitySeq;

        private IQueryable<TestEntity> entitySeqNull;

        private IQueryable<TestEntity> entitySeqEmpty;

        private IQueryable<TestEntity> entitySeqDefault;

        private IQueryable<TestEntity> entitySeqDefaultMany;

        /// <summary>
        /// Настройки.
        /// </summary>
        [OneTimeSetUp]
        public void SetUp()
        {
            this.entitySeqNull = null;

            this.entitySeqEmpty = Array.Empty<TestEntity>().AsQueryable();

            this.entitySeq = new TestEntity[]
            {
                new TestEntity() { Id = Guid.Parse("C5BBF5EB-38FD-4394-A0A5-24912BCC5A63"), Title = "Entity 0" },
                new TestEntity() { Id = Guid.Parse("F354633E-2744-4F73-9B44-5D1D401283D7"), Title = "Entity 1" },
                new TestEntity() { Id = Guid.Parse("71D69566-C5C8-4B56-8268-BAC368F98CF4"), Title = "Entity 2" },
                new TestEntity() { Id = Guid.Parse("E342C5E4-66A6-4105-A050-DA0DF10D8200"), Title = "Entity 3" },
                new TestEntity() { Id = Guid.Parse("F29AB748-08DF-4CA0-92B3-666A7A7C88E2"), Title = "Entity 4" },
            }.AsQueryable();

            this.entitySeqDefault = new TestEntity[]
            {
                new TestEntity() { Id = Guid.Parse("6A6F10FD-36E6-4AB9-A2DD-7CBE064D9A49"), Title = "Entity 5" },
                new TestEntity() { Id = Guid.Parse("36D5AF43-9EA8-45EF-8A77-A484439CE20C"), Title = "Default 1", Default = true },
                new TestEntity() { Id = Guid.Parse("4C11E861-8E9B-48BD-A029-46D8320142D7"), Title = "Entity 7" },
                new TestEntity() { Id = Guid.Parse("58F891F7-52A5-4AEC-9A3A-AD36621A14AF"), Title = "Entity 9" },
            }.AsQueryable();

            this.entitySeqDefaultMany = new TestEntity[]
            {
                new TestEntity() { Id = Guid.Parse("6A6F10FD-36E6-4AB9-A2DD-7CBE064D9A49"), Title = "Entity 5" },
                new TestEntity() { Id = Guid.Parse("36D5AF43-9EA8-45EF-8A77-A484439CE20C"), Title = "Default 1", Default = true },
                new TestEntity() { Id = Guid.Parse("4C11E861-8E9B-48BD-A029-46D8320142D7"), Title = "Entity 7" },
                new TestEntity() { Id = Guid.Parse("F3DD228E-F266-4251-87D0-3F433A8384E9"), Title = "Default 2", Default = true },
                new TestEntity() { Id = Guid.Parse("58F891F7-52A5-4AEC-9A3A-AD36621A14AF"), Title = "Entity 9" },
            }.AsQueryable();
        }

        /// <summary>
        /// Положительные тесты для метода <see cref="QueryableExtensions.Search"/>.
        /// </summary>
        /// <param name="seq">Наименование последовательности.</param>
        /// <param name="guid">UUID.</param>
        /// <param name="expected">Ожидаемый результат.</param>
        [Test]
        [TestCase(nameof(entitySeq),            "C5BBF5EB-38FD-4394-A0A5-24912BCC5A63", "Entity 0")]
        [TestCase(nameof(entitySeqDefaultMany), "4C11E861-8E9B-48BD-A029-46D8320142D7", "Entity 7")]
        public void SearchByIdPositiveTest(string seq, Guid guid, string expected)
        {
            var result = this.GetEnumerable(seq).Search(guid);
            Assert.That(result.Title, Is.EqualTo(expected));
        }

        /// <summary>
        /// Отрицательные тесты для метода <see cref="QueryableExtensions.Search"/>.
        /// </summary>
        /// <param name="seq">Наименование последовательности.</param>
        /// <param name="guid">UUID.</param>
        /// <param name="expected">Ожидаемый результат.</param>
        [Test]
        [TestCase(nameof(entitySeqNull),        "00000000-0000-0000-0000-000000000000", "Checked parameter is null. (Parameter 'queryable')")]
        [TestCase(nameof(entitySeqEmpty),       "00000000-0000-0000-0000-000000000000", "MT-E0011: Entity not found in sequence. ('Mt.Entities.Abstractions.Test.TestEntity'; ID = '00000000-0000-0000-0000-000000000000')")]
        [TestCase(nameof(entitySeq),            "00000000-0000-0000-0000-000000000000", "MT-E0011: Entity not found in sequence. ('Mt.Entities.Abstractions.Test.TestEntity'; ID = '00000000-0000-0000-0000-000000000000')")]
        [TestCase(nameof(entitySeqDefaultMany), "00000000-0000-0000-0000-000000000000", "MT-E0011: Entity not found in sequence. ('Mt.Entities.Abstractions.Test.TestEntity'; ID = '00000000-0000-0000-0000-000000000000')")]
        public void SearchByIdNegotiveTest(string seq, Guid guid, string expected)
        {
            var ex = Assert.Catch(() => this.GetEnumerable(seq).Search(guid));
            Assert.That(ex.Message, Is.EqualTo(expected));
        }

        /// <summary>
        /// Положительные тесты для метода <see cref="QueryableExtensions.Search"/>.
        /// </summary>
        /// <param name="seq">Наименование последовательности.</param>
        /// <param name="guid">UUID.</param>
        /// <param name="title">Заголовок.</param>
        /// <param name="expected">Ожидаемый результат.</param>
        [Test]
        [TestCase(nameof(entitySeq),            "C5BBF5EB-38FD-4394-A0A5-24912BCC5A63", "Entity 0", "Entity 0")]
        [TestCase(nameof(entitySeqDefaultMany), "4C11E861-8E9B-48BD-A029-46D8320142D7", "Entity 7", "Entity 7")]
        public void SearchByEntityPositiveTest(string seq, Guid guid, string title, string expected)
        {
            var entity = new TestEntity()
            {
                Id = guid,
                Title = title,
            };
            var result = this.GetEnumerable(seq).Search(entity);
            Assert.That(result.Title, Is.EqualTo(expected));
        }

        /// <summary>
        /// Отрицательные тесты для метода <see cref="QueryableExtensions.Search"/>.
        /// </summary>
        /// <param name="seq">Наименование последовательности.</param>
        /// <param name="guid">UUID.</param>
        /// <param name="title">Заголовок.</param>
        /// <param name="expected">Ожидаемый результат.</param>
        [Test]
        [TestCase(nameof(entitySeqNull),        "00000000-0000-0000-0000-000000000000", "Entity", "Checked parameter is null. (Parameter 'queryable')")]
        [TestCase(nameof(entitySeqEmpty),       "00000000-0000-0000-0000-000000000000", "Entity", "MT-E0011: Entity not found in sequence. ('ID = 00000000-0000-0000-0000-000000000000; title = Entity')")]
        [TestCase(nameof(entitySeq),            "00000000-0000-0000-0000-000000000000", "Entity", "MT-E0011: Entity not found in sequence. ('ID = 00000000-0000-0000-0000-000000000000; title = Entity')")]
        [TestCase(nameof(entitySeqDefaultMany), "00000000-0000-0000-0000-000000000000", "Entity", "MT-E0011: Entity not found in sequence. ('ID = 00000000-0000-0000-0000-000000000000; title = Entity')")]
        public void SearchByEntityNegotiveTest(string seq, Guid guid, string title, string expected)
        {
            var entity = new TestEntity()
            {
                Id = guid,
                Title = title,
            };
            var ex = Assert.Catch(() => this.GetEnumerable(seq).Search(entity));
            Assert.That(ex.Message, Is.EqualTo(expected));
        }

        /// <summary>
        /// Положительные тесты для метода <see cref="QueryableExtensions.SearchOrDefault"/>.
        /// </summary>
        /// <param name="seq">Наименование последовательности.</param>
        /// <param name="guid">UUID.</param>
        /// <param name="expected">Ожидаемый результат.</param>
        [Test]
        [TestCase(nameof(entitySeqDefault),     "4C11E861-8E9B-48BD-A029-46D8320142D7", "Entity 7")]
        [TestCase(nameof(entitySeqDefaultMany), "4C11E861-8E9B-48BD-A029-46D8320142D7", "Entity 7")]
        [TestCase(nameof(entitySeqDefault),     "00000000-0000-0000-0000-000000000000", "Default 1")]
        public void SearchOrDefaultByIdPositiveTest(string seq, Guid guid, string expected)
        {
            var result = this.GetEnumerable(seq).SearchOrDefault(guid);
            Assert.That(result.Title, Is.EqualTo(expected));
        }

        /// <summary>
        /// Отрицательные тесты для метода <see cref="QueryableExtensions.SearchOrDefault"/>.
        /// </summary>
        /// <param name="seq">Наименование последовательности.</param>
        /// <param name="guid">UUID.</param>
        /// <param name="expected">Ожидаемый результат.</param>
        [Test]
        [TestCase(nameof(entitySeqNull),        "00000000-0000-0000-0000-000000000000", "Checked parameter is null. (Parameter 'queryable')")]
        [TestCase(nameof(entitySeqDefaultMany), "00000000-0000-0000-0000-000000000000", "Sequence contains more than one matching element")]
        [TestCase(nameof(entitySeqEmpty),       "00000000-0000-0000-0000-000000000000", "MT-E0011: Entity or default value not found in sequence. ('Mt.Entities.Abstractions.Test.TestEntity'; ID = '00000000-0000-0000-0000-000000000000')")]
        [TestCase(nameof(entitySeq),            "00000000-0000-0000-0000-000000000000", "MT-E0011: Entity or default value not found in sequence. ('Mt.Entities.Abstractions.Test.TestEntity'; ID = '00000000-0000-0000-0000-000000000000')")]
        public void SearchOrDefaultByIdNegotiveTest(string seq, Guid guid, string expected)
        {
            var ex = Assert.Catch(() => this.GetEnumerable(seq).SearchOrDefault(guid));
            Assert.That(ex.Message, Is.EqualTo(expected));
        }

        /// <summary>
        /// Положительные тесты для метода <see cref="QueryableExtensions.SearchOrDefault"/>.
        /// </summary>
        /// <param name="seq">Наименование последовательности.</param>
        /// <param name="guid">UUID.</param>
        /// <param name="title">Заголовок.</param>
        /// <param name="expected">Ожидаемый результат.</param>
        [Test]
        [TestCase(nameof(entitySeqDefault),     "4C11E861-8E9B-48BD-A029-46D8320142D7", "Entity 7", "Entity 7")]
        [TestCase(nameof(entitySeqDefaultMany), "4C11E861-8E9B-48BD-A029-46D8320142D7", "Entity 7", "Entity 7")]
        [TestCase(nameof(entitySeqDefault),     "00000000-0000-0000-0000-000000000000", "Entity", "Default 1")]
        public void SearchOrDefaultByEntityPositiveTest(string seq, Guid guid, string title, string expected)
        {
            var entity = new TestEntity()
            {
                Id = guid,
                Title = title,
            };
            var result = this.GetEnumerable(seq).SearchOrDefault(entity);
            Assert.That(result.Title, Is.EqualTo(expected));
        }

        /// <summary>
        /// Отрицательные тесты для метода <see cref="QueryableExtensions.SearchOrDefault"/>.
        /// </summary>
        /// <param name="seq">Наименование последовательности.</param>
        /// <param name="guid">UUID.</param>
        /// <param name="title">Заголовок.</param>
        /// <param name="expected">Ожидаемый результат.</param>
        [Test]
        [TestCase(nameof(entitySeqNull),        "00000000-0000-0000-0000-000000000000", "Entity", "Checked parameter is null. (Parameter 'queryable')")]
        [TestCase(nameof(entitySeqDefaultMany), "00000000-0000-0000-0000-000000000000", "Entity", "Sequence contains more than one matching element")]
        [TestCase(nameof(entitySeqEmpty),       "00000000-0000-0000-0000-000000000000", "Entity", "MT-E0011: Entity or default value not found in sequence. ('ID = 00000000-0000-0000-0000-000000000000; title = Entity')")]
        [TestCase(nameof(entitySeq),            "00000000-0000-0000-0000-000000000000", "Entity", "MT-E0011: Entity or default value not found in sequence. ('ID = 00000000-0000-0000-0000-000000000000; title = Entity')")]
        public void SearchOrDefaultByEntityNegotiveTest(string seq, Guid guid, string title, string expected)
        {
            var entity = new TestEntity()
            {
                Id = guid,
                Title = title,
            };
            var ex = Assert.Catch(() => this.GetEnumerable(seq).SearchOrDefault(entity));
            Assert.That(ex.Message, Is.EqualTo(expected));
        }

        /// <summary>
        /// Положительные тесты для метода <see cref="QueryableExtensions.SearchOrNull"/>.
        /// </summary>
        /// <param name="seq">Наименование последовательности.</param>
        /// <param name="guid">UUID.</param>
        /// <param name="expected">Ожидаемый результат.</param>
        [Test]
        [TestCase(nameof(entitySeq),        "C5BBF5EB-38FD-4394-A0A5-24912BCC5A63", "Entity 0")]
        [TestCase(nameof(entitySeq),        "00000000-0000-0000-0000-000000000000", null)]
        [TestCase(nameof(entitySeqEmpty),   "C5BBF5EB-38FD-4394-A0A5-24912BCC5A63", null)]
        public void SearchOrNullByIdPositiveTest(string seq, Guid guid, string expected)
        {
            var result = this.GetEnumerable(seq).SearchOrNull(guid);
            Assert.That(result?.Title, Is.EqualTo(expected));
        }

        /// <summary>
        /// Отрицательные тесты для метода <see cref="QueryableExtensions.SearchOrNull"/>.
        /// </summary>
        /// <param name="seq">Наименование последовательности.</param>
        /// <param name="guid">UUID.</param>
        /// <param name="expected">Ожидаемый результат.</param>
        [Test]
        [TestCase(nameof(entitySeqNull),    "C5BBF5EB-38FD-4394-A0A5-24912BCC5A63", "Checked parameter is null. (Parameter 'queryable')")]
        public void SearchOrNullByIdNegotiveTest(string seq, Guid guid, string expected)
        {
            var ex = Assert.Catch(() => this.GetEnumerable(seq).SearchOrNull(guid));
            Assert.That(ex.Message, Is.EqualTo(expected));
        }

        /// <summary>
        /// Положительные тесты для метода <see cref="QueryableExtensions.IsContained"/>.
        /// </summary>
        /// <param name="seq">Наименование последовательности.</param>
        /// <param name="guid">UUID.</param>
        /// <param name="title">Заголовок.</param>
        /// <param name="expected">Ожидаемый результат.</param>
        [Test]
        [TestCase(nameof(entitySeq),        "F354633E-2744-4F73-9B44-5D1D401283D7", "Entity 1", true)]
        [TestCase(nameof(entitySeq),        "00000000-0000-0000-0000-000000000000", "Entity", false)]
        [TestCase(nameof(entitySeqEmpty),   "C5BBF5EB-38FD-4394-A0A5-24912BCC5A63", "Entity", false)]
        public void IsContainedPositiveTest(string seq, Guid guid, string title, bool expected)
        {
            var entity = new TestEntity()
            {
                Id = guid,
                Title = title,
            };
            var result = this.GetEnumerable(seq).IsContained(entity);
            Assert.That(result, Is.EqualTo(expected));
        }

        /// <summary>
        /// Отрицательные тесты для метода <see cref="QueryableExtensions.IsContained"/>.
        /// </summary>
        /// <param name="seq">Наименование последовательности.</param>
        /// <param name="guid">UUID.</param>
        /// <param name="title">Заголовок.</param>
        /// <param name="expected">Ожидаемый результат.</param>
        [Test]
        [TestCase(nameof(entitySeqNull),    "C5BBF5EB-38FD-4394-A0A5-24912BCC5A63", "Entity", "Checked parameter is null. (Parameter 'queryable')")]
        public void IsContainedNegotiveTest(string seq, Guid guid, string title, string expected)
        {
            var entity = new TestEntity()
            {
                Id = guid,
                Title = title,
            };
            var ex = Assert.Catch(() => this.GetEnumerable(seq).IsContained(entity));
            Assert.That(ex.Message, Is.EqualTo(expected));
        }

        /// <summary>
        /// Положительные тесты для метода <see cref="QueryableExtensions.SearchManyOrDefault"/>.
        /// </summary>
        /// <param name="seq">Наименование последовательности.</param>
        /// <param name="objects">UUIDs.</param>
        [Test]
        [TestCase(nameof(entitySeq),            new object[] { "C5BBF5EB-38FD-4394-A0A5-24912BCC5A63", "E342C5E4-66A6-4105-A050-DA0DF10D8200" })]
        [TestCase(nameof(entitySeqDefault),     new object[] { "00000000-0000-0000-0000-000000000000", "11111111-1111-1111-1111-111111111111" })]
        [TestCase(nameof(entitySeqDefaultMany), new object[] { "00000000-0000-0000-0000-000000000000", "11111111-1111-1111-1111-111111111111" })]
        public void SearchManyOrDefaultPositiveTest(string seq, object[] objects)
        {
            var ids = objects.Select(obj => Guid.Parse((string)obj));
            var resutl = this.GetEnumerable(seq).SearchManyOrDefault(ids);
            Assert.That(resutl.Any());
        }

        /// <summary>
        /// Отрицательные тесты для метода <see cref="QueryableExtensions.SearchManyOrDefault"/>.
        /// </summary>
        /// <param name="seq">Наименование последовательности.</param>
        /// <param name="objects">UUIDs.</param>
        [Test]
        [TestCase(nameof(entitySeqNull),    new object[] { "C5BBF5EB-38FD-4394-A0A5-24912BCC5A63", "E342C5E4-66A6-4105-A050-DA0DF10D8200" }, "Checked parameter is null. (Parameter 'queryable')")]
        [TestCase(nameof(entitySeq),        new object[] { "00000000-0000-0000-0000-000000000000", "11111111-1111-1111-1111-111111111111" }, "MT-E0011: The required entities not found in the sequence by keys. (IDs = '00000000-0000-0000-0000-000000000000, 11111111-1111-1111-1111-111111111111')")]
        public void SearchManyOrDefaultNegotiveTest(string seq, object[] objects, string expected)
        {
            var ids = objects.Select(obj => Guid.Parse((string)obj));
            var ex = Assert.Catch(() => this.GetEnumerable(seq).SearchManyOrDefault(ids));
            Assert.That(ex.Message, Is.EqualTo(expected));
        }

        /// <summary>
        /// Получить последовательность для проведения тестов.
        /// </summary>
        /// <param name="seqName">Наименование последовательности.</param>
        /// <returns></returns>
        private IQueryable<TestEntity> GetEnumerable(string seqName)
        {
            switch (seqName)
            {
                case nameof(entitySeq):
                    return this.entitySeq;

                case nameof(entitySeqEmpty):
                    return this.entitySeqEmpty;

                case nameof(entitySeqDefaultMany):
                    return this.entitySeqDefaultMany;

                case nameof(entitySeqDefault):
                    return this.entitySeqDefault;

                default:
                    return this.entitySeqNull;
            }
        }
    }
}
