using DataPersistence;
using JansLittleHelper;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UI
{
    public class OptionsHandler : MonoBehaviour
    {
        //------------------------------ Fields ------------------------------
        [Header("Needed References to GameObjects and Components")]
        [Space(2)]
        [Tooltip("The Reference to the 'AudioMixer' (Found in Assets/Resources/Audio)")]
        [SerializeField] private AudioMixer _audioMixer;
        [Tooltip("The Reference to the 'OptionsMenu-Panel' (Found in the Hierarchy -> 'Canvas-Ingame')")]
        [SerializeField] private GameObject _optionsMenu;
        [Tooltip("The Reference to the accordigly Slider in the 'OptionsMenu-Panel' (Found in the Hierarchy -> 'Canvas-Ingame')")]
        [SerializeField] private Slider _masterSlider, _musicSlider, _effectsSlider;
        [Space(3)]

        [Header("Needed References to Scriptable Objects")]
        [Space(2)]
        [Tooltip("The Reference to the Scriptable Object 'OptionsSettings' (Found in Assets/Resources/ScriptableObjects)")]
        [SerializeField] private OptionsSettingsSO _optionSettingsSO;
        [Space(5)]

        [Header("OptionsSlider")]
        [SerializeField, ReadOnly] private GameObject[] _audioSliderObjects;
        [SerializeField, ReadOnly] private GameObject[] _menuPanels;

        private string _masterVolume = "MasterVolume";
        private string _musicVolume = "MusicVolume";
        private string _effectsVolume = "EffectsVolume";


        //------------------------------ Methods ------------------------------

        //---------- Unity-Executed Methods ----------
        private void Awake()
        {
            #region Autoreferencing
            // Autoreferencing to Audio-Slider Objects
            _audioSliderObjects = GameObject.FindGameObjectsWithTag("AudioSliders");
            _menuPanels = GameObject.FindGameObjectsWithTag("MenuPanel");

            foreach (GameObject sliderObj in _audioSliderObjects)
            {
                if (sliderObj.name == "MasterVolume_Slider")
                    _masterSlider = sliderObj.GetComponent<Slider>();
                else if (sliderObj.name == "MusicVolume_Slider")
                    _musicSlider = sliderObj.GetComponent<Slider>();
                else if (sliderObj.name == "EffectsVolume_Slider")
                    _effectsSlider = sliderObj.GetComponent<Slider>();
            }

            _optionsMenu = NullChecksAndAutoReferencing.CheckAndGetGameObject(_optionsMenu, "Options", _menuPanels);

            // Autoreferencing to other Objects
            if (_optionSettingsSO == null)
            {
                _optionSettingsSO = Resources.Load("ScriptableObjects/OptionsSettings") as OptionsSettingsSO;
                Debug.Log($"<color=yellow>Caution! Reference for Scriptable Object 'OptionsSettingsSO' was not set in Inspector of '{this}'. Trying to set automatically.</color>");
            }

            if (_audioMixer == null)
            {
                _audioMixer = Resources.Load("Audio/AudioMixer") as AudioMixer;
                Debug.Log($"<color=yellow>Caution! Reference for 'AudioMixer' was not set in Inspector of '{this}'. Trying to set automatically.</color>");
            }

            if (_optionsMenu == null)
            {
                _optionsMenu = GameObject.FindGameObjectWithTag("OptionsMenu");
                Debug.Log($"<color=yellow>Caution! Reference for 'OptionsMenu-Panel' was not set in Inspector in '{this}'. Trying to set automatically.</color>");
            }
            #endregion

            _optionsMenu.SetActive(false);
        }

        private void Start()
        {
            // Loading saved Options-Data
            DataPersistenceManager.Instance.LoadOptionsSettings();

            // setting the values in the Scripteble Object to the loaded Data
            _masterSlider.value = _optionSettingsSO.SavedMasterVolume;
            _musicSlider.value = _optionSettingsSO.SavedMusicVolume;
            _effectsSlider.value = _optionSettingsSO.SavedEffectVolume;

            // setting the actual slider values to the stored data of the Scriptable Object
            _audioMixer.SetFloat(_masterVolume, _optionSettingsSO.SavedMasterVolume);
            _audioMixer.SetFloat(_musicVolume, _optionSettingsSO.SavedMusicVolume);
            _audioMixer.SetFloat(_effectsVolume, _optionSettingsSO.SavedEffectVolume);

            // todo: disable Effectslider since it's currently useless -> remove this if Effectslider will be usfull; JM (21.11.23)
            //_effectsSlider.gameObject.SetActive(false);
            //_masterSlider.gameObject.SetActive(false);
        }


        //---------- Custom Methods ----------        

        /// <summary>
        /// Changes the Master Volume according to transmitted value and stores the value equally to the Scriptable Object 'OptionSettings'.
        /// </summary>
        /// <param name="value"></param>
        public void ChangeAndSaveMasterVolume(float value)
        {
            _audioMixer.SetFloat(_masterVolume, value);
            _optionSettingsSO.SavedMasterVolume = value;   // saves actual MasterValue in according Scriptable Object
        }

        /// <summary>
        /// Changes the Music Volume according to transmitted value and stores the value equally to the Scriptable Object 'OptionSettings'.
        /// </summary>
        /// <param name="value"></param>
        public void ChangeAndSaveMusicVolume(float value)
        {
            _audioMixer.SetFloat(_musicVolume, value);
            _optionSettingsSO.SavedMusicVolume = value;    // saves actual MasterValue in according Scriptable Object
        }

        /// <summary>
        /// Changes the Effects Volume according to transmitted value and stores the value equally to the Scriptable Object 'OptionSettings'.
        /// </summary>
        /// <param name="value"></param>
        public void ChangeAndSaveEffectsVolume(float value)
        {
            _audioMixer.SetFloat(_effectsVolume, value);
            _optionSettingsSO.SavedEffectVolume = value;   // saves actual MasterValue in according Scriptable Object
        }

        /// <summary>
        /// Sets the Volume-Values of all three sliders to the default Mute-Value stored in the Scriptable Object 'OptionSettings' accordingly to the transmitted bool
        /// If bool is true -> mute volumes
        /// If bool is false -> unmute volumes
        /// </summary>
        /// <param name="isMuted"></param>
        public void MuteToggle(bool isMuted)
        {
            if (isMuted)
            {
                // Mute
                _audioMixer.SetFloat(_masterVolume, _optionSettingsSO.MuteAudioValue);
                _audioMixer.SetFloat(_musicVolume, _optionSettingsSO.MuteAudioValue);
                _audioMixer.SetFloat(_effectsVolume, _optionSettingsSO.MuteAudioValue);

                _optionSettingsSO.IsMuted = true;
            }
            else
            {
                // Unmute
                _audioMixer.SetFloat(_masterVolume, _optionSettingsSO.SavedMasterVolume);
                _audioMixer.SetFloat(_musicVolume, _optionSettingsSO.SavedMusicVolume);
                _audioMixer.SetFloat(_effectsVolume, _optionSettingsSO.SavedEffectVolume);

                _optionSettingsSO.IsMuted = false;
            }
        }

        /// <summary>
        /// Sets the Volume-Values of the Sliders to the Default Values stored in the Scriptable Object 'OptionSettings'
        /// </summary>
        public void ResetToDefault()
        {
            _masterSlider.value = _optionSettingsSO.DefaultSettings;
            _musicSlider.value = _optionSettingsSO.DefaultSettings;
            _effectsSlider.value = _optionSettingsSO.DefaultSettings;
        }

        /// <summary>
        /// Calls the SaveOptionsSettings() of DataPersistensManager so the the actual volume-values stored in the Scriptable Object 'OptionSettings' will be saved
        /// </summary>
        public void ApplySettings()
        {
            DataPersistenceManager.Instance.SaveOptionsSettings();
        }
    }
}