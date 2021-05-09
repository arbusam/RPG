using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectRemover : MonoBehaviour
{
    new ParticleSystem particleSystem;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }


    private void Update() {
        if (!particleSystem.IsAlive())
        {
            Destroy(this.gameObject);
        }
    }
}
