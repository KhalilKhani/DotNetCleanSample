using CleanSample.Domain.Enums;
using CleanSample.Utility;

namespace CleanSample.Domain.Exceptions;

public record ValidationError(ErrorCodes ErrorCode, string ErrorMessage)
    : Error((int)HttpStatusCode.BadRequest, new Error((int)ErrorCode, ErrorMessage));