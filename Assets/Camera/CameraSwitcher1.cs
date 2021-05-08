using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraSwitcher1 : MonoBehaviour
{
    Animator animator;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player")
        {
            this.GetComponent<CinemachineStateDrivenCamera>().Priority = 10;
            animator.Play("90 Camera");
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player")
        {
            this.GetComponent<CinemachineStateDrivenCamera>().Priority = 0;
            animator.Play("Normal Camera");
        }
    }
}
