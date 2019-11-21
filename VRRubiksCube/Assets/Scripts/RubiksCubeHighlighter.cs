using UnityEngine;

[RequireComponent(typeof(GrabberInteraction))]
public class RubiksCubeHighlighter : MonoBehaviour
{
    [SerializeField] RubiksCubeController cubeController;
    [SerializeField] GrabberInteraction grabber;
    [SerializeField] RubiksCubePreferences prefs;

    void Start()
    {
        if (!cubeController)
        {
            Debug.LogWarning("RubiksCubeHighlighter cubeController not assigned. Attempting to find a compatible RubiksCubeController");
            cubeController = GetComponent<RubiksCubeController>();

            if (!cubeController)
                Debug.LogError("Unable to find a compatible RubiksCubeController for RubiksCubeHighlighter cubeController");
        }

        if (!grabber)
        {
            Debug.LogWarning("RubiksCubeHighlighter grabber not assigned. Attempting to find a compatible GrabberInteraction");
            grabber = GetComponent<GrabberInteraction>();

            if (!grabber)
                Debug.LogError("Unable to find a compatible GrabberInteraction for RubiksCubeHighlighter grabber");
        }

        if (!prefs)
        {
            Debug.LogWarning("RubiksCubeHighlighter prefs not assigned. Attempting to find a compatible RubiksCubePreferences");
            prefs = GetComponent<RubiksCubePreferences>();

            if (!prefs)
                Debug.LogError("Unable to find a compatible RubiksCubePreferences for RubiksCubeHighlighter prefs");
        }

        if (grabber && cubeController)
        {
            grabber.onTouchStartEvent += (VRController controller) => { ChangeMaterial(cubeController.Pieces, prefs.CubeBlockTouchMat); };
            grabber.onTouchEndEvent += (VRController controller) => { ChangeMaterial(cubeController.Pieces, prefs.BlockNormalMat); }; ;
            grabber.onGrabStartEvent += (VRController controller) => { ChangeMaterial(cubeController.Pieces, prefs.CubeBlockGrabMat); }; ;
            grabber.onGrabEndEvent += (VRController controller) => { ChangeMaterial(cubeController.Pieces, prefs.CubeBlockTouchMat); }; ;
        }
    }

    void ChangeMaterial(RubiksCubePiece[] piecesToChange, Material mat)
    {
        foreach (RubiksCubePiece piece in piecesToChange)
            piece.BlockRenderer.material = mat;
    }
}
