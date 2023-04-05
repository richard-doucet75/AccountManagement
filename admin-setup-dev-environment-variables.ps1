$currentPrincipal = New-Object Security.Principal.WindowsPrincipal([Security.Principal.WindowsIdentity]::GetCurrent())
if ($currentPrincipal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator) -eq $false)
{
	Write-Host 'run this script as Administator' -ForegroundColor Red
	EXIT 1
}

Write-Host 'Setting enviornment variables'
[System.Environment]::SetEnvironmentVariable('ACCOUNT_SERVICES_CONNECTION_STRING','Server=localhost;Database=AccountServices;User Id=sa;Password=l0c@ldbp@ssw0rd;Trusted_Connection=True;MultipleActiveResultSets=true', 'Machine')
[System.Environment]::SetEnvironmentVariable('DOCKER_ACCOUNT_SERVICES_CONNECTION_STRING','Server=sql-server-db,1433;Database=AccountServices;User Id=sa;Password=l0c@ldbp@ssw0rd;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False;integrated security=false;', 'Machine')

Write-Host ''
Write-Host 'Restart Visual Studio after running this command'

