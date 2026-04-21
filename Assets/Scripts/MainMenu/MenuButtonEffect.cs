using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MenuButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Componentes")]
    public TextMeshProUGUI buttonText;
    public Image backgroundBox;

    [Header("Configurações de Cores")]
    public Color textNormalColor = Color.white;
    public Color textHighlightColor = Color.white; // No seu caso, branco
    public Color boxHighlightColor; // Cor do texto original que irá para o fundo

    [Header("Som do Clique")]
    public AudioClip somClique;   

    private Button button; 

    void Start()
    {
        button = GetComponent<Button>();

         if (button != null)
        {
            button.onClick.AddListener(TocarSomClique);
        }

        // Garante que o fundo comece invisível, mas o texto visível
        if (backgroundBox != null)
        {
            backgroundBox.canvasRenderer.SetAlpha(0.0f); // Começa invisível
        }

        if (buttonText != null)
        {
            buttonText.color = textNormalColor;
            // Garante que o Alpha do texto seja 1 (visível)
            Color c = buttonText.color;
            c.a = 1f;
            buttonText.color = c;
        }
    }

    void TocarSomClique()
    {
        Debug.Log("OnClick nativo disparou!");
        if (AudioManager.Instance != null && somClique != null)
            AudioManager.Instance.TocarSFX(somClique);
        else
            Debug.LogWarning("AudioManager null ou somClique não atribuído!");
    }


    // Quando o mouse ENTRA no botão
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Texto fica branco (ou a cor de destaque)
        buttonText.color = textHighlightColor;

        // Fundo aparece com a cor definida e opacidade total
        if (backgroundBox != null)
        {
            backgroundBox.color = boxHighlightColor;
            backgroundBox.canvasRenderer.SetAlpha(1.0f); // Garante visibilidade

            Color c = backgroundBox.color;
            c.a = 1f; // Opacidade 100%
            backgroundBox.color = c;
        }
    }

    // Quando o mouse SAI do botão
    public void OnPointerExit(PointerEventData eventData)
    {
        // Volta o texto para a cor normal (Azul)
        buttonText.color = textNormalColor;

        // Faz o fundo sumir
        if (backgroundBox != null)
        {
            backgroundBox.canvasRenderer.SetAlpha(0.0f);
        }
    }

}