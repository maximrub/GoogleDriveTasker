using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using GoogleDriveTasker.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GoogleDriveTasker.Services
{
    public class Authenticator : IAuthenticator
    {
        private readonly ILogger<Authenticator> _logger;
        private readonly IConfiguration _configuration;

        public Authenticator(ILogger<Authenticator> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<UserCredential> AuthorizeAsync(params string[] scopes)
        {
            await using FileStream stream =
                new FileStream(_configuration["CredentialsFile"], FileMode.Open, FileAccess.Read);
            UserCredential credentials = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.Load(stream).Secrets,
                scopes,
                "user",
                CancellationToken.None,
                new FileDataStore(_configuration["DataStorePath"], true));
            _logger.LogInformation($"Credential file saved to: '{_configuration["DataStorePath"]}'");
            return credentials;
        }
    }
}