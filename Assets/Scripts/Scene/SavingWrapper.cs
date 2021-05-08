using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using RPG.Scene;
using RPG.Cinematics;
using UnityEngine;

public class SavingWrapper : MonoBehaviour
{
    const string defaultSaveFile = "save1";
    [SerializeField] float fadeInTime = 0.2f;

    private IEnumerator Start() {
        Fader fader = FindObjectOfType<Fader>();
        fader.FadeOutImmediate();
        yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
        yield return fader.FadeIn(fadeInTime);
        yield return new WaitForSeconds(0.5f);
        if (FindObjectOfType<CinematicsStartSequence>() != null)
        {
            FindObjectOfType<CinematicsStartSequence>().StartSequence();
        }
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Load();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Save();
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
}
