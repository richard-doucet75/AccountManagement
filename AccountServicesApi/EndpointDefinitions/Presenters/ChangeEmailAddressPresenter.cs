using static AccountServices.UseCases.ChangeEmailAddress;

namespace AccountServicesApi.EndpointDefinitions.Presenters
{
    public class ChangeEmailAddressPresenter
        : IPresenter
    {
        private HttpResponse _response;

        public ChangeEmailAddressPresenter(HttpResponse response)
        {
            _response = response;
        }

        public async Task PresentAccessDenied()
        {
            _response.StatusCode = StatusCodes.Status401Unauthorized;
            await Task.CompletedTask;
        }

        public async Task PresentEmailAddressChanged()
        {
            _response.StatusCode = StatusCodes.Status200OK;
            await Task.CompletedTask;
        }

        public async Task PresentNoChangeRequired()
        {
            _response.StatusCode = StatusCodes.Status200OK;
            await Task.CompletedTask;
        }

        public async Task PresentNotFound()
        {
            _response.StatusCode = StatusCodes.Status404NotFound;
            await Task.CompletedTask;
        }
    }
}
