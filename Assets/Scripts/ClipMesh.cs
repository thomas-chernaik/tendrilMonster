using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class ClipMesh : MonoBehaviour
{
    public ComputeShader shader;
    public float scale;
    public float meshLength;
    MeshFilter meshFilter;
    SkinnedMeshRenderer meshRenderer;
    Mesh newMesh;
    List<Vector3> originalVertices;
    ComputeBuffer verticesInBuffer;
    ComputeBuffer verticesOutBuffer;
    static readonly int vertsInID = Shader.PropertyToID("verticesIn");
    static readonly int vertsOutID = Shader.PropertyToID("verticesOut");
    int kernelHandle;


    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<SkinnedMeshRenderer>();
        originalVertices = meshFilter.sharedMesh.vertices.ToList();
        verticesInBuffer = new ComputeBuffer(originalVertices.Count, sizeof(float) * 3);
        verticesOutBuffer = new ComputeBuffer(originalVertices.Count, sizeof(float) * 3);
        kernelHandle = shader.FindKernel("CSMain");
        shader.SetBuffer(kernelHandle, vertsInID, verticesInBuffer);
        shader.SetBuffer(kernelHandle, vertsOutID, verticesOutBuffer);
        verticesInBuffer.SetData(originalVertices);
        newMesh = new Mesh();
        newMesh.vertices = meshFilter.sharedMesh.vertices;
        newMesh.triangles = meshFilter.sharedMesh.triangles;
        newMesh.bounds = meshFilter.sharedMesh.bounds;
        newMesh.SetUVs(0, meshFilter.sharedMesh.uv);
        newMesh.boneWeights = meshFilter.sharedMesh.boneWeights;
        newMesh.bindposes = meshFilter.sharedMesh.bindposes;
        newMesh.normals = meshFilter.sharedMesh.normals;
        meshFilter.sharedMesh = newMesh;
        meshRenderer.sharedMesh = newMesh;

    }
    List<Vector3> getVertices()
    {
        float[] verticesArr = new float[originalVertices.Count*3];
        verticesOutBuffer.GetData(verticesArr);
        List<Vector3> toReturn = new List<Vector3>();
        for (int i = 0; i < verticesArr.Length / 3; i++)
        {
            toReturn.Add(new Vector3(verticesArr[i * 3], verticesArr[i * 3 + 1], verticesArr[i * 3 + 2]));
        }
        return toReturn;
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("p"))
        {
            print("p");
            shader.SetFloat("length", scale * meshLength);
            shader.Dispatch(kernelHandle, (int)Mathf.Ceil(originalVertices.Count / 64), 1, 1);
            meshRenderer.sharedMesh.SetVertices(getVertices());
        }

    }
}
