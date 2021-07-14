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

        [SerializeField] float healthRegenPercentage = 80;

        [SerializeField] float enemyHealthRegen = 20;

        [SerializeField] float fadeBeforeWaitTime = 2;
        [SerializeField] float fadeOutTime = 0.5f;
        [SerializeField] float fadeWaitTime = 1;
        [SerializeField] float fadeInTime = 0.2f;

        private void Awake()
        {
            GetComponent<Health>().onDie.AddListener(() => StartCoroutine(Respawn()));
        }

        private void Start()
        {
            if (GetComponent<Health>().IsDead) StartCoroutine(Respawn());
        }

        private IEnumerator Respawn()
        {
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.Save();
            Health health = GetComponent<Health>();
            yield return new WaitForSeconds(fadeBeforeWaitTime);
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);
            RespawnPlayer(health);
            ResetEnemies();
            savingWrapper.Save();
            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);
            health.IsDead = false;
            GetComponent<Animator>().SetTrigger("revive");
        }

        private void ResetEnemies()
        {
            foreach (AIController enemyController in FindObjectsOfType<AIController>())
            {
                enemyController.Reset();
                Health health = enemyController.GetComponent<Health>();
                if (!health.IsDead)
                {
                    health.Heal(enemyHealthRegen);
                }
            }
        }

        private void RespawnPlayer(Health health)
        {
            GetComponent<NavMeshAgent>().Warp(respawnLocation.position);
            health.Heal(health.MaxHealthPoints * (healthRegenPercentage / 100));
        }
    }
}