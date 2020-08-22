using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;
    //ID for Powerups
    /*0 = trishot
     * 1=speed
     * 2=shield*/
    [SerializeField]
    private int _powerupID;
    [SerializeField]
    private AudioClip _powerupClip;

    void Update(){
        CalculateMovement();
    }

    void CalculateMovement(){
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -6f){
            Destroy(this.gameObject);
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
                }
            }
        }
    }
}
