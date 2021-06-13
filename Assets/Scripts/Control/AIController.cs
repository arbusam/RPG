using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;
using GameDevTV.Utils;
using System;

namespace RPG.Control
{
    [RequireComponent(typeof(Fighter))]
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;

        [SerializeField] float suspisionTime = 2f;
        [SerializeField] float agroCooldownTime = 5f;
        [SerializeField] float waitTime = 1f;

        [SerializeField] float shoutDistance = 5f;

        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;

        [SerializeField] bool wanderer = false;

        Fighter fighter;
        GameObject player;

        bool hasShout = true;

        LazyValue<Vector3> guardPosition;
        float timeSinceLastSeenPlayer = Mathf.Infinity;
        float timeWaited = Mathf.Infinity;
        float timeSinceAggrevated = Mathf.Infinity;
        int waypointIndex = 0;

        private void Awake() {
            guardPosition = new LazyValue<Vector3>(GetGuardPosition);
            fighter = GetComponent<Fighter>();
            player = GameObject.FindWithTag("Player");
        }

        private Vector3 GetGuardPosition()
        {
            return this.transform.position;
        }

        private void Start() {
            guardPosition.ForceInit();
        }

        private void Update()
        {
            if (GetComponent<Health>().IsDead()) return;

            if (IsAggrevated() && fighter.CanAttack(player))
            {
                AttackBehavior(player);
            }
            else if (timeSinceLastSeenPlayer <= suspisionTime)
            {
                hasShout = true;
                SuspicionBehavior();
            }
            else
            {
                hasShout = true;
                PatrolBehavior();
            }

            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceLastSeenPlayer += Time.deltaTime;
            timeWaited += Time.deltaTime;
            timeSinceAggrevated += Time.deltaTime;
        }

        private bool IsAggrevated()
        {
            float distance = Vector3.Distance(this.transform.position, player.transform.position);
            return distance <= chaseDistance || timeSinceAggrevated < agroCooldownTime;
        }

        private void AttackBehavior(GameObject player)
        {
            timeSinceLastSeenPlayer = 0;
            fighter.Attack(player);

            if (hasShout)
            {
                AggrevateNearbyEnemies();
            }
        }

        public void AggrevateNearbyEnemies()
        {
            hasShout = false;
            RaycastHit[] hits = Physics.SphereCastAll(this.transform.position, shoutDistance, Vector3.up, 0);
            foreach (RaycastHit hit in hits)
            {
                AIController enemyComponent = hit.transform.GetComponent<AIController>();
                if (enemyComponent == null) continue;
                enemyComponent.Aggrevate();
            }
        }

        private void SuspicionBehavior()
        {
            GetComponent<ActionScheduler>().StopCurrentAction();
        }

        private void PatrolBehavior()
        {
            if (wanderer) return;

            Vector3 nextPosition = guardPosition.value;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    CycleWaypoint();
                }
                else
                {
                    timeWaited = 0;
                }
                nextPosition = GetCurrentWaypoint();
            }
            GetComponent<Mover>().StartMovingTo(nextPosition, 1f);
        }
        
        public void Aggrevate()
        {
            timeSinceAggrevated = 0;
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(this.transform.position, GetCurrentWaypoint());

            return distanceToWaypoint <= waypointTolerance;
        }

        private void CycleWaypoint()
        {
            if (timeWaited >= waitTime)
            {
                waypointIndex = patrolPath.GetNextIndex(waypointIndex);
            }
            
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(waypointIndex);
        }

        // Called by Unity when drawing Gizmos
        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(this.transform.position, chaseDistance);
        }
    }
}