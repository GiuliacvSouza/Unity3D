using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI do Game Over")]
    public GameObject painelGameOver; 
    public TextMeshProUGUI textoPontuacaoFinal;

    private ScoreManager scoreManager; 

    void Start()
    {
        // Padrão Unity 6 para não dar aviso no console
        scoreManager = Object.FindFirstObjectByType<ScoreManager>();
        
        // Garante que o painel comece escondido e o tempo rodando
        if(painelGameOver != null) painelGameOver.SetActive(false);
        Time.timeScale = 1; 
    }

    public void AcionarGameOver()
    {
        // 1. Para o tempo e salva o recorde
        if (scoreManager != null)
        {
            scoreManager.PararCronometro(); 
            textoPontuacaoFinal.text = "Pontos: " + scoreManager.PegarPontuacaoFormatada();
        }

        // 2. Pausa o mundo físico
        Time.timeScale = 0; 

        // 3. Mostra a tela
        if (painelGameOver != null)
        {
            painelGameOver.SetActive(true);
        }
    }

    public void ReiniciarJogo()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}