using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;

namespace RPG.Control
{
    [RequireComponent(typeof(Fighter))]
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;

        [SerializeField] float suspisionTime = 2f;
        [SerializeField] float waitTime = 1f;

        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;

        [SerializeField] bool wanderer = false;

        Vector3 guardLocation;
        float timeSinceLastSeenPlayer = Mathf.Infinity;
        float timeWaited = Mathf.Infinity;
        int waypointIndex = 0;

        private void Start() {
            guardLocation = this.transform.position;
        }

        private void Update() {
            if (GetComponent<Health>().IsDead()) return;

            GameObject player = GameObject.FindWithTag("Player");
            
            float distance = Vector3.Distance(this.transform.position, player.transform.position);
            if (distance <= chaseDistance)
            {
                timeSinceLastSeenPlayer = 0;
                GetComponent<Fighter>().Attack(player);
            }
            else if (timeSinceLastSeenPlayer <= suspisionTime)
            {
                GetComponent<ActionScheduler>().StopCurrentAction();
            }
            else
            {
                Patrol();
            }

            timeSinceLastSeenPlayer += Time.deltaTime;
            timeWaited += Time.deltaTime;
        }

        private void Patrol()
        {
            if (wanderer) return;

            Vector3 nextPosition = guardLocation;

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
            GetComponent<Mover>().StartMovingTo(nextPosition);
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