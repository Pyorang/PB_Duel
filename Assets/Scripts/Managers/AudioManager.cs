using System;
using System.Collections.Generic;
using UnityEngine;

// 오디오 타입 열거형: 배경음(BGM), 효과음(SFX)
public enum AudioType
{
    BGM,
    SFX
}

// 오디오 재생을 담당하는 싱글톤 매니저 클래스
public class AudioManager : SingletonBehaviour<AudioManager>
{
    private AudioSource[] _audioSourceArray;                     // BGM, SFX를 위한 AudioSource 배열
    private Dictionary<string, AudioClip> _clipCache = new();    // 파일명을 키로 하는 AudioClip 캐시

    // 초기화 함수: AudioSource들을 세팅
    protected override void Init()
    {
        base.Init();

        int typeCount = Enum.GetValues(typeof(AudioType)).Length;
        _audioSourceArray = new AudioSource[typeCount];

        for (int i = 0; i < typeCount; i++)
        {
            string typeName = Enum.GetName(typeof(AudioType), i);
            GameObject go = new GameObject(typeName);
            go.transform.SetParent(transform, false);
            AudioSource source = go.AddComponent<AudioSource>();

            if ((AudioType)i == AudioType.BGM)
                source.loop = true;

            _audioSourceArray[i] = source;
        }
    }

    // 오디오 재생 함수
    public void PlaySound(AudioType audioType, string clipName)
    {
        AudioClip audioClip = LoadClip(clipName);
        if (audioClip == null)
        {
            Debug.LogWarning($"오디오 클립 '{clipName}' 을(를) 찾을 수 없습니다.");
            return;
        }

        AudioSource audioSource = GetAudioSource(audioType);

        if (audioType == AudioType.BGM)
        {
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else if (audioType == AudioType.SFX)
        {
            audioSource.PlayOneShot(audioClip);
        }
        else
        {
            Debug.LogError($"지원하지 않는 오디오 타입입니다: {audioType}");
        }
    }

    // 피치(재생 속도) 조절 함수
    public void SetPitch(AudioType audioType, float pitch)
    {
        GetAudioSource(audioType).pitch = pitch;
    }

    // 볼륨 조절 함수
    public void SetVolume(AudioType audioType, float volume)
    {
        GetAudioSource(audioType).volume = volume;
    }

    // 일시정지 함수
    public void Pause(AudioType audioType)
    {
        GetAudioSource(audioType).Pause();
    }

    // 일시정지 해제 함수
    public void Resume(AudioType audioType)
    {
        GetAudioSource(audioType).UnPause();
    }

    // 오디오 정지 함수
    public void Stop(AudioType audioType)
    {
        GetAudioSource(audioType).Stop();
    }

    // 모든 오디오 정지
    public void StopAllSounds()
    {
        foreach (var source in _audioSourceArray)
        {
            source.Stop();
        }
    }

    // 전체 음소거
    public void Mute()
    {
        foreach (AudioType type in Enum.GetValues(typeof(AudioType)))
        {
            SetVolume(type, 0f);
        }
    }

    // 음소거 해제
    public void Unmute()
    {
        foreach (AudioType type in Enum.GetValues(typeof(AudioType)))
        {
            SetVolume(type, 1f);
        }
    }

    // 오디오 클립을 Resources 폴더에서 로드하거나 캐시에서 반환
    private const string AUDIO_PATH = "Audio";
    public AudioClip LoadClip(string clipName)
    {
        if (_clipCache.TryGetValue(clipName, out var clip) && clip != null)
            return clip;

        clip = Resources.Load<AudioClip>($"{AUDIO_PATH}/{clipName}");

        if (clip == null)
        {
            Debug.LogWarning($"Resources 폴더에서 '{clipName}' 클립을 불러오지 못했습니다.");
        }

        _clipCache[clipName] = clip;
        return clip;
    }

    // 유효한 AudioType인지 검사하고 AudioSource 반환
    private AudioSource GetAudioSource(AudioType type)
    {
        int index = (int)type;

        if (_audioSourceArray == null || index < 0 || index >= _audioSourceArray.Length)
        {
            Debug.LogError($"잘못된 AudioType입니다: {type}");
            return new GameObject("InvalidAudioSource").AddComponent<AudioSource>();
        }

        return _audioSourceArray[index];
    }
}
