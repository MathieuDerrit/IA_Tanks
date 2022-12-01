using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Engine.Utils;

public class GameStateMachine : Singleton<GameStateMachine>
{
    #region Fields
    private Dictionary<EgameState, BaseGameState> _statesGDict = new Dictionary<EgameState, BaseGameState>();

    public Complete.CameraControl m_CameraControl;       // Reference to the CameraControl script for control during different phases.
    public Text m_MessageText;                  // Reference to the overlay Text to display winning text, etc.
    public GameObject m_TankPrefab;             // Reference to the prefab the players will control.
    public TankManager[] m_Tanks;               // A collection of managers for enabling and disabling different aspects of the tanks.
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
    protected override void Start()
    {
        base.Start();
        _statesGDict = new Dictionary<EgameState, BaseGameState>();
        SubGStateInit();
        CurrentGState.StartState();
        m_CameraControl = GetComponent<Complete.CameraControl>();
    }

    private void SubGStateInit()
    {
        MenuGameState menuState = new MenuGameState();
        menuState.Init(this, EgameState.MENU);
        _statesGDict.Add(EgameState.MENU, menuState);

        JvsIAGameState startJvsIAState = new JvsIAGameState();
        startJvsIAState.Init(this, EgameState.START_IA);
        _statesGDict.Add(EgameState.START_IA, startJvsIAState);

        JvsJGameState startJvsJState = new JvsJGameState();
        startJvsJState.Init(this, EgameState.START_JVSJ);
        _statesGDict.Add(EgameState.START_JVSJ, startJvsJState);
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
    public void startJvsIAState(){
        Debug.Log("Salut");
        changeState(EgameState.START_IA, true);
    }

    public void startJvsJState(){
        changeState(EgameState.START_JVSJ, true);
    }


    public void exitState(){
        Application.Quit();
    }

    #endregion

    #region Functions

    public void SpawnAllTanks()
    {

        // For all the tanks...
        for (int i = 0; i < m_Tanks.Length; i++)
            {
                Debug.Log(m_Tanks[i].m_SpawnPoint);
                // ... create them, set their player number and references needed for control.
                m_Tanks[i].m_Instance = Instantiate(m_TankPrefab, m_Tanks[i].m_SpawnPoint.position, m_Tanks[i].m_SpawnPoint.rotation) as GameObject;
                m_Tanks[i].m_PlayerNumber = i + 1;
                if (m_Tanks[i].m_PlayerNumber == 1) {
                    m_Tanks[i].m_Instance.gameObject.tag = "TankBlue";
                } else {
                    m_Tanks[i].m_Instance.gameObject.tag = "TankRed";
                }
                m_Tanks[i].Setup();
        }
    }

    #endregion

    #endregion
}

public enum EgameState
{
    MENU,
    START_IA,
    START_JVSJ,
    NONE
}