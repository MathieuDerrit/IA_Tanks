using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankIA : MonoBehaviour
{
    public float moveSpeed;
    public Transform player;
    public Transform shotPoint;
    public Transform gun;
 
    public Rigidbody m_Shell; 

    public AudioSource m_ShootingAudio;         // Reference to the audio source used to play the shooting audio. NB: different to the movement audio source.
    public AudioClip m_FireClip;                // Audio that plays when each shot is fired.
 
    public float followPlayerRange;
    private bool inRange;
    public float attackRange;
 
    public float startTimeBtwnShots;
    private float timeBtwnShots;
 
    // Update is called once per frame
    void Update()
    {
        Vector3 differance = player.position - gun.transform.position;
        float rotZ = Mathf.Atan2(differance.y, differance.x) * Mathf.Rad2Deg;
        Debug.Log(differance);
        //gun.transform.rotation = Quaternion.Euler(0f, 0f, rotZ);

        Vector3 targetPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(targetPos);
 
        if (Vector2.Distance(transform.position, player.position) <= followPlayerRange && Vector2.Distance(transform.position, player.position) > attackRange)
        {
            inRange = true;
        }
        else
        {
            inRange = false;
        }
 
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            if (timeBtwnShots <= 0)
            {
                //Instantiate(enemyProjectile, shotPoint.position, shotPoint.transform.rotation);

                // Create an instance of the shell and store a reference to it's rigidbody.
                Rigidbody shellInstance =
                    Instantiate(m_Shell, shotPoint.position, shotPoint.transform.rotation) as Rigidbody;

                // Set the shell's velocity to the launch force in the fire position's forward direction.
                shellInstance.velocity = 15f * shotPoint.forward; 

                // Change the clip to the firing clip and play it.
                m_ShootingAudio.clip = m_FireClip;
                m_ShootingAudio.Play ();

                timeBtwnShots = startTimeBtwnShots;
            }
            else
            {
                timeBtwnShots -= Time.deltaTime;
            }
        }
    }
 
    void FixedUpdate()
    {
        if (inRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }
 
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, followPlayerRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
