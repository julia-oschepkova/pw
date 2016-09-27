using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PwTransferApp.Models.Forms;
using PwTransferApp.Models.Identity;

namespace PwTransferApp.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private readonly IRegistrationManager registrationManager;
        private ApplicationUserManager userManager;

        public AccountController(IRegistrationManager registrationManager)
        {
            this.registrationManager = registrationManager;
        }

        private ApplicationUserManager UserManager
        {
            get { return userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            set { userManager = value; }
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IdentityResult> Register(RegisterationForm model)
        {
            Validate(model);

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
            return await registrationManager.RegisterAsync(user, model.Password, UserManager);
        }

        private static void Validate(RegisterationForm model)
        {
            var emailRegex = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
            if (model.Password != model.ConfirmPassword || string.IsNullOrEmpty(model.FirstName) ||
                string.IsNullOrEmpty(model.Email) || !emailRegex.IsMatch(model.Email))
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && userManager != null)
            {
                userManager.Dispose();
                userManager = null;
            }

            base.Dispose(disposing);
        }
    }
}