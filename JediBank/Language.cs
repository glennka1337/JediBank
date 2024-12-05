using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace JediBank
{
    internal class Language
    {
        private Dictionary<string, Dictionary<string, List<string>>> _data; // To store the deserialized JSON data
        private Dictionary<string, List<string>> _languages; // To store the "languages" dictionary

        private Dictionary<string, List<string>> _translations; // To store the translations dictionary
        public string SelectedLanguage { get; set; }
        public Language()
        {
            //Read and load JSON content
            // Read and load JSON content
            string jsonContent = File.ReadAllText("..//..//..//Languages/languages.json");

            //Deserialize JSON into a dictionary
            // Deserialize JSON into a dictionary
            _data = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, List<string>>>>(jsonContent);

            //Access the "languages" dictionary
            if (_data.ContainsKey("languages"))
                // Access the "language" dictionary
                if (_data.ContainsKey("translations"))
                {
                    _languages = _data["languages"];
                    _translations = _data["translations"];
                }
                else
                {
                    Console.WriteLine("Something wrong???"); // WIP
                    Console.WriteLine("Something went wrong idk.");
                }
        }

        public void SetLanguage(string language)
        {
            SelectedLanguage = language;
        }
        public void ChooseLanguage()
        {
            //Ask the user to choose a language

            Console.WriteLine("Choose a language: English or Swedish?");
            string selectedLanguage = Console.ReadLine();

            //Validate and print translations
            if (_languages.ContainsKey(selectedLanguage))
                // Validate the input and store the selected language
                if (_translations.ContainsKey(selectedLanguage))
                {
                    SelectedLanguage = selectedLanguage;
                }
                else
                {
                    Console.WriteLine("Invalid language selection. Defaulting to English.");
                }
        }
        public string TranslationTool(string key)
        {

            int languageIndex = SelectedLanguage == "English" ? 0 : 1;

            // Validate and print translations
            if (_translations.ContainsKey(key))
            {
                var translations = _languages[SelectedLanguage];
                Console.WriteLine("Log in: " + translations[0]); // Index 0 for "Log in"
                Console.WriteLine("Withdraw: " + translations[1]); // Index 1 for "Withdraw"
                return _translations[key][languageIndex];

            }
            else
            {
                Console.WriteLine("Language not supported.");
                return "Error";
            }
        }

        public string GetUsernamePrompt()
        {
            return TranslationTool("Enter your Username:");
        }
    }
}