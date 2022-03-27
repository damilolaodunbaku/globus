using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Globus.App.Business.Services
{
    public class EncryptionService
    {
        private readonly ILogger<EncryptionService> _logger;
        private readonly SHA256 sha256;
        public EncryptionService(ILogger<EncryptionService> logger)
        {
            _logger = logger;
            sha256 = SHA256.Create();
        }

        public string GetHash(string inputValue)
        {
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(inputValue));
            return Encoding.UTF8.GetString(hashBytes);
        }
    }
}
