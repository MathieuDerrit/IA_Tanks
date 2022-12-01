using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCaptureState : BaseCaptureState
{

    private float bluePts = 0f;
    private float redPts = 0f;

    private string tankTag = "none";





    [Header("Name Of The Red Team")]
    public string redTeam = "TankRed";
    [Header("Name Of The Blue Team")]
    public string blueTeam = "TankBlue";

    //Materials To Change The Capture Point To
    public Color redCapColor = Color.red;
    public Color blueCapColor= Color.blue;

    private Color actualColor= Color.white;

    [Header("Time It Takes To Capture The Point")]
    public float CaptureTime = 3;

    [Header("Radius In Which The Point Is Capturable")]
    public float CapRadius = 3;

    //Time the team has held the capture point
    public float redCapture = 0;
    public float blueCapture = 0;

    //Empty Collider Array
    Collider[] colliders;

    //Radius to hold the statuses of the teams
    bool redCaptureStatus = false;
    bool blueCaptureStatus = false;

    bool redIsFull = false;
    bool blueIsFull = false;

    bool redOnZone = false;
    bool blueOnZone = false;




    public override void StartState(string tag)
    {
        _machine.LastCState = EcaptureState.START;
        if (tag == "") {
            if (redOnZone) {
                tankTag = redTeam;
            }
            if (blueOnZone) {
                tankTag = blueTeam;
            }
        } else {
          tankTag = tag;  
        }
    
        if (tag == redTeam) {
            redOnZone = true;
        }
        if (tag == blueTeam) {
            blueOnZone = true;
        }
        if (redOnZone && blueOnZone) {
            _machine.changeState(EcaptureState.CONFLICT, true);
        }

        Debug.Log("--------------RED----------" + redOnZone);
        Debug.Log("--------------BLUE----------" + blueOnZone);
    }

    public override void UpdateState()
    { 
        //If the collider within the circle is of the blue team
        if (tankTag == blueTeam && blueOnZone)
        {    
            //And the teams capture time is less than the time it takes to capture
            if (blueCapture < CaptureTime)
            {
                redCaptureStatus = false;
                //If the enemy teams capture time is greater than 0
                if (redCapture <= 0)
                {
                    //Add to the teams capture time
                    blueCapture += Time.deltaTime;
                    _machine.SetColorZone(Color.Lerp (Color.white, blueCapColor, blueCapture / CaptureTime));
                }
            }

            //Else the teams capture time is greater than the capture time
            else
            {
                if (!blueIsFull) {
                    _machine.prefabInstance(_machine.Star_B);
                    _machine.prefabInstance(_machine.ShockWave);
                }
                //Set the capture status of the team to true and the enemys team to false
                blueCaptureStatus = true;
                blueIsFull = true;
            }
        }

        //Check if the Team Counts are not equal to each other
        else if (tankTag == redTeam && redOnZone)
        {
            //If the collider within the circle is of the red team
            if (redCapture < CaptureTime)
            {
                blueCaptureStatus = false;
                //If the enemy teams capture time is greater than 0
                if (blueCapture <= 0)
                {
                    //Add to the teams capture time
                    redCapture += Time.deltaTime;
                    _machine.SetColorZone(Color.Lerp (Color.white, redCapColor, redCapture / CaptureTime));
                }
            }

            //Else the teams capture time is greater than the capture time
            else
            {
                if (!redIsFull) {
                    _machine.prefabInstance(_machine.Star_B);
                    _machine.prefabInstance(_machine.ShockWave);
                }
                //Set the capture status of the team to true and the enemys team to false
                redCaptureStatus = true;
                redIsFull = true;
            }
        }

 
        //If the enemy teams capture time is greater than 0
        if (!redOnZone && redCapture > 0) {
            redIsFull = false;
            //Subtract the enemy teams capture time
            if (redCaptureStatus) {
                redCapture -= Time.deltaTime/2;
                _machine.SetColorZone(Color.Lerp (Color.white, redCapColor, redCapture / CaptureTime));
            } else {            
                redCapture -= Time.deltaTime;
                _machine.SetColorZone(Color.Lerp (Color.white, redCapColor, redCapture / CaptureTime));
            }

        } else if (!redOnZone && redCapture <= 0) {
            redCaptureStatus = false;
        }
        //If the enemy teams capture time is greater than 0
        if (!blueOnZone && blueCapture > 0) {
            blueIsFull = false;
            //Subtract the enemy teams capture time
            if (blueCaptureStatus) {
                blueCapture -= Time.deltaTime/2;
                _machine.SetColorZone(Color.Lerp (Color.white, blueCapColor, blueCapture / CaptureTime));
            } else {
                blueCapture -= Time.deltaTime;
                _machine.SetColorZone(Color.Lerp (Color.white, blueCapColor, blueCapture / CaptureTime));
            }
        } else if (!blueOnZone && blueCapture <= 0) {
            blueCaptureStatus = false;
        }
        
        if (redCapture > 0 && redCaptureStatus) {
            redPts += Time.deltaTime;
            _machine.redPts.text = (Mathf.FloorToInt(redPts)).ToString();
        }
        if (blueCapture > 0 && blueCaptureStatus) {
            bluePts += Time.deltaTime;
            _machine.bluePts.text = (Mathf.FloorToInt(bluePts)).ToString();;
        }

    }

    public override void FixedUpdateState()
    {

    }

    public override void LeaveState(string tag)
    {
        _machine.LastCState = EcaptureState.LEAVE;
        if (tag == redTeam) {
            redOnZone = false;
        }
        if (tag == blueTeam) {
            blueOnZone = false;
        }
         Debug.Log(tag + " LEAVE " + redTeam);
        Debug.Log("--------------RED----------" + redOnZone);
        Debug.Log("--------------BLUE----------" + blueOnZone);
    }

    public override void ConflictState() {}
    
    void LowerHealth()
    {
    }

}
