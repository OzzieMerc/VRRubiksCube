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
    Quaternion prevGrabRotation, startingRotation;
    bool grabbed;

    // Start is called before the first frame update
    void Start()
    {
        controllerWithFocus = null;
        prevGrabRotation = Quaternion.identity;
        startingRotation = transform.localRotation;
        grabbed = false;
    }

    void Update()
    {
        if (grabbed)
            transform.localRotation = startingRotation * Quaternion.Euler(Vector3.Scale((controllerWithFocus.transform.rotation * prevGrabRotation).eulerAngles, Vector3.forward));
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
            prevGrabRotation = Quaternion.Inverse(controller.transform.rotation) * transform.rotation;
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