using UnityEngine;
using TMPro; // Necessário para usar o TextMeshPro

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI textoPontuacao; // Arraste seu texto aqui no Inspector
    private float tempoDecorrido = 0f;
    private bool jogoAtivo = true;

    void Update()
    {
        if (jogoAtivo)
        {
            // Soma o tempo que passou desde o último frame
            tempoDecorrido += Time.deltaTime;

            // Formata o tempo para aparecer sem as casas decimais (ou como preferir)
            // "0" significa número inteiro. "F2" mostraria 2 casas decimais.
            textoPontuacao.text = "Pontos: " + tempoDecorrido.ToString("0");
        }
    }

    // Função para parar o contador quando o player morrer, por exemplo
    public void PararCronometro()
    {
        jogoAtivo = false;
    }

    
}