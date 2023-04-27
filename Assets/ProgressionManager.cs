using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionManager : MonoBehaviour
{
    private RectTransform myRect;
    [SerializeField] private float audioDuration;
    private float currentTime;
    private float initialWidth;
    // Start is called before the first frame update
    void Start()
    {
        myRect = GetComponent<RectTransform>();
        initialWidth = myRect.sizeDelta.x;
        myRect.sizeDelta = new Vector2(0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        myRect.sizeDelta = new Vector2((currentTime / audioDuration) * initialWidth, 1f);
        Debug.Log(myRect.sizeDelta.x);
    }
}
