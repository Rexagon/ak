using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class HandController : MonoBehaviour
{
    public enum State
    {
        None,
        Constructing,
        Deconstructing
    }

    public Part part;
    public State state;

    private Joint _joint;

    public bool _triggerDown;

    public bool isValid
    {
        get
        {
            return _trackedObject && _trackedObject.isValid;
        }
    }

    public SteamVR_Controller.Device device
    {
        get
        {
            return SteamVR_Controller.Input((int)_trackedObject.index);
        }
    }

    public Vector3 deltaPosition { get; private set; }
    Vector3 _lastPosition;

    SteamVR_TrackedObject _trackedObject;

    void Start ()
    {
        _trackedObject = GetComponent<SteamVR_TrackedObject>();
    }

    void FixedUpdate()
    {
        deltaPosition = transform.localPosition - _lastPosition;
        _lastPosition = transform.localPosition;

        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            Detach();
            _triggerDown = false;
            state = State.None;
        }

        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            _triggerDown = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Part") &&
            _triggerDown)
        {
            if (part != null && (other.gameObject == part.gameObject))
            {
                return;
            }

            Debug.Log("Attached");

            Part collidedPart = other.GetComponentInParent<Part>();
            if (!collidedPart.accessable)
            {
                return;
            }

            Attach(collidedPart);

            _triggerDown = false;
        }
    }

    public void Attach(Part other)
    {
        Vector3 anchor = Vector3.zero;
        bool hasAnchor = false;
        if (_joint)
        {
            anchor = _joint.connectedAnchor;
            hasAnchor = true;
        }

        Detach();
        part = other.GetComponentInParent<Part>();

        if (!part.accessable)
        {
            return;
        }

        if (other.separated)
        {
            if (state == State.None)
            {
                state = State.Constructing;
            }
            _joint = gameObject.AddComponent<FixedJoint>();
        }
        else
        {
            if (state == State.None)
            {
                state = State.Deconstructing;
            }
            ConfigurableJoint configurableJoint = gameObject.AddComponent<ConfigurableJoint>();
            configurableJoint.xMotion = ConfigurableJointMotion.Limited;
            configurableJoint.yMotion = ConfigurableJointMotion.Limited;
            configurableJoint.zMotion = ConfigurableJointMotion.Limited;
            configurableJoint.anchor = Vector3.zero;
            _joint = configurableJoint;
        }

        _joint.connectedBody = other.GetComponent<Rigidbody>();
        if (hasAnchor)
        {
            _joint.autoConfigureConnectedAnchor = false;
            _joint.connectedAnchor = anchor;
        }
        part.grabbedBy = this;
    }

    void Detach()
    {
        if (part == null || _joint == null)
        {
            return;
        }

        _joint.connectedBody.velocity = -device.velocity;
        _joint.connectedBody.angularVelocity = -device.angularVelocity;
        _joint.connectedBody = null;
        Destroy(_joint);
        
        part.grabbedBy = null;
        part = null;
    }
}
