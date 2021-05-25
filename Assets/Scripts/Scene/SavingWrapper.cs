using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Saving;
using RPG.Scene;
using RPG.Cinematics;
using UnityEngine;

public class SavingWrapper : MonoBehaviour
{
    const string defaultSaveFile = "save1";
    [SerializeField] float fadeInTime = 0.2f;
    [SerializeField] float fadeWaitTime = 0.5f;

    private void Awake()
    {
        StartCoroutine(LoadLastScene());
    }

    private IEnumerator LoadLastScene() {
        Fader fader = FindObjectOfType<Fader>();
        fader.FadeOutImmediate();
        yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
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
        GetComponent<SavingSystem>().Save(defaultSaveFile);
    }

    public void Load()
    {
        GetComponent<SavingSystem>().Load(defaultSaveFile);
    }

    public void Delete()
    {
        GetComponent<SavingSystem>().Delete(defaultSaveFile);
        print("Reset Save");
    }
}
