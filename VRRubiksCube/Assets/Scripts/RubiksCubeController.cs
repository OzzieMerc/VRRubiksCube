using UnityEngine;

public class RubiksCubeController : MonoBehaviour
{
    [SerializeField] TwistInteraction[] faceInteractions; // Each face of the cube that can be rotated
    [SerializeField] RubiksCubePiece[] pieces; // Each piece that makes up the cube
    [SerializeField] GrabberInteraction grabber;
    RubiksCubeFace selectedFace; // Currently selected face;
    RubiksCubePiece[] selectedPieces; // Currently selected pieces
    bool pickedup;
    public RubiksCubePiece[] Pieces { get => pieces; }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(faceInteractions.Length > 0, "RubiksCubeController does not have any faces to work with.");

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
            grabber.onGrabStartEvent += OnCubeGrabbed;
            grabber.onGrabEndEvent += OnCubeReleased;
        }
        
        selectedFace = null;
        selectedPieces = new RubiksCubePiece[0];
        pickedup = false;

        DisableFaces();
    }

    void Update()
    {
    }

    void OnCubeGrabbed(VRController controller)
    {
        if (!pickedup)
        {
            pickedup = true;
            EnableFaces();
        }
    }

    void OnCubeReleased(VRController controller)
    {
        if (pickedup)
        {
            pickedup = false;
            DisableFaces();
        }
    }

    void EnableFaces()
    {
        foreach (TwistInteraction interaction in faceInteractions)
            interaction.enabled = true;
    }

    void DisableFaces()
    {
        foreach (TwistInteraction interaction in faceInteractions)
            interaction.enabled = false;
    }
}
