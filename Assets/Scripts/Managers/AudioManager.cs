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

        var audioTypes = Enum.GetValues(typeof(AudioType));
        int length = audioTypes.Length;
        _audioSourceArray = new AudioSource[length];

        for (int i = 0; i < length; i++)
        {
            string typeName = Enum.GetName(typeof(AudioType), i);
            GameObject go = new GameObject(typeName);
            go.transform.SetParent(transform, false);
            _audioSourceArray[i] = go.AddComponent<AudioSource>();
        }

        // BGM은 루프 재생 설정
        var bgmSource = _audioSourceArray[(int)AudioType.BGM];
        bgmSource.loop = true;
    }

    // 오디오 재생 함수
    public void PlaySound(AudioType audioType, string clipName)
    {
        AudioClip audioClip = LoadClip(clipName);
        AudioSource audioSource = _audioSourceArray[(int)audioType];

        if (audioClip == null)
        {
            Debug.LogWarning($"오디오 클립 '{clipName}' 을(를) 찾을 수 없습니다.");  // 한글 경고문
            return;
        }

        switch (audioType)
        {
            case AudioType.BGM:
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
                audioSource.clip = audioClip;
                audioSource.Play();
                break;

            case AudioType.SFX:
                audioSource.PlayOneShot(audioClip);
                break;

            default:
                Debug.LogError($"지원하지 않는 오디오 타입입니다: {audioType}");  // 한글 오류문
                break;
        }
    }

    // 피치(재생 속도) 조절 함수
    public void SetPitch(AudioType audioType, float pitch)
    {
        if (IsValidAudioType(audioType))
            _audioSourceArray[(int)audioType].pitch = pitch;
    }

    // 볼륨 조절 함수
    public void SetVolume(AudioType audioType, float volume)
    {
        if (IsValidAudioType(audioType))
            _audioSourceArray[(int)audioType].volume = volume;
    }

    // 일시정지 함수
    public void Pause(AudioType audioType)
    {
        if (IsValidAudioType(audioType))
            _audioSourceArray[(int)audioType].Pause();
    }

    // 일시정지 해제 함수
    public void Resume(AudioType audioType)
    {
        if (IsValidAudioType(audioType))
            _audioSourceArray[(int)audioType].UnPause();
    }

    // 오디오 정지 함수
    public void Stop(AudioType audioType)
    {
        if (IsValidAudioType(audioType))
            _audioSourceArray[(int)audioType].Stop();
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
        SetVolume(AudioType.BGM, 0f);
        SetVolume(AudioType.SFX, 0f);
    }

    // 음소거 해제
    public void Unmute()
    {
        SetVolume(AudioType.BGM, 1f);
        SetVolume(AudioType.SFX, 1f);
    }

    // 오디오 클립을 Resources 폴더에서 로드하거나 캐시에서 반환
    private const string AUDIO_PATH = "Audio";
    public AudioClip LoadClip(string clipName)
    {
        if (_clipCache.TryGetValue(clipName, out AudioClip clip))
        {
            return clip;
        }

        clip = Resources.Load<AudioClip>($"{AUDIO_PATH}/{clipName}");
        _clipCache[clipName] = clip;
        return clip;
    }

    // 유효한 AudioType인지 검사하는 헬퍼 함수
    private bool IsValidAudioType(AudioType audioType)
    {
        int index = (int)audioType;
        return _audioSourceArray != null && index >= 0 && index < _audioSourceArray.Length;
    }
}
