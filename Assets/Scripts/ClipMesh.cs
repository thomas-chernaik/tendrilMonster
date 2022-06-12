using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class ClipMesh : MonoBehaviour
{
    public ComputeShader shader;
    public float meshLength;
    //public Slider scaler;
    public float scaler;
    SkinnedMeshRenderer meshRenderer;
    Mesh newMesh;
    List<Vector3> originalVertices;
    ComputeBuffer verticesInBuffer;
    ComputeBuffer verticesOutBuffer;
    static readonly int vertsInID = Shader.PropertyToID("verticesIn");
    static readonly int vertsOutID = Shader.PropertyToID("verticesOut");
    int kernelHandle;

    public void DisposeBuffers()
    {
        verticesInBuffer.Dispose();
        verticesOutBuffer.Dispose();
    }
    // Start is called before the first frame update
    void Start()
    {

        shader = Instantiate(shader);
        //scaler.onValueChanged.AddListener(delegate { UpdateMesh(); });
        meshRenderer = GetComponent<SkinnedMeshRenderer>();

        originalVertices = meshRenderer.sharedMesh.vertices.ToList();
        verticesInBuffer = new ComputeBuffer(originalVertices.Count, sizeof(float) * 3);
        verticesOutBuffer = new ComputeBuffer(originalVertices.Count, sizeof(float) * 3);
        kernelHandle = shader.FindKernel("CSMain");
        shader.SetBuffer(kernelHandle, vertsInID, verticesInBuffer);
        shader.SetBuffer(kernelHandle, vertsOutID, verticesOutBuffer);
        verticesInBuffer.SetData(originalVertices);
        newMesh = new Mesh();
        newMesh.vertices = meshRenderer.sharedMesh.vertices;
        newMesh.triangles = meshRenderer.sharedMesh.triangles;
        newMesh.bounds = meshRenderer.sharedMesh.bounds;
        newMesh.SetUVs(0, meshRenderer.sharedMesh.uv);
        newMesh.boneWeights = meshRenderer.sharedMesh.boneWeights;
        newMesh.bindposes = meshRenderer.sharedMesh.bindposes;
        newMesh.normals = meshRenderer.sharedMesh.normals;
        meshRenderer.sharedMesh = newMesh;
        UpdateMesh();
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
    public void UpdateMesh()
    {
        if(meshRenderer == null)
        {
            print("hey");
        }
        shader.SetFloat("length", scaler * meshLength);
        shader.Dispatch(kernelHandle, 1+(int)Mathf.Ceil(originalVertices.Count / 64), 1, 1);
        meshRenderer.sharedMesh.SetVertices(getVertices());

    }
    private void Update()
    {
        UpdateMesh();
    }
}
