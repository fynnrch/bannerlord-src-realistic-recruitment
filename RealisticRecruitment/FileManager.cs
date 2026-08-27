using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace RealisticRecruitment
{
    internal static class CustomData
    {
        internal static readonly string path_CustomData = Path.Combine(Directory.GetParent(Directory.GetParent(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).FullName).FullName, "CustomData");

        internal static void Verify()
        {
            if (!Directory.Exists(path_CustomData)) Directory.CreateDirectory(path_CustomData);
        }
    }

    internal static class ErrorFile
    {
        internal static readonly string path_ErrorFile = Path.Combine(CustomData.path_CustomData, "error.log");

        internal static void Init()
        {
            CustomData.Verify();
            Verify();
        }

        internal static void Write(string s)
        {
            CustomData.Verify();

            File.AppendAllText(path_ErrorFile, $"{s}" + Environment.NewLine);
        }

        private static void Verify()
        {
            File.WriteAllText(path_ErrorFile, string.Empty);
        }
    }

    internal static class ConfigFile
    {
        internal static readonly string path_ConfigFile = Path.Combine(CustomData.path_CustomData, "config.json");
        internal static ConfigData ConfigData = new ConfigData();

        internal static void Init()
        {
            CustomData.Verify();
            Verify();

            Load();
        }

        private static void Verify()
        {
            if (!File.Exists(path_ConfigFile))
            {
                var data = new ConfigData();
                string json = JsonConvert.SerializeObject(data, Formatting.Indented);

                File.WriteAllText(path_ConfigFile, json);
            }
        }

        private static void Load()
        {
            string json = File.ReadAllText(path_ConfigFile);

            try
            { 
                ConfigData = JsonConvert.DeserializeObject<ConfigData>(json) ?? new ConfigData(); 
                
                Validate();
            }
            catch
            {
                ConfigData = new ConfigData();
                json = JsonConvert.SerializeObject(ConfigData, Formatting.Indented);

                File.WriteAllText(path_ConfigFile, json);
            }
        }

        private static void Validate()
        {
            ConfigData.InternalRecruitmentRelationThreshold = Math.Max(-100, Math.Min(100, ConfigData.InternalRecruitmentRelationThreshold));
            ConfigData.ExternalRecruitmentRelationThreshold = Math.Max(-100, Math.Min(100, ConfigData.ExternalRecruitmentRelationThreshold));

            string json = JsonConvert.SerializeObject(ConfigData, Formatting.Indented);
            File.WriteAllText(path_ConfigFile, json);
        }
    }

    internal class ConfigData
    {
        public int InternalRecruitmentRelationThreshold = 40;
        public int ExternalRecruitmentRelationThreshold = 80;
        public bool RestrictTroopSpawnrate = true;
    }
}
