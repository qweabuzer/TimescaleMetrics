# TimescaleMetrics


1. Запуск базы данных
```
cd ..\
docker-compose up -d
```
- проверка запустилась ли бд в контейнере
```
docker-compose ps
```

2. Применение миграций
```
cd TimescaleMetrics.DataAccess
dotnet ef database update --startup-project ../TimescaleMetrics.API
```
3. Запуск проекта
```
cd ..\TimescaleMetrics.API
dotnet run --launch-profile https
```
4. Браузер
--открыть в браузере ссылку https://localhost:7157/swagger
