﻿using MediatR;
using Shared.Presentation.Models;

namespace IdentityProvider.Application.User.Commands.CreateUser;

public class CreateUserCommand : IRequest<Result>
{
    public string? UserName { get; set; }
    public string Name { get; set; } = default!;
}