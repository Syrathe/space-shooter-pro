using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigzagLaser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10f;
    public int x;
    void Update()    {
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
        transform.Translate(new Vector3(8*x,-10,0) * _speed * Time.deltaTime);

        if (transform.position.y <= -8f){
            Destroy(this.gameObject);
        }
    }
}
