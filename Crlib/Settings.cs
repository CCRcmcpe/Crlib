using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using REVUnit.Crlib.Extension;

namespace REVUnit.Crlib
{
    public abstract class Settings
    {
        protected Settings(string filePath = null)
        {
            FilePath = filePath ?? Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location) + ".json";
            Properties = GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public)
                .ToArray();
            Load(FilePath);
        }

        public string FilePath { get; }

        public IList<PropertyInfo> Properties { get; }

        public void Reload()
        {
            this.PopulateJsonFile(FilePath);
        }

        public void Load(string jsonPath)
        {
            if (!File.Exists(jsonPath) || new FileInfo(jsonPath).Length == 0L)
            {
                File.Create(jsonPath).Close();
                Save();
            }

            this.PopulateJsonFile(jsonPath);
        }

        public void SaveAs(string jsonFilePath)
        {
            this.SerializeToJsonFile(jsonFilePath);
        }

        public void Save()
        {
            this.SerializeToJsonFile(FilePath);
        }
    }
}