using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour 
{ 
    [Tooltip("FX for enemy")] [SerializeField] GameObject DeathFX;
                              [SerializeField] Transform parent;
                              [SerializeField] int HP = 3;
                              [SerializeField] int scorePertHit = 12;
                            

    ScoreBoard scoreBoard;
    // Start is called before the first frame update
    void Start()
    {
        AddBoxCollider();
        scoreBoard = FindObjectOfType<ScoreBoard>();
    }

    private void AddBoxCollider()
    {
        Collider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.isTrigger = false;
    }

    void OnParticleCollision(GameObject other)
    {

        ProcessHit();
        if(HP <= 1)
        {
           KillEnemy();
        }

    }
    void ProcessHit()
    {
        scoreBoard.ScoreHit(scorePertHit);
        HP = HP - 1;
    }

    private void KillEnemy()
    {
        GameObject fx = Instantiate(DeathFX, transform.position, Quaternion.identity);
        fx.transform.parent = parent;
        Destroy(gameObject);
    }
}
