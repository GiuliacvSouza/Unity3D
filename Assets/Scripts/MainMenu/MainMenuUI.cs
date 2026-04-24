using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene"); // nome da cena de jogo
    }

    public void MenuGame()
    {
        SceneManager.LoadScene("MainMenu"); // nome da cena de Main Menu
    }

    public void OpenAbout()
    {
        SceneManager.LoadScene("About"); // nome da cena de Sobre
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Saiu do jogo"); // só funciona no build
    }
}