namespace AccountServicesTests.Gateways;

public abstract class GatewayMode
{
    public abstract Task Execute(Task task);

    public static readonly GatewayMode SuccessMode = new SuccessGatewayMode();
    public static readonly GatewayMode ExceptionMode = new ExceptionGatewayMode();


    private class SuccessGatewayMode : GatewayMode
    {
        public override async Task Execute(Task task)
        {
            await task;
        }
    }

    private class ExceptionGatewayMode : GatewayMode
    {
        public override async Task Execute(Task task)
        {
            await Task.FromException(new Exception());
        }
    }
}