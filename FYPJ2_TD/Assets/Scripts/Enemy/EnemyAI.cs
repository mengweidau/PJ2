using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float speed = 10.0f;
    private Transform target = null;
    private int waypointIndex = 0;

    //public Transform destinationPoint;

    //public StateMachine sm;
    //private AudioSource m_audioSource;
    //[SerializeField] private AudioClip m_NearSound;
    

    // Use this for initialization
    void Start()
    {
        //m_audioSource = GetComponent<AudioSource>();

        //sm = new StateMachine();
        //sm.AddState(new Idle("Idle", this));
        //sm.AddState(new Patrol("Patrol", this));
        //sm.AddState(new Search("Search", this));
        //sm.AddState(new Chase("Chase", this));

        target = Waypoints.waypoints[0];
    }

    // Update is called once per frame
    void Update()
    {
        //sm.Update();
        //print(sm.GetCurrentState());

        Vector3 direction = target.position - transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) <= 0.1f)
        {
            GetNextwaypoint();
        }

        //transform.GetComponent<NavMeshAgent>().destination = destinationPoint.position;
    }

    void GetNextwaypoint()
    {
        if(waypointIndex >= Waypoints.waypoints.Length-1)
        {
            //this.transform.position = new Vector3(-0.59f, -5.0f, -3.22f);
            waypointIndex = 0;
            Destroy(gameObject);
            Debug.Log("Enemy has escaped, health decreases!");
        }
        waypointIndex++;
        target = Waypoints.waypoints[waypointIndex];

    }

    // Collision
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        print("Enemy Collided With Player!");
    //    }
    //}

    //public void PlayPlayerIsNearbySound()
    //{
    //    m_audioSource.clip = m_NearSound;
    //    m_audioSource.Play();
    //}
}
