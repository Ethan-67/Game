using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/* PLAYER UI 
 * Displays UI interactable elements to the screen when player hovers over an interactable mask 
 * Source https://www.youtube.com/watch?v=gPPGnpV1Y1c
*/
public class PlayerUI : MonoBehaviour
{
    // string of a canvas, this is a unity element always centred on screen of camera so when there is a string it will always 
    // be visible on camera 
    [SerializeField]
    private TextMeshProUGUI promptText; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // updates the string on canvas
    // Update is called once per frame
    public void UpdateText(string promptText)
    {
        this.promptText.text = promptText; 
    }
}
