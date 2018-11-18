using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    [SerializeField] private ArcherTower parentTower;
    [SerializeField] private GameObject arrowTarget;
    [SerializeField] private Entity entityClass;
    [SerializeField] private Transform arrowTargetTransform;
    [SerializeField] int arrowTravelSpeed = 3, arrowRotateSpeed = 5;
    
    // Update is called once per frame
    void Update () {
		if (arrowTarget != null)
        {
            Vector3 targetDir = arrowTargetTransform.position - transform.position;
            float step = arrowRotateSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);

            transform.Translate(Vector3.forward * arrowTravelSpeed * Time.deltaTime);

            float dist = Vector3.Distance(arrowTargetTransform.position, transform.position);
            if (dist < 0.5f)
            {
                entityClass.DecreaseHealth(parentTower.GetAttackDmg());
                if (entityClass.GetHealth() <= 0)
                {
                    parentTower.RemoveTarget(arrowTarget);
                }

                Destroy(gameObject);
            }
        }
	}

    public void SetArrowTarget(GameObject target)
    {
        arrowTarget = target;
        arrowTargetTransform = target.transform;
        if (target.GetComponent<Entity>())
            entityClass = target.GetComponent<Entity>();
    }

    public void SetParentTower(ArcherTower parent)
    {
        parentTower = parent;
    }
}
