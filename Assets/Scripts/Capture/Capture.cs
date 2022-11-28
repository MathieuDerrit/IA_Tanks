using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Capture : MonoBehaviour
{
    public TMP_Text scoreBlue;
    public TMP_Text scoreRed;

    // Start is called before the first frame update
    void Start()
    {
        scoreBlue.text = "0";
        scoreRed.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {
      
        Debug.Log(scoreBlue.text);
        if (col.gameObject.tag == "TankBlue") {
            scoreBlue.text = (int.Parse(scoreBlue.text) + 1).ToString();
  
        } else if (col.gameObject.tag == "TankRed"){
            scoreRed.text = (int.Parse(scoreRed.text) + 1).ToString();
        }

    }

}
