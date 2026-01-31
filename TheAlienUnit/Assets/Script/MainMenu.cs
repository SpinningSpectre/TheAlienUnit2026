using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TMP_Text volumeText;

    public void StartGame(string gameScene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(gameScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void setVolumeText()
    {
        volumeText.text = (volumeSlider.value * 100).ToString("F0") + "%";
    }
}
