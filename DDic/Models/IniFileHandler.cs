using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace DDic.Models
{
    public class IniFileHandler
    {
        private readonly string filePath;
        private readonly Dictionary<string, Dictionary<string, string>> data = [];

        public IniFileHandler(string path)
        {
            filePath = path;
            Load();
        }

        private void Load()
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            data.Clear();
            string currentSection = "";
            Encoding encoding = new UTF8Encoding(true); // BOM付きUTF-8
            using (StreamReader reader = new(filePath, encoding))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine()!.Trim();
                    if (string.IsNullOrEmpty(line) || line.StartsWith(';')) continue;

                    if (line.StartsWith('[') && line.EndsWith(']'))
                    {
                        currentSection = line[1..^1];
                        if (!data.ContainsKey(currentSection))
                        {
                            data[currentSection] = [];
                        }
                    }
                    else if (line.Contains('='))
                    {
                        string[] parts = line.Split('=', 2);
                        string key = parts[0].Trim();
                        string value = parts[1].Trim();
                        if (!string.IsNullOrEmpty(currentSection))
                        {
                            data[currentSection][key] = value;
                        }
                    }
                }
            }
        }

        public string ReadValue(string section, string key, string defaultValue = "")
        {
            return data.ContainsKey(section) && data[section].ContainsKey(key)
                ? data[section][key]
                : defaultValue;
        }

        public void WriteValue(string section, string key, string value)
        {
            if (!data.ContainsKey(section))
            {
                data[section] = [];
            }
            data[section][key] = value;
            Save();
        }

        private void Save()
        {
            Encoding encoding = new UTF8Encoding(true); // BOM付きUTF-8
            using (StreamWriter writer = new(filePath, false, encoding))
            {
                foreach (var section in data)
                {
                    writer.WriteLine($"[{section.Key}]");
                    foreach (var kvp in section.Value)
                    {
                        writer.WriteLine($"{kvp.Key}={kvp.Value}");
                    }
                    writer.WriteLine();
                }
            }
        }
        public bool FileExists()
        {
            return File.Exists(filePath);
        }

        public void CreateFile()
        {
            if (!FileExists())
            {
                using (File.Create(filePath)) { }
            }
        }
    }
}
