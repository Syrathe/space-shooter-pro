using System.Collections;
using System.Collections.Generic;
/*using System.Numerics;*/
using UnityEngine;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    void Start()
    {
        
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
                Destroy(this.gameObject);
                
            }
        } 
        //Enemy collides Laser
        else if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }

    //return random x Value for respawning
    int randomValX()
    {
        return Random.Range(-9, 10);
    }
}
