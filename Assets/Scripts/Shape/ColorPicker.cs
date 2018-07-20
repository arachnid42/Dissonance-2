using UnityEngine;
using System.Collections.Generic;

public class ColorPicker : MonoBehaviour
{
    public bool staticColor = false;
    public Color color;
    public LayerMask layer;
    public Vector3 direction = Vector3.forward;
    public float radius = 100f;
    public float distance = 100f;

    public Vector3 GetRayOrigin()
    {
        return transform.position - direction * radius;
    }

    public Ray GetRay()
    {
        return new Ray(GetRayOrigin(),direction);
    }

    public void OnDrawGizmos()
    {
        Color prev = Gizmos.color;
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(GetRayOrigin(),radius/20);
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(GetRayOrigin(), GetRayOrigin() + direction * distance);
        Gizmos.color = prev;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(GetRay());
    }

    public Color GetColor()
    {
        if (staticColor)
        {
            return GetComponentInChildren<MeshRenderer>().sharedMaterial.color;
        }
        
        RaycastHit raycastHit = new RaycastHit();
        Physics.Raycast(GetRay(), out raycastHit, distance, layer);
        

        if(raycastHit.collider == null || raycastHit.triangleIndex < 0 || raycastHit.collider.tag != "ShapeMesh")
        {
            return Color.black;
        }
        //FIX: to avoid problem when two shapes overlap
        Mesh mesh = raycastHit.collider.GetComponent<MeshFilter>().mesh;
        MeshRenderer renderer = raycastHit.collider.GetComponent<MeshRenderer>();

        int[] hittedTriangle = new int[3];
        hittedTriangle[0] = mesh.triangles[raycastHit.triangleIndex * 3];
        hittedTriangle[1] = mesh.triangles[raycastHit.triangleIndex * 3 + 1];
        hittedTriangle[2] = mesh.triangles[raycastHit.triangleIndex * 3 + 2];


        for(int i = 0; i < mesh.subMeshCount; i++)
        {
            int[] subMeshTriangles = mesh.GetTriangles(i);
            for(int j = 0; j < subMeshTriangles.Length; j+=3)
            {
                if(hittedTriangle[0] == subMeshTriangles[j] && hittedTriangle[1] == subMeshTriangles[j+1] && hittedTriangle[2] == subMeshTriangles[j + 2])
                {
                    color = renderer.sharedMaterials[i].color;
                    return color;
                }
            }
        }
        return Color.black;
    }

}
