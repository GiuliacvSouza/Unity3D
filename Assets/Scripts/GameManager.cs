using UnityEngine;
using UnityEngine.SceneManagement;

// Gerencia o estado geral do jogo (game over e reinício)
// Delega toda a lógica de UI e pontuação ao ScoreManager
public class GameManager : MonoBehaviour
{
    private ScoreManager scoreManager;

    void Start()
    {
        // Busca o ScoreManager na cena (padrão Unity 6)
        scoreManager = Object.FindFirstObjectByType<ScoreManager>();
    }

    // Chamado quando o player colide com um obstáculo
    public void AcionarGameOver()
    {
        if (scoreManager != null)
        {
            // O ScoreManager já cuida de parar o tempo, salvar recorde e mostrar o painel
            scoreManager.PararCronometro();
        }
    }

    // Chamado pelo botão Reiniciar no painel de Game Over
    public void ReiniciarJogo()
    {
        // Avisa o ScoreManager que a cena foi reiniciada via botão
        // para que ele pule o menu inicial e comece o jogo direto
        ScoreManager.veioDoReiniciar = true;

        Time.timeScale = 1; // Garante que o tempo volta antes de recarregar
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
