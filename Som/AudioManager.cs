using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;
    static double scheduledTime = 5;
    public AudioMixerGroup audioMixer;
    public SoundBehavior[] sounds;



    private void Awake ( ) {


        if (instance == null) {
            instance = this;

        } else {
            Destroy(this.gameObject);
            return;
        }



        DontDestroyOnLoad(this.gameObject);

        foreach (SoundBehavior s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.playOnAwake = false;
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.inLooping;
            s.source.outputAudioMixerGroup = audioMixer;
                
        }
        SceneManager.sceneLoaded += SceneLoaded;
    }
    private void SceneLoaded ( Scene scene, LoadSceneMode loadSceneMode ) {
        OnLevelLoad();
    }
    private void OnLevelLoad ( ) {
        StartSceneMusic();
    }


    private void StartSceneMusic ( ) {
        StopAllSounds();
        scheduledTime = 5;
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName.Contains("Menu") || sceneName.Equals("Credits")) {
            instance.Play("Menu");

        } else {
            instance.Play(sceneName);

        }
    }

    private void StopAllSounds ( ) {
        foreach (SoundBehavior s in sounds) {
            s.source.Stop();
        }
    }

    public void Play ( string soundName ) {
        instance._Play(soundName);
    }

    public void Stop ( string soundName ) {
        instance._Stop(soundName);
    }

    private void _Play ( string soundName ) {

        SoundBehavior s = System.Array.Find(sounds, sound => sound.name == soundName);
        if (s == null || s.source.isPlaying) return;
        if (s.name.Equals("Win") || s.name.Equals("Lose")) {
            s.source.PlayScheduled(scheduledTime);
            scheduledTime = 5000;
        } else {
            s.source.Play();

        }
    }

    private void _Stop ( string soundName ) {
        SoundBehavior s = System.Array.Find(sounds, sound => sound.name == soundName);
        if (s == null) return;
        s.source.Stop();
    }

    public SoundBehavior GetAudioPlaying ( ) {
        return System.Array.Find(sounds, sound => sound.source.isPlaying);
    }

}
