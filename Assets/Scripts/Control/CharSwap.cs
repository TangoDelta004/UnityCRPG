using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Movement;

namespace Control {
    public class CharSwap : MonoBehaviour {


        public Transform playerChar1;
        public Transform playerChar2;
        public Transform playerChar3;
        public Transform playerChar4;

        public GameObject cameraRig;

        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
        void OnGUI() {
            ChangeFollowers();
        }
        public void ChangeFollowers() {

            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                SwapToPlayer1();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                SwapToPlayer2();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                SwapToPlayer3();
            }
            if (Input.GetKeyDown(KeyCode.Alpha4)) {
                SwapToPlayer4();
            }

        }
        public void SwapToPlayer1() {


            playerChar1.GetComponent<PlayerController>().enabled = true;
            playerChar2.GetComponent<PlayerController>().enabled = false;
            playerChar3.GetComponent<PlayerController>().enabled = false;
            playerChar4.GetComponent<PlayerController>().enabled = false;

            playerChar1.GetComponent<Follower>().enabled = false;
            playerChar2.GetComponent<Follower>().enabled = true;
            playerChar3.GetComponent<Follower>().enabled = true;
            playerChar4.GetComponent<Follower>().enabled = true;

            playerChar1.GetComponent<NavMeshAgent>().stoppingDistance = 0;
            playerChar2.GetComponent<NavMeshAgent>().stoppingDistance = 2;
            playerChar3.GetComponent<NavMeshAgent>().stoppingDistance = 2;
            playerChar4.GetComponent<NavMeshAgent>().stoppingDistance = 2;

            playerChar1.GetComponent<Follower>().player = playerChar1;
            playerChar2.GetComponent<Follower>().player = playerChar1;
            playerChar3.GetComponent<Follower>().player = playerChar1;
            playerChar4.GetComponent<Follower>().player = playerChar1;

            cameraRig.GetComponent<CameraController>().currentPlayer = playerChar1;
        }
        public void SwapToPlayer2() {
            playerChar2.GetComponent<PlayerController>().enabled = true;
            playerChar1.GetComponent<PlayerController>().enabled = false;
            playerChar3.GetComponent<PlayerController>().enabled = false;
            playerChar4.GetComponent<PlayerController>().enabled = false;

            playerChar2.GetComponent<Follower>().enabled = false;
            playerChar1.GetComponent<Follower>().enabled = true;
            playerChar3.GetComponent<Follower>().enabled = true;
            playerChar4.GetComponent<Follower>().enabled = true;

            playerChar2.GetComponent<NavMeshAgent>().stoppingDistance = 0;
            playerChar1.GetComponent<NavMeshAgent>().stoppingDistance = 2;
            playerChar3.GetComponent<NavMeshAgent>().stoppingDistance = 2;
            playerChar4.GetComponent<NavMeshAgent>().stoppingDistance = 2;

            playerChar1.GetComponent<Follower>().player = playerChar2;
            playerChar2.GetComponent<Follower>().player = playerChar2;
            playerChar3.GetComponent<Follower>().player = playerChar2;
            playerChar4.GetComponent<Follower>().player = playerChar2;

            cameraRig.GetComponent<CameraController>().currentPlayer = playerChar2;
        }
        public void SwapToPlayer3() {
            playerChar3.GetComponent<PlayerController>().enabled = true;
            playerChar1.GetComponent<PlayerController>().enabled = false;
            playerChar2.GetComponent<PlayerController>().enabled = false;
            playerChar4.GetComponent<PlayerController>().enabled = false;

            playerChar3.GetComponent<Follower>().enabled = false;
            playerChar1.GetComponent<Follower>().enabled = true;
            playerChar2.GetComponent<Follower>().enabled = true;
            playerChar4.GetComponent<Follower>().enabled = true;

            playerChar3.GetComponent<NavMeshAgent>().stoppingDistance = 0;
            playerChar1.GetComponent<NavMeshAgent>().stoppingDistance = 2;
            playerChar2.GetComponent<NavMeshAgent>().stoppingDistance = 2;
            playerChar4.GetComponent<NavMeshAgent>().stoppingDistance = 2;

            playerChar1.GetComponent<Follower>().player = playerChar3;
            playerChar2.GetComponent<Follower>().player = playerChar3;
            playerChar3.GetComponent<Follower>().player = playerChar3;
            playerChar4.GetComponent<Follower>().player = playerChar3;

            cameraRig.GetComponent<CameraController>().currentPlayer = playerChar3;
        }
        public void SwapToPlayer4() {
            playerChar4.GetComponent<PlayerController>().enabled = true;
            playerChar1.GetComponent<PlayerController>().enabled = false;
            playerChar2.GetComponent<PlayerController>().enabled = false;
            playerChar3.GetComponent<PlayerController>().enabled = false;

            playerChar4.GetComponent<Follower>().enabled = false;
            playerChar1.GetComponent<Follower>().enabled = true;
            playerChar2.GetComponent<Follower>().enabled = true;
            playerChar3.GetComponent<Follower>().enabled = true;

            playerChar4.GetComponent<NavMeshAgent>().stoppingDistance = 0;
            playerChar1.GetComponent<NavMeshAgent>().stoppingDistance = 2;
            playerChar2.GetComponent<NavMeshAgent>().stoppingDistance = 2;
            playerChar3.GetComponent<NavMeshAgent>().stoppingDistance = 2;

            playerChar1.GetComponent<Follower>().player = playerChar4;
            playerChar2.GetComponent<Follower>().player = playerChar4;
            playerChar3.GetComponent<Follower>().player = playerChar4;
            playerChar4.GetComponent<Follower>().player = playerChar4;

            cameraRig.GetComponent<CameraController>().currentPlayer = playerChar4;
        }
    }
}
