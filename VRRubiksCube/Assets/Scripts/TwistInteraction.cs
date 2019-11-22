using UnityEngine;

public class TwistInteraction : MonoBehaviour
{
    public delegate void OnTouchStart(VRController controller);
    public event OnTouchStart onTouchStartEvent;

    public delegate void OnTouchEnd(VRController controller);
    public event OnTouchEnd onTouchEndEvent;

    public delegate void OnGrabStart(VRController controller);
    public event OnGrabStart onGrabStartEvent;

    public delegate void OnGrabEnd(VRController controller);
    public event OnGrabEnd onGrabEndEvent;

    VRController controllerWithFocus;
    Quaternion grabRotationOffset;
    bool grabbed;

    // Start is called before the first frame update
    void Start()
    {
        controllerWithFocus = null;
        grabRotationOffset = Quaternion.identity;
        grabbed = false;
    }

    void Update()
    {
        if (grabbed)
        { 
            // local to world space
            //transform.rotation = controllerWithFocus.transform.rotation * grabRotationOffset;
        }
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        if (!enabled || HasFocus())
            return;

        if (otherCollider.TryGetComponent<VRController>(out VRController controller) && !controller.HasFocus())
        {
            SetFocus(controller);

            controller.onGripPulled += ControllerGrab;
            controller.onTriggerPulled += ControllerGrab;

            if (onTouchStartEvent != null)
                onTouchStartEvent(controller);
        }
    }

    void OnTriggerExit(Collider otherCollider)
    {
        if (!enabled || !HasFocus())
            return;

        if (otherCollider.TryGetComponent<VRController>(out VRController controller) && controller.GetFocus() == transform)
        {
            ClearFocus();

            controller.onGripPulled -= ControllerGrab;
            controller.onTriggerPulled -= ControllerGrab;

            if (onTouchEndEvent != null)
                onTouchEndEvent(controller);
        }
    }

    void ControllerGrab(VRController controller, bool gripped)
    {
        if (!enabled)
            return;

        if (gripped)
        {
            // world to local space
            grabRotationOffset = Quaternion.Inverse(controller.transform.rotation) * transform.rotation;

            grabbed = true;

            //RotateFace()

            if (onGrabStartEvent != null)
                onGrabStartEvent(controller);
        }
        else
        {
            grabbed = false;

            if (onGrabEndEvent != null)
                onGrabEndEvent(controller);
        }
    }

    //void RotateFace(RubiksCubeFace rotFace, RubiksCubePiece[] rotPieces, float degrees)
    //{
    //    Transform[] parents = new Transform[rotPieces.Length];

    //    for (int i = 0; i < rotPieces.Length; i++)
    //    {
    //        parents[i] = rotPieces[i].transform.parent;
    //        rotPieces[i].transform.SetParent(rotFace.transform);
    //    }

    //    rotFace.transform.RotateAround(transform.position, rotFace.transform.forward, degrees);

    //    for (int i = 0; i < parents.Length; i++)
    //        rotPieces[i].transform.SetParent(parents[i]);
    //}

    bool HasFocus()
    {
        return controllerWithFocus != null;
    }

    VRController GetFocus()
    {
        return controllerWithFocus;
    }

    void SetFocus(VRController controller)
    {
        controllerWithFocus = controller;
        controllerWithFocus.SetFocus(transform);
    }

    void ClearFocus()
    {
        controllerWithFocus.SetFocus(null);
        controllerWithFocus = null;
    }
}