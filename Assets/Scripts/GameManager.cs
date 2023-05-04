using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{


    [Header ("Audio")]
    [SerializeField] private AudioSource rewindSound;
    [SerializeField] private AudioSource music;
    private float musicTime;

    [Header("Rewind Mechanics")]
    private float rewindPoint;
    [SerializeField] private float rewindDuration;
    private float currTime;

    [Header("Particle Systems")]
    private GameObject fParticleSystem;
    private GameObject bParticleSystem;

    [Header("Script References")]
    private CheckPoint checkPoint;
    private ProgressionManager pManager;
    private ScoreManager scoreManager;
    private DispenserController dController;
    private PlayerHealth pHealth;

    //Clean up runtime
    private GameObject projectileContainer;

    [Header("UX Prefabs")]
    [SerializeField] private GameObject scoreCanvas;
    [SerializeField] private GameObject screenCanvas;
    [SerializeField] private GameObject dispenser;

    private bool seated;
    private bool playing;

    private enum GameState { 
    rewinding,
    forward
    }

    private GameState gameState;


    private void Start()
    {
        fParticleSystem = GameObject.Find("forwardParticleSystem");
        bParticleSystem = GameObject.Find("backwardParticleSystem");
        checkPoint = GameObject.Find("checkPointCanvas").GetComponent<CheckPoint>();
        gameState = GameState.forward;
        rewindPoint = checkPoint.getLastCheckpoint();
    }

    private void Update()
    {
        try
        {
            pHealth = GameObject.Find("Head").GetComponent<PlayerHealth>();
        }
        catch (Exception e)
        {
            Debug.Log("Head hasn't loaded yet");
        }
        
        if (music != null)
        {
            musicTime = music.time;
        }
            currTime -= Time.deltaTime;
            switch (gameState)
            {
                case (GameState.forward):
                    fParticleSystem.SetActive(true);
                    bParticleSystem.SetActive(false);
                if (playing)
                {
                    dController.SetDispensable(true);
                }
                    break;
                case (GameState.rewinding):
                    fParticleSystem.SetActive(false);
                    bParticleSystem.SetActive(true);
                    if (currTime < 0)
                    {
                        PlayMusicAt(rewindPoint);
                        currTime = rewindDuration;
                        gameState = GameState.forward;
                        break;
                    }
                    if (rewindSound != null && !rewindSound.isPlaying)
                    {
                        rewindSound.Play();
                    }
                    if (music != null)
                    {
                        music.Pause();
                    }
                if (playing)
                {
                    dController.SetDispensable(false);
                }
                    break;
         }
    }
    public void Rewind()
    {
        currTime = rewindDuration;
        gameState = GameState.rewinding;
        for(int i = 0; i < projectileContainer.transform.childCount; i++)
        {
            Destroy(projectileContainer.transform.GetChild(i).gameObject);
        }
        pManager.SetProgress(checkPoint.getLastCheckpoint());
        scoreManager.SetScore(scoreManager.GetScore() - 10);
        dController.SetTime(rewindPoint);
    }

    public void PlayMusicAt(float val)
    {
        if(music != null)
        {
            music.time = val;
            music.Play();
        }
    }
    public void setSeated(bool state)
    {
        seated = state;
    }
    public bool getSeated()
    {
        return seated;
    }
    public void displayTest()
    {
        Debug.Log("test");
    }

    public void StartSeated()
    {
        Instantiate(scoreCanvas);
        Instantiate(screenCanvas);
        Instantiate(dispenser);
        setSeated(true);
        pHealth.SetHealthAnimator();
        pManager = GameObject.Find("ProgressionBar").GetComponent<ProgressionManager>();
        scoreManager = GameObject.Find("Score").GetComponent<ScoreManager>();
        dController = GameObject.Find("ObstacleDispenser(Clone)").GetComponent<DispenserController>();
        projectileContainer = GameObject.Find("ProjectileContainer");
        playing = true;
    }
    public void StartStanding()
    {
        Instantiate(scoreCanvas);
        Instantiate(screenCanvas);
        Instantiate(dispenser);
        setSeated(false);
        pHealth.SetHealthAnimator();
        pManager = GameObject.Find("ProgressionBar").GetComponent<ProgressionManager>();
        scoreManager = GameObject.Find("Score").GetComponent<ScoreManager>();
        dController = GameObject.Find("ObstacleDispenser(Clone)").GetComponent<DispenserController>();
        projectileContainer = GameObject.Find("ProjectileContainer");
        playing = true;
    }
}
