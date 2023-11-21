using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DataPersistence
{
    [DisallowMultipleComponent]
    public class DataPersistenceManager : MonoBehaviour
    {
        //------------------------------ Fields ------------------------------
        //public static event UnityAction<string> OnUserFeedback;

        public static DataPersistenceManager Instance { get; private set; }

        public FileDataHandler DataHandler { get => _dataHandler; private set => _dataHandler = value; }
        public bool LoadGameDataOnSceneStart { get => _loadGameDataOnSceneStart; set => _loadGameDataOnSceneStart = value; }

        [Header("File Storage Config")]
        [Tooltip("The name of the file that actual contains the serialized relevant game data.")]
        [SerializeField] private string _gameDataFileName = null;
        [Tooltip("The name of the file that actual contains the serialized relevant game data.")]
        [SerializeField] private string _optionsDataFileName = null;

        private bool _loadGameDataOnSceneStart;
        private GameData _gameData;
        private OptionsSettingsData _optionsData;
        private List<IGameDataPersistence> _gameDataPersistenceObjects;
        private List<IOptionsDataPersistece> _optionsDataPersistenceObjects;
        private FileDataHandler _dataHandler;

        //------------------------------ Methods ------------------------------
        //---------- Unity-Executed Methods ----------
        private void Awake()
        {
            #region Singleton-Pattern
            if (Instance != null && Instance != this)
            {
                Debug.Log($"Found more than one of {this} in the scene, destroying the newest one.");
                Destroy(this.gameObject);
                return;
            }
            else
                Instance = this;
            #endregion

            DontDestroyOnLoad(this.gameObject);
           
            _gameData = new GameData();
            _optionsData = new OptionsSettingsData();
        }

        private void OnEnable()
        {
            //DataHandler.OnException += TransmitExceptionMessage;
            //FileDataHandler.OnUserFeedback += TransmitExceptionMessage;

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnDisable()
        {
            //DataHandler.OnException -= TransmitExceptionMessage;
            //FileDataHandler.OnUserFeedback -= TransmitExceptionMessage;

            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            Debug.Log($"'OnSceneLoaded()' was called in '{this}'");
            // instatiate List for Objects that implement the IGameDataPersistence.cs
            _gameDataPersistenceObjects = FindAllGameDataPersistenceObjects();

            // instatiate List for Objects that implement the IOptionsDataPersistence.cs
            _optionsDataPersistenceObjects = FindAllOptionsDataPersistenceObjects();


            if (LoadGameDataOnSceneStart)
            {
                LoadGameData();
                LoadGameDataOnSceneStart = false;
            }
            else
                NewGameData();
        }

        public void OnSceneUnloaded(Scene scene)
        {
            Debug.Log($"'OnSceneUnloaded()' was called in '{this}'");
            // fill with logic if needed; JM
        }

        private void Start()
        {
            if (_gameDataFileName == "" || _gameDataFileName == null)
                _gameDataFileName = "DogTag_SaveGame.json";

            if (_optionsDataFileName == "" || _optionsDataFileName == null)
                _optionsDataFileName = "DogTag_config.jsom";
        }


        //---------- Custom Methods ----------

        //--- Save/Load GameData ---

        /// <summary>
        /// Resetting relevant GameDate to its Default and enable posibility for GameStart-Cutscene to be started
        /// </summary>
        public void NewGameData()
        {
            _gameData = new GameData();
        }

        /// <summary>
        /// Serialize Actual GameData to the specific GamePath and starting Coroutine for Feedback to user, that saving was succsessfull
        /// </summary>
        public void SaveGameData()
        {
            #region OldCode
            //if (PersRelevantGameData == null)
            //    PersRelevantGameData = new GameDataPersistence();

            //PersRelevantGameData.GetActualGameData();  // Actualizing the stored GameData

            //_fileHandler = new FileDataHandler(Application.persistentDataPath, _gameDataFileName);

            //_fileHandler.SerializeGameData(PersRelevantGameData);   // Serialize GameData to declared SavePath
            //Debug.Log($"<color=orange>Game data should have been saved successfully to '{Application.persistentDataPath}/{_gameDataFileName}'</color>");

            //Debug.Log($"SaveGameData() was called in {this}; GameData that should have been stored = PlayerHealth: {PersRelevantGameData.PlayerHealth} | PlayerEnergy: {PersRelevantGameData.PlayerEnergy} | PlayerPosition: {PersRelevantGameData.PlayerPosition} | CastleDugeonLvl: {PersRelevantGameData.CastleDungeonLvl} | CaveDugeonLvl: {PersRelevantGameData.CaveDungeonLvl} | CryptDugeonLvl: {PersRelevantGameData.CryptDungeonLvl} | ToolQuest: {PersRelevantGameData.IsToolQuestAccomplished} | StoneQuest: {PersRelevantGameData.IsStoneQuestAccomplished} | WoodQuest: {PersRelevantGameData.IsWoodQuestAccomplished}");

            //StartCoroutine(nameof(ShowingUserInformation));
            #endregion

            DataHandler = new FileDataHandler(Application.persistentDataPath, _gameDataFileName);

            if (_gameData != null)
            {
                // update the relevant gameData in GameData() via Interface (all Classes that holds the actual relevant GameDate need to update those to the GameData.cs)
                foreach (IGameDataPersistence dataPersistenceObj in _gameDataPersistenceObjects)
                {
                    dataPersistenceObj.SaveData(ref _gameData);
                }
            }
            else
            {
                _gameData = new GameData();

                // update the relevant gameData in GameData() via Interface (all Classes that holds the actual relevant GameDate need to update those to the GameData.cs)
                foreach (IGameDataPersistence dataPersistenceObj in _gameDataPersistenceObjects)
                {
                    dataPersistenceObj.SaveData(ref _gameData);
                }
            }

            // serialize GameData via the FileDataHandler;
            DataHandler.SerializeGameData(_gameData);

            //Debug.Log($"saved Data: PotionCount: '<color=cyan>{_gameData.PotionCount}</color>'; HealthPoints: '<color=cyan>{_gameData.HealthPoints}</color>'; Stamina: '<color=cyan>{_gameData.Stamina}</color>'");

            //OnUserFeedback?.Invoke($"Saving was successfull!");
        }

        /// <summary>
        /// Start Corouting for Feedback to user that it's tried to load game data, deserialize saved GameData from SavePath and set the runtime Values of the Scriptable Objects that store the 
        /// GameRelevant data accordingly to the loaded data; If Loading is failed e.g. due to missing SaveFiles in SavePath User will be informed about it.
        /// </summary>
        public void LoadGameData()
        {
            DataHandler = new FileDataHandler(Application.persistentDataPath, _gameDataFileName);

            if (_gameData != null)
            {
                // deserialize GameData via the FileDataHandler;
                _gameData = DataHandler.DeserializeGameData();

                // todo: inform User if no Data is there to load; JM

                // push all loaded data to all the classes that needs them
                foreach (IGameDataPersistence dataPersistenceObj in _gameDataPersistenceObjects)
                {
                    dataPersistenceObj.LoadData(_gameData);
                }
            }
            else
            {
                _gameData = new GameData();

                // deserialize GameData via the FileDataHandler;
                _gameData = DataHandler.DeserializeGameData();

                // todo: inform User if no Data is there to load; JM

                // push all loaded data to all the classes that needs them
                foreach (IGameDataPersistence dataPersistenceObj in _gameDataPersistenceObjects)
                {
                    dataPersistenceObj.LoadData(_gameData);
                }
            }

            //Debug.Log($"loaded Data: PotionCount: '<color=lime>{_gameData.PotionCount}</color>'; HealthPoints: '<color=lime>{_gameData.HealthPoints}</color>'; Stamina: '<color=lime>{_gameData.Stamina}</color>'");
            //OnUserFeedback?.Invoke($"Loading data...");
        }

        //--- Save/Load OptionsSettings ---

        /// <summary>
        /// Serialize the actual OptionSettings
        /// </summary>
        public void SaveOptionsSettings()
        {
            DataHandler = new FileDataHandler(Application.persistentDataPath, _optionsDataFileName);

            if (_optionsData != null)
            {
                //update the relevant gameData in GameData() via Interface(all Classes that holds the actual relevant GameDate need to update those to the GameData.cs)
                foreach (IOptionsDataPersistece dataPersistenceObj in _optionsDataPersistenceObjects)
                {
                    dataPersistenceObj.SaveOptionsData(ref _optionsData);
                }
            }
            else
            {
                _optionsData = new OptionsSettingsData();

                // update the relevant gameData in GameData() via Interface (all Classes that holds the actual relevant GameDate need to update those to the GameData.cs)
                foreach (IOptionsDataPersistece dataPersistenceObj in _optionsDataPersistenceObjects)
                {
                    dataPersistenceObj.SaveOptionsData(ref _optionsData);
                }
            }

            // serialize GameData via the FileDataHandler;
            DataHandler.SerializeOptionsSettings(_optionsData);
        }

        /// <summary>
        /// Deserialize the Optionssettings and set the values in the Scriptable Object 'OptionSettings' to the deserialized ones
        /// </summary>
        public void LoadOptionsSettings()
        {
            DataHandler = new FileDataHandler(Application.persistentDataPath, _optionsDataFileName);

            if (_optionsData != null)
            {
                // deserialize GameData via the FileDataHandler;
                _optionsData = DataHandler.DeserializeOptionsSettings();

                // todo: inform User if no Data is there to load; JM

                // push all loaded data to all the classes that needs them
                foreach (IOptionsDataPersistece dataPersistenceObj in _optionsDataPersistenceObjects)
                {
                    dataPersistenceObj.LoadOptionsData(_optionsData);
                }
            }
            else
            {
                _optionsData = new OptionsSettingsData();

                // deserialize GameData via the FileDataHandler;
                _optionsData = DataHandler.DeserializeOptionsSettings();

                // todo: inform User if no Data is there to load; JM

                // push all loaded data to all the classes that needs them
                foreach (IOptionsDataPersistece dataPersistenceObj in _optionsDataPersistenceObjects)
                {
                    dataPersistenceObj.LoadOptionsData(_optionsData);
                }
            }
        }

        private List<IGameDataPersistence> FindAllGameDataPersistenceObjects()
        {
            IEnumerable<IGameDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IGameDataPersistence>();

            return new List<IGameDataPersistence>(dataPersistenceObjects);
        }

        private List<IOptionsDataPersistece> FindAllOptionsDataPersistenceObjects()
        {
            IEnumerable<IOptionsDataPersistece> dataPersistenceObjects = FindObjectsOfType<ScriptableObject>().OfType<IOptionsDataPersistece>();

            return new List<IOptionsDataPersistece>(dataPersistenceObjects);
        }
    }
}