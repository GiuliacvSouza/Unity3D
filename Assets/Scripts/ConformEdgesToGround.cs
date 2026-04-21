using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class ConformEdgesToGround : MonoBehaviour
{
    public float rayHeight = 5f;
    public float rayDistance = 15f;
    public LayerMask groundLayer;

    [Range(0f, 0.5f)]
    public float edgeThresholdNormalized = 0.12f;

    void Awake()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        Mesh mesh = Instantiate(mf.sharedMesh);
        mesh.MarkDynamic();
        mf.mesh = mesh;

        Vector3[] baseVertices = mesh.vertices;

        Vector3 min = baseVertices[0];
        Vector3 max = baseVertices[0];
        foreach (var v in baseVertices)
        {
            min = Vector3.Min(min, v);
            max = Vector3.Max(max, v);
        }

        float threshX = (max.x - min.x) * edgeThresholdNormalized;
        float threshZ = (max.z - min.z) * edgeThresholdNormalized;

        Vector3[] vertices = new Vector3[baseVertices.Length];

        for (int i = 0; i < baseVertices.Length; i++)
        {
            Vector3 local = baseVertices[i];
            Vector3 worldPos = transform.TransformPoint(local);

            bool isEdge =
                (local.x - min.x) < threshX ||
                (max.x - local.x) < threshX ||
                (local.z - min.z) < threshZ ||
                (max.z - local.z) < threshZ;

            if (isEdge)
            {
                Vector3 rayOrigin = worldPos + Vector3.up * rayHeight;
                RaycastHit hit;

                if (Physics.Raycast(rayOrigin, Vector3.down, out hit, rayDistance, groundLayer))
                {
                    // Se o chão está abaixo do vértice, desce até ele
                    // Se o chão está acima (vértice enterrado), sobe até ele
                    // Assim funciona nos dois casos
                    worldPos.y = hit.point.y;
                }
            }

            vertices[i] = transform.InverseTransformPoint(worldPos);
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}