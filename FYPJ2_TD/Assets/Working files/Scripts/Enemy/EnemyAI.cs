using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : Entity
{
    //public float speed = 10.0f;
    private Transform _target = null;
    private int waypointIndex = 0;

    //public Transform destinationPoint;

    //public StateMachine sm;
    //private AudioSource m_audioSource;
    //[SerializeField] private AudioClip m_NearSound;

    //enum EnemyType
    //{
    //    INFANTRY = 0,
    //    HOUND
    //};
    //EnemyType enemyType = EnemyType.INFANTRY;

    public EnemyAI() //constructor, this is for one type of enemy <-- i think that depends on how you plan to make different types of enemies tho
    {
        s_name = "enemy";
    }

    // Use this for initialization
    void Start()
    {
        //switch (enemyType)
        //{
        //    case EnemyType.INFANTRY:

        //        i_health = 1;
        //        i_moveSpeed = 1;
        //        break;
        //    case EnemyType.HOUND:

        //        i_health = 2;
        //        i_moveSpeed = 1;
        //        break;
        //}

        //m_audioSource = GetComponent<AudioSource>();

        //sm = new StateMachine();
        //sm.AddState(new Idle("Idle", this));
        //sm.AddState(new Patrol("Patrol", this));
        //sm.AddState(new Search("Search", this));
        //sm.AddState(new Chase("Chase", this));

        _target = Waypoints.waypoints[0];


    }

    // Update is called once per frame
    void Update()
    {
        //sm.Update();
        //print(sm.GetCurrentState());

        Vector3 direction = _target.position - transform.position;
        transform.Translate(direction.normalized * f_moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _target.position) <= 0.1f)
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
        _target = Waypoints.waypoints[waypointIndex];

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
