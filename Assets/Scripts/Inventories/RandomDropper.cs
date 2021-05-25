using UnityEngine;
using GameDevTV.Inventories;
using UnityEngine.AI;

namespace RPG.Inventories
{
    public class RandomDropper: ItemDropper
    {
        [SerializeField] float scatterDistance = 1f;
        [SerializeField] InventoryItem[] dropLibrary;
        [SerializeField] int numberOfDrops = 2;

        const int ATTEMPTS = 30;

        public void RandomDrop()
        {
            for (int i = 0; i < numberOfDrops; i++)
            {
                var item = dropLibrary[Random.Range(0, dropLibrary.Length)];
                DropItem(item, 1);
            }
        }

        protected override Vector3 GetDropLocation()
        {
            for (int i = 0; i < ATTEMPTS; i++)
            {
                Vector3 randomPoint = transform.position + Random.insideUnitSphere * scatterDistance;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, 0.1f, NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }
            return base.GetDropLocation();
        }
    }
}