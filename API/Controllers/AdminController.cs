using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoService _photoService;
        public AdminController(UserManager<AppUser> userManager, IUnitOfWork unitOfWork, IPhotoService photoService)
        {
            _photoService = photoService;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles()
        {
            var users = await _userManager.Users
                        .Include(r => r.UserRoles)
                        .ThenInclude(r => r.Role)
                        .OrderBy(u => u.UserName)
                        .Select(u => new
                        {
                            u.Id,
                            Username = u.UserName,
                            Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
                        })
                        .ToListAsync();

            return Ok(users);
        }

        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username, [FromQuery] string roles)
        {
            var selectedRoles = roles.Split(',').ToArray();

            var user = await _userManager.FindByNameAsync(username);

            if (user == null) return NotFound("Could not find user");

            var userRoles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded) return BadRequest("Unable to add roles.");

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded) return BadRequest("Unable to remove roles.");

            return Ok(await _userManager.GetRolesAsync(user));
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photos-to-moderate")]
        public async Task<ActionResult> GetPhotosForModeration()
        {
            var photos = await _unitOfWork.photoRepository.GetUnapprovedPhotos();

            return Ok(photos);
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpPost("approve-photo/{photoid}")]
        public async Task<ActionResult> ApprovePhoto(int photoId)
        {
            var photo = await _unitOfWork.photoRepository.GetPhotoById(photoId);
            var user = await _unitOfWork.UserRepository.GetUserByPhotoId(photoId);

            photo.IsApproved = true;


            if (user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }

            await _unitOfWork.Complete();

            return Ok();
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpPost("reject-photo/{photoid}")]
        public async Task<ActionResult> RejectPhoto(int photoId)
        {
            var photo = await _unitOfWork.photoRepository.GetPhotoById(photoId);

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);

                if (result.Result == "ok")
                {
                    _unitOfWork.photoRepository.RemovePhoto(photo);
                }
            }
            else
            {
                _unitOfWork.photoRepository.RemovePhoto(photo);
            }

            await _unitOfWork.Complete();

            return Ok();
        }

    }
}