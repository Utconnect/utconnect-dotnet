using Shared.Application.Exceptions.Core;

namespace Shared.UtconnectIdentity.Exceptions;

internal class NoClaimException()
    : InternalServerErrorException("No user claims, user cannot be identified");