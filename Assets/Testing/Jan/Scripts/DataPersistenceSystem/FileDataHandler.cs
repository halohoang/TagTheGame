using UnityEngine;
using System;
using System.IO;
using UnityEngine.Events;
using System.Runtime.Serialization.Json;

namespace DataPersistence
{
    public class FileDataHandler
    {
        public static event UnityAction<string> OnUserFeedback;

        //------------------------------ Fields ------------------------------
        private string _dataDirectoryPath = "";
        private string _dataFileName = "";


        //---------- Constructor ----------
        public FileDataHandler(string dataDirectoryPath, string dataFileName)
        {
            _dataDirectoryPath = dataDirectoryPath;
            _dataFileName = dataFileName;
        }

        //------------------------------ Methods ------------------------------

        //---------- Save/Load GameData ----------
        /// <summary>
        /// Serializes the game data to a Json File
        /// </summary>
        /// <param name="gameDataToSerialize"></param>
        public void SerializeGameData(GameData gameDataToSerialize)
        {
            // use Path.Combine to connect for different OS's having different path separators
            string savePath = Path.Combine(_dataDirectoryPath, _dataFileName);
            try
            {
                // create the directory file will be written to if it doesn't already exist
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));

                //// serialize the C# gameData Object into Json
                //string dataToStore = JsonUtility.ToJson(gameDataToSerialize, true);

                //write the serialized data to the file
                using (FileStream fileStream = new FileStream(savePath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(fileStream))
                    {
                        DataContractJsonSerializer jsonSer = new DataContractJsonSerializer(typeof(GameData));
                        jsonSer.WriteObject(fileStream, gameDataToSerialize);
                        //writer.Write(dataToStore);
                    }
                }

