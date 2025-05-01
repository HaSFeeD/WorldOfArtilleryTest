using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject hudPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button restartButton;

    void Awake()
    {
        gameOverPanel.SetActive(false);
        restartButton.onClick.AddListener(Restart);
    }

    public void ShowHUD()    => hudPanel.SetActive(true);
    public void HideHUD()    => hudPanel.SetActive(false);

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        hudPanel.SetActive(false);
    }

    public void HideGameOver()
    {
        gameOverPanel.SetActive(false);
        hudPanel.SetActive(true);
    }

    private void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
