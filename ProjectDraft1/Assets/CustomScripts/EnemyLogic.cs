using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    public GameObject SpawnLoc;

    private Vector3 DestPos;
    private float DistFrac = 0;
    private static float SPEED = 0.03f;

    public int Rank;

    private float health;
    private float armor;
    private float shield;

    private enum EnemyState{
        Moving,
        Shooting
    }

    //Assume every enemy has to move
    private EnemyState currState = EnemyState.Moving;

    void Start()
    {
        Debug.Log("EnemyActive");

        //Set current poition as final position
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
                }

                else if (transform.CompareTag("ArmoredEnemy"))
                {
                    health = 20;
                    armor = 10;
                    shield = 0;
                }

                else if (transform.CompareTag("ShieldedEnemy"))
                {
                    health = 20;
                    armor = 0;
                    shield = 20;
                }

                else if (transform.CompareTag("ProtectedEnemy"))
                {
                    health = 20;
                    armor = 10;
                    shield = 20;
                }

                else{Debug.Log("Self tage not recognized, self terminating to maintain integrity");}
                break;

            case 2:
                if (transform.CompareTag("BasicEnemy"))
                {
                    health = 50;
                    armor = 0;
                    shield = 0;
                }

                else if (transform.CompareTag("ArmoredEnemy"))
                {
                    health = 50;
                    armor = 20;
                    shield = 0;
                }

                else if (transform.CompareTag("ShieldedEnemy"))
                {
                    health = 50;
                    armor = 0;
                    shield = 30;
                }

                else if (transform.CompareTag("ProtectedEnemy"))
                {
                    health =50;
                    armor = 20;
                    shield = 30;
                }

                else { Debug.Log("Self tage not recognized, self terminating to maintain integrity"); }
                break;

            case 3:
                if (transform.CompareTag("BasicEnemy"))
                {
                    health = 80;
                    armor = 0;
                    shield = 0;
                }

                else if (transform.CompareTag("ArmoredEnemy"))
                {
                    health = 80;
                    armor = 30;
                    shield = 0;
                }

                else if (transform.CompareTag("ShieldedEnemy"))
                {
                    health = 80;
                    armor = 0;
                    shield = 40;
                }

                else if (transform.CompareTag("ProtectedEnemy"))
                {
                    health = 80;
                    armor = 30;
                    shield = 40;
                }

                else { Debug.Log("Self tage not recognized, self terminating to maintain integrity"); }
                break;

            default:
                Debug.Log("Rank does exist, self terminating to ensure integrity");
                Destroy(transform);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (currState)
        {
            case EnemyState.Moving:
                if (DistFrac < 1)
                {
                    DistFrac += Time.deltaTime * SPEED;
                    transform.position = Vector3.Lerp(transform.position, DestPos, DistFrac);
                }
                break;

            case EnemyState.Shooting:
                break;
        }
    }

    //Enemy will handle incoming projectile
    public void getHit(float bulletDamage, string bulletType, int bulletRank)
    {
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

            else if (bulletType == " Blessed")
            {
                //In accordance with Rock-paper-scissors template, ignore all damage from energy weapons
                
            }
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
        }

        Debug.Log("New health: " + health);

        //Once health reaches 0 destroy self
        if(health <= 0) {Destroy(gameObject);}
    }
}
