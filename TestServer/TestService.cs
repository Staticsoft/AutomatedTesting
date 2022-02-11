namespace Staticsoft.TestServer
{
    public interface TestService
    {
        void SetTestResponse(string message);
        string ReturnTestResponse();
    }
}