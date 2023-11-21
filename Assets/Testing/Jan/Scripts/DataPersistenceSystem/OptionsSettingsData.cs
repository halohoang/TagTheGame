using System.Runtime.Serialization;

[System.Serializable, DataContract]
public class OptionsSettingsData
{
    // --- Constructors ---
    public OptionsSettingsData() 
    {
        // Setting Values to Default
        MasterVolumeValue = 0;
        MusicVolumeValue = 0;
        EffectsVolumeValue = 0;
        MuteAudio = false;
    }

    // --- Fields ---
    private float _masterVolumeValue;
    private float _musicVolumeValue;
    private float _effectsVolumeValue;
    private bool _muteAudio;

    // --- Properties ---
    [DataMember] public float MasterVolumeValue { get => _masterVolumeValue; set => _masterVolumeValue = value; }
    [DataMember] public float MusicVolumeValue { get => _musicVolumeValue; set => _musicVolumeValue = value; }
    [DataMember] public float EffectsVolumeValue { get => _effectsVolumeValue; set => _effectsVolumeValue = value; }
    [DataMember] public bool MuteAudio { get => _muteAudio; set => _muteAudio = value; }
}
