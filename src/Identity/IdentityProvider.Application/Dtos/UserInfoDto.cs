using Utconnect.Teacher.Models;

namespace IdentityProvider.Application.Dtos;

public class UserInfoDto
{
    public Guid Id { get; set; }
    public bool? IsMale { get; set; }
    public List<string> Roles { get; set; } = [];
    public List<string> Scopes { get; set; } = [];
    public TeacherResponse? Teacher { get; set; }
}