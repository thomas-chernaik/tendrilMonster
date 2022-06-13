using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DitzelGames.FastIK;

public class FootPoint : MonoBehaviour
{
    public Transform centre;
    public float despawnDist;
    public LayerMask mask;
    public GameObject tentacle;
    public MonsterLegs legs;
    GameObject selfTentacle;
    // Start is called before the first frame update
    void Start()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + 10 * Vector3.up, transform.TransformDirection(-Vector3.up), out hit, Mathf.Infinity, mask))
        {
            transform.position = hit.point;
        }
        else
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit, Mathf.Infinity, mask))
            {

                transform.position = hit.point;
            }
            else
            {
                legs.Fall();
            }
        }
        selfTentacle = Instantiate(tentacle);
        selfTentacle.transform.parent = centre;
        selfTentacle.transform.localPosition = Vector3.zero;
        Destroy(selfTentacle.GetComponent<MonsterLeg>().foot.GetComponent<FastIKFabric>().Target.gameObject);
        selfTentacle.GetComponent<MonsterLeg>().foot.GetComponent<FastIKFabric>().Target = transform;
    }
    void Despawn()
    {
        selfTentacle.GetComponent<MonsterLeg>().Despawn();
        MonsterLegs.footCount--;
        //print("despawn");
        centre.GetComponent<MonsterLegs>().SpawnFootPoint(0);
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position - Vector3.up * transform.position.y, centre.position - centre.position.y * Vector3.up) > despawnDist)
        {
            Despawn();
        }
    }
}
