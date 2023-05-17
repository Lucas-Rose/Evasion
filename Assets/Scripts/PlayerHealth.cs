using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float damageAmount;
    [SerializeField] private float hitCooldown;
    [SerializeField] private float healAmount;
    [SerializeField] private float currentHealth;
    [SerializeField] private bool canTakeDamage;
    [SerializeField] private float currentTime;

    private Animator healthAnimator;
    private GameManager gManager;
    


    private void Start()
    {
        canTakeDamage = true;
        currentTime = hitCooldown;
        currentHealth = maxHealth;
        gManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        currentTime -= Time.deltaTime;
        if(currentTime < 0)
        {
            canTakeDamage = true;
        }
        if(currentHealth == 0)
        {
            healDamage(maxHealth);
            gManager.Rewind();
        }
        else
        {
            //healDamage(healAmount * Time.deltaTime);
        }

        // manual rewind + skip 
        // if(Input.GetKeyDown(KeyCode.R))
        // {
        //     gManager.Rewind();
        // } 
        // if(Input.GetKeyDown(KeyCode.Z))
        // {
        //     gManager.SkipToSix();
        // } 
    }

    public void takeDamage(float damage)
    {
        if (canTakeDamage)
        {
            currentHealth -= damage;
            Debug.Log(currentHealth);
            canTakeDamage = false;
            healthAnimator.SetTrigger("hit");
            currentTime = hitCooldown;
        }
    }

    public void healDamage(float healing)
    {
        currentHealth += healing;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public float GetPlayerHealth()
    {
        return currentHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("projectile"))
        {
            takeDamage(damageAmount);
            Debug.Log("hit");
        }
    }
    public void SetHealthAnimator()
    {
        healthAnimator = GameObject.Find("screenCanvas(Clone)").GetComponent<Animator>();
    }
}    
