using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace DDic.Models
{
    internal class IniFileHandler
    {
        private string filePath;

        public IniFileHandler(string filePath)
        {
            this.filePath = filePath;
        }

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string section, string key, string defaultValue, StringBuilder result, int size, string filePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern long WritePrivateProfileString(string section, string key, string value, string filePath);

        public string ReadValue(string section, string key)
        {
            StringBuilder result = new StringBuilder(2048);
            GetPrivateProfileString(section, key, "", result, result.Capacity, filePath);
            return result.ToString();
        }

        public void WriteValue(string section, string key, string? value)
        {
            WritePrivateProfileString(section, key, value??"", filePath);
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
