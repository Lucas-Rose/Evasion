using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionPlayer : MonoBehaviour
{
    [SerializeField] Transform startingPosition;
    [SerializeField] GameObject vrController;
    [SerializeField] Camera playerHead;
    public float startElevation;

    private GameObject dispenser;

    //Used to disable when starting play
    public GameObject gaze;
    public GameObject reticle;

    public void CalibrateSeated()
    {
        //takes the position of the player's head at time of pressing 'play' as the height they will be playing the game at.
        //Used to spawn blocks at position based on the player's height / seated height.
        startElevation = playerHead.transform.position.y;

        //Positions the dispenser. Can be different based on Seated/Standing play if implemented by changing values here.

        //The values substracted from startElevation may be affected by our time using SteamVR and unnecessary.
        dispenser = GameObject.Find("ObstacleDispenser(Clone)");
        dispenser.transform.position = new Vector3(0, startElevation - .5f, 15);

        //turns off the reticle to prevent it from distracting player once game starts.
        gaze.SetActive(false);
        reticle.SetActive(false);
    }

    public void CalibrateStanding()
    {
        startElevation = playerHead.transform.position.y;
        dispenser = GameObject.Find("ObstacleDispenser(Clone)");
        dispenser.transform.position = new Vector3(0, startElevation - 2f, 15);
        gaze.SetActive(false);
        reticle.SetActive(false);
    }
}
