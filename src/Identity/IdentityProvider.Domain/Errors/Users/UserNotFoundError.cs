using IdentityProvider.Domain.Models;
using Utconnect.Common.Models.Errors;

namespace IdentityProvider.Domain.Errors.Users;

public class UserNotFoundError(string id) : NotFoundError<User>(id);