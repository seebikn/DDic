using DDic.Models;

namespace DDic.Controllers
{
    internal class IniController
    {
        private readonly IniFileHandler iniFileHandler;

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

        private string Get(string section, string key)
        {
            return iniFileHandler.ReadValue(section, key);
        }

        private void Set(string section, string key, string? value)
        {
            iniFileHandler.WriteValue(section, key, value ?? "");
        }

        public T Get<T>(string section, string key, T defaultValue = default!)
        {
            string value = Get(section, key);
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return defaultValue; // 変換失敗時はデフォルト値を返す
            }
        }

        public void Set<T>(string section, string key, T value)
        {
            Set(section, key, value?.ToString());
        }

    }
}
