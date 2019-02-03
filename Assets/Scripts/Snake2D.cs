using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Snake2D : MonoBehaviour
{
    public float speed;
    public Joystick joystick;
    public GameObject tailPrefab;
    public float maxScaleDiff = 0.1f;
    public float curveFactor = 10f;
    public float unitDistance = 0.1f;
    public int startLength = 10;
    private List<Trans> historyQ;
    private List<GameObject> trailList;
    private Rigidbody2D rb;
    private TrailRenderer tail;
    private GameController gameController;
    private class Trans {
        public Vector3 pos; //current position
        public Quaternion quat; //current rotation
        public Trans(Vector3 pos,Quaternion quat) { this.pos = pos; this.quat = quat;}
        public Trans Clone() {
            return new Trans(pos,quat);
        }
    }
    void Start()
    {
        historyQ = new List<Trans>();
        trailList = new List<GameObject>();
        rb = GetComponent<Rigidbody2D>();
        tail = GetComponent<TrailRenderer>();
        tail.time = 0.1f;
        for(int i = 0; i < startLength; ++i) {
            historyQ.Insert(0,new Trans(transform.position,transform.rotation));
            GameObject tail = 
                Instantiate(tailPrefab,transform.position,transform.rotation);
            trailList.Insert(0,tail);
            tail.transform.localScale += getScaleFactorFor(trailList.Count);
            tail.GetComponent<BoxCollider2D>().enabled = false;
            Color semitrans = tail.GetComponent<SpriteRenderer>().color;
            semitrans.a = 0.51f;
            tail.GetComponent<SpriteRenderer>().color = semitrans;
        }

        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if(gameControllerObject != null) {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        else Debug.Log("Unable to find game contoller");
    }
    public void setLength(int length)
    {
        tail.time = length * 0.1f;
    }

    private void increaseLength() {
        for(int i = 0;i < 4; ++i) {
            GameObject trail = 
                Instantiate(tailPrefab,historyQ[0].pos,historyQ[0].quat);
            historyQ.Insert(0,historyQ[0].Clone());
            trailList.Insert(0,trail);
            trail.transform.localScale += getScaleFactorFor(trailList.Count);
        }
    }
    private Vector3 getScaleFactorFor(float index) {
        float scaleFactor = (float) Math.Sin((Math.PI/180)*index* curveFactor) * maxScaleDiff;
        return new Vector3(scaleFactor,scaleFactor,1);
    }
    // Update is called once per frame
    void Update()
    {
        float xVelo = (joystick != null ? joystick.Horizontal : 0) 
                            + Input.GetAxis("Horizontal");
        float yVelo = (joystick != null ? joystick.Vertical : 0) 
                            + Input.GetAxis("Vertical");
        Vector2 velocity = Vector2.ClampMagnitude(new Vector2(xVelo, yVelo),1);
        if(velocity.magnitude < 0.5f)
            velocity.Set(0,0);        
        if(velocity != Vector2.zero)
            transform.rotation = Quaternion.LookRotation(Vector3.forward, velocity);

        rb.velocity = velocity*speed;
        if(Vector2.Distance(trailList[trailList.Count-1].transform.position,transform.position) > unitDistance) {
            historyQ.Add(new Trans(transform.position,transform.rotation));
            historyQ.RemoveAt(0);
            for(int i = 0; i < trailList.Count; ++i) {
                trailList[i].transform.position = historyQ[i].pos;
                trailList[i].transform.rotation = historyQ[i].quat;
            }
        }
        if(Input.GetKeyUp("space"))
            increaseLength();
    }
    void OnTriggerEnter2D(Collider2D other) 
    {
        Debug.Log("OnTrigger with "+ other.tag);
        if (other.gameObject.CompareTag ("Consumable2D"))
        {
            Debug.Log("Collided with consumable");
            speed += 1;
            increaseLength();
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

        else if(other.gameObject.CompareTag("Tail")) {
            //gameController.addTimePenalty();
            gameController.endGame();
        }
         
    }
}
