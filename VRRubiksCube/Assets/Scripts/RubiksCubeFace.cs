using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RubiksCubeFace : MonoBehaviour
{
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
}
