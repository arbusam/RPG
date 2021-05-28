using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OppositeOtherVisibility : MonoBehaviour
{
    [SerializeField] GameObject[] toHide;
    [SerializeField] GameObject toMirror;

    void Update()
    {
        foreach (GameObject hide in toHide)
        {
            hide.SetActive(!toMirror.activeInHierarchy);
        }
        
    }
}
