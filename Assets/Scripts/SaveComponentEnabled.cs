using System.Collections;
using System.Collections.Generic;
using GameDevTV.Saving;
using UnityEngine;

public class SaveComponentEnabled : MonoBehaviour, ISaveable
{
    [SerializeField] MonoBehaviour componentToSave;

    public object CaptureState()
    {
        return componentToSave.enabled;
    }

    public void RestoreState(object state)
    {
        componentToSave.enabled = (bool)state;
    }
}
