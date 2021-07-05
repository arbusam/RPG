using System;
using RPG.Attributes;
using RPG.Combat;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Spawn Projectile Effect", menuName = "RPG/Abilities/Effects/Spawn Projectile")]
    public class SpawnProjectileEffect : EffectStrategy
    {
        [SerializeField] Projectile projectileToSpawn;
        [SerializeField] float damage;
        [SerializeField] bool isRightHand = true;

        public override void StartEffect(AbilityData data, Action finished)
        {
            Fighter fighter = data.User.GetComponent<Fighter>();
            Vector3 spawnPosition = fighter.GetHandTransform(isRightHand).position;
            foreach (GameObject target in data.Targets)
            {
                Health health = target.GetComponent<Health>();
                if (health != null)
                {
                    Projectile projectile = Instantiate(projectileToSpawn);
                    projectile.transform.position = spawnPosition;
                    projectile.SetTarget(health, data.User, damage);
                }
            }
            finished();
        }
    }
}