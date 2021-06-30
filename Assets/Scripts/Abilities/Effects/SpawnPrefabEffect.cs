using System;
using System.Collections;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Abilities.Filters
{
    [CreateAssetMenu(fileName = "Spawn Prefab Effect", menuName = "RPG/Abilities/Effects/Spawn Prefab")]
    public class SpawnPrefabEffect : EffectStrategy
    {
        [SerializeField] Transform prefab;
        [SerializeField] float destroyDelay = -1;

        public override void StartEffect(AbilityData data, Action finished)
        {
            data.StartCoroutine(Effect(data, finished));
        }

        private IEnumerator Effect(AbilityData data, Action finished)
        {
            Transform instance = Instantiate(prefab);
            instance.position = data.TargetedPoint;
            if (destroyDelay > 0)
            {
                yield return new WaitForSeconds(destroyDelay);
                Destroy(instance.gameObject);
            }
        }
    }
}