using GameDevTV.Saving;
using UnityEngine;

namespace RPG
{
    public class Settings : MonoBehaviour
    {

        private void Start()
        {
            GameObject.FindWithTag("Background Music").GetComponent<AudioSource>().volume = PlayerPrefs.GetInt("Background Music", 1) == 1 ? 0.5f : 0;
        }

        public void BackgroundMusicChanged(bool value)
        {
            PlayerPrefs.SetInt("Background Music", value ? 1 : 0);
            GameObject.FindWithTag("Background Music").GetComponent<AudioSource>().volume = value ? 0.5f : 0;
        }

        public void SFXChanged(bool value)
        {
            PlayerPrefs.SetInt("SFX", value ? 1 : 0);
        }

        private void Update()
        {
            foreach (AudioSource soundEmmiter in GameObject.FindObjectsOfType<AudioSource>())
            {
                if (soundEmmiter.gameObject.tag == "Background Music") continue;
                soundEmmiter.volume = PlayerPrefs.GetInt("SFX", 1) == 1 ? 1 : 0;
            }
        }
    }
}