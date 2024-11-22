using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace JediBank
{
    internal class DataBase
    {
        // Configure JsonSerializer Options polymorphism
        private static readonly JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public static void ArchiveUsers(List<User> users)
        {
            WriteToJsonFile("./users.json", users);
        }
        public static List<User> LoadUsers()
        {
            List<User> users = new List<User>();
            users = ReadFromJsonFile<List<User>>("./users.json");
            return users;
        }



        private static void WriteToJsonFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            try
            {
                // serialize to jsoN
                string contentsToWriteToFile = JsonSerializer.Serialize(objectToWrite, options);

                using (var writer = new StreamWriter(filePath, append))
                {
                    writer.Write(contentsToWriteToFile);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to file: {ex.Message}");
            }
        }

        private static T ReadFromJsonFile<T>(string filePath) where T : new()
        {
            try
            {
                string fileContents;
                using (var reader = new StreamReader(filePath))
                {
                    fileContents = reader.ReadToEnd();
                }

                return JsonSerializer.Deserialize<T>(fileContents, options);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading from file: {ex.Message}");
                return default;
            }
        }


    }
}
