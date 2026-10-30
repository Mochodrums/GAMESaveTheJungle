using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManage : MonoBehaviour
{
    [SerializeField] private GameObject _instructionsPanel;
    
    public void PlayGame()
    {
        SceneManager.LoadScene("lvl 1");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Closed");
    }
    
    public void ShowInstructions()
    {
        _instructionsPanel.SetActive(true);
    }

    public void HideInstructions()
    {
        _instructionsPanel.SetActive(false);
    }
}
