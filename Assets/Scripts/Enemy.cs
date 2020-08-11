﻿using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;

    private Player _player;
    //handle to animator component
    [SerializeField]
    private Animator _anim;
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.Log("Player is NULL");
        }
        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.Log("Animator is NULL");
        }
    }

    void Update()
    {
        //respawns enemy on top when reaching bottom
        transform.Translate(Vector3.down* _speed * Time.deltaTime);
        if (transform.position.y < -9)
        {
            transform.position = new Vector3(randomValX(), 9, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Enemy collides Player
        if (other.tag == "Player") 
        {
            //getting component to prevent null pointer exception error
            Player player = other.transform.GetComponent<Player>();
            //null checking
            if (player != null)
            {
                player.Damage();
                _anim.SetTrigger("OnEnemyDeath");
                _speed = 0;
                Destroy(this.gameObject, 2.8f);
                
            }
        } 
        //Enemy collides Laser
        else if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
               _player.AddScore(Random.Range(8,12));
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            Destroy(this.gameObject, 2.8f);
        }
    }

    //return random x Value for respawning
    int randomValX()
    {
        return Random.Range(-9, 10);
    }
}
