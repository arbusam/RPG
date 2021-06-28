using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities.Filters
{
    [CreateAssetMenu(fileName = "Tag Filter", menuName = "RPG/Abilities/Filters/Tag")]
    public class TagFilter : FilterStrategy
    {
        [SerializeField] string tagToFilter = "";

        public override IEnumerable<GameObject> Filter(IEnumerable<GameObject> objects)
        {
            foreach (GameObject gameObject in objects)
            {
                if (gameObject.CompareTag(tagToFilter)) yield return gameObject;
            }
        }
    }
}