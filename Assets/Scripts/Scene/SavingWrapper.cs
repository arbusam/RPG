using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

public class SavingWrapper : MonoBehaviour
{
    const string defaultSaveFile = "save1";

    // Update is called once per frame
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

    private void Save()
    {
        GetComponent<SavingSystem>().Save(defaultSaveFile);
    }

    private void Load()
    {
        GetComponent<SavingSystem>().Load(defaultSaveFile);
    }
}
