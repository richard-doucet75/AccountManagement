using AccountServices.UseCases;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace AccountServicesApi.EndpointDefinitions.Presenters;

public class CreateAccountPresenter : CreateAccount.IPresenter
{
    private readonly HttpResponse _response;

    public CreateAccountPresenter(HttpResponse response)
    {
        _response = response;
    }

    public async Task PresentAccountCreateError(string message)
    {
        _response.StatusCode = StatusCodes.Status500InternalServerError;
        _response.ContentType = Text.Plain;
        await _response.BodyWriter.WriteAsync(Encoding.UTF8.GetBytes(message));
    }

    public async Task PresentAccountCreated()
    {
        _response.StatusCode = StatusCodes.Status200OK;
        await Task.CompletedTask;
    }

    public async Task PresentAccountExists()
    {
        _response.StatusCode = StatusCodes.Status422UnprocessableEntity;
        _response.ContentType = Text.Plain;
        await _response.WriteAsync("Account already exists");
    }

    public async Task PresentPasswordMismatch()
    {
        _response.StatusCode = StatusCodes.Status422UnprocessableEntity;
        _response.ContentType = Text.Plain;
        await _response.WriteAsync("Passwords must match");
    }
}