using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        trianglesBuffer = new ComputeBuffer(90, sizeof(int));
        shader.SetBuffer(kernelHandle, vertID, verticesBuffer);
        shader.SetBuffer(kernelHandle, triID, trianglesBuffer);

        meshFilter = GetComponent<MeshFilter>();
    }
    void updateMesh()
    {
        var mesh = new Mesh();
        List<Vector3> vertices = getVertices();
        int[] triangles = getTriangles();
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();
        meshFilter.sharedMesh = mesh;
        foreach(Vector3 vertex in vertices)
        {
            print(vertex);
        }
    }
    
    List<Vector3> getVertices()
    {
        float[] verticesArr = new float[90];
        verticesBuffer.GetData(verticesArr);
        List<Vector3> toReturn = new List<Vector3>();
        for(int i=0; i<verticesArr.Length/3; i++)
        {
            toReturn.Add(new Vector3(verticesArr[i * 3], verticesArr[i * 3 + 1], verticesArr[i * 3 + 2]));
        }
        return toReturn;
    }
    int[] getTriangles()
    {
        int[] triangleArr = new int[90];
        trianglesBuffer.GetData(triangleArr);
        return triangleArr;

    }
    // Update is called once per frame
    void Update()
    {
        
        kernelHandle = shader.FindKernel("CSMain");
        shader.Dispatch(kernelHandle, 8, 1, 1);
        updateMesh();
    }
}
