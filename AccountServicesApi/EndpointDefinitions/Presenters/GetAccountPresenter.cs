using AccountServices.UseCases;
using static System.Net.Mime.MediaTypeNames;

namespace AccountServicesApi.EndpointDefinitions.Presenters
{
    public class GetAccountPresenter : GetAccount.IPresenter
    {
        public GetAccountPresenter(HttpResponse response)
        {
            _response = response;
        }

        private HttpResponse _response { get; }

        public async Task Present(GetAccount.PresentableAccount presentableAccount)
        {
            _response.StatusCode = StatusCodes.Status200OK;
            _response.ContentType = Text.Plain;
            await _response.WriteAsJsonAsync<GetAccount.PresentableAccount>(presentableAccount);
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
    }
}
