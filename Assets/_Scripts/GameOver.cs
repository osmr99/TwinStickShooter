using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] GameObject button;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(button);
    }

    public void Restart()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void TitleScreen()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
