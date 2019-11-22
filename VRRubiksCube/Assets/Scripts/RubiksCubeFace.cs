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
    VRController controllerWithFocus;

    public BoxCollider Area { get => area; }
    public RubiksCubePiece[] Pieces { get => FindFacePieces(); }
    bool m_Started = false;

    void Start()
    {
        controllerWithFocus = null;

        if (!area)
        {
            Debug.LogWarning("RubiksCubeFace area not assigned. Attempting to find a compatible BoxCollider");
            area = GetComponent<BoxCollider>();

            if (!area)
                Debug.LogError("Unable to find a compatible BoxCollider for RubiksCubeFace area");
        }
        m_Started = true;
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        if (!enabled || HasFocus())
            return;

        if (otherCollider.TryGetComponent<VRController>(out VRController controller) && !controller.HasFocus())
        {
            SetFocus(controller);

            controller.onGripPulled += FaceGripped;
            controller.onTriggerPulled += FaceGripped;

            if (onTouchStartEvent != null)
                onTouchStartEvent(this);
        }
    }

    void OnTriggerExit(Collider otherCollider)
    {
        if (!enabled || !HasFocus())
            return;

        if (otherCollider.TryGetComponent<VRController>(out VRController controller) && controller.GetFocus() == transform)
        {
            ClearFocus();

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
            if (onGrabStartEvent != null)
                onGrabStartEvent(this);
        }
        else
        {
            if (onGrabEndEvent != null)
                onGrabEndEvent(this);
        }
    }

    RubiksCubePiece[] FindFacePieces()
    {
        List<RubiksCubePiece> overlappingPieces = new List<RubiksCubePiece>();
        Vector3 faceNormal = GetNormal();

        foreach (RubiksCubePiece piece in cubeController.Pieces)
            if (Vector3.Dot(faceNormal, piece.GetNormal()) > 0.4f)
                overlappingPieces.Add(piece);

        print(overlappingPieces.Count);
        return overlappingPieces.ToArray();
    }

    public Vector3 GetNormal()
    {
        return transform.forward;
    }

    bool HasFocus()
    {
        return controllerWithFocus != null;
    }

    VRController GetFocus()
    {
        return controllerWithFocus;
    }

    void SetFocus(VRController controller)
    {
        controllerWithFocus = controller;
        controllerWithFocus.SetFocus(transform);
    }

    void ClearFocus()
    {
        controllerWithFocus.SetFocus(null);
        controllerWithFocus = null;
    }
}
