using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Movement {
    public class Follower : MonoBehaviour {

        public Transform player;
        public NavMeshAgent nav;


        public Transform playerChar1;
        public Transform playerChar2;
        public Transform playerChar3;
        public Transform playerChar4;


        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            transform.LookAt(player.transform);
            nav.SetDestination(player.position);
        }
    }
}



