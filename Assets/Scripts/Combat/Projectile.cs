using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {

        Health target = null;
        [SerializeField] float speed = 1;
        float damage = 0;

        private void Start() {
            this.transform.LookAt(GetAimLocation());
        }

        void Update()
        {
            if (target == null) return;
            
            this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, float damage)
        {
            this.target = target;
            this.damage = damage;
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
                target.TakeDamage(damage);
                Destroy(this.gameObject);
            }
        }
    }
}
