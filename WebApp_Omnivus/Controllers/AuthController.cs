using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp_Omnivus.Models;
using WebApp_Omnivus.Models.Forms;
using WebApp_Omnivus.Service;

namespace WebApp_Omnivus.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IProfileManager _profileManager;

        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, IProfileManager profileManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _profileManager = profileManager;
        }


        #region SignUp

        [Route("signup")]
        [HttpGet]
        public IActionResult SignUp(string returnUrl = null)
        {
            if (_signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");

            var form = new SignUpForm();
            if (returnUrl != null)
                form.ReturnUrl = returnUrl;

            return View(form);
        }


        [Route("signup")]
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpForm form)
        {
            if (ModelState.IsValid)
            {
                if (!await _roleManager.Roles.AnyAsync())
                {
                    await _roleManager.CreateAsync(new IdentityRole("admin"));
                    await _roleManager.CreateAsync(new IdentityRole("user"));
                }

                if (!await _userManager.Users.AnyAsync()) //skapar en admin om det inte finns någon User(användare) i databasen
                    form.Role = "admin";


                var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == form.Email);
                if (user == null) //kollar om det redan finns en användare med samma email innan ny användare skapas
                {
                    user = new IdentityUser()
                    {
                        UserName = form.Email,
                        Email = form.Email
                    };

                    var userResult = await _userManager.CreateAsync(user, form.Password);
                    if (userResult.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, form.Role);

                        var profile = new UserProfile
                        {
                            FirstName = form.FirstName,
                            LastName = form.LastName,
                            Email = user.Email, //Sparas egentligen in på User delen med UserName som email också
                            StreetName = form.StreetName,
                            PostalCode = form.PostalCode,
                            City = form.City,
                            Country = form.Country
                        };

                        var profileResult = await _profileManager.CreateAsync(user, profile);
                        if (profileResult.Succeeded)
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);

                            if (form.ReturnUrl == null || form.ReturnUrl == "/")
                                return RedirectToAction("Index", "Home");
                            else
                                return LocalRedirect(form.ReturnUrl);
                        }
                        else
                        {
                            form.ErrorMessage = "Det var ett problem när din profil skapades. Försök att skapa en ny profil igen";
                        }
                    }
                }
                else
                {
                    form.ErrorMessage = "Det finns redan en användare med samma E-postadress";
                }

            }

            return View(form);
        }

        #endregion

        #region SignIn 

        [Route("signin")]
        [HttpGet]
        public IActionResult SignIn(string returnUrl = null)
        {
            if (_signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");

            var form = new SignInForm();
            if (returnUrl != null)
                form.ReturnUrl = returnUrl;

            return View(form);
        }


        [Route("signin")]
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInForm form)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(form.Email, form.Password, isPersistent: false, false);
                if (result.Succeeded)
                {
                    if (form.ReturnUrl == null || form.ReturnUrl == "/")
                        return RedirectToAction("Index", "Home");
                    else
                        return LocalRedirect(form.ReturnUrl);
                }
            }

            form.ErrorMessage = "Fel E-postadress eller lösenord";
            return View(form);
        }


        #endregion

        #region SignOut

        [Route("signout")]
        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            if (_signInManager.IsSignedIn(User))
                await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region AccessDenied

        [Route("access-denied")]
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        #endregion

    }
}
