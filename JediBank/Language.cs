using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace JediBank
{
    internal class Language
    {
        private Dictionary<string, Dictionary<string, List<string>>> _data; // To store the deserialized JSON data
        private Dictionary<string, List<string>> _translations; // To store the translations dictionary
        public string SelectedLanguage { get; set; }
        public Language()
        {
            // Read and load JSON content
            string jsonContent = File.ReadAllText("..//..//..//Languages/languages.json");

            // Deserialize JSON into a dictionary
            _data = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, List<string>>>>(jsonContent);

            // Access the "language" dictionary
            if (_data.ContainsKey("translations"))
            {
                _translations = _data["translations"];
            }
            else
            {
                Console.WriteLine("Something went wrong idk.");
            }
        }

        public void SetLanguage(string language)
        {
            SelectedLanguage = language;
        }
        public void ChooseLanguage()
        {

            Console.WriteLine("Choose a language: English or Swedish?");
            string selectedLanguage = Console.ReadLine();

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
              return _translations[key][languageIndex];
 
            }
            else
            {
                return "Error";
            }
        }

        public string GetUsernamePrompt()
        {
            return TranslationTool("Enter your Username:");
        }
    }
}