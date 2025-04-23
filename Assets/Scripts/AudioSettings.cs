using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider volumeSlider;


    private static float volume = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        audioMixer.SetFloat("Volumen", volume);
        volumeSlider.value = volume;
        volumeSlider.onValueChanged.AddListener(delegate { ChangeVolume(volumeSlider.value); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ChangeVolume(float newVolume)
    {
        volume = newVolume;
        audioMixer.SetFloat("Volumen", volume);
    }
}
