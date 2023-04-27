using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private Text fText; //foreground
    private Text bText; //background
    private float score;
    // Start is called before the first frame update
    void Start()
    {
        fText = transform.GetChild(1).GetComponent<Text>();
        bText = transform.GetChild(0).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        score += Time.deltaTime;
        fText.text = score.ToString("f1");
        bText.text = score.ToString("f1");
    }
    public void SetScore(float val)
    {
        score = val;
    }
    public float GetScore()
    {
        return score;
    }
}
