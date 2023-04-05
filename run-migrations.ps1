if (!(Test-Path 'env:ACCOUNT_SERVICES_CONNECTION_STRING'))
{ 
	Write-Host 'Envirnonment Varaible ACCOUNT_SERVICES_CONNECTION_STRING not set'
	Write-Host 'run admin-setup-dev-environment-variables.ps1 as admin and restart visual studio'
	exit 1
}

Write-Host 'running database migrations'
docker-compose -p account-services up sql-server --detach
dotnet ef database update --project .\AccountServices.Infrastructure\AccountServices.Infrastructure.csproj --startup-project .\AccountServicesApi\AccountServicesApi.csproj
