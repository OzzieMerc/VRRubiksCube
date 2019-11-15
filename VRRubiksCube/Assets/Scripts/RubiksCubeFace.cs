using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RubiksCubeFace : MonoBehaviour
{
    public delegate void OnTouchStart(RubiksCubeFace face);
    public event OnTouchStart onTouchStartEvent;

    public delegate void OnTouchEnd(RubiksCubeFace face);
    public event OnTouchEnd onTouchEndEvent;

    [SerializeField] BoxCollider area; // A tirgger volume overlapping all the cubes on one side of a Rubik's Cube.

    public BoxCollider Area { get => area; }

    void Start()
    {
        if (!area)
        {
            Debug.LogWarning("RubiksCubeFace area not assigned. Attempting to find a compatible BoxCollider");
            area = GetComponent<BoxCollider>();

            if (!area)
                Debug.LogError("Unable to find a compatible BoxCollider for RubiksCubeFace area");
        }
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        if (onTouchStartEvent != null)
            onTouchStartEvent(this);
    }

    void OnTriggerExit(Collider otherCollider)
    {
        if (onTouchEndEvent != null)
            onTouchEndEvent(this);
    }
}
