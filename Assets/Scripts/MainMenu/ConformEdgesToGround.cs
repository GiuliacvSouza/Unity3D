using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class ConformEdgesToGround : MonoBehaviour
{
    public float rayHeight = 10f;
    public float rayDistance = 20f;
    public float maxOffset = 0.5f;
    public LayerMask groundLayer;

    [Range(0f, 1f)]
    public float edgeThreshold = 0.45f; // define o que é "borda"

    private Mesh mesh;
    private Vector3[] baseVertices;

    void Start()
    {
        MeshFilter mf = GetComponent<MeshFilter>();

        // 🔥 Clona o mesh corretamente
        mesh = Instantiate(mf.sharedMesh);

        // 🔥 Marca como dinâmico (importante!)
        mesh.MarkDynamic();

        mf.mesh = mesh;

        baseVertices = mesh.vertices;
    }

    void Update()
    {
        Vector3[] vertices = new Vector3[baseVertices.Length];

        // calcula limites da malha
        Bounds bounds = mesh.bounds;

        for (int i = 0; i < baseVertices.Length; i++)
        {
            Vector3 local = baseVertices[i];

            // 🔥 detecta se é borda (X ou Z perto do limite)
            bool isEdge =
                Mathf.Abs(local.x - bounds.min.x) < edgeThreshold ||
                Mathf.Abs(local.x - bounds.max.x) < edgeThreshold ||
                Mathf.Abs(local.z - bounds.min.z) < edgeThreshold ||
                Mathf.Abs(local.z - bounds.max.z) < edgeThreshold;

            Vector3 worldPos = transform.TransformPoint(local);

            if (isEdge)
            {
                RaycastHit hit;
                if (Physics.Raycast(worldPos + Vector3.up * rayHeight, Vector3.down, out hit, rayDistance, groundLayer))
                {
                    float delta = hit.point.y - worldPos.y;

                    // limita deformação
                    delta = Mathf.Clamp(delta, -maxOffset, maxOffset);

                    worldPos.y += delta;
                }
            }

            vertices[i] = transform.InverseTransformPoint(worldPos);
        }

        mesh.vertices = vertices;
        
    }
}