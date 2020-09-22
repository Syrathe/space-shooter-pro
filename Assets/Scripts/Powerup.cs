using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;
    [SerializeField]
    private int _powerupID;
    private Player _player;
    private GameObject _myPlayer;
    [SerializeField]
    private AudioClip _powerupClip;

    void Start(){
        _myPlayer = GameObject.Find("Player");
        _player = _myPlayer.GetComponent<Player>();
        if (_player == null){
            Debug.Log("Player is NULL");
        }
    }     
    void Update(){

        if (Input.GetKeyDown(KeyCode.C)){
            Debug.Log("Will narrow in");
            StartCoroutine("Move");
        } else {
            CalculateMovement();
            Debug.Log("Moving as normal");
        }
    }

    void CalculateMovement(){
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y <= -6f){
            Destroy(this.gameObject);
        }
    }

    IEnumerator Move(){
        /* while(true){
        Debug.Log("Coming closer");
        } */
        while(true){
            if(_myPlayer != null){
                transform.position = Vector3.MoveTowards(transform.position, _myPlayer.transform.position, 2 * _speed * Time.deltaTime);
                if (Input.GetKeyUp(KeyCode.C)){
                    StopCoroutine("Move");
                }
            }
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        if (other.tag == ("Player")){
            Player player = other.transform.GetComponent<Player>();
            if (player != null){
                switch(this._powerupID){
                    
                    case 0:
                        player.TrishotActive();
                        AudioSource.PlayClipAtPoint(_powerupClip, transform.position);
                        Destroy(this.gameObject);
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        AudioSource.PlayClipAtPoint(_powerupClip, transform.position);
                        Destroy(this.gameObject);
                        break;
                    case 2:
                        player.ShieldBoostActive();
                        AudioSource.PlayClipAtPoint(_powerupClip, transform.position);
                        Destroy(this.gameObject);
                        break;
                    case 3:
                        player.AmmoReload();
                        AudioSource.PlayClipAtPoint(_powerupClip, transform.position);
                        Destroy(this.gameObject);
                        break;
                    case 4:
                        player.Heal();
                        AudioSource.PlayClipAtPoint(_powerupClip, transform.position);
                        Destroy(this.gameObject);
                        break;
                    case 5:
                        player.Nuke();
                        AudioSource.PlayClipAtPoint(_powerupClip, transform.position);
                        Destroy(this.gameObject);
                        break;
                    case 6:
                        player.Slow();
                        AudioSource.PlayClipAtPoint(_powerupClip, transform.position);
                        Destroy(this.gameObject);
                        break;
                }
            }
        }
    }
}
