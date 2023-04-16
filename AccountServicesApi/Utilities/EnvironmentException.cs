namespace AccountServicesApi.Utilities
{
    public class EnvironmentException : Exception
    {
        public EnvironmentException(string variableName)
            : base($"Environment variable {variableName} was not found.")
        {
            
        }
    }
}
