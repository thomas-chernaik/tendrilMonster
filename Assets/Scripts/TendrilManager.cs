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
    static readonly int vertID = Shader.PropertyToID("vertices");
    static readonly int triID = Shader.PropertyToID("triangles");


    // Start is called before the first frame update
    void Start()
    {
        verticesBuffer = new ComputeBuffer(30, sizeof(float) * 3);
        trianglesBuffer = new ComputeBuffer(30, sizeof(float) * 3);
        shader.SetBuffer(kernelHandle, vertID, verticesBuffer);
        shader.SetBuffer(kernelHandle, triID, trianglesBuffer);

        meshFilter = GetComponent<MeshFilter>();
    }
    void updateMesh()
    {
        var mesh = new Mesh();

    }

    // Update is called once per frame
    void Update()
    {
        
        kernelHandle = shader.FindKernel("CSMain");
        shader.Dispatch(kernelHandle, 8, 1, 1);

    }
}
