using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGameState : BaseGameState
{
    #region fileds

    #endregion

    #region Properties

    #endregion

    #region Methods   
    public override void StartState()
    {
        _machine.LastGState = EgameState.MENU;
    }

    public override void UpdateState()
    {

    }

    public override void FixedUpdateState()
    {

    }

    public override void LeaveState()
    {
        _machine.LastGState = EgameState.NONE;
    }

    #endregion
}
