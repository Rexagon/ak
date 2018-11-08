using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachTrigger : MonoBehaviour
{
    public Part shouldActivate;
    private AttachTriggerSocket[] _sockets;

    void Start()
    {
        _sockets = GetComponentsInChildren<AttachTriggerSocket>();
    }

    void Update()
    {
        if (_sockets.Length == 0)
        {
            return;
        }

        bool allOverlapped = true;
        foreach (var socket in _sockets)
        {
            if (!socket.partOverlapped)
            {
                allOverlapped = false;
                break;
            }
        }

        if (allOverlapped && shouldActivate)
        {
            Part part = _sockets[0].partOverlapped;
            if (part == null)
            {
                return;
            }

            HandController handController = part.grabbedBy;
            if (handController == null || handController.state != HandController.State.Constructing)
            {
                return;
            }

            shouldActivate.gameObject.SetActive(true);
            shouldActivate.separated = false;
            shouldActivate.RestoreConnection();

            Destroy(part.gameObject);
            foreach (var socket in _sockets)
            {
                socket.partOverlapped = null;
            }

            handController.Attach(shouldActivate);
        }
    }
}
