using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RubiksCubePiece : MonoBehaviour
{
    [SerializeField] BoxCollider area; // A tirgger volume covering the piece
    [SerializeField] MeshRenderer blockRenderer; // The block's renderer to use for swapping materials
    [SerializeField] Transform[] stickers;

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

        if (stickers.Length == 0)
        {
            Debug.LogError("RubiksCubePiece stickers is empty. All stickers must added to the array.");
        }

        Debug.Assert(blockRenderer, "RubiksCubePiece blockRenderer not assigned");
    }

    public Vector3 GetNormal()
    {
        Vector3 v = Vector3.zero;
        foreach (Transform tform in stickers)
            v += tform.forward;

        v = -v; // The direction needs to be reverse since the sticker's forward is not on the visible side
        v.Normalize();

        return v;
    }
}
