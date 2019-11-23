using UnityEngine;

public class TwistInteraction : MonoBehaviour
{
    public delegate void InteractionEvent(VRController controller);

    public event InteractionEvent onTouchStartEvent;
    public event InteractionEvent onTouchEndEvent;
    public event InteractionEvent onPreGrabStartEvent;
    public event InteractionEvent onGrabStartEvent;
    public event InteractionEvent onGrabEndEvent;
    public event InteractionEvent onPostGrabEndEvent;

    VRController controllerWithFocus;
    Quaternion prevGrabRotation;
    bool grabbed;

    // Start is called before the first frame update
    void Start()
    {
        controllerWithFocus = null;
        prevGrabRotation = Quaternion.identity;
        grabbed = false;
    }

    void Update()
    {
        if (grabbed)
        {
            float angle = Quaternion.Angle(controllerWithFocus.transform.rotation, prevGrabRotation);
            transform.RotateAround(transform.position, transform.forward, angle);
            prevGrabRotation = controllerWithFocus.transform.rotation;
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
            
            if (grabbed)
                Ungrab(controller);
        }
    }

    void ControllerGrab(VRController controller, bool gripped)
    {
        if (!enabled)
            return;

        if (gripped)
        {
            prevGrabRotation = controller.transform.rotation;

            grabbed = true;

            if (onPreGrabStartEvent != null)
                onPreGrabStartEvent(controller);
                       
            if (onGrabStartEvent != null)
                onGrabStartEvent(controller);
        }
        else
            Ungrab(controller);
    }

    void Ungrab(VRController controller)
    {
        grabbed = false;

        if (onGrabEndEvent != null)
            onGrabEndEvent(controller);

        if (onPostGrabEndEvent != null)
            onPostGrabEndEvent(controller);
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