using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get { return _soundManager; } }

    public AudioSource BaseTrack;
    public AudioSource OverCalmTrack;
    public AudioSource OverTenseTrack;
    public AudioSource Sfx;
    public Animator Animator;

    private static SoundManager _soundManager;
    private const string _paramName = "Tense";

    private void Awake()
    {
        if (_soundManager != null && _soundManager != this)
            Destroy(_soundManager);
        else
            _soundManager = this;
    }

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
}