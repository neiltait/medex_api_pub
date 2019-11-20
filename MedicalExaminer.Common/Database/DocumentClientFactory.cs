using System;
using System.Linq;
using System.Net.Http;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Cosmonaut;
using MedicalExaminer.Common.ConnectionSettings;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace MedicalExaminer.Common.Database
{
    /// <summary>
    /// Document Client Factory.
    /// </summary>
    public class DocumentClientFactory : IDocumentClientFactory
    {
        private IDocumentClient _client;

        /// <summary>
        /// Create Client.
        /// </summary>
        /// <param name="connectionSettings">Connection Settings.</param>
        /// <returns><see cref="IDocumentClient"/>.</returns>
        public IDocumentClient CreateClient(IClientSettings connectionSettings, bool bypassSsl)
        {
            if (_client == null)
            {
                var connectionPolicy = new ConnectionPolicy
                {
                    RequestTimeout = TimeSpan.FromSeconds(5),
                    MediaRequestTimeout = TimeSpan.FromSeconds(5),
                    ConnectionMode = ConnectionMode.Direct,
                    ConnectionProtocol = Protocol.Tcp
                };

                // OpenSSL on Linux won't accept the emulator certificate so we have to ignore it.
                if (bypassSsl)
                {
                    // If bypassing ssl also switch to a simpler connection method.
                    connectionPolicy.ConnectionMode = ConnectionMode.Gateway;
                    connectionPolicy.ConnectionProtocol = Protocol.Https;

                    var handler = new HttpClientHandler
                    {
                        ClientCertificateOptions = ClientCertificateOption.Manual,
                        ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true
                    };

                    // All connections will now use our handler and ignore the SSL certificate.
                    _client = new DocumentClient(
                        connectionSettings.EndPointUri,
                        connectionSettings.PrimaryKey,
                        handler,
                        connectionPolicy);
                }
                else
                {
                    _client = new DocumentClient(
                        connectionSettings.EndPointUri,
                        connectionSettings.PrimaryKey,
                        connectionPolicy);
                }
            }

            return _client;
        }
    }
}
