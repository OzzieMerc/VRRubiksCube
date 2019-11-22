using UnityEngine;

[RequireComponent(typeof(RubiksCubeFace))]
public class RubiksCubeFaceHighlighter : MonoBehaviour
{
    [SerializeField] RubiksCubeFace face;
    [SerializeField] TwistInteraction twister;
    [SerializeField] RubiksCubePreferences prefs;

    void Start()
    {
        if (!face)
        {
            Debug.LogWarning("RubiksCubeFaceHighlighter face not assigned. Attempting to find a compatible RubiksCubeFace");
            face = GetComponent<RubiksCubeFace>();

            if (!face)
                Debug.LogError("Unable to find a compatible RubiksCubeFace for RubiksCubeFaceHighlighter face");
        }

        if (!prefs)
        {
            Debug.LogWarning("RubiksCubeFaceHighlighter prefs not assigned. Attempting to find a compatible RubiksCubePreferences");
            prefs = GetComponentInParent<RubiksCubePreferences>();

            if (!prefs)
                Debug.LogError("Unable to find a compatible RubiksCubePreferences for RubiksCubeFaceHighlighter prefs");
        }

        if (twister && face)
        {
            twister.onTouchStartEvent += (VRController controller) => { ChangeMaterial(face.Pieces, prefs.FaceBlockTouchMat); };
            twister.onTouchEndEvent += (VRController controller) => { ChangeMaterial(face.Pieces, prefs.BlockNormalMat); }; ;
            twister.onGrabStartEvent += (VRController controller) => { ChangeMaterial(face.Pieces, prefs.FaceBlockGrabMat); }; ;
            twister.onGrabEndEvent += (VRController controller) => { ChangeMaterial(face.Pieces, prefs.FaceBlockTouchMat); }; ;
        }
    }

    void ChangeMaterial(RubiksCubePiece[] piecesToChange, Material mat)
    {
        foreach (RubiksCubePiece piece in piecesToChange)
            piece.BlockRenderer.material = mat;
    }
}
