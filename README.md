# <p><img src="iconMt.png" width="64px" height="64px" align="middle"/> Mt Entities abstractions</p>

Абстракции сущностей используемых в Мt-Relay.

## Перечень технологий

net8.0, [Mt.Utilities](https://github.com/g-aa/mt-utilities), SonarAnalyzer.CSharp, NUnit.

## [История изменения](CHANGELOG.md)

## Покрытие кода тестами

Перед первым запуском `.test.bat`, для просмотра покрытия кода тестами проверьте наличие `dotnet-reportgenerator-globaltool` выполнив команду:

`dotnet tool list --global`

в перечне пакетов должен присутствовать необходимый пакет:

```
Идентификатор пакета                   Версия      Команды
------------------------------------------------------------------
dotnet-reportgenerator-globaltool      5.1.19      reportgenerator
```

при его отсутствии необходимо выполнить команду:

`dotnet tool install -g dotnet-reportgenerator-globaltool`

## Основной состав функционала пакета

| Компонент                                                | Описание                               |
|----------------------------------------------------------|----------------------------------------|
| Mt.Entities.Abstractions.Interfaces.IDefaultable         | Сущность МТ по умолчанию.              |
| Mt.Entities.Abstractions.Interfaces.IEntity              | Сущность МТ.                           |
| Mt.Entities.Abstractions.Interfaces.IEqualityPredicate   | Сущность МТ с предикатом сравнения.    |
| Mt.Entities.Abstractions.Interfaces.IRemovable           | Сущность МТ, удаляемая.                |
| Mt.Entities.Abstractions.Extensions.EnumerableExtensions | Методы расширения для типа Enumerable. |
| Mt.Entities.Abstractions.Extensions.QueryableExtensions  | Методы расширения для типа Queryable.  |