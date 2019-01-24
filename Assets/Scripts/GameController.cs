﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public enum GameState {
        NOT_STARTED,
        IN_PROGRESS,
        PAUSED,
        FINISHED
    }
    public static GameState currentGameState;
    public const int MAX_REQUIRED = 2; 
    public Text scoreText;
    public Text winText;
    public Text restartText;
    public GameObject consumable;
    public MainMenu mainMenu; 
    public Text timerText;
    private int remainingItems;
    private float timer;
    void Start()
    {
        currentGameState = GameState.NOT_STARTED;
        Time.timeScale = 0f;
        GameObject menuObject = GameObject.FindWithTag("MainMenu");
        if(menuObject != null)
            mainMenu = menuObject.GetComponent<MainMenu>();
        else Debug.Log("Unable to find main menu controller");
    }
    public void startGame() {
        Debug.Log("Starting new Game!");
        currentGameState = GameState.IN_PROGRESS;
        timer = 0;
        Time.timeScale = 1f;
        remainingItems = MAX_REQUIRED;
        scoreText.text = "Remaining : " + remainingItems.ToString();
        createConsumable();
    }
    public void endGame() {
        Debug.Log("Game finished!");
        currentGameState = GameState.FINISHED;
        Time.timeScale = 0;
        mainMenu.endGame((int)timer);
    }
    public void pauseGame() {
        Debug.Log("Game is paused");
        currentGameState = GameState.PAUSED;
        Time.timeScale = 0f;
    }
    public void resumeGame() {
        Debug.Log("Resuming game!");
        currentGameState = GameState.IN_PROGRESS;
        Time.timeScale = 1f;
    }
    public void updateScoreBy(int delta) {
        remainingItems -= delta;
        scoreText.text = "Remaining : "+ remainingItems.ToString();
        if(remainingItems <= 0)
            Invoke("endGame",1f);
        else 
            Invoke("createConsumable",1f);     
    }

    //TODO : adds time penalty on every invalid collision
    public void addTimePenalty() {
        timer += 2f;
    }
    public void createConsumable() {
        Instantiate(
                consumable, 
                new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10,10), 0), 
                Quaternion.identity);
    }
    public int getScore() {
        return remainingItems;
    }
    void Update() {
        if(currentGameState == GameState.IN_PROGRESS) {
            timer += Time.deltaTime;
            timerText.text =  "Time " + ((int)timer).ToString();
        }
    }
}
