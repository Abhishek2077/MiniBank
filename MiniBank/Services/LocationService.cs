using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace MiniBank.Services
{
    public class LocationService
    {
        private readonly Dictionary<string, List<string>> _citiesByState;

        public LocationService()
        {
            // Load states and cities from JSON file
            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "IndianStatesCities.json");
            if (File.Exists(jsonPath))
            {
                var json = File.ReadAllText(jsonPath);
                _citiesByState = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json) ?? new();
            }
            else
            {
                _citiesByState = new();
            }
        }

        public List<string> GetStates()
        {
            return new List<string>(_citiesByState.Keys);
        }

        public List<string> GetCities(string state)
        {
            if (_citiesByState.ContainsKey(state))
            {
                return _citiesByState[state];
            }
            return new List<string>();
        }
    }
}