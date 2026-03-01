# Social Network Full

Небольшой учебный проект социальной сети на **ASP.NET Core MVC** с разделением на слои:
- `Social_Network` — веб-приложение (Presentation Layer),
- `Social_network.BLL` — бизнес-логика и валидации,
- `Social_network.DLL` — доступ к данным (EF Core + SQL Server),
- `Social_network.DAL.Tests` — тестовый проект для DAL.

## Возможности

- Регистрация пользователя.
- Авторизация через cookie-аутентификацию.
- Просмотр и редактирование профиля.
- Добавление и удаление друзей.
- Отправка сообщений друзьям.
- Просмотр входящих сообщений.

## Технологии

- **.NET 10**
- **ASP.NET Core MVC**
- **Entity Framework Core 10**
- **SQL Server**
- **xUnit + Moq** (в тестах)

## Структура решения

```text
Social_Network_Full/
├── Social_Network.slnx
├── Social_Network/               # MVC-приложение
├── Social_network.BLL/           # Бизнес-логика
├── Social_network.DLL/           # DAL, контекст и репозитории
└── Social_network.DAL.Tests/     # Тесты
```

## Быстрый старт

### 1) Требования

- Установленный **.NET SDK 10**.
- Доступный **SQL Server** (локально или удалённо).

### 2) Настройка подключения к базе данных

В `Social_Network/appsettings.Development.json` укажите актуальную строку подключения:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=Social_Network;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

> Примечание: в проекте используется `Database.EnsureCreated()`, поэтому при первом запуске таблицы создаются автоматически.

### 3) Восстановление зависимостей

```bash
dotnet restore Social_Network.slnx
```

### 4) Запуск приложения

```bash
dotnet run --project Social_Network/Social_Network.PLL.csproj
```

После запуска откройте URL из вывода консоли (обычно `https://localhost:<port>`).

## Запуск тестов

```bash
dotnet test Social_Network.slnx
```
