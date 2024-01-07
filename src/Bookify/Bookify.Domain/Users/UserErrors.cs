
using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Users;

public static class UserErrors
{
    public static Error NotFound = new(
        "Error.Found",
        "The user was not found");
}
