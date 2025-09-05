using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace MiniBank.Services
{
    public class BankService
    {
        private readonly Dictionary<string, BankInfo> _banks;

        public BankService()
        {
            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "IndianBanks.json");
            if (File.Exists(jsonPath))
            {
                var json = File.ReadAllText(jsonPath);
                _banks = JsonSerializer.Deserialize<Dictionary<string, BankInfo>>(json) ?? new();
            }
            else
            {
                _banks = new();
            }
        }

        public List<string> GetBankNames() => new List<string>(_banks.Keys);

        public List<string> GetStates(string bankName)
        {
            if (_banks.ContainsKey(bankName))
                return new List<string>(_banks[bankName].States.Keys);
            return new List<string>();
        }

        public List<string> GetCities(string bankName, string state)
        {
            if (_banks.ContainsKey(bankName) && _banks[bankName].States.ContainsKey(state))
                return new List<string>(_banks[bankName].States[state].Keys);
            return new List<string>();
        }

        public List<string> GetBranches(string bankName, string state, string city)
        {
            if (_banks.ContainsKey(bankName) && _banks[bankName].States.ContainsKey(state) && _banks[bankName].States[state].ContainsKey(city))
                return new List<string>(_banks[bankName].States[state][city].Keys);
            return new List<string>();
        }

        public string GetIFSC(string bankName, string state, string city, string branch)
        {
            if (_banks.ContainsKey(bankName) && _banks[bankName].States.ContainsKey(state) && _banks[bankName].States[state].ContainsKey(city) && _banks[bankName].States[state][city].ContainsKey(branch))
                return _banks[bankName].States[state][city][branch].IFSC;
            return string.Empty;
        }

        public string GetBankZipCode(string bankName, string state, string city, string branch)
        {
            if (_banks.ContainsKey(bankName) && _banks[bankName].States.ContainsKey(state) && _banks[bankName].States[state].ContainsKey(city) && _banks[bankName].States[state][city].ContainsKey(branch))
                return _banks[bankName].States[state][city][branch].ZipCode;
            return string.Empty;
        }
    }

    public class BankInfo
    {
        public Dictionary<string, Dictionary<string, Dictionary<string, BranchInfo>>> States { get; set; } = new();
    }

    public class BranchInfo
    {
        public string IFSC { get; set; }
        public string ZipCode { get; set; }
    }
}
