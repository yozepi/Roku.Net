using System.Net;

namespace yozepi.Roku
{
    public interface ICommandResponse
    {
        bool IsSuccess { get; }
        HttpStatusCode StatusCode { get; }
        string StatusDescription { get; }
    }
}