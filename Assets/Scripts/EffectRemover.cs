using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectRemover : MonoBehaviour
{
    [SerializeField] GameObject targetToDestroy = null;
    new ParticleSystem particleSystem;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }


    private void Update() {
        if (!particleSystem.IsAlive())
        {
            if (targetToDestroy != null)
            {
                Destroy(targetToDestroy);
            }
            else
            {
                Destroy(this.gameObject);
            }
            
        }
    }
}
