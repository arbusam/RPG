using UnityEngine;
using RPG.Movement;
using RPG.Core;
using GameDevTV.Saving;
using RPG.Attributes;
using RPG.Stats;
using System.Collections.Generic;
using GameDevTV.Utils;
using GameDevTV.Inventories;
using System;

namespace RPG.Combat
{
    [RequireComponent(typeof(ActionScheduler))]
    [RequireComponent(typeof(Mover))]
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {

        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] WeaponConfig defaultWeapon = null;
        public bool rapidFire = false;
        WeaponConfig currentWeaponConfig;
        LazyValue<Weapon> currentWeapon;

        Equipment equipment;

        private void Awake() {
            currentWeaponConfig = defaultWeapon;
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
            equipment = GetComponent<Equipment>();
            if (equipment)
            {
                equipment.equipmentUpdated += UpdateWeapon;
            }
        }

        private void UpdateWeapon()
        {
            var weapon = equipment.GetItemInSlot(EquipLocation.Weapon) as WeaponConfig;
            if (weapon == null)
            {
                EquipWeapon(defaultWeapon);
            }
            else
            {
                EquipWeapon(weapon);
            }
        }

        private Weapon SetupDefaultWeapon()
        {
            return AttachWeapon(defaultWeapon);
        }

        private void Start() {
            currentWeapon.ForceInit();
        }

        public WeaponConfig CurrentWeapon
        {
            get
            {
                return currentWeaponConfig;
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

            if (timeSinceLastAttack >= currentWeaponConfig.TimeBetweenAttacks || rapidFire)
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
            if (target.GetComponent<Health>().IsDead) return;
            if (!GetComponent<Mover>().canMove) return;

            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.position, 1f);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                if (timeSinceLastAttack >= currentWeaponConfig.TimeBetweenAttacks || rapidFire)
                {
                    AttackBehavior();
                    timeSinceLastAttack = 0;
                }
                
            }
        }

        public void EquipWeapon(WeaponConfig weapon)
        {
            currentWeaponConfig = weapon;
            currentWeapon.value = AttachWeapon(weapon);
        }

        private Weapon AttachWeapon(WeaponConfig weapon)
        {
            Animator animator = GetComponent<Animator>();
            return weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        public Transform GetHandTransform(bool isRightHand)
        {
            if (isRightHand) return rightHandTransform;
            else return leftHandTransform;
        }

        private void AttackBehavior()
        {
            this.transform.LookAt(target);
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(this.transform.position, target.transform.position) < currentWeaponConfig.WeaponRange;
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
            if (target.GetComponent<Health>().IsDead) return false;
            return true;
        }

        // Animation Events
        void Hit()
        {

            if (currentWeapon.value != null)
            {
                currentWeapon.value.OnHit();
            }

            if (target == null) return;
            Health healthComponent = target.GetComponent<Health>();
            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage) - target.GetComponent<BaseStats>().GetStat(Stat.Protection);
            if (damage < 0)
            {
                damage = 0;
            }
            if (currentWeaponConfig.HasProjectile())
            {
                currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, healthComponent, gameObject, damage);
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
            return currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            WeaponConfig weapon = UnityEngine.Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weapon);
        }
    }

}