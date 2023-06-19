using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public Slider slider;
    private float oldVolume;
    public string volumeName;

    private void Awake()
    {
        oldVolume = slider.value;
        if (!PlayerPrefs.HasKey(volumeName))
            slider.value = 1;
        else slider.value = PlayerPrefs.GetFloat(volumeName);
    }

    private void Update()
    {
        if (volumeName == "volumeSound")
        {
            ChangeSliderValue(volumeName);
        }
        else if (volumeName == "volumeMusic")
        {
            ChangeSliderValue(volumeName);
        }
    }

    private void ChangeSliderValue(string _volumeName)
    {
        if (oldVolume != slider.value)
        {
            PlayerPrefs.SetFloat(_volumeName, slider.value);
            PlayerPrefs.Save();
            oldVolume = slider.value;
        }
    }
}
