using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject m_menuPanel;
    [SerializeField] private GameObject m_controlsPanel;

    private bool m_controlsPanelOpen = false;

    public void ToggleControlsPanel() 
    {
        if (m_controlsPanelOpen) 
        {
            m_menuPanel.SetActive(true);//open the menu
            m_controlsPanel.SetActive(false);//close the control panel
        }
        else 
        {
            m_menuPanel.SetActive(false);//otherwise close the menu
            m_controlsPanel.SetActive(true);//and open the control panel
        }
        m_controlsPanelOpen = !m_controlsPanelOpen;
    }
    public void LoadGame()
    {
        SceneManager.LoadScene("DevelopmentScene");
    }
    public void Quit() 
    {
        Application.Quit();
    }
}
