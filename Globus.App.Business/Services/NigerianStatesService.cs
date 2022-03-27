using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globus.App.Business.Services
{
    public class NigerianStatesService
    {
        string _statesFileName;
        Dictionary<string, List<string>> _states;
        private readonly ILogger<NigerianStatesService> _logger;

        public NigerianStatesService(
            ILogger<NigerianStatesService> logger,
            IConfiguration configuration)
        {
            _statesFileName = configuration["statesFileName"];
            _logger = logger;
            LoadStates();
        }

        private void LoadStates()
        {
            try
            {
                string filePath = Path.Combine(Environment.CurrentDirectory, _statesFileName);

                if (File.Exists(filePath))
                {
                    _states = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(File.ReadAllText(filePath));
                }
                else
                {
                    _states = new Dictionary<string, List<string>>();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occured while reading state and LGA information");
                throw;
            }
        }

        public List<string> GetAllStates()
        {
            return _states.Keys.ToList();
        }

        public List<string> GetLgaForState(string state)
        {
            var lgas = _states.
                        Where(k => k.Key.Equals(state, StringComparison.OrdinalIgnoreCase))
                        .SelectMany(k => k.Value)
                        .ToList();

            return lgas;
        }
    }
}
