using RPG.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {

        Health target = null;
        [SerializeField] float speed = 1;
        [SerializeField] bool isHoming = false;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float lifetimeSeconds = 10f;
        [SerializeField] float lifeAfterImpact = 1f;
        [SerializeField] GameObject[] destroyOnHit = null;

        [SerializeField] UnityEvent hit;

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

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) return;
            if (target.IsDead()) return;
            target.TakeDamage(instigator, damage);

            speed = 0;

            hit.Invoke();

            if (hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }
            foreach(GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }
            Destroy(gameObject, lifeAfterImpact);
        }
    }
}
