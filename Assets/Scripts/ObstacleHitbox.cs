using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleHitbox : MonoBehaviour
{
    //public float hitLockout; 
    public float damage;

    private Transform source; //the object the hitbox is attached to

    public GameObject collisionParent;
    public GameObject dispenser;
    public GameObject deathPoint;

    //private ProjectileAudio projectileAudio;
    //public PlayerHealth health;

    // void Awake()
    // {
    //     projectileAudio = GetComponent<ProjectileAudio>();
    // }


    //do when colliding with another object
    public void OnTriggerEnter(Collider other)
    {
        //read the tag of the object collided with
        switch (other.tag)
        {
           
            //if it collides with the player
            case "Player":
            //find the VR controller
                collisionParent = other.transform.parent.gameObject;
                PlayerHealth health = collisionParent.GetComponent<PlayerHealth>();
                
                //sound effect
                //anim
                spawnDestroyedPoint();

                //band-aid fix
                dispenser = transform.parent.gameObject;
                DispenserController disp = dispenser.GetComponent<DispenserController>();

                //disp.destroyedElsewhere();

                //Destroy MUST be last handling object.
                //Nothing else can persist from object after this.
                Destroy(gameObject);

                //Player not dead
                if (health.currentHealth > 0)
                {
                    health.takeDamage(damage);
                }

                // if (hitLockout > 0) 
                // {
                //     StartCoroutine(lockout(hitLockout));
                // }
                break;
        }
        
    }

    private void spawnDestroyedPoint()
    {
        GameObject newDeath = Instantiate(deathPoint, transform.position, transform.rotation);
        //projectileAudio.PlayAudio();
    }

    //Hitbox disabled after collision based on hitLockout value. Prevents multiple collisions with the same object
    public IEnumerator lockout(float lockoutTime) 
    {
        // gameObject.GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(lockoutTime);
        // gameObject.GetComponent<Collider>().enabled = true;
    }
}