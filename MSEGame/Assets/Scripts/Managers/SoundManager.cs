using UnityEngine;

public class SoundManager : Manager<SoundManager>
{
    [SerializeField] private AudioSource musicSource;

    private bool musicOn = true;
    private bool soundOn = true;

    public bool MusicOn
    {
        get { return musicOn; }
        set
        {
            musicOn = value;
            if (SoundOn)
                musicSource.UnPause();
            else
                musicSource.Pause();
        }
    }

    public bool SoundOn
    {
        get { return soundOn; }
        set
        {
            soundOn = value;
            AudioListener.pause = !soundOn;
            AudioListener.volume = soundOn ? 1 : 0;
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
