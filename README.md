# Account Service

- В ObjectStorage находятся классы для хранения данных счетов, транзакций, пользователей. На данный момент хранение данных происходит in-memory в List списках и enum перечислениях
- В Infrastructure хранятся мапперы и реализации репозиториев
- Во Features находятся контроллеры, сервисы, классы для работы с MediatR, классы валидации, модели данных
- Папка Exceptions предназначена для класса перехвата ошибок валидации
- Папка PipelineBehavior предназначена для класса внедрения валидации в пайплайн MediatR
- Папка Keycloak предназначена для файла с данными импорта в базу Keycloak сервиса

## Назначение

Сервис Account Service предназначен для сотрудников розничного банка

## API

http://localhost:5203/index.html

http://localhost/index.html (развёртка в докере)

## Запуск

- Скачать или клонировать проект с Гитхаба

# Локально

- Запустить проект с помощью Visual Studio, либо открыть папку проекта в решении (Account Service/Account Service) в консоли и запустить проект командой 'dotnet run'
- Далее можно открыть в браузере страницу со сваггером http://localhost:5203/

# Docker

- Запустить Docker Desktop
- Открыть в консоли папку с решением (Account Service, с файлом docker-compose.yml) и ввести команду docker-compose up
- После развёртки сервисов с postgresql бд, программой и keycloak можно открыть в браузере страницу со сваггером http://localhost/

## Keycloak

Для получения токенов из Keycloak сервиса, развёрнутого с помощью docker-compose, можно использовать client_id: account-service, grant_type: password, username: employee, password: employee

http://localhost:7080/realms/account-service/protocol/openid-connect/token

## RabbitMQ

Логин: user
Пароль: password

http://localhost:15672

## Тесты

В консоли тесты можно запустить с помощью команды dotnet test "Account Service.sln" в папке с решением