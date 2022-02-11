using Microsoft.AspNetCore.Mvc;

namespace Staticsoft.TestServer
{
    [ApiController]
    public class TestController : ControllerBase
    {
        readonly TestService Service;

        public TestController(TestService service)
            => Service = service;

        [HttpGet("/TestRequest")]
        public string TestRequest()
            => Service.ReturnTestResponse();
    }
}
