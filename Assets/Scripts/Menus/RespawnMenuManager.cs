using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnMenuManager : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene("DevelopmentScene");
    }
    public void Quit() 
    {
        Application.Quit();
    }
}
