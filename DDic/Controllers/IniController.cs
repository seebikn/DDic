using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDic.Models;

namespace DDic.Controllers
{
    internal class IniController
    {
        private IniFileHandler iniFileHandler;

        public IniController(string iniFilePath)
        {
            iniFileHandler = new IniFileHandler(iniFilePath);
        }
        public void InitializeFile()
        {
            if (!iniFileHandler.FileExists())
            {
                iniFileHandler.CreateFile();
            }
        }

        public string GetSetting(string section, string key)
        {
            return iniFileHandler.ReadValue(section, key);
        }

        public void SetSetting(string section, string key, string value)
        {
            iniFileHandler.WriteValue(section, key, value);
        }

    }
}
