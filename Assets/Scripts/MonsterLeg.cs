using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLeg : MonoBehaviour
{
    public float despawnTime;
    public GameObject foot;
    public float timeLeft;
    bool isDespawning;
    public float spawnTime;
    bool isSpawning = true;
    // Start is called before the first frame update
    void Start()
    {
        timeLeft = spawnTime;
    }
    public void Despawn()
    {
        if(isSpawning)
        {
            isSpawning = false;
            isDespawning = true;
            return;
        }
        timeLeft = despawnTime;
        isDespawning = true;
    }
    // Update is called once per frame
    void Update()
    {
        if(timeLeft < 0)
        {
            transform.GetComponentInChildren<ClipMesh>().UpdateMesh();

            if (isDespawning)
            {
                transform.GetComponentInChildren<ClipMesh>().DisposeBuffers();
                Destroy(gameObject);
            }
            if(isSpawning)
            {
                isSpawning = false;
                timeLeft = despawnTime;

            }
        }
        else if(isDespawning)
        {
            timeLeft -= Time.deltaTime;
            transform.GetComponentInChildren<ClipMesh>().scaler = timeLeft / despawnTime;
            transform.GetComponentInChildren<ClipMesh>().UpdateMesh();

        }
        else if(isSpawning)
        {
            timeLeft -= Time.deltaTime;
            transform.GetComponentInChildren<ClipMesh>().scaler = 1- timeLeft / spawnTime;
            transform.GetComponentInChildren<ClipMesh>().UpdateMesh();

        }
    }

}
