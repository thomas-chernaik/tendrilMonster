using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    public Transform target;
    public GameObject arm;
    public float reachDistance;
    public float timeBetweenArms;
    public bool spawnedArm;
    float timer;
    MonsterArm theSpawnedArm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //see if the target is in range
        if(Vector3.Distance(target.position, transform.position) < reachDistance)
        {
             //if there isn't a spawned arm right now
            if(!spawnedArm)
            {
                //count up the timer
                timer += Time.deltaTime;
                //if the timers up
                if(timer > timeBetweenArms)
                {
                    //spawn an arm
                    timer = 0;
                    spawnedArm = true;
                    var obj = Instantiate(arm);
                    obj.transform.parent = transform;
                    obj.transform.localPosition = Vector3.zero;
                    obj.transform.localRotation = Quaternion.identity;
                    theSpawnedArm = obj.GetComponentInChildren<MonsterArm>();
                    theSpawnedArm.target = target;
                }

            }
            
        }
        //else if there is a spawned arm and it isn't in range
        else if(spawnedArm)
        {
            //despawn the spawned arm
            theSpawnedArm.Despawn();
            spawnedArm = false;
        }
    }
}
