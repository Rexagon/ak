using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessManager : MonoBehaviour
{
    //
    // HEEEEEEEEEEELP
    //
    // I want to sleep

    // magazine;    // 0
    // ramrod;      // 1
    // topCover;    // 2
    // spring;      // 3
    // gate;        // 4
    // frontCover;  // 5

    private bool[] _beginAccessState = {
        true,
        true,
        true,
        false,
        false,
        false
    };

    public List<Part> _partsList;
    private bool[] _lastState = new bool[6];

    private int _layer;

    void Start()
    {
        ApplyAccessState(_beginAccessState);

        _layer = LayerMask.NameToLayer("Part");
    }

    void Update()
    {
        if (Attached(2))
        {
            SetAccessable(_partsList[3], true);
        }
        else if (Detached(2)) {
            SetAccessable(_partsList[3], false);
            SetAccessable(_partsList[4], false);
            SetAccessable(_partsList[5], false);
        }

        if (Attached(3))
        {
            SetAccessable(_partsList[4], true);
        }
        else if (Detached(3))
        {
            SetAccessable(_partsList[4], false);
            SetAccessable(_partsList[5], false);
        }

        if (Attached(4))
        {
            SetAccessable(_partsList[5], true);
        }
        else if (Detached(4))
        {
            SetAccessable(_partsList[5], false);
        }

        SaveState();
    }

    bool Attached(int index)
    {
        return !_lastState[index] && GetState(index);
    }

    bool Detached(int index)
    {
        return _lastState[index] && !GetState(index);
    }

    void ApplyAccessState(bool[] state)
    {
        for (int i = 0; i < state.Length; ++i)
        {
            SetAccessable(_partsList[i], state[i]);
        }
    }

    void SaveState()
    {
        for (int i = 0; i < _lastState.Length; ++i)
        {
            _lastState[i] = GetState(i);
        }
    }

    void SetAccessable(Part part, bool accessable)
    {
        part.accessable = accessable;
    }

    bool GetState(int index)
    {
        return !_partsList[index].gameObject.activeSelf;
    }
}
