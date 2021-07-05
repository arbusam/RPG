using RPG.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 1;
        [SerializeField] bool isHoming = false;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float lifetimeSeconds = 10f;
        [SerializeField] float lifeAfterImpact = 1f;
        [SerializeField] GameObject[] destroyOnHit = null;

        [SerializeField] UnityEvent hit;

        Health target = null;

        float damage = 0;

        GameObject instigator = null;
        Vector3 targetPoint;
        string tagToTarget;

        private void Start() {
            Destroy(this.gameObject, lifetimeSeconds);
        }

        void Update()
        {
            if (target != null && isHoming && !target.IsDead()) this.transform.LookAt(GetAimLocation());
            this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            SetTarget(instigator, damage, target);
        }

        public void SetTarget(Vector3 targetPoint, GameObject instigator, float damage, string tagToTarget=null)
        {
            SetTarget(instigator, damage, null, targetPoint, tagToTarget);
        }

        public void SetTarget(GameObject instigator, float damage, Health target=null, Vector3 targetPoint=default, string tagToTarget=null)
        {
            this.target = target;
            this.targetPoint = targetPoint;
            this.damage = damage;
            this.instigator = instigator;
            this.tagToTarget = tagToTarget;
            this.transform.LookAt(GetAimLocation());
        }

        private Vector3 GetAimLocation()
        {
            if (target == null) return targetPoint;
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
            Health health = other.GetComponent<Health>();
            if (target != null && health != target) return;
            if (health == null || health.IsDead()) return;
            if (other.gameObject == instigator) return;
            if (tagToTarget != null && other.tag != tagToTarget) return;

            health.TakeDamage(instigator, damage);

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
