using UnityEngine;

public class SoundManager : Manager<SoundManager>
{
    [SerializeField] private AudioSource musicSource;

    private bool musicOn;
    private bool soundOn;

    public bool MusicOn
    {
        get { return musicOn; }
        set
        {
            musicOn = value;
            musicSource.enabled = SoundOn;
        }
    }

    public bool SoundOn
    {
        get { return soundOn; }
        set
        {
            soundOn = value;
            if (Camera.main.TryGetComponent<AudioListener>(out var listener))
            {
                listener.enabled = soundOn;
            }
        }
    }

    public void ToggleMusic()
    {
        MusicOn = !MusicOn;
    }

    public void ToggleSound()
    {
        SoundOn = !SoundOn;
    }
}
