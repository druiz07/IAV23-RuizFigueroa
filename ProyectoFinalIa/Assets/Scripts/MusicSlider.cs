using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFXSlider : MonoBehaviour
{
    public AudioMixer mixer;
    // Start is called before the first frame update

    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat("SFXVol", Mathf.Log10(sliderValue) * 20);
    }
}
