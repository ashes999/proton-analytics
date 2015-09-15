using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using ProtonAnalytics.Web.Models;

namespace ProtonAnalytics.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        IAuthenticationManager Authentication
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        [GET("signin")]
        public ActionResult SignIn()
        {
            return View();
        }

        [POST("signin")]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(LoginModel input)
        {
            if (ModelState.IsValid)
            {
                if (input.HasValidUsernameAndPassword)
                {
                    var identity = new ClaimsIdentity(new[] {
                            new Claim(ClaimTypes.Name, input.Username),
                        },
                        DefaultAuthenticationTypes.ApplicationCookie,
                        ClaimTypes.Name, ClaimTypes.Role);

                    // if you want roles, just add as many as you want here (for loop maybe?)
                    // identity.AddClaim(new Claim(ClaimTypes.Role, "guest"));
                    // tell OWIN the identity provider, optional
                    // identity.AddClaim(new Claim(IdentityProvider, "Simplest Auth"));

                    Authentication.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = input.RememberMe
                    }, identity);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewData["Error"] = "Authentication failed.";
                }
            }

            return View("SignIn", input);
        }

        [GET("signout")]
        public ActionResult Logout()
        {
            Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("signin");
        }
    }
}