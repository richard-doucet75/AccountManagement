docker-compose -p account-services down
docker volume rm account-services_mssql
& .\docker-compose-up.ps1
& .\run-migrations.ps1