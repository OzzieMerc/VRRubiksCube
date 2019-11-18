using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubiksCubeController : MonoBehaviour
{
    [SerializeField] RubiksCubeFace[] faces; // Each face of the cube that can be rotated
    [SerializeField] RubiksCubePiece[] pieces; // Each piece that makes up the cube
    [SerializeField] Material normalMat; // Normal material of pieces when they are not selected
    [SerializeField] Material highlightMat; // Material to highlight pieces when they are selected
    [SerializeField] GrabberInteraction grabber;
    RubiksCubeFace selectedFace; // Currently selected face;
    RubiksCubePiece[] selectedPieces; // Currently selected pieces
    bool pickedup;
    VRController pickupController;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(faces.Length > 0, "RubiksCubeController does not have any faces to work with.");

        if (pieces.Length == 0)
        {
            Debug.LogWarning("RubiksCubeController pieces is empty. Attempting to find all components of type RubiksCubePiece.");

            pieces = GetComponentsInChildren<RubiksCubePiece>();

            if (pieces.Length == 0)
                Debug.LogError("Unable to find components of type RubiksCubePiece for RubiksCubeController pieces.");
        }

        if (!grabber)
        {
            Debug.LogWarning("RubiksCubeController grabber is null. Attempting to find all components of type GrabberInteraction.");

            grabber = GetComponentInChildren<GrabberInteraction>();

            if (!grabber)
                Debug.LogError("Unable to find components of type GrabberInteraction for RubiksCubeController grabber.");
        }
        else
        {
            grabber.onTouchStartEvent += OnCubeTouchStart;
            grabber.onTouchEndEvent += OnCubeTouchExit;
            grabber.onGrabStartEvent += OnCubeGrabbed;
            grabber.onGrabEndEvent += OnCubeReleased;
        }
        
        selectedFace = null;
        selectedPieces = new RubiksCubePiece[0];
        pickedup = false;
        pickupController = null;

        DisableFaces();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
            OnFaceTouchStart(faces[0]);
        else if (Input.GetKeyUp(KeyCode.Keypad0))
            OnFaceTouchEnd(faces[0]);

        if (Input.GetKeyDown(KeyCode.Keypad1))
            OnFaceTouchStart(faces[1]);
        else if (Input.GetKeyUp(KeyCode.Keypad1))
            OnFaceTouchEnd(faces[1]);

        if (Input.GetKeyDown(KeyCode.Keypad2))
            OnFaceTouchStart(faces[2]);
        else if (Input.GetKeyUp(KeyCode.Keypad2))
            OnFaceTouchEnd(faces[2]);

        if (Input.GetKeyDown(KeyCode.Keypad3))
            OnFaceTouchStart(faces[3]);
        else if (Input.GetKeyUp(KeyCode.Keypad3))
            OnFaceTouchEnd(faces[3]);

        if (Input.GetKeyDown(KeyCode.Keypad4))
            OnFaceTouchStart(faces[4]);
        else if (Input.GetKeyUp(KeyCode.Keypad4))
            OnFaceTouchEnd(faces[4]);

        if (Input.GetKeyDown(KeyCode.Keypad5))
            OnFaceTouchStart(faces[5]);
        else if (Input.GetKeyUp(KeyCode.Keypad5))
            OnFaceTouchEnd(faces[5]);

        if (!selectedFace)
        {
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                RotateFace(selectedFace, selectedPieces, -15);
            else if (Input.GetKeyUp(KeyCode.RightArrow))
                RotateFace(selectedFace, selectedPieces, 15);
        }
    }

    // Handler for the moment when a face is touched
    void OnFaceTouchStart(RubiksCubeFace face)
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

                // Subscribe to grab events on the face
                face.onGrabStartEvent += OnFaceGrabbed;
                face.onGrabEndEvent += OnFaceReleased;

                // Highlight them for the player
                HighlightPieces(facePieces);
            }
        }
    }

    // Handler for the moment when a face is no longer touched
    void OnFaceTouchEnd(RubiksCubeFace face)
    {
        if (face == selectedFace)
        {
            // Unhighlight pieces
            UnhighlightPieces(selectedPieces);

            // Unsubscribe to grab events on the face
            face.onGrabStartEvent -= OnFaceGrabbed;
            face.onGrabEndEvent -= OnFaceReleased;

            // Release focus 
            selectedFace = null;
            selectedPieces = new RubiksCubePiece[0];
        }
    }

    void OnFaceGrabbed(RubiksCubeFace face)
    {
        RotateFace(selectedFace, selectedPieces, 45);
    }

    void OnFaceReleased(RubiksCubeFace face)
    {

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

    void RotateFace(RubiksCubeFace rotFace, RubiksCubePiece[] rotPieces, float degrees)
    {
        List<Transform> parents = new List<Transform>();

        foreach (RubiksCubePiece piece in rotPieces)
        {
            parents.Add(piece.transform.parent);
            piece.transform.SetParent(rotFace.transform);
        }

        rotFace.transform.RotateAround(transform.position, rotFace.transform.forward, degrees);

        for (int i = 0; i < parents.Count; i++)
        {
            rotPieces[i].transform.SetParent(parents[i]);
        }
    }

    void OnCubeTouchStart(VRController controller)
    {
        if (!pickupController)
            HighlightPieces(pieces);
    }

    void OnCubeTouchExit(VRController controller)
    {
        if (!pickupController || pickupController == controller)
            UnhighlightPieces(pieces);
    }

    void OnCubeGrabbed(VRController controller)
    {
        if (!pickedup)
        {
            pickedup = true;
            pickupController = controller;
            EnableFaces();
        }
    }

    void OnCubeReleased(VRController controller)
    {
        if (pickedup)
        {
            pickedup = false;
            pickupController = null;
            DisableFaces();
        }
    }

    void EnableFaces()
    {
        foreach (RubiksCubeFace face in faces)
        {
            face.onTouchStartEvent += OnFaceTouchStart;
            face.onTouchEndEvent += OnFaceTouchEnd;
            face.enabled = true;
        }
    }

    void DisableFaces()
    {
        foreach (RubiksCubeFace face in faces)
        {
            face.onTouchStartEvent -= OnFaceTouchStart;
            face.onTouchEndEvent -= OnFaceTouchEnd;
            face.enabled = false;
        }
    }
}
