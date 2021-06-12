using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using GameDevTV.Saving;
using RPG.Attributes;

namespace RPG.Movement
{
    [RequireComponent(typeof(ActionScheduler))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] Transform target;
        [SerializeField] float maxSpeed = 7f;

        NavMeshAgent navMeshAgent;

        private void Awake() {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            navMeshAgent.enabled = !GetComponent<Health>().IsDead();

            if (Debug.isDebugBuild && this.tag == "Player")
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    navMeshAgent.speed = 20f;
                    
                }
            }

            UpdateAnimator();
        }

        public void StartMovingTo(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            navMeshAgent.enabled = false;
            SerializableVector3 pos = (SerializableVector3)state;
            transform.position = pos.ToVector();
            navMeshAgent.enabled = true;
        }
    }

}