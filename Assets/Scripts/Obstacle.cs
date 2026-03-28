using UnityEngine;
using UnityEngine.SceneManagement;

public class Obstacle : MonoBehaviour
{
    // Coloque aqui a MESMA velocidade que está no seu GroundManager (ex: 5)
    public float speed = 5f;
    public float lifeTime = 10f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // O chão move para a esquerda (Vector3.left), o obstáculo também deve mover!
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    // logica de fim de jogo removida daqui
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("O Obstáculo detectou o Player, mas quem manda é o GameManager.");
        }
    }
}