using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/OptionsSettings")]
public class OptionsSettingsSO : ScriptableObject, IOptionsDataPersistece
{
    //------------------------------ Fields ------------------------------
    [Header("Default Values")]
    [SerializeField] private float _defaultSettings = -10.0f;
    [SerializeField] private float _muteAudioValue = -80.0f;

    [Header("Stored Mute-Toggle Value")]
    [SerializeField] private bool _isMuted = false;

    [Header("Stored Slider Values")]
    [SerializeField] private float _savedMasterVolume = -10.0f;
    [SerializeField] private float _savedMusicVolume = -10.0f;
    [SerializeField] private float _savedEffectVolume = -10.0f;

    //---------- Properties ----------
    public float DefaultSettings { get => _defaultSettings; set => _defaultSettings = value; }
    public float MuteAudioValue { get => _muteAudioValue; set => _muteAudioValue = value; }
    public bool IsMuted { get => _isMuted; set => _isMuted = value; }

    public float SavedMasterVolume { get => _savedMasterVolume; set => _savedMasterVolume = value; }
    public float SavedMusicVolume { get => _savedMusicVolume; set => _savedMusicVolume = value; }
    public float SavedEffectVolume { get => _savedEffectVolume; set => _savedEffectVolume = value; }


    //------------------------------ Methods ------------------------------

    //---------- Custom Methods ----------
    #region obsolete
    ///// <summary>
    ///// Returns the actual runtime data stored to the scriptable object 'playerstats' as float[] (PlayerHealth = idx 0; PlayerEnergy = idx 1)
    ///// </summary>
    ///// <returns></returns>
    //public float[] GetSliderSettings()
    //{
    //    float[] optionsSettings = new float[] { SavedMasterVolume, SavedMusicVolume, SavedEffectVolume };
    //    return optionsSettings;
    //}

    ///// <summary>
    ///// returns the actual value for the bool 'IsMuted'
    ///// </summary>
    ///// <returns></returns>
    //public bool GetMuteSettings()
    //{
    //    return IsMuted;
    //}
    #endregion

    /// <summary>
    /// Sets the runtime data to its deafult values
    /// </summary>
    public void SetOptionsSettingsToDefault()
    {
        SavedMasterVolume = _defaultSettings;
        SavedMusicVolume = _defaultSettings;
        SavedEffectVolume = _defaultSettings;

        IsMuted = false;
    }

    /// <summary>
    /// Sets the runtime data according to the loaded ones of the <see cref="OptionsSettingsData"/>
    /// </summary>
    /// <param name="optData"></param>
    public void LoadOptionsData(OptionsSettingsData optData)
    {
        SavedMasterVolume = optData.MasterVolumeValue;
        SavedMusicVolume = optData.MusicVolumeValue;
        SavedEffectVolume = optData.EffectsVolumeValue;

        IsMuted = optData.MuteAudio;
    }

    /// <summary>
    /// Gets the runtime data and sets the values of the according variables in the <see cref="OptionsSettingsData"/>.
    /// </summary>
    /// <param name="optData"></param>
    public void SaveOptionsData(ref OptionsSettingsData optData)
    {
         optData.MasterVolumeValue = SavedMasterVolume;
         optData.MusicVolumeValue = SavedMusicVolume;
         optData.EffectsVolumeValue = SavedEffectVolume;

         optData.MuteAudio = IsMuted;
    }
}
