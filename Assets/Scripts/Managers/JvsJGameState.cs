using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JvsJGameState : BaseGameState
{
    #region Fields

    #endregion

    #region Properties

    #endregion

    #region Methods   
    public override void StartState()
    {
        Debug.Log("START J vs J");
        _machine.LastGState = EgameState.START_JVSJ;
        SceneManager.LoadScene(1);
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