                //Debug.Log($"SerializeGameData() was called in {this}; GameData that should have been stored = PlayerHealth: '<color=cyan>{gameDataToSerialize.HealthPoints}</color>' | PlayerStamina: '<color=cyan>{gameDataToSerialize.Stamina}</color>' | Potion: '<color=cyan>{gameDataToSerialize.PotionCount}</color>'");
                OnUserFeedback?.Invoke($"Saving was successfull!");
            }
            catch (Exception ex)
            {
                Debug.LogError($"<color=red>Error occured</color> while trying to serialize data to file: '{savePath}'; \n Exception: '{ex}'");
                OnUserFeedback?.Invoke($"Error occured while trying to save data to file: '{savePath}'; \n Exception: '{ex}'");
            }
        }

        /// <summary>
        /// Deserializes the previously serialized Game Data from a Json File
        /// </summary>
        /// <returns></returns>
        public GameData DeserializeGameData()
        {
            // use Path.Combine to connect for different OS's having different path separators
            string savePath = Path.Combine(_dataDirectoryPath, _dataFileName);

            GameData loadedData = null;

            if (File.Exists(savePath))
            {
                try
                {   // read the deserialized datat and write them into a new GameDataPersistence (loadedData)
                    //string dataToLoad = "";
                    using (FileStream fileStream = new FileStream(savePath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(fileStream))
                        {
                            DataContractJsonSerializer jsonSer = new DataContractJsonSerializer(typeof(GameData));
                            loadedData = jsonSer.ReadObject(fileStream) as GameData;
                            //dataToLoad = reader.ReadToEnd();
                        }
                    }

                    //deserialize teh Date from Json back into the C# object
                    //loadedData = JsonUtility.FromJson<GameData>(dataToLoad);

                    //Debug.Log($"DeserializeGameData() was called in {this}; GameData that should have been stored = PlayerHealth: '<color=lime>{loadedData.HealthPoints}</color>' | PlayerStamina: '<color=lime>{loadedData.Stamina}</color>' | Potion: '<color=lime>{loadedData.PotionCount}</color>'");
                    OnUserFeedback?.Invoke($"Loading...");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"<color=red>Error occured</color> while trying to deserialize data from file: '{savePath}'; \n Exception: '{ex}'");
                    OnUserFeedback?.Invoke($"Error occured while trying to load data from file: '{savePath}'; \n Exception: '{ex}'");
                }
            }
            return loadedData;
        }


        //---------- Save/Load OptionsSettings ----------

        /// <summary>
        /// Serializing OptionsSettingsData stored in OptionsDataPersistence
        /// </summary>
        /// <param name="optionsaDataToSerialize"></param>
        public void SerializeOptionsSettings(OptionsSettingsData optionsaDataToSerialize)
        {
            // use Path.Combine to connect for different OS's having different path separators
            string savePath = Path.Combine(_dataDirectoryPath, _dataFileName);

            try
            {
                // create the directory file will be written to if it doesn't already exist
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));

                //// serialize the C# optionsData Object into Json
                //string dataToStore = JsonUtility.ToJson(optionsDataToSerialize, true);

                //write the serialized data to the file
                using (FileStream fileStream = new FileStream(savePath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(fileStream))
                    {
                        DataContractJsonSerializer jsonSer = new DataContractJsonSerializer(typeof(OptionsSettingsData));
                        jsonSer.WriteObject(fileStream, optionsaDataToSerialize);
                    }
                }

                Debug.Log($"SerializeOptionsSettings() was called in {this}; OptionsData that should have been stored -> MasterVolume: '<color=cyan>{optionsaDataToSerialize.MasterVolumeValue}</color>' | MusicVolume: '<color=cyan>{optionsaDataToSerialize.MusicVolumeValue}</color>' | EffectsVolume: '<color=cyan>{optionsaDataToSerialize.EffectsVolumeValue}</color>'");
                OnUserFeedback?.Invoke($"Saving was successfull!");
            }
            catch (Exception ex)
            {
                Debug.LogError($"<color=red>Error occured</color> while trying to serialize data to file: '{savePath}'; \n Exception: '{ex}'");
                OnUserFeedback?.Invoke($"Error occured while trying to save data to file: '{savePath}'; \n Exception: '{ex}'");
            }
        }

        /// <summary>
        /// Deserialize OptionSettings Data, wirte them into a new OptionsDataPersistens and return it
        /// </summary>
        /// <returns></returns>
        public OptionsSettingsData DeserializeOptionsSettings()
        {
            // use Path.Combine to connect for different OS's having different path separators
            string savePath = Path.Combine(_dataDirectoryPath, _dataFileName);

            OptionsSettingsData loadedData = null;

            if (File.Exists(savePath))
            {
                try
                {   // read the deserialized datat and write them into a new OptionsSettingsData (loadedData)
                    //string dataToLoad = "";
                    using (FileStream fileStream = new FileStream(savePath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(fileStream))
                        {
                            DataContractJsonSerializer jsonSer = new DataContractJsonSerializer(typeof(OptionsSettingsData));
                            loadedData = jsonSer.ReadObject(fileStream) as OptionsSettingsData;
                            //dataToLoad = reader.ReadToEnd();
                        }
                    }

                    //deserialize the Data from Json back into the C# object
                    //loadedData = JsonUtility.FromJson<OptionsSettingsData>(dataToLoad);

                    //Debug.Log($"DeserializeGameData() was called in {this}; OptionsData that should have been stored -> MasterVolume: '<color=cyan>{loadedData.MasterVolumeValue}</color>' | MusicVolume: '<color=cyan>{loadedData.MusicVolumeValue}</color>' | EffectsVolume: '<color=cyan>{loadedData.EffectsVolumeValue}</color>'");
                    OnUserFeedback?.Invoke($"Loading...");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"<color=red>Error occured</color> while trying to deserialize data from file: '{savePath}'; \n Exception: '{ex}'");
                    OnUserFeedback?.Invoke($"Error occured while trying to load data from file: '{savePath}'; \n Exception: '{ex}'");
                }
            }
            return loadedData;
        }
    }
}