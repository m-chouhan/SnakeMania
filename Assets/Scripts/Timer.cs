using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float timeTaken;
    private Text timerText;
    // Start is called before the first frame update
    void Start()
    {
        timerText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {     
    //  timeTaken += Time.deltaTime;
    //  timerText.text = "Time "+ ((int)timeTaken).ToString();
    }
}
