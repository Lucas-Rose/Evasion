using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    [Header("Time")]
    [SerializeField] private float endTime;
    private float totalTime;
    private bool playing;

    [Header("Audio")]
    [SerializeField] private AudioSource rewindSound;
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource menuMusic;

    [Header("Rewind Mechanics")]
    [SerializeField] private float rewindDuration;
    private float currTime;
    

    [Header("Particle Systems")]
    private GameObject fParticleSystem;
    private GameObject bParticleSystem;

    [Header("Script References")]
    private CheckPoint checkPoint;
    private ProgressionManager pManager;
    private ScoreManager scoreManager;
    private PlayerHealth pHealth;
    private Animator dAnimator;

    //Clean up runtime
    private GameObject projectileContainer;

    [Header("UX Prefabs")]
    [SerializeField] private GameObject scoreCanvas;
    [SerializeField] private GameObject screenCanvas;
    private GameObject activeScreen;
    [SerializeField] private GameObject dispenser;
    [SerializeField] private GameObject checkPointCanvas;
    [SerializeField] private GameObject completeCanvas;
    [SerializeField] private Camera camera;
    [SerializeField] private GameObject missDetector;

    private bool seated;

    private enum GameState
    {
        rewinding,
        forward
    }

    private GameState gameState;

    private void Start()
    {
        fParticleSystem = GameObject.Find("forwardParticleSystem");
        bParticleSystem = GameObject.Find("backwardParticleSystem");
        gameState = GameState.forward;
        totalTime = 0;
        playing = false;
    }

    private void Update()
    {
        if (playing)
        {
            totalTime += Time.deltaTime;
        }
        if(totalTime > endTime && GameObject.Find("CompleteCanvas(Clone)") == null) 
        {
            Instantiate(completeCanvas);
        }
        currTime -= Time.deltaTime;
        switch (gameState)
        {
            case (GameState.forward):
                fParticleSystem.SetActive(true);
                bParticleSystem.SetActive(false);
                break;
            case (GameState.rewinding):
                fParticleSystem.SetActive(false);
                bParticleSystem.SetActive(true);
                if (currTime < 0)
                {
                    PlayMusicAt(checkPoint.getLastCheckpointTime());
                    totalTime = checkPoint.getLastCheckpointTime();
                    currTime = rewindDuration;
                    dAnimator.SetTrigger("Section" + (checkPoint.getLastCheckPointIndex() + 1));
                    gameState = GameState.forward;
                    break;
                }
                if (!rewindSound.isPlaying)
                {
                    rewindSound.Play();
                    music.Pause();
                }
                break;
        }
    }
    public void Rewind()
    {
        currTime = rewindDuration;
        gameState = GameState.rewinding;

        DestroyAllProjectiles();

        pManager.SetProgress(checkPoint.getLastCheckpointTime());
        checkPoint.SetCurrentTime(checkPoint.getLastCheckpointTime());
        //scoreManager.SetScore(scoreManager.GetScore() - 10);
    }
    
    // forbiddencode
    // public void SkipToSix()
    // {
    //     currTime = rewindDuration;
    //     gameState = GameState.rewinding;

    //     DestroyAllProjectiles();

    //     checkPoint.setSection6();
    //     pManager.SetProgress(checkPoint.getLastCheckpointTime());
    //     checkPoint.SetCurrentTime(checkPoint.getLastCheckpointTime());
    //     scoreManager.SetScore(scoreManager.GetScore() - 10);
    // }

    public void PlayMusicAt(float val)
    {
        if (music != null)
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

    public void StartSeated()
    {
        CreateSystems();
        setSeated(true);
        LinkSystems();
    }
    public void StartStanding()
    {
        CreateSystems();
        setSeated(false);
        LinkSystems();
    }

    public void DestroyAllProjectiles()
    {
        for (int i = 0; i < projectileContainer.transform.childCount; i++)
        {
            Destroy(projectileContainer.transform.GetChild(i).gameObject);
        }
    }
    
    public void CreateSystems()
    {
        //Instantiate(scoreCanvas);
        activeScreen = Instantiate(screenCanvas);
        Instantiate(dispenser);
        Instantiate(checkPointCanvas);
        playing = true;
        menuMusic.Stop();
    }
    public void LinkSystems()
    {
        pHealth = GameObject.Find("PlayerHitbox").GetComponent<PlayerHealth>();
        pHealth.SetHealthAnimator();
        pManager = GameObject.Find("ProgressionBar").GetComponent<ProgressionManager>();
        //scoreManager = GameObject.Find("Score").GetComponent<ScoreManager>();
        projectileContainer = GameObject.Find("ProjectileContainer");
        dAnimator = GameObject.Find("ObstacleDispenser(Clone)").GetComponent<Animator>();
        checkPoint = GameObject.Find("checkPointCanvas(Clone)").GetComponent<CheckPoint>();
        activeScreen.GetComponent<Canvas>().worldCamera = camera;
        activeScreen.GetComponent<Canvas>().planeDistance = .5f;
        missDetector.SetActive(true);
    }
}
