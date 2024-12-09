using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace JediBank
{
    public class Language
    {
        private Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> _data; // Full JSON data
        private Dictionary<string, List<string>> _translations; // Store only the translations
        public static string SelectedLanguage { get; set; } = "English"; // Default language

        public Language(string? choosenLanguage = null)
        {

            string path = Path.Combine("Languages", "languages.json");
            // Read and load JSON content
            //string jsonContent = File.ReadAllText("..//..//..//Languages/languages.json");
            string jsonContent = File.ReadAllText(path);
            // Deserialize JSON into a nested dictionary
            _data = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>>>(jsonContent);


            // Access the translations dictionary
            if (_data["languages"].ContainsKey("translations"))
            {
                _translations = _data["languages"]["translations"];
            }
            else
            {
                Console.WriteLine("Error: Could not find 'translations' in JSON data.");
                _translations = new Dictionary<string, List<string>>();
            }

            if (choosenLanguage is not null)
            {
                SelectedLanguage = choosenLanguage;
            }

        }

        public string ChooseLanguage()
        {
            // Prompt the user to select a language
            Console.WriteLine("Choose a language: English or Swedish?");
            string selectedLanguage = Console.ReadLine();
            SelectedLanguage = selectedLanguage;
            
            if ( selectedLanguage == "English")
            {
                Console.WriteLine("Choosen Languge: English");
                selectedLanguage = "English";
            }
            else if ( selectedLanguage == "Swedish")
            {
                Console.WriteLine("Vald språk: Svenska");
                selectedLanguage = "Swedish";
            }
            else
            {
                Console.WriteLine("Invalid language selection. Defaulting to English.");
                SelectedLanguage = "English";
            }

            return selectedLanguage;
        }

        public string TranslationTool(string key)
        {
            // Determine the language index: 0 for English, 1 for Swedish
            int languageIndex = SelectedLanguage == "English" ? 0 : 1;

            // Fetch and return the translation
            if (_translations.ContainsKey(key))
            {
                return _translations[key][languageIndex];
            }
            else
            {
                Console.WriteLine($"Translation key '{key}' not found.");
                return "Error";
            }
        }
    }
}