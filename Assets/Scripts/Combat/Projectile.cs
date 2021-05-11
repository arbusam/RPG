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
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] float lifeAfterImpact = 0f;
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

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject == target.gameObject)
            {
                if (target.GetComponent<Health>().IsDead()) return;
                if (hitEffect != null)
                {
                    GameObject newHitEffect = Instantiate(hitEffect, GetAimLocation(), this.transform.rotation);
                    newHitEffect.name = "Hit Effect";
                }
                target.TakeDamage(instigator, damage);
                foreach (GameObject toDestroy in destroyOnHit)
                {
                    Destroy(toDestroy);
                }
                Destroy(this.gameObject, lifeAfterImpact);
            }
        }
    }
}
