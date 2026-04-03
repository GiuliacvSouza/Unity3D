using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

// Gerencia UI, pontuação, recorde e fluxo do jogo (sem menu inicial)
public class ScoreManager : MonoBehaviour
{
    [Header("Textos de UI")]
    public TextMeshProUGUI textoPontuacao;
    public TextMeshProUGUI textoRecorde;
    public TextMeshProUGUI textoPontuacaoFinal;
    public TextMeshProUGUI textoContagem;

    [Header("Painel Game Over")]
    public GameObject PainelGameOver;

    [Header("Dificuldade Incremental")]
    public float velocidadeInicial = 5f;
    public float velocidadeMaxima = 20f;
    public float incrementoVelocidade = 0.5f;
    public GroundManager groundManager;
    public ObstacleSpawner obstacleSpawner;

    private float tempoDecorrido = 0f;
    private bool jogoAtivo = false;


    public static bool veioDoReiniciar = false;

    void Awake()
    {
        // Sempre começa pausado para fazer a contagem
        Time.timeScale = 0;
    }

    void Start()
    {
        float recordeSalvo = PlayerPrefs.GetFloat("HighScore", 0);
        textoRecorde.text = "Recorde: " + recordeSalvo.ToString("0");

        if (textoContagem != null)
            textoContagem.gameObject.SetActive(false);

        if (PainelGameOver != null)
            PainelGameOver.SetActive(false);

        // Inicia automaticamente a contagem ao entrar na cena
        StartCoroutine(ContagemRegressiva());
    }

    void Update()
    {
        if (jogoAtivo)
        {
            tempoDecorrido += Time.deltaTime;
            textoPontuacao.text = "Pontos: " + tempoDecorrido.ToString("0");

<<<<<<< HEAD
            // Aumenta velocidade gradualmente até ao máximo
=======
>>>>>>> a17b238 (Velocidade, quebra de chão e camera)
            AtualizarDificuldade();
        }
    }

    private IEnumerator ContagemRegressiva()
    {
        Time.timeScale = 0;

        if (textoContagem != null)
        {
            textoContagem.gameObject.SetActive(true);

            textoContagem.text = "3";
            yield return new WaitForSecondsRealtime(1f);

            textoContagem.text = "2";
            yield return new WaitForSecondsRealtime(1f);

            textoContagem.text = "1";
            yield return new WaitForSecondsRealtime(1f);

            textoContagem.text = "JÁ!";
            yield return new WaitForSecondsRealtime(0.6f);

            textoContagem.gameObject.SetActive(false);
        }

        Time.timeScale = 1;
        jogoAtivo = true;
    }

    public void PararCronometro()
    {
        jogoAtivo = false;
        Time.timeScale = 0;

        VerificarRecorde();

        if (PainelGameOver != null)
            PainelGameOver.SetActive(true);

        if (textoPontuacaoFinal != null)
            textoPontuacaoFinal.text = "PONTOS: " + tempoDecorrido.ToString("0");
    }

    public string PegarPontuacaoFormatada()
    {
        return tempoDecorrido.ToString("0");
    }

    void VerificarRecorde()
    {
        float recordeAtual = PlayerPrefs.GetFloat("HighScore", 0);
        if (tempoDecorrido > recordeAtual)
        {
            PlayerPrefs.SetFloat("HighScore", tempoDecorrido);
            PlayerPrefs.Save();
        }
    }

    public void Reiniciar()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void AtualizarDificuldade()
<<<<<<< HEAD
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
=======
    {
        float novaVelocidade = Mathf.Min(
            velocidadeInicial + (tempoDecorrido * incrementoVelocidade),
            velocidadeMaxima
        );

        if (groundManager != null)
            groundManager.speed = novaVelocidade;

        if (obstacleSpawner != null)
        {
            obstacleSpawner.obstacleSpeed = novaVelocidade;

            obstacleSpawner.spawnInterval = Mathf.Max(
                2.0f - (tempoDecorrido * 0.01f),
                0.8f
            );
        }
    }
}
>>>>>>> a17b238 (Velocidade, quebra de chão e camera)
