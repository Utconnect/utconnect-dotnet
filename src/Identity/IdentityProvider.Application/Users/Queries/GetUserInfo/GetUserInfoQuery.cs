using IdentityProvider.Application.Dtos;
using MediatR;
using Utconnect.Common.Models;

namespace IdentityProvider.Application.Users.Queries.GetUserInfo;

public record GetUserInfoQuery(string? UserId) : IRequest<Result<UserInfoDto>>;