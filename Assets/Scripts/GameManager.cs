using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private Material mat;
    [SerializeField] private GameObject dispenser;
    [SerializeField] private Color defaultRoomColor;

    [SerializeField] private GameObject foreText;
    [SerializeField] private GameObject backText;
    private Text foregroundText;
    private Text backgroundText;
    [SerializeField] private Text scoreText;
    [SerializeField] float score;
    [SerializeField] float startingScoreMultiplier;
    [SerializeField] float scoreMultiplierIncresase;
    [SerializeField] float scoreMultiplierCap;
    [SerializeField] float scoreCounterThreshold;
    private float scoreCounter;
    public float scoreMultiplier; //needs to be public to be edited by collision
    [SerializeField] bool playing;
    // Start is called before the first frame update
    void Start()
    {
        playing = false;
        //playing = true; // set for testing
        canvas.SetActive(true);
        dispenser.SetActive(false);
        mat.color = defaultRoomColor;
        foreText.SetActive(false);
        backText.SetActive(false);
        foregroundText = foreText.GetComponent<Text>();
        backgroundText = backText.GetComponent<Text>();
        scoreMultiplier = startingScoreMultiplier;
    }

    // Update is called once per frame
    void Update()
    {
        if (playing)
        {
            //near misses are a to-do down the line
            score += Time.deltaTime * scoreMultiplier;
            scoreCounter += Time.deltaTime;
            scoreText.text = "Score: " + score.ToString();
            if(scoreCounter > scoreCounterThreshold)
            {
                ScoreMultiplierIncrease();
                scoreCounter = 0;
            }

        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            dispenser.SetActive(true);
            canvas.SetActive(false);
            playing = true;
            foreText.SetActive(true);
            backText.SetActive(true);
        }
        if (playing)
        {
            if (mat.color.a > 0)
            {
                mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, mat.color.a - (Time.deltaTime / 2));
            }
            foregroundText.text = score.ToString("F1");
            backgroundText.text = score.ToString("F1");
        } 
    }

    public void ScoreReset()
    {
        scoreMultiplier = startingScoreMultiplier;
        scoreCounter = 0;
    }

    void ScoreMultiplierIncrease()
    {
        scoreMultiplier += scoreMultiplierIncresase;
        if(scoreMultiplier > scoreMultiplierCap)
        {
            scoreMultiplier = scoreMultiplierCap;
        }
    }
    

}
