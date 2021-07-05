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
        [SerializeField] bool useTargetPoint = true;
        [SerializeField] string tagToTarget = "";

        public override void StartEffect(AbilityData data, Action finished)
        {
            Fighter fighter = data.User.GetComponent<Fighter>();
            Vector3 spawnPosition = fighter.GetHandTransform(isRightHand).position;
            if (useTargetPoint)
            {
                SpawnProjectileForTargetPoint(data, spawnPosition);
            }
            else
            {
                SpawnProjectilesForTargets(data, spawnPosition);   
            }
            finished();
        }

        private void SpawnProjectileForTargetPoint(AbilityData data, Vector3 spawnPosition)
        {
            Projectile projectile = Instantiate(projectileToSpawn);
            projectile.transform.position = spawnPosition;
            if (tagToTarget != "")
            {
                projectile.SetTarget(data.TargetedPoint, data.User, damage, tagToTarget);
            }
            else
            {
                projectile.SetTarget(data.TargetedPoint, data.User, damage);
            }
        }

        private void SpawnProjectilesForTargets(AbilityData data, Vector3 spawnPosition)
        {
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
        }
    }
}