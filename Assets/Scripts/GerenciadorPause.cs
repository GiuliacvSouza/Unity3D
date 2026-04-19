using UnityEngine;

public class GerenciadorPause : MonoBehaviour
{
    public GameObject menuPauseUI; // Arraste o objeto MenuPause para cá
    private bool jogoPausado = false;

    void Update()
    {
        // Verifica se a tecla ESC foi pressionada
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (jogoPausado)
            {
                Retomar();
            }
            else
            {
                Pausar();
            }
        }
    }

    public void Retomar()
    {
        menuPauseUI.SetActive(false); // Esconde o menu
        Time.timeScale = 1f;          // Faz o tempo do jogo voltar ao normal
        jogoPausado = false;

        // Opcional: Esconde o mouse ou trava o cursor se seu jogo for 3D
        // Cursor.lockState = CursorLockMode.Locked;
    }

    void Pausar()
    {
        menuPauseUI.SetActive(true);  // Mostra o menu
        Time.timeScale = 0f;          // Congela o tempo do jogo
        jogoPausado = true;

        // Opcional: Libera o mouse para clicar nos botões
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}