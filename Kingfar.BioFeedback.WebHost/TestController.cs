using Kingfar.BioFeedback.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kingfar.BioFeedback.WebHost
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public TestController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("timenow")]
        public Task<string> GetDateTimeNow()
        {
            return Task.FromResult(DateTime.Now.ToString());
        }

        [HttpGet("test123")]
        public Task<PageModel<AppUserDto>> GetTest()
        {
            return Task.FromResult(_accountService.GetListByPage(0, 10, Shared.AppUserType.Subject, null, null));
        }
    }
}