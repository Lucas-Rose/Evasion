using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private Material mat;
    [SerializeField] private GameObject dispenser;
    [SerializeField] private Color defaultRoomColor;
    private bool playing;
    // Start is called before the first frame update
    void Start()
    {
        playing = false;
        canvas.SetActive(true);
        dispenser.SetActive(false);
        mat.color = defaultRoomColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            dispenser.SetActive(true);
            canvas.SetActive(false);
            playing = true;
        }
        if (playing)
        {
            if (mat.color.a > 0)
            {
                mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, mat.color.a - (Time.deltaTime / 2));
            }
        } 
    }
}
