# <p><img src="iconMt.png" width="64px" height="64px" align="middle"/> Mt Entities abstractions</p>

Абстракции сущностей используемых в Мt Rele.

## Перечень технологий (зависимости):

netstandard2.1, [Mt.Utilities](https://github.com/g-aa/mt-utilities), SonarAnalyzer.CSharp, NUnit.

## [История изменения.](CHANGELOG.md)

## Покрытие кода тестами:

Перед первым запуском ```.test.bat```, для просмотра покрытия кода тестами необходимо выполнить команду:

```dotnet tool list --global```

```
Идентификатор пакета                   Версия      Команды
------------------------------------------------------------------
dotnet-reportgenerator-globaltool      5.1.19      reportgenerator
```


```dotnet tool install -g dotnet-reportgenerator-globaltool```

## Основной состав функционала пакета:

| Компонент                                                | Описание                               |
|----------------------------------------------------------|----------------------------------------|
| Mt.Entities.Abstractions.Interfaces.IDefaultable         | Сущность МТ по умолчанию.              |
| Mt.Entities.Abstractions.Interfaces.IEntity              | Сущность МТ.                           |
| Mt.Entities.Abstractions.Interfaces.IEqualityPredicate   | Сущность МТ с предикатом сравнения.    |
| Mt.Entities.Abstractions.Interfaces.IRemovable           | Сущность МТ, удаляемая.                |
| Mt.Entities.Abstractions.Extensions.EnumerableExtensions | Методы расширения для типа Enumerable. |
| Mt.Entities.Abstractions.Extensions.QueryableExtensions  | Методы расширения для типа Queryable.  |