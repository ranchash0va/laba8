namespace lab8
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Runtime.Serialization.Json;

    class DataHandler<T>
    {
        public void SaveToBinaryFile(T data, string fileName)
        {
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, data);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении в бинарном файле: {ex.Message}");
            }
        }

        public T LoadFromBinaryFile(string fileName)
        {
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    return (T)formatter.Deserialize(fs);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке из бинарного файла: {ex.Message}");
                return default(T);
            }
        }

        public void SaveToJsonFile(T data, string fileName)
        {
            try
            {
                DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings
                {
                    UseSimpleDictionaryFormat = true,
                    EmitTypeInformation = System.Runtime.Serialization.EmitTypeInformation.Never
                };

                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(T), settings);

                using (FileStream fs = new FileStream(fileName, FileMode.Create))
                {
                    jsonSerializer.WriteObject(fs, data);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении в JSON файле: {ex.Message}");
            }
        }

        public T LoadFromJsonFile(string fileName)
        {
            try
            {
                DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings
                {
                    UseSimpleDictionaryFormat = true,
                    EmitTypeInformation = System.Runtime.Serialization.EmitTypeInformation.Never
                };

                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                {
                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(T), settings);
                    return (T)jsonSerializer.ReadObject(fs);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке из JSON файла: {ex.Message}");
                return default(T);
            }
        }
    }
}