using System.Security.Claims;
using System.Web.Http;
using ECA.WebApi.Common;

namespace ECA.WebApi.Controllers
{
    public class TicketController : ApiController
    {
        [HttpGet]
        [Route("api/tickets")]

        public IHttpActionResult GetTicket()
        {
            var user = ClaimsPrincipal.Current.Identity;
            return Ok(new {
                Value = TicketMaster.Create(user.Name)
            });
        }

        [HttpDelete]
        [Route("api/tickets")]

        public IHttpActionResult DeleteTicket()
        {
            var user = ClaimsPrincipal.Current.Identity;
            TicketMaster.Delete(user.Name);
            return Ok();
        }
    }
}
