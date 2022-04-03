using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource myAudioSource;

    private Slider mySlider;
    private void Start()
    {
        mySlider = GetComponent<Slider>();
        mySlider.value = myAudioSource.volume;
    }

    public void ChangeVolume(float volIn)
    {
        myAudioSource.volume = volIn;
    }

}
