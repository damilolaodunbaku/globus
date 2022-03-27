using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globus.App.Business.Services
{
    public class MnoCodeService
    {
        private readonly ILogger<MnoCodeService> _logger;
        string _mnoCodesFileName;
        HashSet<string> _hashSet;
        public MnoCodeService(ILogger<MnoCodeService> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _mnoCodesFileName = configuration["mnoCodesFileName"];
            LoadMnoCodes();

        }

        private void LoadMnoCodes()
        {
            try
            {
                _hashSet = new HashSet<string>();
                string filePath = Path.Combine(Environment.CurrentDirectory, _mnoCodesFileName);
                if (File.Exists(filePath))
                {
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(fileStream))
                        {
                            while (!streamReader.EndOfStream)
                            {
                                string line = streamReader.ReadLine();
                                if (line != null)
                                {
                                    _hashSet.Add(line.Split(",")[0]);
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occured while loading Mobile network operator codes");
                throw;
            }
        }

        public bool IsValidMnoCode(string code)
        {
            return _hashSet.Contains(code);
        }
    }
}
