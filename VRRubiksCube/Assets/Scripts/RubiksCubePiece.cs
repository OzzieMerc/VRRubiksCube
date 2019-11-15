using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RubiksCubePiece : MonoBehaviour
{
    [SerializeField] BoxCollider area; // A tirgger volume covering the piece
    [SerializeField] MeshRenderer blockRenderer; // The block's renderer to use for swapping materials

    public BoxCollider Area { get => area; }
    public MeshRenderer BlockRenderer { get => blockRenderer; }

    void Start()
    {
        if (!area)
        {
            Debug.LogWarning("RubiksCubePiece area not assigned. Attempting to find a compatible BoxCollider");
            area = GetComponent<BoxCollider>();

            if (!area)
                Debug.LogError("Unable to find a compatible BoxCollider for RubiksCubePiece area");
        }

        Debug.Assert(blockRenderer, "RubiksCubePiece blockRenderer not assigned");
    }
}
