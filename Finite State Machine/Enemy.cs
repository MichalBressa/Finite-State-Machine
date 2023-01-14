using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : StatsBase
{

    int attackDamage = 2;
    float health = 10f;

    public Transform target;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    public void Die()
    {
        Destroy(gameObject);
        // maybe do some particle effect or ragdoll
    }

    public override void takeDamage(int dmg)
    {
        Debug.Log("Enemy got hit!");
        health -= dmg;
        if (health <= 0f)
        {
            Die();
        }
    }

    public override void healSelf(int heal)
    {

    }

    public override void attack(StatsBase _statsbase)
    {
        _statsbase.takeDamage(attackDamage);
        Debug.Log("Enemy is attacking");
    }


}
