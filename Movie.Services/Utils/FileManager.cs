using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Movie.Services.Utils
{
    public class FileManager
    {
        string filePath;
        public FileManager(string _filePath)
        {
            filePath = _filePath;
        }
        public IEnumerable<T> ReadFile<T>()
        {
            string fullFilePath = filePath + typeof(T).Name + ".json";

            if (!File.Exists(fullFilePath))
            {
                File.Create(fullFilePath);
            }

            using (StreamReader r = new StreamReader(filePath + typeof(T).Name + ".json"))
            {
                string json = r.ReadToEnd();

                List<T> items = JsonConvert.DeserializeObject<List<T>>(json);

                return items;
            }
        }
        public bool AppendToFile<T>(T obj) where T : class
        {
            try
            {
                string fullFilePath = filePath + typeof(T).Name + ".json";

                if (!File.Exists(fullFilePath))
                {
                    File.Create(fullFilePath);
                }
                string currentFile = File.ReadAllText(fullFilePath);
                
                List<T> firstState = new List<T>();

                if (currentFile.Length > 0)
                {
                    firstState = JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(fullFilePath));
                }
                firstState.Add(obj);

                File.WriteAllText(fullFilePath, JsonConvert.SerializeObject(firstState));
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
