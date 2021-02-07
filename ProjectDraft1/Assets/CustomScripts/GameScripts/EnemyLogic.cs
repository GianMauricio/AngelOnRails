using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    public GameObject SpawnLoc;
    public AudioSource bangSound;

    private Vector3 DestPos;
    private float DistFrac = 0;

    private bool hasDied = true;

    public int Rank;
    private int worth;

    private float health;
    private float armor;
    private float shield;
    private float speed;


    private enum EnemyState{
        Moving,
        Shooting
    }

    //Assume every enemy has to move
    private EnemyState currState = EnemyState.Moving;

    void Start()
    {
        //Debug.Log("EnemyActive");
        hasDied = false;

        //Set current position as final position
        DestPos = transform.position;
        transform.position = SpawnLoc.GetComponent<Transform>().position;

        //Assign stats according to Tag and rank
        switch (Rank)
        {
            case 1:
                if (transform.CompareTag("BasicEnemy"))
                {
                    health = 20;
                    armor = 0;
                    shield = 0;

                    worth = 10;
                }

                else if (transform.CompareTag("ArmoredEnemy"))
                {
                    health = 20;
                    armor = 10;
                    shield = 0;

                    worth = 20;
                }

                else if (transform.CompareTag("ShieldedEnemy"))
                {
                    health = 20;
                    armor = 0;
                    shield = 20;

                    worth = 25;
                }

                else if (transform.CompareTag("ProtectedEnemy"))
                {
                    health = 20;
                    armor = 10;
                    shield = 20;

                    worth = 30;
                }

                else
                {
                    Debug.Log("Self tag not recognized, self terminating to maintain integrity");
                    Destroy(this.gameObject);
                }

                speed = 0.1f;
                break;

            case 2:
                if (transform.CompareTag("BasicEnemy"))
                {
                    health = 50;
                    armor = 0;
                    shield = 0;

                    worth = 10;
                }

                else if (transform.CompareTag("ArmoredEnemy"))
                {
                    health = 50;
                    armor = 20;
                    shield = 0;

                    worth = 20;
                }

                else if (transform.CompareTag("ShieldedEnemy"))
                {
                    health = 50;
                    armor = 0;
                    shield = 30;

                    worth = 25;
                }

                else if (transform.CompareTag("ProtectedEnemy"))
                {
                    health =50;
                    armor = 20;
                    shield = 30;

                    worth = 30;
                }

                else
                {
                    Debug.Log("Self tag not recognized, self terminating to maintain integrity");
                    Destroy(this.gameObject);
                }
                speed = 0.2f;
                break;

            case 3:
                if (transform.CompareTag("BasicEnemy"))
                {
                    health = 80;
                    armor = 0;
                    shield = 0;

                    worth = 10;
                }

                else if (transform.CompareTag("ArmoredEnemy"))
                {
                    health = 80;
                    armor = 30;
                    shield = 0;

                    worth = 20;
                }

                else if (transform.CompareTag("ShieldedEnemy"))
                {
                    health = 80;
                    armor = 0;
                    shield = 40;

                    worth = 25;
                }

                else if (transform.CompareTag("ProtectedEnemy"))
                {
                    health = 80;
                    armor = 30;
                    shield = 40;

                    worth = 30;
                }

                else
                {
                    Debug.Log("Self tag not recognized, self terminating to maintain integrity");
                    Destroy(this.gameObject);
                }
                speed = 0.3f;
                break;

            default:
                Debug.Log("Rank does exist, self terminating to ensure integrity");
                Destroy(transform);
                break;
        }

        //Determine true worth by multiplying worth to rank
        worth *= Rank;
    }

    // Update is called once per frame
    void Update()
    {
        if (currState == EnemyState.Moving)
        {
            if (DistFrac < 1)
            {
                DistFrac += Time.deltaTime * speed;
                transform.position = Vector3.Lerp(transform.position, DestPos, DistFrac);
            }

            else
            {
                currState = EnemyState.Shooting;
            }
        }

        else if (currState == EnemyState.Shooting)
        {
           //Debug.Log("Enemy can now fire on player");
        }
    }

    //Enemy will handle incoming projectile
    public bool getHit(float bulletDamage, string bulletType, int bulletRank)
    {
        //Debug.Log(health);

        //If the enemy has a shield absorb damage
        if (shield > 0)
        {
            if (bulletType == "Lead" || bulletType == "HeavyLead")
            {
                //Do direct damage to shield, additive rank scaling
                shield -= bulletDamage + bulletRank;
            }

            else if (bulletType == "Blessed")
            {
                //Do percentage type damage, multiplicative rank scaling
                shield -= shield * (bulletDamage * bulletRank) / 100;
            }

            return false;
        }

        //If the enemy has armor refract damage
        else if (armor > 0)
        {
            if (bulletType == "Lead")
            {
                //Take 65% of the incoming damage, additive rank scaling
                armor -= bulletDamage * 0.65f + bulletRank;

                //Translate only 10% damage to health, disregard rank
                health -= bulletDamage * 0.10f;
            }

            else if (bulletType == "HeavyLead")
            {
                //Take 65% of the incoming damage, doubly additive rank scaling
                armor -= bulletDamage * 0.65f + (bulletRank * 2);

                //Translate only 20% damage to health, additive rank scaling
                health -= bulletDamage * 0.20f + bulletRank;
            }

            else if (bulletType == "Blessed")
            {
                //In accordance with Rock-paper-scissors template, ignore all damage from energy weapons
                
            }

            return false;
        }

        //If the enemy still has health
        else if (health > 0)
        {
            //Direct damage taken
            if (bulletType == "Lead" || bulletType == "HeavyLead")
            {
                //Take direct damage, multiplicative rank scaling
                health -= bulletDamage * bulletRank;
            }

            else if (bulletType == "Blessed")
            {
                //Take direct damage, doubly multiplicative rank scaling
                health -= bulletDamage * bulletRank * 2; /*Thi shit is OP, why is this like this?*/
            }

            //Debug.Log("New health: " + health);
            return false;
        }

        //Once health reaches 0
        if (health <= 0)
        {
            this.GetComponent<MeshRenderer>().enabled = false;
            this.GetComponent<MeshCollider>().enabled = false;

            hasDied = true;

            DataHolder.deposit(worth);
            return true;
        }

        //Should never happen
        Debug.Log("Log weird enemy death sequence");
        return true;
    }

    public bool isDead()
    {
        return hasDied;
    }

    public float requestDamage()
    {
        float damageDone = 0;

        //Formula is simple AF this needs to change because if enemy is rank three then player is almost guranteed to take 30 dmg
        damageDone = Rank * 10;

        if (!bangSound.isPlaying)
        {
            bangSound.Play();
        }

        return damageDone;
    }
}
