using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CaptureState : MonoBehaviour
{
    private Dictionary<EcaptureState, BaseCaptureState> _statesCDict = new Dictionary<EcaptureState, BaseCaptureState>();

    public BaseCaptureState CurrentCState => _statesCDict[_curentCState];
    private EcaptureState _curentCState;
    private EcaptureState _lastCState;
    private EcaptureState EcaptureState = EcaptureState.NONE;
    public EcaptureState CurrentStateType => _curentCState;
    public EcaptureState LastCState { get => _lastCState; set => _lastCState = value; }
    public Image m_FillImage;  // The image component of the slider.

    public TMP_Text _bluePts; 
    public TMP_Text _redPts;  
                     
    public TMP_Text bluePts { get => _bluePts; set => _bluePts = value; }
    public TMP_Text redPts { get => _redPts; set => _redPts = value; }
    


    // Start is called before the first frame update
    void Start()
    {
        _statesCDict = new Dictionary<EcaptureState, BaseCaptureState>();
        SubCStateInit();

    }

    private void SubCStateInit()
    {
        StartCaptureState startState = new StartCaptureState();
        startState.Init(this, EcaptureState.START);
        _statesCDict.Add(EcaptureState.START, startState);

        ConflictCaptureState conflictState = new ConflictCaptureState();
        conflictState.Init(this, EcaptureState.CONFLICT);
        _statesCDict.Add(EcaptureState.CONFLICT, conflictState);
    }

    void Update()
    {
        CurrentCState.UpdateState();
     }
     
     //Uses Team Tag and Collider Array To Find The Team Count
     int TeamCount(string tag, Collider[] colliders)
     {
         //Set the count to 0
         int count = 0;
 
         //Loop through the colliders array
         for (int i = 0; i < colliders.Length; i++)
         {
             //If the collider's tag within the array equal the team tag
             if (colliders[i].tag == tag)
             {
                 //Add to the count
                 count += 1;
             }
         }
         //Then return the complete count of the tag within the array
         return count;
    }

    private void FixedUpdate()
    {
        CurrentCState.FixedUpdateState();
    }

    private void OnTriggerEnter(Collider col)
    {
        Debug.Log(col.gameObject.tag + " ENTER");

        CurrentCState.StartState(col.gameObject.tag);

    }

    private void OnTriggerExit(Collider col)
    {
        _curentCState = EcaptureState.START;
        CurrentCState.LeaveState(col.gameObject.tag);
        CurrentCState.StartState();
        Debug.Log("Leave capture");
    }

    public void changeState(EcaptureState nextState,bool switchSong)
    {
        //CurrentCState.LeaveState();
        _curentCState = nextState;
        CurrentCState.StartState();
    }

    public void SetColorZone(Color color) {
        m_FillImage.color = color;
    } 
    public Color GetColorZone() {
        return m_FillImage.color;
    } 
}

public enum EcaptureState
{
    START,
    LEAVE,
    CONFLICT,
    NONE
}

