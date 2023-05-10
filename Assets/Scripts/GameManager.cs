using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource rewindSound;
    [SerializeField] private AudioSource music;

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
    private PlayerHealth pHealth;
    private Animator dAnimator;

    //Clean up runtime
    private GameObject projectileContainer;

    [Header("UX Prefabs")]
    [SerializeField] private GameObject scoreCanvas;
    [SerializeField] private GameObject screenCanvas;
    [SerializeField] private GameObject dispenser;
    [SerializeField] private GameObject checkPointCanvas;

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
    }

    private void Update()
    { 
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
        checkPoint.SetCurrentTime(getLastCheckpointTime);
        scoreManager.SetScore(scoreManager.GetScore() - 10);
    }

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
        Instantiate(scoreCanvas);
        Instantiate(screenCanvas);
        Instantiate(dispenser);
        Instantiate(checkPointCanvas);
    }
    public void LinkSystems()
    {
        pHealth = GameObject.Find("Head").GetComponent<PlayerHealth>();
        pHealth.SetHealthAnimator();
        pManager = GameObject.Find("ProgressionBar").GetComponent<ProgressionManager>();
        scoreManager = GameObject.Find("Score").GetComponent<ScoreManager>();
        projectileContainer = GameObject.Find("ProjectileContainer");
        dAnimator = GameObject.Find("ObstacleDispenser(Clone)").GetComponent<Animator>();
        checkPoint = GameObject.Find("checkPointCanvas(Clone)").GetComponent<CheckPoint>();
    }
}
