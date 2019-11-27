using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonInteraction : MonoBehaviour
{
    [Serializable]
    public class TouchInteraction : UnityEvent<VRController> { }

    [SerializeField] TouchInteraction onTouchStartEvent;
    [SerializeField] TouchInteraction onTouchEndEvent;
    [SerializeField] Transform push;
    [SerializeField] float pushOffset = 0.007f;

    VRController controllerWithFocus;

    // Start is called before the first frame update
    void Start()
    {
        controllerWithFocus = null;
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        if (HasFocus())
            return;

        if (otherCollider.TryGetComponent<VRController>(out VRController controller) && !controller.HasFocus())
        {
            SetFocus(controller);

            push.position = push.position - Vector3.up * pushOffset;

            if (onTouchStartEvent != null)
                onTouchStartEvent.Invoke(controller);
        }
    }

    void OnTriggerExit(Collider otherCollider)
    {
        if (!HasFocus())
            return;

        if (otherCollider.TryGetComponent<VRController>(out VRController controller) && controller.GetFocus() == transform)
        {
            ClearFocus();

            push.position = push.position + Vector3.up * pushOffset;

            if (onTouchEndEvent != null)
                onTouchEndEvent.Invoke(controller);
        }
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
