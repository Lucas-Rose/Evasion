using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private GameObject fParticleSystem;
    private GameObject bParticleSystem;

    private CheckPoint checkPoint;
    private ProgressionManager pManager;
    private ScoreManager scoreManager;
    private DispenserController dController;

    private GameObject projectileContainer;

    private enum GameState { 
    rewinding,
    forward
    }

    private GameState gameState;


    private void Start()
    {
        fParticleSystem = GameObject.Find("forwardParticleSystem");
        bParticleSystem = GameObject.Find("backwardParticleSystem");
        projectileContainer = GameObject.Find("ProjectileContainer");
        checkPoint = GameObject.Find("checkPointCanvas").GetComponent<CheckPoint>();
        pManager = GameObject.Find("ProgressionBar").GetComponent<ProgressionManager>();
        gameState = GameState.forward;
        scoreManager = GameObject.Find("Score").GetComponent<ScoreManager>();
        dController = GameObject.Find("ObstacleDispenser").GetComponent<DispenserController>();
        rewindPoint = checkPoint.getLastCheckpoint();
    }

    private void Update()
    {
        if(music != null)
        {
            musicTime = music.time;
        }

        currTime -= Time.deltaTime;
        switch (gameState) {
            case (GameState.forward):
                fParticleSystem.SetActive(true);
                bParticleSystem.SetActive(false);
                dController.SetDispensable(true);
                break;
            case (GameState.rewinding):
                fParticleSystem.SetActive(false);
                bParticleSystem.SetActive(true);
                if(currTime < 0)
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
                if(music != null)
                {
                    music.Pause();
                }
                dController.SetDispensable(false);
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
}
