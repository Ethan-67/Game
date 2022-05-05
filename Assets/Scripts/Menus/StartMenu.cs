using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

/* START MENU
 * Handles start menu, menu first shown when user plays game 
*/
public class StartMenu : MonoBehaviour
{
    public Button startBtn;
    public Button optionsBtn;
    public Button exitBtn;

    private void Start()
    {
        startBtn.onClick.AddListener(StartOnClick);
        optionsBtn.onClick.AddListener(OptionsOnClick);
        exitBtn.onClick.AddListener(ExitOnClick); 
    }
    
    private void StartOnClick()
    {
        Debug.Log("Start pressed");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
    }

    private void OptionsOnClick()
    {

    }

    private void ExitOnClick()
    {
        Application.Quit(); 
    }
}
