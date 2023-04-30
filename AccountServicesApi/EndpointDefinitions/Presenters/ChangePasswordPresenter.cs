using Azure;
using static AccountServices.UseCases.ChangePassword;

namespace AccountServicesApi.EndpointDefinitions.Presenters
{
    public class ChangePasswordPresenter : IPresenter
    {
        private readonly HttpResponse _response;

        public ChangePasswordPresenter(HttpResponse response)
        {
            _response = response;
        }

        public async Task PasswordChanged()
        {
            _response.StatusCode = StatusCodes.Status200OK;
            await Task.CompletedTask;
        }

        public async Task PresentAccessDenied()
        {
            _response.StatusCode = StatusCodes.Status401Unauthorized;
            await Task.CompletedTask;
        }

        public async Task PresentAccountNotFound()
        {
            _response.StatusCode = StatusCodes.Status404NotFound;
            await Task.CompletedTask;
        }

        public async Task PresentPasswordNotVerified()
        {         
            _response.StatusCode = StatusCodes.Status400BadRequest;
            await Task.CompletedTask;
        }

        public async Task PresentWrongPassword()
        {
            _response.StatusCode = StatusCodes.Status401Unauthorized;
            await Task.CompletedTask;
        }
    }
}
