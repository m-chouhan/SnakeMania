using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Echo : MonoBehaviour
{
    public float timeBtwSpawns;
    public float startTimeBtwSpwans;
    public GameObject echo;
    public GameObject player;
    // Update is called once per frame
    void Update() {
        
        if(timeBtwSpawns <= 0 && 
            player.GetComponent<Rigidbody2D>().velocity != Vector2.zero) {
            GameObject trail = Instantiate(echo, transform.position, Quaternion.identity);
            Destroy(trail,1f);
            timeBtwSpawns = startTimeBtwSpwans;
        }
        else timeBtwSpawns -= Time.deltaTime;
    }
}
