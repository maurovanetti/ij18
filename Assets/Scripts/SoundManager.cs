using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get { return _soundManager; } }

    [SerializeField]

    public AudioSource BaseTrack;
    public AudioSource OverCalmTrack;
    public AudioSource OverTenseTrack;
    public AudioSource Sfx;
    public Animator Animator;

    [Header("Ink SFX")]
    public SoundFx[] SoundFxs;
#if UNITY_EDITOR
    public bool PlayDebugSelectedSound;
#endif
    private static SoundManager _soundManager;
    private const string _paramName = "Tense";

    private void Awake()
    {
        if (_soundManager != null && _soundManager != this)
            Destroy(_soundManager);
        else
            _soundManager = this;
    }

#if UNITY_EDITOR

    private void Update()
    {
        if (PlayDebugSelectedSound)
        {
            PlayDebugSelectedSound = false;

            SoundFx sfx = SoundFxs.FirstOrDefault(sfx => sfx.SelectedForDebug);

            if (!sfx.PlayMoreThanOnce)
                sfx.Play();
            else
                StartCoroutine(PlaySfxRepeating(sfx));
        }
    }

#endif

    public void Intensify()
    {
        Animator.SetBool("Tense", true);
    }

    public void Abate()
    {
        Animator.SetBool("Tense", false);
    }

    public void PlaySingleSound(AudioClip audio)
    {
        Sfx.PlayOneShot(audio);
    }

    public void PlaySfx(string sfxKey)
    {
        SoundFx sfx = SoundFxs.SingleOrDefault(sfx => sfx.Key.Equals(sfxKey));

        if (sfx != null)
        {
            if (!sfx.PlayMoreThanOnce)
                sfx.Play();
            else
                StartCoroutine(PlaySfxRepeating(sfx));
        }
    }

    private IEnumerator PlaySfxRepeating(SoundFx sfx)
    {
        for(int i = 0; i < sfx.NumberOfPlay; i++)
        {
            sfx.Play();
            yield return new WaitForSeconds(UnityEngine.Random.Range(sfx.MinInterval, sfx.MaxInterval));
        }
    }
}

public enum SoundFxPlayMode
{
    //suona una volta
    OneShot,
    //suona una volta scegliendo random da una lista di clip
    Random
}

[Serializable]
public class SoundFx
{
#if UNITY_EDITOR
    public bool SelectedForDebug;
#endif

    [Header("General")]
    public AudioSource Source;
    public string Key;
    public AudioClip[] Clips;
    [Range(0, 1)]
    public float Volume;
    public SoundFxPlayMode Mode;
    public bool PlayMoreThanOnce;
    public int NumberOfPlay;
    public float MinInterval;
    public float MaxInterval;

    public void Play()
    {
        if (Mode == SoundFxPlayMode.OneShot)
        {
            Source.volume = Volume;
            Source.PlayOneShot(Clips[0]);
        }

        if (Mode == SoundFxPlayMode.Random)
        {
            Source.volume = Volume;
            Source.PlayOneShot(Clips[UnityEngine.Random.Range(0, Clips.Length)]);
        }
    }
}