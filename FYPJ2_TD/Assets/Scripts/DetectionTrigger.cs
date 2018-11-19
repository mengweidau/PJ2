using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionTrigger : MonoBehaviour {
    [SerializeField] private Entity parent;

    private void Awake()
    {
        parent = transform.parent.GetComponent<Entity>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //add other.gameobj if it is an enemy/has enemy tag
        if (other.CompareTag("Enemy")) //find out why it adds the 1st detected enemy twice when enemy moves too fast
        {
            for (int i = 0; i < parent.GetTargets().Count; ++i)
            {
                if (other.gameObject != parent.GetTargets()[i])
                    parent.GetTargets().Add(other.gameObject);
            }
        }
    }

    //remove potential target
    private void OnTriggerExit(Collider other)
    {
        //add other.gameobj if it is an enemy/has enemy tag
        if (other.CompareTag("Enemy"))
        {
            parent.GetTargets().Remove(other.gameObject);
            parent.GetAttackingTargets().Remove(other.gameObject);
        }
    }
}
