using System;
using System.Collections;
using RPG.Attributes;
using RPG.Scene;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class Respawner : MonoBehaviour
    {
        [SerializeField] Transform respawnLocation;

        [SerializeField] float healthPercentage = 80;

        [SerializeField] float fadeBeforeWaitTime = 2;
        [SerializeField] float fadeOutTime = 0.5f;
        [SerializeField] float fadeWaitTime = 1;
        [SerializeField] float fadeInTime = 0.2f;

        private void Awake()
        {
            GetComponent<Health>().onDie.AddListener(() => StartCoroutine(Respawn()));
        }

        private IEnumerator Respawn()
        {
            yield return new WaitForSeconds(fadeBeforeWaitTime);
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);
            GetComponent<NavMeshAgent>().Warp(respawnLocation.position);
            Health health = GetComponent<Health>();
            health.Heal(health.MaxHealthPoints * (healthPercentage / 100));
            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);
            health.IsDead = false;
            GetComponent<Animator>().SetTrigger("revive");
        }
    }
}