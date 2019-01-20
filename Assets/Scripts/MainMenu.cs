using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void Quit() {
        Debug.Log("Exiting the game");
        Application.Quit();
    }
    public void PlayGame() {
        Debug.Log("Loading Game");
    }
    public void ToggleMusic() {
        Debug.Log("Toggle Music");
    }
}
