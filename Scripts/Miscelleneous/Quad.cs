using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quad : MonoBehaviour
{
    [SerializeField]
    Material material;


    Mesh mesh;
    // Start is called before the first frame update
    void Start()
    {
        Color colour = material.color;
        colour.a = 0.3f;
        material.color = colour;
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = material;

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

        mesh = new Mesh();
        mesh.vertices = new Vector3[4];
        mesh.triangles = new int[6]
        {
            0, 2, 1,
            2, 3, 1
        };

        mesh.normals = new Vector3[4]
            {
                Vector3.up,
                Vector3.up,
                Vector3.up,
                Vector3.up
            };

        mesh.uv = new Vector2[4]
            {
                new Vector2(0, 0), 
                new Vector2(0, 1), 
                new Vector2(1, 0), 
                new Vector2(1, 1)
            
            
            };

        meshFilter.mesh = mesh;
    }

    public void SetVertices(List<Vector3>points)
    {
        mesh.SetVertices(points);
    }

    // Update is called once per frame
    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

}
