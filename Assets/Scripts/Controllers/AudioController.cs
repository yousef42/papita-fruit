using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SoundType { Click, CorrectMove, IncorrectMove, Win }

public class AudioController : MonoBehaviour
{
    public static AudioController instance;

    [SerializeField] AudioClip[] bgMusics;
    [SerializeField] AudioClip[] sfxMusics;

    [Space]
    [SerializeField] AudioSource bgAudioSource;
    [SerializeField] AudioSource sfxSource_1;
    [SerializeField] AudioSource sfxSource_2;
    [SerializeField] AudioSource loopSoundAudioSource;

    [Space]
    [Range(0, 1), SerializeField] float bgVolumeSound = 1;
    [Range(0, 1), SerializeField] float sfxVolumeSound = 1;

    bool isBgOn;
    bool isSFXOn;
    bool bgIsPlaying;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(this.gameObject);
    }
    void Start()
    {
        isBgOn = GameController.Instance.GetBGMusic();
        isSFXOn = GameController.Instance.GetSFXMusic();

        bgAudioSource.volume = bgVolumeSound;
        sfxSource_1.volume = sfxVolumeSound;
    }

    int currentIndex = -1;
    public void PlayBGMusic(int index)
    {
        if (currentIndex != index)
        {
            currentIndex = index;
        }
        else
        {
            if (isBgOn && bgIsPlaying)
            {
                //print("00");
                return;
            }
        }
        if (!isBgOn) return;
        if (index > bgMusics.Length || index < 0) return;
        if (bgMusics[index] == null) return;
        bgAudioSource.clip = bgMusics[index];
        bgAudioSource.loop = true;
        bgAudioSource.Play();
        bgIsPlaying = true;
    }
    void StopBGAudio()
    {
        bgAudioSource.Stop();
        bgIsPlaying = false;
    }

    public void UpdateBGAudio(bool isBgOn)
    {
        this.isBgOn = isBgOn;
        if (isBgOn)
        {
            PlayBGMusic(currentIndex);
        }
        else
        {
            StopBGAudio();
        }
    }

    public void UpdateSfxAudio(bool isSfxOn)
    {
        this.isSFXOn = isSfxOn;
    }

    //صداهای شادی تو ترک 0 انداخته میشه و صدای گوینده کلمات تو ترک 1 انداخته میشه
    public void PlaySound(AudioClip clip, int sourceTrack)
    {
        if (!isSFXOn) return;
        if (clip == null) return;
        if (sourceTrack > 1 || sourceTrack < 0) return;
        AudioSource source = sourceTrack == 0 ? sfxSource_1 : sfxSource_2;
        //source.PlayOneShot(clip);
        source.clip = clip;
        source.Play();
    }

    public void PlayLoopSound(AudioClip clip)
    {
        if (!isSFXOn) return;
        if (clip == null) return;
        loopSoundAudioSource.clip = clip;
        loopSoundAudioSource.Play();
    }

    public void StopLoopSound()
    {
        loopSoundAudioSource.Stop();
    }

    public void PlaySound(SoundType type)
    {
        if (!isSFXOn) return;

        int index = -1;
        switch (type)
        {
            case SoundType.Click:
                index = 0;
                break;
            case SoundType.CorrectMove:
                index = 1;
                break;
            case SoundType.IncorrectMove:
                index = 2;
                break;
            case SoundType.Win:
                index = 3;
                break;
        }

        if (index >= sfxMusics.Length || index < 0) return;
        if (sfxMusics[index] == null) return;

        sfxSource_1.clip = sfxMusics[index];
        sfxSource_1.Play();
    }

    public void PlayOneShot(AudioClip audio)
    {
        PlayOneShot(audio, Vector3.zero, sfxVolumeSound);
    }

    public void PlayOneShot(AudioClip audio, Vector3 position)
    {
        PlayOneShot(audio, position, sfxVolumeSound);
    }

    public void PlayOneShot(AudioClip audio, Vector3 position, float volume)
    {
        if (audio == null || volume == 0) return;

        GameObject oneShotAudio = new GameObject("oneShotAudio");
        AudioSource tempAudio = oneShotAudio.AddComponent<AudioSource>();
        tempAudio.clip = audio;
        tempAudio.volume = volume;
        tempAudio.loop = false;
        tempAudio.Play();
        Destroy(oneShotAudio, audio.length);
    }

    public AudioSource PlaySound(AudioClip audio, bool loop) => PlaySound(audio, Vector3.zero, sfxVolumeSound, loop);
    public AudioSource PlaySound(AudioClip audio, Vector3 position, float volume, bool loop)
    {
        if (audio == null || volume == 0) return null;

        GameObject oneShotAudio = new GameObject("oneAudio");
        AudioSource tempAudio = oneShotAudio.AddComponent<AudioSource>();
        tempAudio.clip = audio;
        tempAudio.volume = volume;
        tempAudio.loop = loop;
        tempAudio.Play();
        //Destroy(oneShotAudio, audio.length);

        return tempAudio;
    }
}
