using RPG.Resources;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {

        Health target = null;
        [SerializeField] float speed = 1;
        [SerializeField] bool isHoming = false;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float lifetimeSeconds = 10f;
        float damage = 0;

        GameObject instigator = null;

        private void Start() {
            Destroy(this.gameObject, lifetimeSeconds);
        }

        void Update()
        {
            if (target == null) return;
            
            if (isHoming && !target.IsDead()) this.transform.LookAt(GetAimLocation());
            this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;
            this.transform.LookAt(GetAimLocation());
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null) return target.transform.position;
            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        private Vector3 GetAimLocation(Transform enemy)
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null) return enemy.position;
            return enemy.position + Vector3.up * targetCapsule.height / 2;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.GetComponent<Health>() != null && other.gameObject != instigator)
            {
                if (other.GetComponent<Health>().IsDead()) return;
                if (hitEffect != null)
                {
                    GameObject newHitEffect = Instantiate(hitEffect, GetAimLocation(other.transform), this.transform.rotation);
                    newHitEffect.name = "Hit Effect";
                }
                other.GetComponent<Health>().TakeDamage(instigator, damage);
                Destroy(this.gameObject);
            }
        }
    }
}
