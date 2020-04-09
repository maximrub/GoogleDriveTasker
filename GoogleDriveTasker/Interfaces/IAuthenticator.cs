using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;

namespace GoogleDriveTasker.Interfaces
{
    public interface IAuthenticator
    {
        Task<UserCredential> AuthorizeAsync(params string[] scopes);
    }
}
