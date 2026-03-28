using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI textoPontuacao;
    public TextMeshProUGUI textoRecorde; // campo para mostrar o High Score
    
    private float tempoDecorrido = 0f;
    private bool jogoAtivo = true;

    void Start()
    {
        // Ao começar, carrega o recorde salvo. Se não houver nada, o padrão é 0.
        float recordeSalvo = PlayerPrefs.GetFloat("HighScore", 0);
        textoRecorde.text = "Recorde: " + recordeSalvo.ToString("0");
    }

    void Update()
    {
        if (jogoAtivo)
        {
            tempoDecorrido += Time.deltaTime;
            textoPontuacao.text = "Pontos: " + tempoDecorrido.ToString("0");
        }
    }

    public void PararCronometro()
    {
        jogoAtivo = false;
        VerificarRecorde();
    }

    void VerificarRecorde()
    {
        float recordeAtual = PlayerPrefs.GetFloat("HighScore", 0);

        // Se a pontuação de agora for maior que o recorde salvo...
        if (tempoDecorrido > recordeAtual)
        {
            // Salva o novo valor!
            PlayerPrefs.SetFloat("HighScore", tempoDecorrido);
            
            PlayerPrefs.Save(); // Garante que foi escrito no disco
            
            textoRecorde.text = "Novo Recorde: " + tempoDecorrido.ToString("0");
            Debug.Log("Novo Recorde Salvo!");
        }
    }

   public string PegarPontuacaoFormatada()
   {
    return tempoDecorrido.ToString("0");
    }
}