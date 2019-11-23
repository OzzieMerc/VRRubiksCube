using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(TwistInteraction))]
public class RubiksCubeFace : MonoBehaviour
{
    [SerializeField] RubiksCubeController cubeController;
    [SerializeField] TwistInteraction twister;
    [SerializeField] BoxCollider area; // A tirgger volume overlapping all the cubes on one side of a Rubik's Cube.
    VRController controllerWithFocus;

    public BoxCollider Area { get => area; }
    public RubiksCubePiece[] Pieces { get => FindFacePieces(); }
    bool started = false;

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

        if (!twister)
        {
            Debug.LogWarning("RubiksCubeFace twister not assigned. Attempting to find a compatible TwistInteraction");
            twister = GetComponent<TwistInteraction>();

            if (!twister)
                Debug.LogError("Unable to find a compatible TwistInteraction for RubiksCubeFace twister");
        }

        started = true;

        if (twister)
        {
            twister.onPreGrabStartEvent += (VRController controller) => cubeController.RequestPieces(Pieces, this);
            twister.onPostGrabEndEvent += (VRController controller) => cubeController.ReturnPieces(Pieces, this);
        }
    }

    RubiksCubePiece[] FindFacePieces()
    {
        List<RubiksCubePiece> overlappingPieces = new List<RubiksCubePiece>();
        Vector3 faceNormal = GetNormal();

        foreach (RubiksCubePiece piece in cubeController.Pieces)
            if (Vector3.Dot(faceNormal, piece.GetNormal()) > 0.4f)
                overlappingPieces.Add(piece);

        return overlappingPieces.ToArray();
    }

    public Vector3 GetNormal()
    {
        return transform.forward;
    }
}
