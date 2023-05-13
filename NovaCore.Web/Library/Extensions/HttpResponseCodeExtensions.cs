using NovaCore.Web.Server.Responses;

namespace NovaCore.Web.Extensions;

public static class HttpResponseCodeExtensions
{
    public static bool HasSucceeded(this HttpResponseCode responseCode)
    {
        // Get first digit of HTTP code
        switch ((int) responseCode / 100)
        {
            case 1:
            case 2:
            case 3:
                return true;
            case 4:
            case 5:
            default:
                return false;
        }
    }

    public static string Formatted(this HttpResponseCode responseCode)
    {
        return $"{(int) responseCode}: {responseCode}";
    }
}