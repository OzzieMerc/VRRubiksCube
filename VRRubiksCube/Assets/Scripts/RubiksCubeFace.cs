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

    public delegate void OnGrabStart(RubiksCubeFace face);
    public event OnGrabStart onGrabStartEvent;

    public delegate void OnGrabEnd(RubiksCubeFace face);
    public event OnGrabEnd onGrabEndEvent;

    [SerializeField] RubiksCubeController cubeController;
    [SerializeField] BoxCollider area; // A tirgger volume overlapping all the cubes on one side of a Rubik's Cube.
    bool hasFocus; // True if this face is being touched by a controller.
    VRController controllerWithFocus;

    public BoxCollider Area { get => area; }
    public bool HasFocus { get => hasFocus; }
    public RubiksCubePiece[] Pieces { get => GetPiecesWithinArea(area); }

    void Start()
    {
        hasFocus = false;
        controllerWithFocus = null;

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
        if (!enabled)
            return;

        if (otherCollider.TryGetComponent<VRController>(out VRController controller))
        {
            controller.onGripPulled += FaceGripped;
            controller.onTriggerPulled += FaceGripped;

            if (onTouchStartEvent != null)
                onTouchStartEvent(this);
        }
    }

    void OnTriggerExit(Collider otherCollider)
    {
        if (!enabled)
            return;

        if (otherCollider.TryGetComponent<VRController>(out VRController controller))
        {
            controller.onGripPulled -= FaceGripped;
            controller.onTriggerPulled -= FaceGripped;

            if (onTouchEndEvent != null)
                onTouchEndEvent(this);
        }
    }

    void FaceGripped(VRController controller, bool gripping)
    {
        if (!enabled)
            return;

        if (gripping)
        {
            controllerWithFocus = controller;

            if (onGrabStartEvent != null)
                onGrabStartEvent(this);
        }
        else
        {
            controllerWithFocus = null;

            if (onGrabEndEvent != null)
                onGrabEndEvent(this);
        }
    }

    RubiksCubePiece[] GetPiecesWithinArea(BoxCollider area)
    {
        List<RubiksCubePiece> overlappingPieces = new List<RubiksCubePiece>();

        foreach (RubiksCubePiece piece in cubeController.Pieces)
        {
            if (area.bounds.Intersects(piece.Area.bounds))
                overlappingPieces.Add(piece);
        }

        return overlappingPieces.ToArray();
    }
}
