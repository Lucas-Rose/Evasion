using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public float defence; //used for variable health scaling
    private float defenceAsPercentage;
    public float shakeScale;

    //anything hand related is commented out for this build here.

    // public GameObject hand1;
    // public GameObject hand2;
    // public GameObject handTEST;
    //Done here rather than in obstacle hitbox to prevent needing to re-call
    [SerializeField] GameObject gManager;

    [HideInInspector]
    public bool invulnerable = false;
    //public float deathDuration = 4;
    // public HandColour handP;
    // public HandColour handS;
    // public HandColour handT;
    private GameManager gameMan;
    

    private void Awake()
    {
        currentHealth = maxHealth;
        // handP = hand1.GetComponent<HandColour>();
        // handS = hand2.GetComponent<HandColour>();
        // handT = handTEST.GetComponent<HandColour>();
        GameManager gameMan = gManager.GetComponent<GameManager>();
    }

    public void Update()
    {
        defenceAsPercentage = (100 / (100 + defence));
    }

    public void takeDamage(float damage)
    {
        Debug.Log(invulnerable);
        if (invulnerable) { Debug.Log("Damage Taken"); return; }

        //AkSoundEngine.PostEvent("Player_Damage", gameObject);
        //CameraListener.instance.CameraShake(damage * shakeScale, 0.25f);
        //UIDamageIn.instance.DamageVis();

        damage *= (100/(100 + defence));
        damage = Mathf.Clamp(damage, 0f, Mathf.Infinity);
        currentHealth -= damage;
        gameMan.ScoreReset();

        switch (Alive()) 
        {
            case false:
                StartCoroutine(Death());
                break;
            case true:
                if(currentHealth > 0)
                {
                    // handP.healthLost(currentHealth);
                    // handS.healthLost(currentHealth);
                    // handT.healthLost(currentHealth);
                }
                
                break;
        }
    }

    public bool Alive()
    {
        return currentHealth > 0;
    }

    public void healDamage(float healing)
    {
        currentHealth += healing;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
    }
    
    public IEnumerator Death()
    {
        invulnerable = true;
        yield return new WaitForSeconds(1); //placeholder.

        //do events on death.


        //controller._animator.SetTrigger("Death");

        //yield return new WaitForSeconds(controller._animator.GetCurrentAnimatorStateInfo(0).length * 0.5f);
        //yield return AnnouncementHandler.instance.Announcement("You Died.", deathDuration);

        //HealthbarManager.instance.ClearBoss();
        //GameManager.instance.LoadDelegate(GameManager.instance.OnDeath()); //to be moved to whatever is handling health
    }

    public float GetPlayerHealth()
    {
        return currentHealth;
    }
}    
