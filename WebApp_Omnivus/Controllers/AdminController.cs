using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp_Omnivus.Models;

namespace WebApp_Omnivus.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {

            return View();
        }


        public async Task<IActionResult> Users()
        {

            return View(await _context.Profiles.Include(x => x.User).ToListAsync());
        }

        // Admin edit med rolechange
        [Route("admin/RoleChange/{id}")]
        public async Task<IActionResult> RoleChange(string id)
        {
            var _UserRoleId = await _context.UserRoles.FirstOrDefaultAsync(x => x.UserId == id);
            var _UserRole = await _context.Roles.FirstOrDefaultAsync(x => x.Id == _UserRoleId.RoleId);

            var userProfileEntity = await _context.Profiles.FirstOrDefaultAsync(x => x.UserId == id);
            var userProfile = new UserProfile
            {
                FirstName = userProfileEntity.FirstName,
                LastName = userProfileEntity.LastName,
                StreetName = userProfileEntity.StreetName,
                PostalCode = userProfileEntity.PostalCode,
                City = userProfileEntity.City,
                Country = userProfileEntity.Country,
                Role = _UserRole.Name
                
            };

            return View(userProfile);
        }

        [Route("admin/RoleChange/{id}")]
        [HttpPost]
        public async Task<IActionResult> RoleChange(string id, UserProfile profile)
        {

            var user = await _context.Profiles.FirstOrDefaultAsync(u => u.UserId == id);
            var _user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            var _userRole = await _context.UserRoles.FirstOrDefaultAsync(u => u.UserId == id);

            try
            {       
                if (profile.Role == "admin" || profile.Role =="user" )
                {
                    user.FirstName = profile.FirstName;
                    user.LastName = profile.LastName;
                    user.StreetName = profile.StreetName;
                    user.PostalCode = profile.PostalCode;
                    user.City = profile.City;
                    user.Country = profile.Country;
                    
                    var _roleId = await _context.UserRoles.FirstOrDefaultAsync( x=> x.UserId == id);
                    var _role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == _roleId.RoleId);

                    if (_role.Name != profile.Role) //Kollar om role är samma som innan, om det är samma så görs ingen ny role
                    {
                        await _userManager.RemoveFromRoleAsync(_user, profile.Role);
                        _context.UserRoles.Remove(_userRole);
                        await _context.SaveChangesAsync();
                        await _userManager.AddToRoleAsync(_user, profile.Role);
                    }                   


                    _context.Profiles.Update(user);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Users", "Admin");
                }
                else
                {
                    profile.ErrorMessage = "Role must be admin or user";
                    return View(profile);
                }      
            }
            catch
            {                
                return View(profile);
            }
        }

    }
}
