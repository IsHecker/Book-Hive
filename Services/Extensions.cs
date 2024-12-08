using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace BookHive.Services;

public static class Extensions
{
    public static bool IsUserAuthenticated(this ISession session)
    {
        return session.GetInt32("Auth") != null;
    }

    public static int? GetUserId(this ISession session)
    {
        return session.GetInt32("UserId");
    }

    public static bool IsAdmin(this ISession session)
    {
        return !session.GetString("IsAdmin").IsNullOrEmpty();
    }
}