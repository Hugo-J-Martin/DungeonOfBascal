using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainGame"); 
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game"); 
        Application.Quit(); 
    }
}
