using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    
    private Vector3 offset;

    void Start()
    {
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        transform.position = new Vector3(
            player.position.x + offset.x,
            transform.position.y,  // mantém Y atual da câmera
            transform.position.z   // mantém Z atual da câmera
        );
    }
}