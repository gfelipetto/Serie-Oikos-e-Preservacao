using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeBehavior : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI volumeText;
    public AudioMixerGroup audioMixer;
    public Slider volume;



    private void Start ( ) {
        audioMixer.audioMixer.SetFloat("MainVolume", -15f);
        UpdateVolume();
    }

    private void UpdateVolume ( ) {
        volumeText.text = $"{(volume.normalizedValue * 100)}%";
        audioMixer.audioMixer.SetFloat("MainVolume",volume.value);


    }


}
