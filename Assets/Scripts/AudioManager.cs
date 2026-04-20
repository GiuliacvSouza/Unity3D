using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Música Ambiente")]
    public AudioClip musicaFloresta;

    [Header("Efeitos Sonoros")]
    public AudioClip sfxPulo;
    public AudioClip sfxGameOver;
    public AudioClip sfxBotao;

    void Awake()
    {
        // SINGLETON - Garante que só existe um AudioManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Não some ao trocar de cena
            Debug.Log("AudioManager iniciado com sucesso!");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("AudioManager duplicado foi destruído");
        }
    }

    void OnEnable()
{
    // Toda vez que uma cena carregar, esse método é chamado
    UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
}

void OnDisable()
{
    UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
}

void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
{
    StartCoroutine(IniciarMusicaComDelay());
    // Reinicia a música automaticamente em qualquer cena (exceto menu se quiser)
    if (musicSource != null && musicaFloresta != null)
    {
        musicSource.Stop();
        musicSource.clip = musicaFloresta;
        musicSource.loop = true;
        musicSource.volume = 0.4f;
        musicSource.Play();
        Debug.Log("Música reiniciada na cena: " + scene.name);
    }
}

private System.Collections.IEnumerator IniciarMusicaComDelay()
{
    yield return new WaitForSecondsRealtime(0.1f); // espera o Start dos outros scripts

    if (musicSource != null && musicaFloresta != null)
    {
        musicSource.Stop();
        musicSource.clip = musicaFloresta;
        musicSource.loop = true;
        musicSource.volume = 0.4f;
        musicSource.Play();
    }
}

    void Start()
    {
        // Começa a tocar a música da floresta
        if (musicSource != null && musicaFloresta != null)
        {
            musicSource.clip = musicaFloresta;
            musicSource.loop = true;
            musicSource.Play();
            Debug.Log("Música da floresta tocando!");
        }
        else
        {
            Debug.LogWarning("Faltou arrastar a música ou o AudioSource no Inspector!");
        }
    }

    // Método público para tocar efeitos sonoros
    public void TocarSFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    // Métodos específicos para facilitar
    public void TocarPulo() => TocarSFX(sfxPulo);
    public void TocarGameOver() => TocarSFX(sfxGameOver);
    public void TocarBotao() => TocarSFX(sfxBotao);

    // Controle de volume
    public void SetVolumeMusica(float volume)
    {
        if (musicSource != null) musicSource.volume = volume;
    }

    public void SetVolumeSFX(float volume)
    {
        if (sfxSource != null) sfxSource.volume = volume;
    }
    
    public void PararMusica()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
            Debug.Log("Música parada!");
        }
    }

    // PAUSA a música (mantém a posição)
    public void PausarMusica()
    {
        if (musicSource != null)
        {
            musicSource.Pause();
            Debug.Log("Música pausada!");
        }
    }

    // VOLTA a tocar de onde parou
    public void RetomarMusica()
    {
        if (musicSource != null)
        {
            musicSource.UnPause();
            Debug.Log("Música retomada!");
        }
    }

   
    public void FadeOutMusica(float duracao = 1f)
    {
        if (musicSource != null)
        {
            StartCoroutine(FadeOutCoroutine(duracao));
        }
    }

    private System.Collections.IEnumerator FadeOutCoroutine(float duracao)
    {
        float volumeInicial = musicSource.volume;
        float tempo = 0f;
        
        while (tempo < duracao)
        {
            tempo += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(volumeInicial, 0f, tempo / duracao);
            yield return null;
        }
        
        musicSource.Stop();
        musicSource.volume = volumeInicial; // Reseta o volume para a próxima vez
    }
}
