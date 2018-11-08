using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    public Joint originJoint;
    public HandController grabbedBy;
    public bool separated = true;

    public bool accessable = false;

    private Vector3 _originPosition;
    private Quaternion _originRotation;

    void Awake()
    {
        _originPosition = transform.position;
        _originRotation = transform.rotation;
    }

    public void RestoreConnection()
    {
        if (originJoint && !separated)
        {
            transform.position = _originPosition;
            transform.rotation = _originRotation;
            originJoint.connectedBody = GetComponent<Rigidbody>();
        }
    }
}
