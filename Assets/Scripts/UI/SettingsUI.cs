using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class SettingsUI : MonoBehaviour
    {
        [SerializeField] Toggle sfxToggle;
        [SerializeField] Toggle backgroundMusicToggle;

        private void Start()
        {
            sfxToggle.isOn = PlayerPrefs.GetInt("SFX", 1) == 1;
            backgroundMusicToggle.isOn = PlayerPrefs.GetInt("Background Music", 1) == 1;
            sfxToggle.onValueChanged.AddListener(FindObjectOfType<Settings>().SFXChanged);
            backgroundMusicToggle.onValueChanged.AddListener(FindObjectOfType<Settings>().BackgroundMusicChanged);
        }
    }
}