using Shared.Application.Exceptions.Core;

namespace Shared.UtconnectIdentity.Exceptions;

internal class InvalidClaimUnixDateTimeException()
    : InternalServerErrorException("Unix DateTime cannot be parsed, user cannot be identified");