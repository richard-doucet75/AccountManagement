using AccountServices.UseCases;
using System.Text;

using static System.Net.Mime.MediaTypeNames;
namespace AccountServicesApi.EndpointDefinitions.Presenters;

public class CreateAccountPresenter : CreateAccount.IPresenter
{
    private readonly HttpResponse _response;
    private readonly LinkGenerator _linker;

    public CreateAccountPresenter(HttpResponse response, LinkGenerator linker)
    {
        _response = response;
        _linker = linker;
    }

    public async Task PresentAccountCreateError(string message)
    {
        _response.StatusCode = StatusCodes.Status500InternalServerError;
        _response.ContentType = Text.Plain;
        await _response.BodyWriter.WriteAsync(Encoding.UTF8.GetBytes(message));
    }

    public async Task PresentAccountCreated(Guid accountId)
    {
        _response.Headers.Location = _linker.GetPathByName("GetAccount", values: new { id = accountId });
        _response.StatusCode = StatusCodes.Status201Created;
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