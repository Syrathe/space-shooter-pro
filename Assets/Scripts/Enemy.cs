using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    private Player _player;
    [SerializeField]
    private GameObject _enemyLaser;
    //private float _canFire = -1f;
    [SerializeField]
    //private float _fireRate = 0.15f;
    //handle to animator component
    //[SerializeField]
    private Animator _anim;

    [SerializeField]
    private AudioClip _explosionClip;
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
    }

    void Update(){
        //respawns enemy on top when reaching bottom
        transform.Translate(Vector3.down* _speed * Time.deltaTime);
        if (transform.position.y < -9){
            transform.position = new Vector3(randomValX(), 9, 0);
        }
        //if (enemy is zigzag){
            //start moving 
        //}
        
    }

    private IEnumerator EnemyShoot(){
        while (true){
            yield return new WaitForSeconds(Random.Range(3,8));
            /*Instantiate(_enemyLaser, new Vector3(transform.position.x, transform.position.y, 0), this.transform.rotation);
            _enemyLaser.transform.localPosition = new Vector3(0,2,0);*/

            //(Object original, Vector3 position, Quaternion rotation, Transform parent);
            GameObject newLaser = Instantiate(_enemyLaser,  new Vector3(transform.position.x, transform.position.y, 0), this.transform.rotation, this.transform) as GameObject;
            newLaser.transform.parent = transform;
            newLaser.transform.localPosition = new Vector3(0,-1,0);
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

    //return random x Value for respawning
    int randomValX(){
        return Random.Range(-9, 10);
    }
    int randomZigZag(){
        return Random.Range(0,2);
    }
}
