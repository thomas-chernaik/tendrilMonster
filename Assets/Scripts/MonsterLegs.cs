using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLegs : MonoBehaviour
{
    public int legCount;
    public float legRadius;
    public GameObject footPoint;
    public float selfFloorOffset;
    public LayerMask mask;
    public float moveAngle;
    public static float footCount = 0;
    public float jumpSpeed;
    float speed = 0;
    float height = 0;
    bool grounded = true;
    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i < legCount; i++)
        {
            SpawnFootPoint((Random.value - 0.5f) * 2 * legRadius);
        }
    }
    public void SpawnFootPoint(float offset)
    {
        footCount++;
        if (footCount > legCount)
        {
            return;
        }
        //set the offset to legRadius if its given as zero
        if(offset == 0)
        {
            offset = legRadius*0.9f;
        }
        //get a random angle to spawn this at
        float angle = (Random.value * Mathf.PI) + moveAngle;
        Vector3 spawnPoint = new Vector3(offset * Mathf.Sin(angle), 0, offset * Mathf.Cos(angle)) + transform.position;
        var obj = Instantiate(footPoint, spawnPoint, Quaternion.identity);
        obj.GetComponent<FootPoint>().centre = transform;
        obj.GetComponent<FootPoint>().legs = this;
        obj.GetComponent<FootPoint>().despawnDist = legRadius;

    }
    public void Jump()
    {
        if(grounded)
        {
            grounded = false;
            height = selfFloorOffset;
            speed = jumpSpeed;
        }

    }
    public void Fall()
    {
        if(grounded)
        {
            grounded = false;
            height = selfFloorOffset;
            speed = 0;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
        if (grounded)
        {
            if (Physics.Raycast(transform.position + 10 * Vector3.up, transform.TransformDirection(-Vector3.up), out hit, Mathf.Infinity, mask))
            {
                transform.position = hit.point + Vector3.up * selfFloorOffset;
            }
        }
        else
        {
            if (Physics.Raycast(transform.position + 10 * Vector3.up, transform.TransformDirection(-Vector3.up), out hit, Mathf.Infinity, mask))
            {
                height = transform.position.y - hit.point.y;
                speed -= 9.81f * Time.deltaTime;
                height = height + speed * Time.deltaTime;
                if(height < selfFloorOffset)
                {
                    grounded = true;
                    height = selfFloorOffset;
                }
                transform.position = hit.point + Vector3.up * height;
            }
            else
            {
                speed -= 9.81f * Time.deltaTime;
                height = height + speed * Time.deltaTime;
                transform.position += transform.up * speed * Time.deltaTime;
            }



        }

    }
}
