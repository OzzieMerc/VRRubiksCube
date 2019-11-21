using UnityEngine;

public class RubiksCubePreferences : MonoBehaviour
{
    [SerializeField] Material blockNormalMat; // Normal material of pieces when they are not selected
    [SerializeField] Material cubeBlockTouchMat; // Material to highlight pieces when the cube is touched
    [SerializeField] Material cubeBlockGrabMat; // Material to highlight pieces when the cube is grabbed
    [SerializeField] Material faceBlockTouchMat; // Material to highlight pieces when a face is touched
    [SerializeField] Material faceBlockGrabMat; // Material to highlight pieces when a face is grabbed

    public Material BlockNormalMat { get => blockNormalMat; }
    public Material CubeBlockTouchMat { get => cubeBlockTouchMat; }
    public Material CubeBlockGrabMat { get => cubeBlockGrabMat; }
    public Material FaceBlockTouchMat { get => faceBlockTouchMat; }
    public Material FaceBlockGrabMat { get => faceBlockGrabMat; }
}
