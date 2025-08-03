# Account Service

- В ObjectStorage находятся классы для хранения данных счетов, транзакций, пользователей. На данный момент хранение данных происходит in-memory в List списках и enum перечислениях
- В Infrastructure хранятся мапперы и реализации репозиториев
- Во Features находятся контроллеры, сервисы, классы для работы с MediatR, классы валидации, модели данных
- Папка Exceptions предназначена для класса перехвата ошибок валидации
- Папка PipelineBehavior предназначена для класса внедрения валидации в пайплайн MediatR
- Папка Keycloak предназначена для файла с данными импорта в базу Keycloak сервиса

## API

http://localhost:5203/swagger/index.html

http://localhost/swagger/index.html (развёртка в докере)

## Keycloak

Для получения токенов из Keycloak сервиса, развёрнутого с помощью docker-compose, можно использовать client_id: account-service, grant_type: password, username: employee, password: employee