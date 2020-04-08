using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace PigeonCorp.Persistence.Gateway
{
    public class BinaryGateway : IUserDataGateway
    {
        public T Get<T>() where T : class, new()
        {
            var savePath = GetSavePath<T>();
            using (var fileStream = File.Open(savePath, FileMode.Open))
            {
                T data;
                var binaryFormatter = new BinaryFormatter();
                try
                {
                    data = (T) binaryFormatter.Deserialize(fileStream);
                    fileStream.Close();
                }
                catch (SerializationException e)
                {
                    fileStream.Close();
                    data = new T();
                    Update(data);
                }

                return data;
            }
        }

        public void Update<T>(T data) where T : class
        {
            var savePath = GetSavePath<T>();
            using (var fileStream = File.Open(savePath, FileMode.Create))
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fileStream, data);
                fileStream.Close();
            }
        }
    
        public void DeleteAll()
        {
            var directoryInfo = new DirectoryInfo(GetFolderPath());
            foreach (var file in directoryInfo.GetFiles())
            {
                file.Delete();
            }
        }

        private static string GetSavePath<T>()
        {
            var savePath = Path.Combine(GetFolderPath(), typeof(T).Name);

            if (!File.Exists(savePath))
            {
                var fileStream = File.Create(savePath);
                fileStream.Close();
            }

            return savePath;
        }

        private static string GetFolderPath()
        {
            var folderName = "SaveData";
            var folderPath = Path.Combine(Application.persistentDataPath, folderName);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            return folderPath;
        }
    }
}