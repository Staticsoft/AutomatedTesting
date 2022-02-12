using Staticsoft.TestServer;

namespace Staticsoft.Testing.Integration.Tests
{
    public class TestServiceMock : TestService
    {
        string Body = string.Empty;

        public string ReturnTestResponse()
            => Body;

        public void SetTestResponse(string body)
            => Body = body;
    }
}
