using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TendrilManager : MonoBehaviour
{
    public ComputeShader shader;
    public int numTendrils;
    MeshFilter meshFilter;
    int kernelHandle;
    ComputeBuffer verticesBuffer;
    ComputeBuffer trianglesBuffer;


    // Start is called before the first frame update
    void Start()
    {
        verticesBuffer = new ComputeBuffer(30, sizeof(float) * 3);

        meshFilter = GetComponent<MeshFilter>();
    }

    // Update is called once per frame
    void Update()
    {
        
        kernelHandle = shader.FindKernel("CSMain");
        shader.Dispatch(kernelHandle, 8, 1, 1);

    }
}
