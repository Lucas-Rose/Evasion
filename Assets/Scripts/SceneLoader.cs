using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManager;
public class SceneLoader : MonoBehaviour
{
    public Animator transition;
    // variable for time wait
    public float transitionTime = 1f;

    // Update is called once per frame
    void Update()
    {
        // trigger next level on click (can change later)
        if(Input.GetMouseButtonDown(0)){
            LoadNextLevel();
        }
    }


    public void LoadNextLevel(){
        // progesses the scene based on order of scene order 
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1))
        }
   
    IEnumerator LoadLevel(int levelIndex){
        // paly animation 
        transition.SetTrigger("start");
        // waiting for animation to stop 
        yeild return new WaitForSecond(transitionTime);
        // load the scene 
        SceneManager.LoadScene(levelIndex)
    }
}