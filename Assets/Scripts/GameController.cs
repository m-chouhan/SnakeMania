using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    enum GameState {
        NOT_STARTED,
        IN_PROGRESS,
        PAUSED,
        FINISHED
    }
    GameState currentGameState;
    public const int MAX_REQUIRED = 2; 
    public Text scoreText;
    public Text winText;
    public Text restartText;
    public GameObject consumable;
    public GameObject gamePanel; 
    public Text timerText;
    private int remainingItems;
    private float timer;
    void Start()
    {
        currentGameState = GameState.NOT_STARTED;
    }

    void startGame() {
        currentGameState = GameState.IN_PROGRESS;
        timer = 0;
        setRemainingItems(MAX_REQUIRED);
        CreateConsumable();
    }
    void setRemainingItems(int score) {
        remainingItems = score;
        scoreText.text = "Remaining : " + remainingItems.ToString();
    }
    public void updateScoreBy(int delta) {
        remainingItems -= delta;
        scoreText.text = "Remaining : "+ remainingItems.ToString();
        if(remainingItems <= 0)
            GameOver();
        else 
            Invoke("CreateConsumable",1f);     
    }
    public void addTimePenalty() {
        timer += 2f;
    }
    public void CreateConsumable() {
        Instantiate(
                consumable, 
                new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10,10), 0), 
                Quaternion.identity);
    }
    public void GameOver() {
        currentGameState = GameState.FINISHED;
        winText.text = "Congrats!!\nYou finished the game in "+((int)timer).ToString()+" seconds";
    }
    public int getScore() {
        return remainingItems;
    }
    void Update() {
        switch(currentGameState) {
            case GameState.NOT_STARTED : 
                    //Application.LoadLevel(Application.loadedLevel);
            break;
            case GameState.IN_PROGRESS :
                timer += Time.deltaTime;
                timerText.text =  "Time " + ((int)timer).ToString();
            break;
            case GameState.PAUSED :
            break;
        }
    }
}
