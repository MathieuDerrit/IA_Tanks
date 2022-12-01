using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCaptureState
{

    protected EcaptureState _state = EcaptureState.NONE;
    protected EcaptureState LastCState = EcaptureState.NONE;
    protected CaptureState _machine = null;

    public EcaptureState State => _state;


    public void Init(CaptureState machine, EcaptureState state)
    {
        _machine = machine;
        _state = state;
    }

    public abstract void StartState(string tag = "");
    public abstract void UpdateState();
    public abstract void FixedUpdateState();
    public abstract void LeaveState(string tag = "");
    public abstract void ConflictState();

}
