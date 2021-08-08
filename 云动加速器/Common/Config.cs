using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudsMove.Common
{
    public class Config
    {
        public Dictionary<string, string> configData;
        string fullFileName;
        public Config(string _fileName)
        {
            configData = new Dictionary<string, string>();
            fullFileName = Directory.GetCurrentDirectory() + @"\" + _fileName;
            bool hasCfgFile = File.Exists(Directory.GetCurrentDirectory() + @"\" + _fileName);
            if (hasCfgFile == false)
            {
                StreamWriter writer = new StreamWriter(File.Create(Directory.GetCurrentDirectory() + @"\" + _fileName), Encoding.Default);
                writer.Close();
            }
            StreamReader reader = new StreamReader(Directory.GetCurrentDirectory() + @"\" + _fileName, Encoding.Default);
            string line;

            int indx = 0;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith(";") || string.IsNullOrEmpty(line))
                    configData.Add(";" + indx++, line);
                else
                {
                    string[] key_value = line.Split('~');
                    if (key_value.Length >= 2)
                        configData.Add(key_value[0], key_value[1]);
                    else
                        configData.Add(";" + indx++, line);
                }
            }
            reader.Close();
        }

        public string Get(string key)
        {
            if (configData.Count <= 0)
                return null;
            else if (configData.ContainsKey(key))
                return configData[key].ToString();
            else
                return null;
        }

        public void Set(string key, string value)
        {
            if (configData.ContainsKey(key))
                configData[key] = value;
            else
                configData.Add(key, value);
        }

        public void Save()
        {
            StreamWriter writer = new StreamWriter(fullFileName, false, Encoding.Default);
            IDictionaryEnumerator enu = configData.GetEnumerator();
            while (enu.MoveNext())
            {
                if (enu.Key.ToString().StartsWith(";"))
                    writer.WriteLine(enu.Value);
                else
                    writer.WriteLine(enu.Key + "~" + enu.Value);
            }
            writer.Close();
        }
    }
}
