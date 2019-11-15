using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubiksCubeController : MonoBehaviour
{
    [SerializeField] RubiksCubeFace[] faces; // Each face of the cube that can be rotated
    [SerializeField] RubiksCubePiece[] pieces; // Each piece that makes up the cube
    [SerializeField] Material normalMat; // Normal material of pieces when they are not selected
    [SerializeField] Material highlightMat; // Material to highlight pieces when they are selected
    RubiksCubeFace selectedFace; // Currently selected face;
    RubiksCubePiece[] selectedPieces; // Currently selected pieces

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(faces.Length > 0, "RubiksCubeController does not have any faces to work with.");

        // Set up the event listeners for each face
        foreach (RubiksCubeFace face in faces)
        {
            face.onTouchStartEvent += FaceTouchStart;
            face.onTouchEndEvent += FaceTouchEnd;
        }

        if (pieces.Length == 0)
        {
            Debug.LogWarning("RubiksCubeController pieces is empty. Attempting to find all components of type RubiksCubePiece.");

            pieces = GetComponentsInChildren<RubiksCubePiece>();

            if (pieces.Length == 0)
                Debug.LogError("Unable to find components of type RubiksCubePiece for RubiksCubeController pieces.");
        }

        selectedFace = null;
        selectedPieces = new RubiksCubePiece[0];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
            FaceTouchStart(faces[0]);
        else if (Input.GetKeyUp(KeyCode.Keypad0))
            FaceTouchEnd(faces[0]);

        if (Input.GetKeyDown(KeyCode.Keypad1))
            FaceTouchStart(faces[1]);
        else if (Input.GetKeyUp(KeyCode.Keypad1))
            FaceTouchEnd(faces[1]);

        if (Input.GetKeyDown(KeyCode.Keypad2))
            FaceTouchStart(faces[2]);
        else if (Input.GetKeyUp(KeyCode.Keypad2))
            FaceTouchEnd(faces[2]);

        if (Input.GetKeyDown(KeyCode.Keypad3))
            FaceTouchStart(faces[3]);
        else if (Input.GetKeyUp(KeyCode.Keypad3))
            FaceTouchEnd(faces[3]);

        if (Input.GetKeyDown(KeyCode.Keypad4))
            FaceTouchStart(faces[4]);
        else if (Input.GetKeyUp(KeyCode.Keypad4))
            FaceTouchEnd(faces[4]);

        if (Input.GetKeyDown(KeyCode.Keypad5))
            FaceTouchStart(faces[5]);
        else if (Input.GetKeyUp(KeyCode.Keypad5))
            FaceTouchEnd(faces[5]);
    }

    // Handler for the moment when a face is touched
    void FaceTouchStart(RubiksCubeFace face)
    {
        if (face != selectedFace)
        {
            // Find pieces within the face area
            RubiksCubePiece[] facePieces = GetPiecesWithinArea(face.Area);

            if (facePieces.Length > 0)
            {
                // Give focus to that face and those pieces
                selectedFace = face;
                selectedPieces = facePieces;

                // Highlight them for the player
                HighlightPieces(facePieces);
            }
        }
    }

    // Handler for the moment when a face is no longer touched
    void FaceTouchEnd(RubiksCubeFace face)
    {
        if (face == selectedFace)
        {
            // Unhighlight pieces
            UnhighlightPieces(selectedPieces);

            // Release focus 
            selectedFace = null;
            selectedPieces = new RubiksCubePiece[0];
        }
    }

    RubiksCubePiece[] GetPiecesWithinArea(BoxCollider area)
    {
        List<RubiksCubePiece> overlappingPieces = new List<RubiksCubePiece>();

        foreach (RubiksCubePiece piece in pieces)
        {
            if (area.bounds.Intersects(piece.Area.bounds))
                overlappingPieces.Add(piece);
        }

        return overlappingPieces.ToArray();
    }

    void HighlightPieces(RubiksCubePiece[] piecesForHighlight)
    {
        foreach (RubiksCubePiece piece in piecesForHighlight)
            HighlightPiece(piece);
    }

    void UnhighlightPieces(RubiksCubePiece[] piecesForHighlight)
    {
        foreach (RubiksCubePiece piece in piecesForHighlight)
            UnhighlightPiece(piece);
    }

    void HighlightPiece(RubiksCubePiece piece)
    {
        piece.BlockRenderer.material = highlightMat;
    }

    void UnhighlightPiece(RubiksCubePiece piece)
    {
        piece.BlockRenderer.material = normalMat;
    }
}
