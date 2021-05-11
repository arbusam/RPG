using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Resources;

namespace RPG.Combat
{
    [RequireComponent(typeof(ActionScheduler))]
    [RequireComponent(typeof(Mover))]
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {

        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;
        public bool rapidFire = false;
        Weapon currentWeapon;

        public Weapon CurrentWeaapon
        {
            get
            {
                return currentWeapon;
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

        private void Start()
        {
            if (currentWeapon == null)
            {
                EquipWeapon(defaultWeapon);
            }
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (timeSinceLastAttack >= currentWeapon.TimeBetweenAttacks || rapidFire)
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
                if (timeSinceLastAttack >= currentWeapon.TimeBetweenAttacks || rapidFire)
                {
                    AttackBehavior();
                    timeSinceLastAttack = 0;
                }
                
            }
        }

        public void EquipWeapon(Weapon weapon)
        {
            if (weapon == null) return;
            currentWeapon = weapon;
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
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, healthComponent, gameObject);
            }
            else
            {
                healthComponent.TakeDamage(this.gameObject, currentWeapon.WeaponDamage);
            }
            
        }

        void Shoot()
        {
            Hit();
        }

        public object CaptureState()
        {
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon = UnityEngine.Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
    }

}