using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using System;

namespace RPG.Combat
{
    [RequireComponent(typeof(ActionScheduler))]
    [RequireComponent(typeof(Mover))]
    public class Fighter : MonoBehaviour, IAction
    {

        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;
        Weapon currentWeapon;

        Transform target;
        float timeSinceLastAttack = Mathf.Infinity;

        private void Start()
        {
            currentWeapon = defaultWeapon;
            EquipWeapon(currentWeapon);
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.GetComponent<Health>().IsDead()) return;

            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.position);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                if (timeSinceLastAttack >= currentWeapon.TimeBetweenAttacks)
                {
                    AttackBehavior();
                    timeSinceLastAttack = 0;
                }
                
            }
        }

        public void EquipWeapon(Weapon weapon)
        {
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        private void AttackBehavior()
        {
            this.transform.LookAt(target);
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(this.transform.position, target.transform.position) < currentWeapon.WeaponRange;
        }

        public void Attack(GameObject combatTarget)
        {
            target = combatTarget.transform;
            GetComponent<ActionScheduler>().StartAction(this);
        }

        public void Cancel()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
            target = null;
            GetComponent<Mover>().Cancel();
        }

        public bool CanAttack(GameObject target)
        {
            if (target == null) return false;
            if (target.GetComponent<Health>().IsDead()) return false;
            return true;
        }

        // Animation Events
        void Hit()
        {
            if (target == null) return;
            Health healthComponent = target.GetComponent<Health>();
            if (currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, healthComponent);
            }
            else
            {
                healthComponent.TakeDamage(currentWeapon.WeaponDamage);
            }
            
        }

        void Shoot()
        {
            Hit();
        }
    }

}