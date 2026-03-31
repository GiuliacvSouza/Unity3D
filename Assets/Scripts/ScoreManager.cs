using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections; // necessário para IEnumerator


// Gerencia toda a UI do jogo: pontuação, recorde, painéis de menu e game over
public class ScoreManager : MonoBehaviour
{
    [Header("Textos de UI")]
    public TextMeshProUGUI textoPontuacao;       // Texto da pontuação em tempo real
    public TextMeshProUGUI textoRecorde;         // Texto do recorde salvo
    public TextMeshProUGUI textoPontuacaoFinal;  // Texto exibido no painel de game over
    public TextMeshProUGUI textoContagem;        // Texto exibido ao iniciar o jogo

    [Header("Paineis de UI")]
    public GameObject PainelMenu;       // Painel do menu inicial (botão Jogar)
    public GameObject PainelGameOver;   // Painel de game over (botão Reiniciar)

    [Header("Dificuldade Incremental")]
    public float velocidadeInicial = 5f;
    public float velocidadeMaxima = 20f;
    public float incrementoVelocidade = 0.5f; // aumenta X por segundo
    public GroundManager groundManager;
    public ObstacleSpawner obstacleSpawner;

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

        // Garante que o texto de contagem começa invisível
        if (textoContagem != null) textoContagem.gameObject.SetActive(false);

        // Garante que ambos os painéis começam ocultos
        PainelMenu.SetActive(false);
        PainelGameOver.SetActive(false);

        if (veioDoReiniciar)
        {
            // Veio do botão Reiniciar: reseta a flag e começa o jogo direto
            veioDoReiniciar = false;
            // Ao reiniciar, vai direto para a contagem sem mostrar o menu
            StartCoroutine(ContagemRegressiva());
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

            // Aumenta velocidade gradualmente até ao máximo
            AtualizarDificuldade();
        }
    }

   // Chamado pelo botão Jogar — agora inicia a contagem em vez de começar direto
    public void IniciarJogo()
    {
        PainelMenu.SetActive(false);
        PainelGameOver.SetActive(false);
        StartCoroutine(ContagemRegressiva());
    }

     // Coroutine da contagem regressiva
    private IEnumerator ContagemRegressiva()
    {
        // O jogo continua pausado durante a contagem
        Time.timeScale = 0;

        if (textoContagem != null)
        {
            textoContagem.gameObject.SetActive(true);

            textoContagem.text = "3";
            yield return new WaitForSecondsRealtime(1f); // WaitForSecondsRealtime ignora o timeScale

            textoContagem.text = "2";
            yield return new WaitForSecondsRealtime(1f);

            textoContagem.text = "1";
            yield return new WaitForSecondsRealtime(1f);

            textoContagem.text = "JÁ!";
            yield return new WaitForSecondsRealtime(0.6f);

            textoContagem.gameObject.SetActive(false);
        }

        // Só agora libera o jogo de verdade
        Time.timeScale = 1;
        jogoAtivo = true;
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

    void AtualizarDificuldade()
{
    float novaVelocidade = Mathf.Min(
        velocidadeInicial + (tempoDecorrido * incrementoVelocidade),
        velocidadeMaxima
    );

    // Sincroniza chão e obstáculos
    if (groundManager != null)
        groundManager.speed = novaVelocidade;

    if (obstacleSpawner != null)
    {
        obstacleSpawner.obstacleSpeed = novaVelocidade;

        // Reduz intervalo de spawn conforme acelera (min 0.8s)
        obstacleSpawner.spawnInterval = Mathf.Max(
            2.0f - (tempoDecorrido * 0.01f),
            0.8f
        );
    }
}
}
