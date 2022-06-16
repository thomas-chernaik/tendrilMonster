using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterWalk : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Transform player;
    public LayerMask ground;
    public float walkPointRange;
    public float sightRange;
    Vector3 walkPoint;
    RaycastHit hit;
    
    // Start is called before the first frame update
    void Start()
    {
        GetNewWalkPoint();
    }
    bool IsPlayerInRange()
    {
        return Vector3.Distance(player.position, transform.position) < sightRange;
    }
    void GetNewWalkPoint()
    {
        //get a new random walkpoint
        walkPoint = new Vector3(Random.Range(-walkPointRange, walkPointRange), 0, Random.Range(-walkPointRange, walkPointRange));
        //check its in the world
        if (Physics.Raycast(walkPoint + 10 * Vector3.up, -Vector3.up, out hit, Mathf.Infinity, ground))
        {
            walkPoint = hit.point;
            navMeshAgent.SetDestination(walkPoint);
        }
        else
        {
            //we haven't found a point in the world
            GetNewWalkPoint();
        }
    }
    void Wander()
    {
        if(Vector3.Distance(walkPoint, transform.position) < 1f)
        {
            GetNewWalkPoint();
        }

    }
    void Chase()
    {
        navMeshAgent.SetDestination(player.position);
    }
    // Update is called once per frame
    void Update()
    {
        if (IsPlayerInRange())
        {
            Chase();
        }
        else
        {
            Wander();
        }
    }

}
