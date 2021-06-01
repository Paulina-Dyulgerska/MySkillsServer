namespace MySkillsServer.Web.Infrastructure
{
    using System.Security.Claims;

    //// JWT Authentication services 2
    public static class ClaimsPrincipalExtensions
    {
        public static string GetId(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal == null)
            {
                return null;
            }

            if (claimsPrincipal.Identity.IsAuthenticated)
            {
                return claimsPrincipal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            }

            return null;
        }
    }
}
