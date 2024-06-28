using System.Net;

namespace TTAdmin.WebNew.Handlers;

public interface IRequestHandler
{
    bool CanHandle(HttpListenerRequest request);
    void Handle(HttpListenerContext context);
}