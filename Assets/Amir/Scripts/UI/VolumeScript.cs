using UnityEngine;
using UnityEngine.Audio;

public class VolumeChange : MonoBehaviour
{
    [SerializeField] AudioMixerGroup _masterGroup;
    [SerializeField] AudioMixerGroup _effectsGroup;
    [SerializeField] AudioMixerGroup _musicGroup;
    [SerializeField] AudioMixerGroup _environmentGroup;

    // 0 - 10 ---> -50 - 0
    const byte _minDBValue = 50; // модуль числа

    public void SetMasterVolume(float value)
    {
        _masterGroup.audioMixer.SetFloat(NamesTags.VOLUME_MASTER, SetVolume(value));
    }

    public void SetMusicVolume(float value)
    {
        _masterGroup.audioMixer.SetFloat(NamesTags.VOLUME_MUSIC, SetVolume(value));
    }

    public void SetEffectsVolume(float value)
    {
        _masterGroup.audioMixer.SetFloat(NamesTags.VOLUME_EFFECTS, SetVolume(value));
    }

    public void SetEnvironmentVolume(float value)
    {
        _masterGroup.audioMixer.SetFloat(NamesTags.VOLUME_ENVIRONMENT, SetVolume(value));
    }



    float SetVolume(float value)
    {
        float new_value = (value * _minDBValue / 10) - _minDBValue;

        if (new_value == -_minDBValue)
            new_value = -80;

        return new_value;
    }
}
