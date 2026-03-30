using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// Gerencia toda a UI do jogo: pontuação, recorde, painéis de menu e game over
public class ScoreManager : MonoBehaviour
{
    [Header("Textos de UI")]
    public TextMeshProUGUI textoPontuacao;       // Texto da pontuação em tempo real
    public TextMeshProUGUI textoRecorde;         // Texto do recorde salvo
    public TextMeshProUGUI textoPontuacaoFinal;  // Texto exibido no painel de game over

    [Header("Paineis de UI")]
    public GameObject PainelMenu;       // Painel do menu inicial (botão Jogar)
    public GameObject PainelGameOver;   // Painel de game over (botão Reiniciar)

    private float tempoDecorrido = 0f;  // Pontuação atual (em segundos)
    private bool jogoAtivo = false;     // Controla se o cronômetro está rodando

    // Pública para que o GameManager possa setar antes de recarregar a cena
    // Estática: não é resetada quando a cena recarrega
    public static bool veioDoReiniciar = false;

    void Awake()
    {
        // Se for a primeira vez abrindo o jogo, pausa para mostrar o menu
        // Se vier do reiniciar, o tempo já estará em 1 (setado pelo GameManager)
        if (!veioDoReiniciar)
        {
            Time.timeScale = 0;
        }
    }

    void Start()
    {
        // Carrega e exibe o recorde salvo no disco
        float recordeSalvo = PlayerPrefs.GetFloat("HighScore", 0);
        textoRecorde.text = "Recorde: " + recordeSalvo.ToString("0");

        // Garante que ambos os painéis começam ocultos
        PainelMenu.SetActive(false);
        PainelGameOver.SetActive(false);

        if (veioDoReiniciar)
        {
            // Veio do botão Reiniciar: reseta a flag e começa o jogo direto
            veioDoReiniciar = false;
            IniciarJogo();
        }
        else
        {
            // Primeira abertura: exibe o menu inicial
            PainelMenu.SetActive(true);
            jogoAtivo = false;
        }
    }

    void Update()
    {
        // Incrementa a pontuação a cada frame enquanto o jogo estiver ativo
        if (jogoAtivo)
        {
            tempoDecorrido += Time.deltaTime;
            textoPontuacao.text = "Pontos: " + tempoDecorrido.ToString("0");
        }
    }

    // Chamado pelo botão Jogar no menu inicial
    public void IniciarJogo()
    {
        Time.timeScale = 1;
        jogoAtivo = true;
        PainelMenu.SetActive(false);
        PainelGameOver.SetActive(false);
    }

    // Chamado pelo GameManager quando o player colide com um obstáculo
    public void PararCronometro()
    {
        jogoAtivo = false;
        Time.timeScale = 0; // Pausa o jogo

        VerificarRecorde(); // Verifica e salva se bateu o recorde

        // Exibe o painel de game over com a pontuação final
        PainelGameOver.SetActive(true);
        PainelMenu.SetActive(false);

        if (textoPontuacaoFinal != null)
            textoPontuacaoFinal.text = "PONTOS: " + tempoDecorrido.ToString("0");
    }

    // Retorna a pontuação formatada (usado pelo GameManager se necessário)
    public string PegarPontuacaoFormatada()
    {
        return tempoDecorrido.ToString("0");
    }

    // Verifica se a pontuação atual é maior que o recorde salvo
    void VerificarRecorde()
    {
        float recordeAtual = PlayerPrefs.GetFloat("HighScore", 0);
        if (tempoDecorrido > recordeAtual)
        {
            PlayerPrefs.SetFloat("HighScore", tempoDecorrido);
            PlayerPrefs.Save(); // Persiste no disco imediatamente
        }
    }

    // Método alternativo de reinício (caso queira ligar direto ao ScoreManager no botão)
    public void Reiniciar()
    {
        veioDoReiniciar = true;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
