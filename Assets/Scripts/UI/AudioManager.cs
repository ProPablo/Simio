using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class AudioManager : MonoBehaviour
{
    public static AudioManager i;
    public AudioMixer mixer;
    public AudioSource[] musicSources;
    public AudioClip[] levelTheme;
    public float fadeDuration = 5f;
    readonly AudioClip[] musicQueue = new AudioClip[2];
    double nextStartTime;
    int toggle;
    private void Awake()
    {
        if (i == null)
        {
            i = this;
            DontDestroyOnLoad(i);
            nextStartTime = AudioSettings.dspTime;
        }
        else
            Destroy(gameObject);
    }
    private void Update()
    {
        EnqueueMusic(levelTheme);
        DequeueMusic();
    }
    void EnqueueMusic(AudioClip[] theme)
    {
        if (musicQueue[1] != null) return;
        if (musicQueue[0] == null)
            musicQueue[0] = theme[Random.Range(0, theme.Length)];
        AudioClip nonRepeated;
        do nonRepeated = theme[Random.Range(0, theme.Length)];
        while (nonRepeated == musicQueue[0]);
        musicQueue[1] = nonRepeated;
    }
    void DequeueMusic()
    {
        if (AudioSettings.dspTime > nextStartTime - 1 - fadeDuration)
        {
            // Set next track in musicQueue to be played
            musicSources[toggle].clip = musicQueue[0];
            musicSources[toggle].PlayScheduled(nextStartTime - fadeDuration);

            // Iterate dspTime and toggle between the 2 music sources
            double clipDuration = (double)musicQueue[0].samples / musicQueue[0].frequency;
            nextStartTime += clipDuration;

            // Fade in/out mixer volume and switch active music sources
            if (toggle == 0)
            {
                StartCoroutine(FadeMixerGroup.StartFade(mixer, "music1", fadeDuration, 1));
                StartCoroutine(FadeMixerGroup.StartFade(mixer, "music2", fadeDuration, 0));
            }
            else
            {
                StartCoroutine(FadeMixerGroup.StartFade(mixer, "music2", fadeDuration, 1));
                StartCoroutine(FadeMixerGroup.StartFade(mixer, "music1", fadeDuration, 0));
            }
            toggle = 1 - toggle;

            // Dequeue musicQueue so that EnqueueMusic will select a new random track
            musicQueue[0] = musicQueue[1];
            musicQueue[1] = null;
        }
    }
}
