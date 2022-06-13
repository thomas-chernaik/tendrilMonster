using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public MonsterLegs legs;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float xValue = Input.GetAxis("Horizontal");
        float zValue = Input.GetAxis("Vertical");
        legs.moveAngle = Mathf.Atan2(xValue, zValue) - 0.5f * Mathf.PI;
        transform.position += new Vector3(xValue, 0, zValue)*speed;
        if(Input.GetAxis("Jump") > 0)
        {
            legs.Jump();
        }
    }
}
