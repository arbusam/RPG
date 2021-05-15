using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Resources;
using RPG.Stats;
using System.Collections.Generic;
using GameDevTV.Utils;
using System;

namespace RPG.Combat
{
    [RequireComponent(typeof(ActionScheduler))]
    [RequireComponent(typeof(Mover))]
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {

        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;
        public bool rapidFire = false;
        LazyValue<Weapon> currentWeapon;

        private void Awake() {
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }

        private Weapon SetupDefaultWeapon()
        {
            AttachWeapon(defaultWeapon);
            return defaultWeapon;
        }

        private void Start() {
            currentWeapon.ForceInit();
        }

        public Weapon CurrentWeaapon
        {
            get
            {
                return currentWeapon.value;
            }
        }

        bool isReady = false;
        public bool IsReady
        {
            get
            {
                return isReady;
            }
        }

        Transform target;
        public Transform Target
        {
            get
            {
                return target;
            }
        }
        float timeSinceLastAttack = Mathf.Infinity;

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (timeSinceLastAttack >= currentWeapon.value.TimeBetweenAttacks || rapidFire)
            {
                isReady = true;
            }
            else
            {
                isReady = false;
            }

            if (Input.GetKey(KeyCode.R))
            {
                rapidFire = true;
            }
            else
            {
                rapidFire = false;
            }

            if (target == null) return;
            if (target.GetComponent<Health>().IsDead()) return;

            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.position);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                if (timeSinceLastAttack >= currentWeapon.value.TimeBetweenAttacks || rapidFire)
                {
                    AttackBehavior();
                    timeSinceLastAttack = 0;
                }
                
            }
        }

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon.value = weapon;
            AttachWeapon(weapon);
        }

        private void AttachWeapon(Weapon weapon)
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
            return Vector3.Distance(this.transform.position, target.transform.position) < currentWeapon.value.WeaponRange;
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
            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            if (currentWeapon.value.HasProjectile())
            {
                currentWeapon.value.LaunchProjectile(rightHandTransform, leftHandTransform, healthComponent, gameObject, damage);
            }
            else
            {
                healthComponent.TakeDamage(this.gameObject, damage);
            }
            
        }

        void Shoot()
        {
            Hit();
        }

        public object CaptureState()
        {
            return currentWeapon.value.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon = UnityEngine.Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeapon.value.WeaponDamage;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            yield return 0;
        }
    }

}