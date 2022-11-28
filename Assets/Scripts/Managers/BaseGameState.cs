using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGameState
{
    #region Fields
    protected EgameState _state = EgameState.NONE;
    protected EgameState LastGState = EgameState.NONE;
    protected GameStateMachine _machine = null;
    #endregion

    #region Properties
    public EgameState State => _state;
    #endregion

    #region Methods
    public void Init(GameStateMachine machine, EgameState state)
    {
        _machine = machine;
        _state = state;
    }

    public abstract void StartState();
    public abstract void UpdateState();
    public abstract void FixedUpdateState();
    public abstract void LeaveState();
    #endregion
}
