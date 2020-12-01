﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
public class Enemy : MonoBehaviour{
    
    [SerializeField] private float _speed = 4f;
    private Player _player;
    private float _fireRateRear = 1f;
    private float _canFire = -1f;
    private float _fireRatePowerup = 0f;
    private float _canFirePowerup = -1f;
    private bool _canfireBool = true;
    private GameObject _myPlayer;
    [SerializeField] private GameObject _enemyLaser;
    [SerializeField] private GameObject _enemyLaserRear;
    [SerializeField] private Animator _anim;
    [SerializeField] private AudioClip _explosionClip;
    private float _detectionRange = 2.5f;
    private bool _closeEnough = false;
    private Vector3 _ramTarget;
    private int layerMask = 1 << 8;
    
    void Start(){

        /*_player = GameObject.Find("Player").GetComponent<Player>();*/
        _myPlayer = GameObject.Find("Player");
        _player = _myPlayer.GetComponent<Player>();
        if (_player == null){
            Debug.Log("Player is NULL");
        }
        _anim = GetComponent<Animator>();
        if (_anim == null){
            Debug.Log("Animator is NULL");
        }
        StartCoroutine("Move");
        StartCoroutine("EnemyShoot"); 
    }
    
    void Update(){
        var hit = Physics2D.CircleCast(transform.position, 3f, -transform.forward, Mathf.Infinity);
        if (hit != null)
        {
            Debug.Log($"Hit something... {hit.collider.name}");
        }
        /* RaycastHit hit;



        Ray enemyRay = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(enemyRay, out hit, 3f)){
            Debug.Log("I hit something");
            if(hit.collider.tag == "Powerup"){
                Debug.Log("I hit a powerup, will shoot");
                GameObject newLaser = Instantiate(_enemyLaser,  new Vector3(transform.position.x, transform.position.y, 0), this.transform.rotation, this.transform) as GameObject;
                newLaser.transform.parent = transform;
                newLaser.transform.localPosition = new Vector3(0,-1,0);
            }
        } */
        /* var results = 0;
        if (Physics2D.CircleCast(transform.position, 3f, -transform.up, layerMask, results, 5)){
            Debug.Log("I hit something");
            Debug.Log(results);
        } */
        /* if (Physics2D.CircleCast(transform.position, 3f, -transform.up, 0f, layerMask)){
            Debug.Log("I hit something");
        } */
/* 
        RaycastHit2D hit = new CircleCast(transform.position, 3f, -transform.up, 0f, layerMask, -Mathf.Infinity, Mathf.Infinity);
        if (hit != null){
            
        } */
    }


    IEnumerator EnemyShoot(){
        while (true){
            yield return new WaitForSeconds(Random.Range(3,8));
            //(Object original, Vector3 position, Quaternion rotation, Transform parent);
            GameObject newLaser = Instantiate(_enemyLaser,  new Vector3(transform.position.x, transform.position.y, 0), this.transform.rotation, this.transform) as GameObject;
            newLaser.transform.parent = transform;
            newLaser.transform.localPosition = new Vector3(0,-1,0);
        }
    }

    void ShootPowerup(){
        if (_canfireBool == true){
            GameObject newLaser = Instantiate(_enemyLaser,  new Vector3(transform.position.x, transform.position.y, 0), this.transform.rotation, this.transform) as GameObject;
            newLaser.transform.parent = transform;
            newLaser.transform.localPosition = new Vector3(0,-1,0);
            _canfireBool = false;
            StartCoroutine("FalseIt");
        }
    }
 
    IEnumerator Move(){
        while (_player != null){
            if( Vector3.Distance(transform.position, _myPlayer.transform.position) <= _detectionRange && transform.position.y > _myPlayer.transform.position.y){
                transform.position = Vector3.MoveTowards(transform.position, _myPlayer.transform.position, 1 * _speed * Time.deltaTime);
            } else {
                transform.Translate(Vector3.down* _speed * Time.deltaTime);
                if (transform.position.y < -9){
                    transform.position = new Vector3(randomValX(), 9, 0);
                }
            }
            if( Vector3.Distance(transform.position, _myPlayer.transform.position) <= _detectionRange && transform.position.y < _myPlayer.transform.position.y){
                if (Time.time > _canFire){
                    _canFire = Time.time + _fireRateRear;
                    GameObject newLaser = Instantiate(_enemyLaserRear,  new Vector3(transform.position.x, transform.position.y, 0), this.transform.rotation, this.transform) as GameObject;
                    newLaser.transform.parent = transform;
                    newLaser.transform.localPosition = new Vector3(0,1,0);
                }
            }
            yield return null;
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
                StopCoroutine("Move");
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
            StopCoroutine("Move");
            StopCoroutine("EnemyShoot");
            Destroy(this.gameObject, 2.8f);
        }
    }
 
    int randomValX(){
        return Random.Range(-9, 10);
    }
    int randomZigZag(){
        return Random.Range(0,2);
    }
}
