using UnityEngine;
using UnityEngine.XR;

// This class is designed for use with the Oculus Touch controller
public class VRController : MonoBehaviour
{
    public delegate void OnTriggerPulled(VRController controller, bool pulled);
    public event OnTriggerPulled onTriggerPulled;

    public delegate void OnGripPulled(VRController controller, bool pulled);
    public event OnGripPulled onGripPulled;

    public enum ControllerHand { LEFT, RIGHT };

    public float triggerDeadZone = 0.5f; // How far the trigger is pulled on the device to consider it pulled.
    public float gripDeadZone = 0.5f; // How far the grip is pulled on the device to consider it pulled.
    [SerializeField] ControllerHand hand = ControllerHand.LEFT; // The hand this controller represents.

    InputDevice device;
    bool triggerPulled;
    bool gripPulled;
    Vector2 thumbstickAxis;
    Vector3 linearVelocity;
    Vector3 angularVelocity;
    Transform focus;

    public ControllerHand Hand { get => hand; }
    public bool TriggerPulled { get => triggerPulled; }
    public bool GripPulled{ get => gripPulled; }
    public Vector2 ThumbstickAxis { get => thumbstickAxis; }
    public Vector3 LinearVelocity { get => linearVelocity; }
    public Vector3 AngularVelocity { get => angularVelocity; }
    
    // Start is called before the first frame update
    void Start()
    {
        SetupDevice();
        ResetInput();
    }

    // Update is called once per frame
    void Update()
    {
        // There is a possibilty the controller might disconnect. This will allow us to recover the controller after it reconnects.
        if (!device.isValid)
        {
            SetupDevice();
            ResetInput();
        }
        else
        {
            float fval;
            Vector2 v2val;
            Vector3 v3val;
            Quaternion qval;
            float invertedTime = 1 / Time.deltaTime;
            Vector3 invertedTimeVec = new Vector3(invertedTime, invertedTime, invertedTime);

            // Update state of the trigger button
            if (device.TryGetFeatureValue(CommonUsages.trigger, out fval))
            {
                // Has the trigger been pulled enough to consider it pulled?
                bool pulled = fval > triggerDeadZone;
                
                // If the trigger is in a new state
                if (pulled != triggerPulled)
                {
                    // Update and notify
                    triggerPulled = pulled;
                    onTriggerPulled?.Invoke(this, pulled);
                }
            }

            // Update state of the grip button
            if (device.TryGetFeatureValue(CommonUsages.grip, out fval))
            {
                bool pulled = fval > gripDeadZone;

                if (pulled != gripPulled)
                {
                    gripPulled = pulled;
                    onGripPulled?.Invoke(this, pulled);
                }
            }

            if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out v2val))
                thumbstickAxis = v2val;

            // Set the hand position to the device position
            if (device.TryGetFeatureValue(CommonUsages.devicePosition, out v3val))
            {
                linearVelocity = (v3val - transform.localPosition) * invertedTime;
                transform.localPosition = v3val;
            }

            // Set the hand orientation to the device orientation
            if (device.TryGetFeatureValue(CommonUsages.deviceRotation, out qval))
            {
                angularVelocity = (qval * Quaternion.Inverse(transform.localRotation)).eulerAngles * invertedTime;
                transform.localRotation = qval;
            }
        }
    }

    void SetupDevice()
    {
        device = InputDevices.GetDeviceAtXRNode(hand == ControllerHand.LEFT ? XRNode.LeftHand : XRNode.RightHand);
    }

    void ResetInput()
    {
        triggerPulled = false;
        gripPulled = false;
        thumbstickAxis = Vector2.zero;
        linearVelocity = Vector3.zero;
        angularVelocity = Vector3.zero;
    }

    public bool HasFocus()
    {
        return focus != null;
    }

    public Transform GetFocus()
    {
        return focus;
    }

    public void SetFocus(Transform focusObj)
    {
        focus = focusObj;
    }

    public void ClearFocus()
    {
        focus = null;
    }
}
