using UnityEngine;


public class InstructionPanel : MonoBehaviour
{
    public GameObject storyPanel;
    public GameObject instructionPanel;
    public void SetActivePanel()
    {
        Time.timeScale = 0f;
    }

    public void OnContinueHandler()
    {   
        storyPanel.SetActive(false);            
        instructionPanel.SetActive(true);
    }
    public void PlayHandler()
    {
        Time.timeScale = 1f;
        instructionPanel.SetActive(false);
    }

}
