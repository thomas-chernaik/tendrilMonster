using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterArm : MonoBehaviour
{
    public float intervalBetweenAttacks;
    public float attacksPerGo;
    //the amount to change the velocity of the sway each frame
    public float swayAmount;
    public float radiusChangeAmount;
    //the radius of the swayer
    public float swayRadius;
    public float minSwayRadius;
    public float maxSwayRadius;
    //the amount to accelerate the tentacle back to the centre based on the distance it is from it
    //public float swayDistanceMultiplier;
    public Transform centre;
    public Transform target;
    public ClipMesh clipper;
    public bool swaying;
    //the velocity the arm is swaying at
    //Vector3 swayVelocity;
    //the coords of the arm relative to the centre
    Vector3 armPosition;
    public float num;

   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    /*
     //This method has issues due to it going through the tentacle, which causes upsetting behaviour from the tentacle
    void Swayer()
    {
        //add a random amount of velocity to the velocity
        swayVelocity += Time.deltaTime * swayAmount * new Vector3(Mathf.PerlinNoise(0.1f*Time.time, 0.1f * (1 + Time.time)) - 0.47f, 0, Mathf.PerlinNoise(0.1f * (10000 + Time.time), 0.1f   * (10001 + Time.time)) - 0.47f);
        print(swayVelocity);
        num += Mathf.PerlinNoise(10000 * Time.time, 10000 * (1 + Time.time)) - 0.47f;
        //add a velocity toward the centre
        swayVelocity += Time.deltaTime * swayDistanceMultiplier * (centre.position - transform.position);
        //move in the direction of the velocity
        transform.position += Time.deltaTime * swayVelocity;
    }
    */
    void RadiusChanger()
    {
        //add a random value to the radius using structured perlin noise to give continuous movement
        swayRadius += Time.deltaTime * radiusChangeAmount * (Mathf.PerlinNoise(1f * (1000000 + Time.time), 1f * (10000000 + Time.time)) - 0.47f);
        if(swayRadius > maxSwayRadius)
        {
            swayRadius = maxSwayRadius;
        }
        else if(swayRadius < minSwayRadius)
        {
            swayRadius = minSwayRadius;
        }
    }
    void Swayer()
    {
        RadiusChanger();
        //add a random value to the arm position using structured perlin noise to give continuous movement
        armPosition += Time.deltaTime * swayAmount * new Vector3(Mathf.PerlinNoise(0.1f * Time.time, 0.1f * (1 + Time.time)) - 0.47f, 0, Mathf.PerlinNoise(0.1f * (10000 + Time.time), 0.1f * (10001 + Time.time)) - 0.47f);
        armPosition.Normalize();
        armPosition *= swayRadius;
        transform.position = centre.position + armPosition;
    }
    // Update is called once per frame
    void Update()
    {
        if (swaying)
        {
            Swayer();
        }
    }
}
