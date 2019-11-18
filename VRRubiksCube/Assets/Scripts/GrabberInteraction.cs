using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabberInteraction : MonoBehaviour
{
    public delegate void OnTouchStart(VRController controller);
    public event OnTouchStart onTouchStartEvent;

    public delegate void OnTouchEnd(VRController controller);
    public event OnTouchEnd onTouchEndEvent;

    public delegate void OnGrabStart(VRController controller);
    public event OnGrabStart onGrabStartEvent;

    public delegate void OnGrabEnd(VRController controller);
    public event OnGrabEnd onGrabEndEvent;

    [SerializeField] Rigidbody rb;

    VRController controllerWithFocus;
    Vector3 grabOffset;
    Quaternion grabRotationOffset;

    // Start is called before the first frame update
    void Start()
    {
        if (!rb)
        {
            Debug.LogWarning("GrabberInteraction rb not assigned. Attempting to find a compatible Rigidbody");
            rb = GetComponentInParent<Rigidbody>();

            if (!rb)
                Debug.LogError("Unable to find a compatible Rigidbody for GrabberInteraction rb");
        }

        controllerWithFocus = null;
        grabOffset = Vector3.zero;
        grabRotationOffset = Quaternion.identity;
    }

    void Update()
    {
        if (controllerWithFocus)
        {
            rb.transform.rotation = controllerWithFocus.transform.rotation * grabRotationOffset;
            rb.transform.position = controllerWithFocus.transform.TransformPoint(grabOffset);
        }
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.TryGetComponent<VRController>(out VRController controller))
        {
            controller.onGripPulled += ControllerGrab;
            controller.onTriggerPulled += ControllerGrab;

            if (onTouchStartEvent != null)
                onTouchStartEvent(controller);
        }
    }

    void OnTriggerExit(Collider otherCollider)
    {
        if (otherCollider.TryGetComponent<VRController>(out VRController controller))
        {
            controller.onGripPulled -= ControllerGrab;
            controller.onTriggerPulled -= ControllerGrab;

            if (onTouchEndEvent != null)
                onTouchEndEvent(controller);
        }
    }

    void ControllerGrab(VRController controller, bool gripped)
    {
        if (gripped)
        {
            rb.isKinematic = true;

            grabRotationOffset = transform.rotation * Quaternion.Inverse(controller.transform.rotation);
            grabOffset = controller.transform.InverseTransformPoint(rb.transform.position);
            controllerWithFocus = controller;

            if (onGrabStartEvent != null)
                onGrabStartEvent(controller);
        }
        else
        {
            rb.isKinematic = false;
            rb.velocity = controller.LinearVelocity;
            rb.angularVelocity = controller.AngularVelocity;
            controllerWithFocus = null;

            if (onGrabEndEvent != null)
                onGrabEndEvent(controller);
        }
    }
}