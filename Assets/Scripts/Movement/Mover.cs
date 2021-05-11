using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using RPG.Resources;

namespace RPG.Movement
{
    [RequireComponent(typeof(ActionScheduler))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] Transform target;

        NavMeshAgent navMeshAgent;

        private void Start() {
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
                else
                {
                    navMeshAgent.speed = 5.66f;
                }
            }

            UpdateAnimator();
        }

        public void StartMovingTo(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)
        {
            navMeshAgent.destination = destination;
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
            GetComponent<NavMeshAgent>().enabled = false;
            SerializableVector3 pos = (SerializableVector3)state;
            transform.position = pos.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
        }
    }

}