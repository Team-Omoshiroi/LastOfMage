using System.Collections.Generic;
using UnityEngine;
using System.Resources;
using UnityEngine.Audio;

public class SoundManager : CustomSingleton<SoundManager>
{
    AudioSource[] _audioSources = new AudioSource[(int)eSoundType.MaxCount];
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
    [Header("0 BGM, 1 Effect")]
    [SerializeField] private List<AudioMixerGroup> Mixer;

    private void Awake()
    {
        string[] soundNames = System.Enum.GetNames(typeof(eSoundType));
        for (int i = 0; i < soundNames.Length - 1; i++)
        {
            GameObject go = new GameObject { name = soundNames[i] };
            var source = go.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = Mixer[i];
            _audioSources[i] = source;
            go.transform.parent = gameObject.transform;
        }

        _audioSources[(int)eSoundType.Bgm].loop = true;
    }

    public void Clear()
    {
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
        _audioClips.Clear();
    }

    public void Play(string path, eSoundType type = eSoundType.Bgm, float pitch = 1.0f, float volume = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        Play(audioClip, type, pitch, volume);
    }

    public void Play(AudioClip audioClip, eSoundType type = eSoundType.Bgm, float pitch = 1.0f, float volume = 1.0f)
    {
        if (audioClip == null)
            return;

        AudioSource audioSource;
        switch (type)
        {
            case eSoundType.Bgm:
                audioSource = _audioSources[(int)eSoundType.Bgm];
                if (audioSource.isPlaying)
                    audioSource.Stop();
                audioSource.pitch = pitch;
                audioSource.volume = volume;
                audioSource.clip = audioClip;
                audioSource.Play();
                break;
            case eSoundType.Effect:
                audioSource = _audioSources[(int)eSoundType.Effect];
                audioSource.pitch = pitch;
                audioSource.volume = volume;
                audioSource.PlayOneShot(audioClip);
                break;
        }
    }

    AudioClip GetOrAddAudioClip(string path, eSoundType type = eSoundType.Bgm)
    {
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}";

        AudioClip audioClip = null;

        if (type == eSoundType.Bgm)
        {
            audioClip = Resources.Load<AudioClip>(path);
        }
        else
        {
            if (_audioClips.TryGetValue(path, out audioClip) == false)
            {
                audioClip = Resources.Load<AudioClip>(path);
                _audioClips.Add(path, audioClip);
            }
        }

        if (audioClip == null)
            Debug.Log($"AudioClip Missing ! {path}");

        return audioClip;
    }

    public void VolumeSetting(eSoundType type, float volume)
    {
        switch (type)
        {
            case eSoundType.Bgm:
                AudioSource bgmAudioSource = _audioSources[(int)eSoundType.Bgm];
                bgmAudioSource.volume = volume;
                break;
            case eSoundType.Effect:
                AudioSource effectAudioSource = _audioSources[(int)eSoundType.Effect];
                effectAudioSource.volume = volume;
                break;
        }
    }
}
