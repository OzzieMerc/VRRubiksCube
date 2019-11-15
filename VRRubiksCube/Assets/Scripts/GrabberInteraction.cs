using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RubiksCubeController))]
public class GrabberInteraction : MonoBehaviour
{
    [SerializeField] RubiksCubeController cubeController; // Reference to the Rubik's Cube Controller the grabber with toggle.

    // Start is called before the first frame update
    void Start()
    {
        if (!cubeController)
        {
            Debug.LogWarning("GrabberInteraction cubeController not assigned. Attempting to find a compatible RubiksCubeController");
            cubeController = GetComponent<RubiksCubeController>();

            if (!cubeController)
                Debug.LogError("Unable to find a compatible RubiksCubeController for GrabberInteraction cubeController");
        }
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        //if (otherCollider.GetComponent<Interactor>())
        //cubeController.enabled = true;
    }

    void OnTriggerExit(Collider otherCollider)
    {
        //cubeController.enabled = false;
    }
}
