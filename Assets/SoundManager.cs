using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] Slider sliderVolume;

    void Start()
    {
        if (!PlayerPrefs.HasKey("gameVolume"))
        {
            PlayerPrefs.SetFloat("gameVolume", 1);
            Load();
        }
        else
        {
            Load();
        }
    }

    public void ChangeVolume()
    {
        AudioListener.volume = sliderVolume.value;
        Save();
    }

    public void Load()
    {
        sliderVolume.value = PlayerPrefs.GetFloat("gameVolume");
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("gameVolume", sliderVolume.value);
    }
}
