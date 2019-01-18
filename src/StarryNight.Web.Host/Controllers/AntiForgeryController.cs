using Microsoft.AspNetCore.Antiforgery;
using StarryNight.Controllers;

namespace StarryNight.Web.Host.Controllers
{
    public class AntiForgeryController : StarryNightControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
