using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Movement {
    public class Mover : MonoBehaviour {
        // Start is called before the first frame update
        public GameObject targetDest;
        public NavMeshAgent player;


        // Update is called once per frame
        void Update() {
            updateAnimator();
        }

        public void MoveTo(Vector3 destination) {
            targetDest.transform.position = destination;
            player.SetDestination(destination);

        }

        private void updateAnimator() {
            Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("ForwardSpeed", speed);
        }
    }
}

