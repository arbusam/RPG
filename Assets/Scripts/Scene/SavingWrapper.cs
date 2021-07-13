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

        public void LoadGame(string saveFile)
        {
            SetCurrentSave(saveFile);
            ContinueGame();
        }

        public IEnumerator LoadMenu()
        {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(0);
            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);
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
            if (Input.GetKeyDown(KeyCode.L) && Debug.isDebugBuild && SceneManager.GetActiveScene().buildIndex != 0)
            {
                Load();
            }
            if (Input.GetKeyDown(KeyCode.S) && Debug.isDebugBuild && SceneManager.GetActiveScene().buildIndex != 0)
            {
                Save();
            }
            if (Input.GetKeyDown(KeyCode.Delete) && Debug.isDebugBuild && SceneManager.GetActiveScene().buildIndex != 0)
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

        public IEnumerable<string> ListSaves()
        {
            return GetComponent<SavingSystem>().ListSaves();
        }
    }

}