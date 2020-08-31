﻿using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private SpawnManager _spawnManager;
    [SerializeField]
    private AudioClip _explosionClip;
    
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * 20f * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser"){
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _spawnManager.StartSpawning();
            AudioSource.PlayClipAtPoint(_explosionClip, transform.position);
            Destroy(this.gameObject, 0.25f);
        }
    }
}