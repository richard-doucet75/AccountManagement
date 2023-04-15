namespace AccountServicesApi.Utilities
{
    public class EnvironmentException : Exception
    {
        public EnvironmentException(string variableName)
            : base($"Environment varible {variableName} was not found.")
        {
            
        }
    }
}
