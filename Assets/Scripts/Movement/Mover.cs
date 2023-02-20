using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour {
    // Start is called before the first frame update
    public Camera camera;
    public GameObject targetDest;
    public NavMeshAgent player;


    // Update is called once per frame
    void Update() {


        if (Input.GetMouseButtonDown(0)) {
            MoveToCursor();
        }
        updateAnimator();

    }

    private void MoveToCursor() {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit);
        if (hasHit) {
            targetDest.transform.position = hit.point;
            player.SetDestination(hit.point);
        }


    }
    private void updateAnimator() {
        Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;
        GetComponent<Animator>().SetFloat("ForwardSpeed", speed);
    }
}
