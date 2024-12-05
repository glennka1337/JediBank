using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JediBank
{
    internal class Language
    {
        private Dictionary<string, Dictionary<string, List<string>>> _data; // To store the deserialized JSON data
        private Dictionary<string, List<string>> _languages; // To store the "languages" dictionary

        public Language()
        {
            //Read and load JSON content
            string jsonContent = File.ReadAllText("..//..//..//Languages/languages.json");

            //Deserialize JSON into a dictionary
            _data = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, List<string>>>>(jsonContent);

            //Access the "languages" dictionary
            if (_data.ContainsKey("languages"))
            {
                _languages = _data["languages"];
            }
            else
            {
                Console.WriteLine("Something wrong???"); // WIP
            }
        }

        public void ChooseLanguage()
        {
            //Ask the user to choose a language
            Console.WriteLine("Choose a language: English or Swedish?");
            string selectedLanguage = Console.ReadLine();

            //Validate and print translations
            if (_languages.ContainsKey(selectedLanguage))
            {
                var translations = _languages[selectedLanguage];
                Console.WriteLine("Log in: " + translations[0]); // Index 0 for "Log in"
                Console.WriteLine("Withdraw: " + translations[1]); // Index 1 for "Withdraw"
            }
            else
            {
                Console.WriteLine("Language not supported.");
            }
        }
    }
}