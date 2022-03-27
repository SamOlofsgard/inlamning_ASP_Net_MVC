using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp_Omnivus.Models;
using WebApp_Omnivus.Models.Entities;
using WebApp_Omnivus.Models.Forms;
using WebApp_Omnivus.Models.ViewModels;
using WebApp_Omnivus.Service;

namespace WebApp_Omnivus.Controllers
{
    //[Authorize]//måste var inloggad för att komma åt sin profilsida
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IProfileManager _profileManager;
        private readonly IWebHostEnvironment _host;               

        public ProfileController(ApplicationDbContext context, IProfileManager profileManager, IWebHostEnvironment host)
        {
            _context = context;
            _profileManager = profileManager;
            _host = host;
        }

        [Route("profile/{id}")]
        public async Task<IActionResult> Index(string id)
        {
            var viewModel = new ProfileViewModel();
            var _UserRoleId = await _context.UserRoles.FirstOrDefaultAsync(x => x.UserId == User.FindFirst("UserId").Value);
            var _UserRole = await _context.Roles.FirstOrDefaultAsync(x => x.Id == _UserRoleId.RoleId);


            var userProfileEntity = await _context.Profiles.FirstOrDefaultAsync(x => x.UserId == id);
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            viewModel.Profile = new UserProfile
            {
                UserId = userProfileEntity.UserId,
                FirstName = userProfileEntity.FirstName,
                LastName = userProfileEntity.LastName,
                Email = user.Email,
                StreetName = userProfileEntity.StreetName,
                PostalCode = userProfileEntity.PostalCode,
                City = userProfileEntity.City,
                Country = userProfileEntity.Country,
                Role = _UserRole.NormalizedName
            };
            var profileImageEntity = await _context.ProfileImages.FirstOrDefaultAsync(x => x.UserId == id);

            if(profileImageEntity == null)
            {
                viewModel.ProfileImage = new UserProfileImage
                {
                    FileName = "nopicture.jpg"//Om det inte finns någon profilbild registrerad i databasen så läggs denna upp istället.
                };
            }
            else
            {
                viewModel.ProfileImage = new UserProfileImage
                {
                    FileName = profileImageEntity.FileName
                };
            }
            
            return View(viewModel);
        }


        // GET: ProfileEntities/Edit/5
        [Route("profile/edit/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            var userProfileEntity = await _context.Profiles.FirstOrDefaultAsync(x => x.UserId == id);
            var userProfile = new UserProfile
            {
                FirstName = userProfileEntity.FirstName,
                LastName = userProfileEntity.LastName,
                StreetName = userProfileEntity.StreetName,
                PostalCode = userProfileEntity.PostalCode,
                City = userProfileEntity.City,
                Country = userProfileEntity.Country
            };

            return View(userProfile);
        }

        [Route("profile/edit/{id}")]
        [HttpPost]
        public async Task<IActionResult> Edit(string id, ProfileEntity profile)
        {

            var user = await _context.Profiles.FirstOrDefaultAsync(u => u.UserId == id);

            try
            {
                
                user.FirstName = profile.FirstName;
                user.LastName = profile.LastName;
                user.StreetName = profile.StreetName;
                user.PostalCode = profile.PostalCode;
                user.City = profile.City;
                user.Country = profile.Country;              
                

                _context.Profiles.Update(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Profile", new { id = id });
            }
            catch
            {
                return View(profile);
            }
        }

        [Route("profile/FileUpload/{id}")]
        public IActionResult FileUpload()
        {
            return View();
        }
        [Route("profile/FileUpload/{id}")]
        [HttpPost]
        public async Task<IActionResult> FileUpload(ProfileImageUploadForm form)
        {
            
            var userId = User.FindFirst("UserId").Value;

            var profileImageExists = await _context.ProfileImages.FirstOrDefaultAsync(x => x.UserId == userId);
            if (profileImageExists == null)
            {
                if (ModelState.IsValid)
                {
                    var profileImageEntity = new ProfileImageEntity
                    {
                        FileName = $"{userId}_{form.File.FileName}",
                        UserId = userId
                    };

                    string filePath = Path.Combine($"{_host.WebRootPath}/profileImages", profileImageEntity.FileName);

                    // Laddar upp filen till filePath sökvägen
                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        await form.File.CopyToAsync(fs);
                    }

                    // sparar till databasen                    
                    _context.ProfileImages.Add(profileImageEntity);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Profile");
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    //Tar bort den befintliga profilbilden ur databasen
                    _context.ProfileImages.Remove(profileImageExists);
                    await _context.SaveChangesAsync();

                    //lägger till en ny profilbild i databasen
                    var profileImageEntity = new ProfileImageEntity
                    {
                        FileName = $"{userId}_{form.File.FileName}",
                        UserId = userId
                    };

                    string filePath = Path.Combine($"{_host.WebRootPath}/profileImages", profileImageEntity.FileName);

                    // Laddar upp filen till filePath sökvägen
                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        await form.File.CopyToAsync(fs);
                    }

                    // sparar till databasen                    
                    _context.ProfileImages.Add(profileImageEntity);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Profile", new { id = userId });
                }
            }
            return View(form);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var profileEntity = await _context.Profiles.FindAsync(id);
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == profileEntity.UserId);
            _context.Profiles.Remove(profileEntity);
            _context.Users.Remove(user);           

            await _context.SaveChangesAsync();
            return RedirectToAction("Users", "Admin");
        }

    }
}
