using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void Quit()
    {
        Debug.Log("Pressed");
        Application.Quit();//quits the game
    }
}
