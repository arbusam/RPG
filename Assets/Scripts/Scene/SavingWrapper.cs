using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Saving;
using RPG.Scene;
using RPG.Cinematics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Scene
{
    public class SavingWrapper : MonoBehaviour
    {
        const string currentSaveKey = "currentSaveName";
        [SerializeField] float fadeInTime = 0.2f;
        [SerializeField] float fadeWaitTime = 1;
        [SerializeField] float fadeOutTime = 0.5f;

        public void ContinueGame()
        {
            StartCoroutine(LoadLastScene());
        }

        public void NewGame(string saveFile)
        {
            SetCurrentSave(saveFile);
            StartCoroutine(LoadFirstScene());
        }

        private void SetCurrentSave(string saveFile)
        {
            PlayerPrefs.SetString(currentSaveKey, saveFile);
        }

        private string GetCurrentSave()
        {
            return PlayerPrefs.GetString(currentSaveKey);
        }

        private IEnumerator LoadLastScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);
            yield return GetComponent<SavingSystem>().LoadLastScene(GetCurrentSave());
            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);
            if (FindObjectOfType<CinematicsStartSequence>() != null)
            {
                FindObjectOfType<CinematicsStartSequence>().StartSequence();
            }
        }

        private IEnumerator LoadFirstScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(1);
            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);
            if (FindObjectOfType<CinematicsStartSequence>() != null)
            {
                FindObjectOfType<CinematicsStartSequence>().StartSequence();
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L) && Debug.isDebugBuild)
            {
                Load();
            }
            if (Input.GetKeyDown(KeyCode.S) && Debug.isDebugBuild)
            {
                Save();
            }
            if (Input.GetKeyDown(KeyCode.Delete) && Debug.isDebugBuild)
            {
                Delete();
            }
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(GetCurrentSave());
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(GetCurrentSave());
        }

        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(GetCurrentSave());
            print("Reset Save");
        }
    }

}