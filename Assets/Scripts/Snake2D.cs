﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Snake2D : MonoBehaviour
{
    public float speed;
    public Joystick joystick;
    private List<Vector3> historyQ;
    private Rigidbody2D rb;
    private TrailRenderer tail;
    private GameController gameController;
    void Start()
    {
        historyQ = new List<Vector3>();
        dir = DIR.LEFT;
        rb = GetComponent<Rigidbody2D>();
        tail = GetComponent<TrailRenderer>();
        tail.time = 0.2f;
        for(int i = 0; i < transform.childCount; ++i) {
            if(!transform.GetChild(i).CompareTag("eye"))
                historyQ.Add(transform.position);
        }

        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if(gameControllerObject != null) {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        else Debug.Log("Unable to find game contoller");
    }

    enum DIR { UP, LEFT, DOWN, RIGHT }
    DIR dir;

    DIR getDirection() {
        float moveHorizontal = //joystick.Horizontal + 
                Input.GetAxis ("Horizontal");
        float moveVertical = //joystick.Vertical + 
                Input.GetAxis ("Vertical");
        DIR currentDir = dir;
        if(moveHorizontal < 0)
           currentDir = DIR.LEFT;
        else if(moveHorizontal > 0)
            currentDir = DIR.RIGHT;
        else if(moveVertical < 0)
            currentDir = DIR.DOWN;
        else if(moveVertical > 0)
            currentDir = DIR.UP;
        return currentDir;    
    }
    // Update is called once per frame
    void Update()
    {
        dir = getDirection();
        Vector2 velocity = new Vector2 (0, 0);
        switch(dir) {
            case DIR.UP:
                velocity.y = speed;
            break;
            case DIR.LEFT:
                velocity.x = -speed;
            break;
            case DIR.DOWN:
                velocity.y = -speed;
            break;
            case DIR.RIGHT:
                velocity.x = speed;
            break;
        }

        velocity = new Vector2((joystick.Horizontal + Input.GetAxis("Horizontal")) * speed,
                                  (joystick.Vertical + Input.GetAxis("Vertical")) * speed);

        transform.rotation = Quaternion.LookRotation(Vector3.forward, velocity);

        rb.velocity = velocity;
        historyQ.Add(transform.position);
        historyQ.RemoveAt(0);
        for(int i = 0; i < transform.childCount; ++i) {
            if(!transform.GetChild(i).CompareTag("eye"))
               transform.GetChild(i).position = historyQ[i];
        }
    }
    void OnTriggerEnter2D(Collider2D other) 
    {
        Debug.Log("OnTrigger");
        if (other.gameObject.CompareTag ("Consumable2D"))
        {
            Debug.Log("Collided with consumable");
            tail.time += 0.1f;
            speed += 1;
            other.gameObject.GetComponent<Renderer> ().material.color = Color.red;
            other.gameObject.tag = "Finish";
            GetComponent<AudioSource>().Play();
            Destroy(other.gameObject,0.5f);
            gameController.updateScoreBy(1);
        }

        else if(other.gameObject.CompareTag("Wall")) {
            Debug.Log("Collided with Wall");
            gameController.addTimePenalty();
        }
    }
}
