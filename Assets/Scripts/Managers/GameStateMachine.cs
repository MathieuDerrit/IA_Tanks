using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
    #region fields
    private Dictionary<EgameState, BaseGameState> _statesGDict = new Dictionary<EgameState, BaseGameState>();
    #endregion

    #region Properties
    public BaseGameState CurrentGState => _statesGDict[_curentGState];
    private EgameState _curentGState;
    private EgameState _lastGState;
    private EgameState EgameState = EgameState.NONE;
    public EgameState CurrentStateType => _curentGState;
    public EgameState LastGState { get => _lastGState; set => _lastGState = value; }
    #endregion

    #region Methods

    #region Start And Init
    private void Start()
    {
        _statesGDict = new Dictionary<EgameState, BaseGameState>();
        SubGStateInit();
        CurrentGState.StartState();
    }

    private void SubGStateInit()
    {
        StartGameState startState = new StartGameState();
        startState.Init(this, EgameState.START);
        _statesGDict.Add(EgameState.START, startState);

        MenuGameState menuState = new MenuGameState();
        menuState.Init(this, EgameState.MENU);
        _statesGDict.Add(EgameState.MENU, menuState);

    }
    #endregion

    #region RunTime
    private void Update()
    {
        CurrentGState.UpdateState();
    }

    private void FixedUpdate()
    {
        CurrentGState.FixedUpdateState();
    }

    public void changeState(EgameState nextState,bool switchSong)
    {
        CurrentGState.LeaveState();
        _curentGState = nextState;
        CurrentGState.StartState();
    }

    #endregion

    #region LoadFunction
    public void start1PlayerState(){
        changeState(EgameState.START, true);
    }

    public void start2PlayersState(){
        changeState(EgameState.START, true);
    }


    public void exitState(){
        Application.Quit();
    }

    #endregion

    #endregion
}

public enum EgameState
{
    START,
    MENU,
    NONE
}

