$currentPrincipal = New-Object Security.Principal.WindowsPrincipal([Security.Principal.WindowsIdentity]::GetCurrent())
if ($currentPrincipal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator) -eq $false)
{
	Write-Host 'run this script as Administator' -ForegroundColor Red
	EXIT 1
}

Write-Host 'Setting up environment valiables for development environment'
Write-Host 'Setting connection string environment variables'
[System.Environment]::SetEnvironmentVariable('ACCOUNT_SERVICES_CONNECTION_STRING','Server=localhost;Database=AccountServices;User Id=sa;Password=l0c@ldbp@ssw0rd;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False;integrated security=false', 'Machine')
[System.Environment]::SetEnvironmentVariable('DOCKER_ACCOUNT_SERVICES_CONNECTION_STRING','Server=sql-server-db,1433;Database=AccountServices;User Id=sa;Password=l0c@ldbp@ssw0rd;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False;integrated security=false;', 'Machine')


Write-Host 'Setting Jwt environment variables'
[System.Environment]::SetEnvironmentVariable('ACCOUNT_SERVICES_JWT_ISSUER','https://account-services/', 'Machine')
[System.Environment]::SetEnvironmentVariable('ACCOUNT_SERVICES_JWT_VALID_AUDIENCE','https://account-services/', 'Machine')
[System.Environment]::SetEnvironmentVariable('ACCOUNT_SERVICES_JWT_KEY','his is a sample secret key - please don''t use in production environment.', 'Machine')
Write-Host ''
Write-Host 'Please Restart Visual Studio' -ForegroundColor Red

