using IdentityProvider.Application.Dtos;
using IdentityProvider.Domain.Constants;
using IdentityProvider.Domain.Errors.Users;
using Microsoft.AspNetCore.Identity;
using Utconnect.Common.Models;
using Utconnect.Teacher.Models;
using Utconnect.Teacher.Services.Abstract;

namespace IdentityProvider.Application.Services;

internal class UserService(UserManager<Domain.Models.User> userManager, ITeacherService teacherService)
{
    public async Task<Result<UserInfoDto>> GetUserByIdAsync(string userId)
    {
        Domain.Models.User? user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return Result<UserInfoDto>.Failure(new UserNotFoundError(userId));
        }

        IList<string> userRoles = await userManager.GetRolesAsync(user);

        UserInfoDto result = new()
        {
            Id = user.Id,
            IsMale = user.IsMale,
            Roles = userRoles.ToList()
        };

        Guid userGuid = Guid.Parse(userId);

        if (userRoles.Contains(RoleConstant.Teacher))
        {
            Result<TeacherResponse> teacherData = await teacherService.GetByUserId(userGuid);
            result.Teacher = teacherData.Data;
        }

        return Result.Succeed(result);
    }
}