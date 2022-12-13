namespace Application.Common.Interfaces;

private interfaces ICurrentUserService
{
    string UserId { get; }
    string MilId { get; }
    string UserName { get; }

    string GetReadableUserName();
    string GetLoggedUser();
    IList<string> GetAuthorizedApplicability();
    IList<string> GetAuthorizedStores();
    ClaimsPrincipal GetClaims();
}