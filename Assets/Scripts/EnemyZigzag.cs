using UnityEngine;
using System.Collections;

public class EnemyZigzag : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;
    private Player _player;
    [SerializeField]
    private GameObject _zigzagLaser;
    [SerializeField]
    private Animator _anim;

    [SerializeField]
    private AudioClip _explosionClip;
    private int x = 1;
    void Start(){
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null){
            Debug.Log("Player is NULL");
        }
        _anim = GetComponent<Animator>();
        if (_anim == null){
            Debug.Log("Animator is NULL");
        }
        StartCoroutine("EnemyShoot");
        StartCoroutine("Zigzag");
    }

    void Update(){
        if (transform.position.y < -9){
            transform.position = new Vector3(randomValX(), 9, 0);
        }
        transform.Translate(new Vector3(x,-1,0) * Time.deltaTime * _speed);
        
    }

    private IEnumerator Zigzag(){
        while (true){
            x *= -1;
            yield return new WaitForSeconds(randomZigZag());
        }
    }
    private IEnumerator EnemyShoot(){
        while (true){
            yield return new WaitForSeconds(Random.Range(3,8));
            GameObject newLaser = Instantiate(_zigzagLaser,  new Vector3(transform.position.x, transform.position.y, 0), this.transform.rotation, this.transform) as GameObject;
            newLaser.transform.localPosition = new Vector3(0,-1,0);
            newLaser.transform.parent = null;
            newLaser.GetComponent<ZigzagLaser>().x = x;
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        //Enemy collides Player
        if (other.tag == "Player"){
            //getting component to prevent null pointer exception error
            Player player = other.transform.GetComponent<Player>();
            //null checking
            if (player != null){
                player.Damage();
                _anim.SetTrigger("OnEnemyDeath");
                _speed = 0;
                AudioSource.PlayClipAtPoint(_explosionClip, transform.position);
                Destroy(GetComponent<Collider2D>());
                StopCoroutine("EnemyShoot");
                Destroy(this.gameObject, 2.8f);
                
            }
        } 
        //Enemy collides Laser
        else if (other.tag == "Laser"){
            Destroy(other.gameObject);
            if (_player != null){
               _player.AddScore(Random.Range(8,12));
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            AudioSource.PlayClipAtPoint(_explosionClip, transform.position);
            Destroy(GetComponent<Collider2D>());
            StopCoroutine("EnemyShoot");
            Destroy(this.gameObject, 2.8f);
        }
    }
    
    int randomValX(){
        return Random.Range(-9, 10);
    }
    float randomZigZag(){
        return Random.Range(0.0f, 2.0f);
    }
}