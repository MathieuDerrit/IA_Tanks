using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConflictCaptureState : BaseCaptureState
{
    public override void StartState(string tag) {
        Debug.Log("Start conflict");
        _machine.LastCState = EcaptureState.CONFLICT;
    }
    public override void UpdateState() {}
    public override void FixedUpdateState() {}
    public override void LeaveState(string tag) {
        
    }
    public override void ConflictState() {}
}
