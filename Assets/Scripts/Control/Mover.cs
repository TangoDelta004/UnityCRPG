using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Control {
    public class Mover : MonoBehaviour {
        public GameObject targetDestPrefab; 
        private GameObject targetDest;
        public NavMeshAgent player;
        private bool isMoving = false;

        private void Start() {
            targetDest = Instantiate(targetDestPrefab);
            targetDest.SetActive(false); // Hide the target destination marker initially
        }

        // Update is called once per frame
        void Update() {
            updateAnimator();

            if (isMoving && !player.pathPending && player.remainingDistance <= player.stoppingDistance && !player.hasPath) {
                targetDest.SetActive(false); // Hide the target destination marker when player reaches the destination
                isMoving = false; // Reset moving state
            }
        }

        public void MoveTo(Vector3 destination) {
            if (targetDest == null) {
                targetDest = Instantiate(targetDestPrefab);
            }
            targetDest.transform.position = destination;
            targetDest.SetActive(true); // Show the target destination marker
            player.SetDestination(destination);
            isMoving = true; // Set moving state
        }

        private void updateAnimator() {
            Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("ForwardSpeed", speed);
        }

        public void SetSpeed(float speed) {
            player.speed = speed;
        }
    }
}