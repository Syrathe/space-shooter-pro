using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10f;
    void Start()
    {
        
    }

    void Update()
    {
        CalculateMovement();
    }

    private void OnTriggerEnter2D(Collider2D other){
        if (other.tag == ("Player")){
            Player player = other.transform.GetComponent<Player>();
            if (player != null){
                Debug.Log("Player is null");
            }
            player.Damage();
            Destroy(this.gameObject);
        }
    }
    void CalculateMovement(){
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -8f){
            /* if (transform.parent != null){
                Destroy(transform.parent.gameObject);
            } */
            Destroy(this.gameObject);
        }
    }
}