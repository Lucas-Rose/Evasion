using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    public Animator transition;
    // variable for time wait
    public float transitionTime = 1f;

    // Update is called once per frame
    void Update()
    {
        // trigger next level on click (can change later)
        if(Input.GetKey(KeyCode.Space)){
            LoadNextLevel();
        }
    }


    public void LoadNextLevel()
        {
        // progesses the scene based on order of scene order 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
   
    IEnumerator LoadLevel(int levelIndex){
        // paly animation 
        transition.SetTrigger("start");
        // waiting for animation to stop 
        yield return new WaitForSeconds(transitionTime);
        // load the scene 
        SceneManager.LoadScene(levelIndex);
    }
}