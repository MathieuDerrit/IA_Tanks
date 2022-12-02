using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace Complete
{
    public class GameManager : MonoBehaviour
    {
        public float m_StartDelay = 3f;             // The delay between the start of RoundStarting and RoundPlaying phases.
        public float m_EndDelay = 3f;               // The delay between the end of RoundPlaying and RoundEnding phases.
        public CameraControl m_CameraControl;       // Reference to the CameraControl script for control during different phases.
        public Text m_MessageText;                  // Reference to the overlay Text to display winning text, etc.
        public GameObject m_TankPrefab;             // Reference to the prefab the players will control.
        public GameObject m_TankPrefabIA;
        public TankManager[] m_Tanks;               // A collection of managers for enabling and disabling different aspects of the tanks.

        public TMP_Text bluePts; 
        public TMP_Text redPts;

        public TMP_Text Timer;  
        public float timing = 30.0f;

        private WaitForSeconds m_StartWait;         // Used to have a delay whilst the round starts.
        private WaitForSeconds m_EndWait;           // Used to have a delay whilst the round or game ends.
        private TankManager m_RoundWinner;          // Reference to the winner of the current round.  Used to make an announcement of who won.
        private TankManager m_GameWinner;           // Reference to the winner of the game.  Used to make an announcement of who won.


        const float k_MaxDepenetrationVelocity = float.PositiveInfinity;

        
        private void Start()
        {
            // This line fixes a change to the physics engine.
            Physics.defaultMaxDepenetrationVelocity = k_MaxDepenetrationVelocity;
            
            // Create the delays so they only have to be made once.
            m_StartWait = new WaitForSeconds (m_StartDelay);
            m_EndWait = new WaitForSeconds (m_EndDelay);

            //SpawnAllTanks();
            SpawnAllTanks_JVSIA();
            SetCameraTargets();

            // Once the tanks have been created and the camera is using them as targets, start the game.
            StartCoroutine (GameLoop());
        }

        void Update() 
        {     
            if(timing>0)     
            {         
                timing -= Time.deltaTime;     
            }     
            double b = System.Math.Round (timing, 2);     
            Timer.text = b.ToString ();     
        }

        private void SpawnAllTanks()
        {
            // For all the tanks...
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                // ... create them, set their player number and references needed for control.
                m_Tanks[i].m_Instance =
                    Instantiate(m_TankPrefab, m_Tanks[i].m_SpawnPoint.position, m_Tanks[i].m_SpawnPoint.rotation) as GameObject;
                m_Tanks[i].m_PlayerNumber = i + 1;
                if (m_Tanks[i].m_PlayerNumber == 1) {
                    m_Tanks[i].m_Instance.gameObject.tag = "TankBlue";
                } else {
                    m_Tanks[i].m_Instance.gameObject.tag = "TankRed";
                }
                m_Tanks[i].Setup();
            }
        }

        private void SpawnAllTanks_JVSIA()
        { 
            m_Tanks[0].m_Instance =
                Instantiate(m_TankPrefab, m_Tanks[0].m_SpawnPoint.position, m_Tanks[0].m_SpawnPoint.rotation) as GameObject;
            m_Tanks[0].m_PlayerNumber = 1;
            m_Tanks[0].m_Instance.gameObject.tag="TankBlue";
            Debug.Log(m_Tanks[0].m_Instance.gameObject.tag);
            m_Tanks[0].Setup();   

            m_Tanks[1].m_Instance =
                Instantiate(m_TankPrefabIA, m_Tanks[1].m_SpawnPoint.position, m_Tanks[1].m_SpawnPoint.rotation) as GameObject;
            m_Tanks[1].m_PlayerNumber = 2;
            m_Tanks[1].m_Instance.gameObject.tag="TankRed";
            m_Tanks[1].isPlayer = false;
            m_Tanks[1].ennemy = m_Tanks[0].m_Instance;
            Debug.Log(m_Tanks[1].m_Instance.gameObject.tag);
            m_Tanks[1].Setup();   
        }


        private void SetCameraTargets()
        {
            // Create a collection of transforms the same size as the number of tanks.
            Transform[] targets = new Transform[m_Tanks.Length];

            // For each of these transforms...
            for (int i = 0; i < targets.Length; i++)
            {
                // ... set it to the appropriate tank transform.
                targets[i] = m_Tanks[i].m_Instance.transform;
            }

            // These are the targets the camera should follow.
            m_CameraControl.m_Targets = targets;
        }


        // This is called from start and will run each phase of the game one after another.
        private IEnumerator GameLoop ()
        {
            // Start off by running the 'RoundStarting' coroutine but don't return until it's finished.
            yield return StartCoroutine (GameStarting ());

            // Once the 'RoundStarting' coroutine is finished, run the 'RoundPlaying' coroutine but don't return until it's finished.
            yield return StartCoroutine (GamePlaying());

            // Once execution has returned here, run the 'RoundEnding' coroutine, again don't return until it's finished.
            yield return StartCoroutine (GameEnding());

            // This code is not run until 'RoundEnding' has finished.  At which point, check if a game winner has been found.

                SceneManager.LoadScene (0);
 
        }


        private IEnumerator GameStarting ()
        {
            // As soon as the round starts reset the tanks and make sure they can't move.
            DisableTankControl ();

            // Snap the camera's zoom and position to something appropriate for the reset tanks.
            m_CameraControl.SetStartPositionAndSize ();

            // Wait for the specified length of time until yielding control back to the game loop.
            yield return m_StartWait;
        }


        private IEnumerator GamePlaying ()
        {
            // As soon as the round begins playing let the players control the tanks.
            EnableTankControl ();

            // Clear the text from the screen.
            m_MessageText.text = string.Empty;   

            // While there is not one tank left...
            while (!OneTankLeft() && timing > 0)
            {
                // ... return on the next frame.
                yield return null;
            }
        }


        private IEnumerator GameEnding ()
        {
            // Stop tanks from moving.
            DisableTankControl ();

            // Clear the winner from the previous round.
            m_RoundWinner = null;

            // See if there is a winner now the round is over.
            m_RoundWinner = GetRoundWinner ();

            // If there is a winner, increment their score.
            if (m_RoundWinner != null)
                m_RoundWinner.m_Wins++;

            // Now the winner's score has been incremented, see if someone has one the game.
            m_GameWinner = GetGameWinner ();

            // Get a message based on the scores and whether or not there is a game winner and display it.
            string message = EndMessage ();
            m_MessageText.text = message;

            // Wait for the specified length of time until yielding control back to the game loop.
            yield return m_EndWait;
        }


        // This is used to check if there is one or fewer tanks remaining and thus the round should end.
        private bool OneTankLeft()
        {
            // Start the count of tanks left at zero.
            int numTanksLeft = 0;

            // Go through all the tanks...
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                // ... and if they are active, increment the counter.
                if (m_Tanks[i].m_Instance.activeSelf)
                    numTanksLeft++;
            }

            // If there are one or fewer tanks remaining return true, otherwise return false.
            return numTanksLeft <= 1;
        }
        
        
        // This function is to find out if there is a winner of the round.
        // This function is called with the assumption that 1 or fewer tanks are currently active.
        private TankManager GetRoundWinner()
        {
            // Go through all the tanks...
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                // ... and if one of them is active, it is the winner so return it.
                if (m_Tanks[i].m_Instance.activeSelf)
                    return m_Tanks[i];
            }

            // If none of the tanks are active it is a draw so return null.
            return null;
        }


        // This function is to find out if there is a winner of the game.
        private TankManager GetGameWinner()
        {
            if (int.Parse(bluePts.text) > int.Parse(redPts.text)) {
                return m_Tanks[0];
            }
            else if (int.Parse(bluePts.text) < int.Parse(redPts.text)) {
                return m_Tanks[1];
            }
            /*
            // Go through all the tanks...
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                // ... and if one of them has enough rounds to win the game, return it.
                if (m_Tanks[i].m_Wins == 1)
                    return m_Tanks[i];
            }
*/
            // If no tanks have enough rounds to win, return null.
            return null;
        }


        // Returns a string message to display at the end of each round.
        private string EndMessage()
        {

            string message = "";
            
            // If there is a game winner, change the entire message to reflect that.
            if (m_GameWinner != null) {
                message = m_GameWinner.m_ColoredPlayerText + " GAGNE LA PARTIE !";
            } else {
                message = "Egalite";
            }
                
                message += "\n\n";
                if(m_GameWinner == m_Tanks[0]){
                    message += "Son score : " + bluePts.text;
                }

                if(m_GameWinner == m_Tanks[1]){
                    message += "Son score : " + redPts.text;
                }

            return message;
        }

        private void EnableTankControl()
        {
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                m_Tanks[i].EnableControl();
            }
        }


        private void DisableTankControl()
        {
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                m_Tanks[i].DisableControl();
            }
        }
    }
}