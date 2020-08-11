using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.15f;
    [SerializeField]
    private int _lives = 3;
    private float _canFire = -1f;
    private SpawnManager _spawnManager;
    
    [SerializeField]
    private bool _isTripleShotActive = false;

    [SerializeField]
    private bool _shieldActive = false;

    [SerializeField]
    private GameObject _shieldVisualizer;

    [SerializeField]
    private int _score;

    private UIManager _uiManager;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is null");
        }

        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is null");
        }
    }

    void Update()
    {
        CalculateMovement();

        //upon hitting SPACE key, spawn game object
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (_isTripleShotActive == true)
        {
            _canFire = Time.time + _fireRate;
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        } 
        else if (_isTripleShotActive == false)
        {
            _canFire = Time.time + _fireRate;
            Instantiate(_laserPrefab, new Vector3(transform.position.x, transform.position.y + 1.05f, 0), Quaternion.identity);
        }
    }

    void CalculateMovement()
    {
        //These will get input from default keys WASD/D-Pad
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        //When WASD/D-Pad are pressed, these will be performed
        transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
        transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);
        //limit vertical movement by clamping.
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);
        //Scroll back around from/to left/right
        if (transform.position.x > 10)
        {
            transform.position = new Vector3(-10, transform.position.y, 0);
        }
        else if (transform.position.x < -10)
        {
            transform.position = new Vector3(10, transform.position.y, 0);
        }
    }

    public void Damage ()
    {
        if (_shieldActive == true)
        {
            _shieldVisualizer.SetActive(false);
            _shieldActive = false;
            return;
        }
        else
        {
            _lives--;
            _uiManager.UpdateLives(_lives);
            if (_lives < 1)
            {
                _spawnManager.OnPlayerDeath();
                Destroy(this.gameObject);
            }
        }
    }

    public void TrishotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TrishotOff());
    }

    private IEnumerator TrishotOff()
    {
        yield return new WaitForSeconds(5);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _speed = 10f;
        StartCoroutine(SpeedBoostOff());
    }

    private IEnumerator SpeedBoostOff()
    {
        yield return new WaitForSeconds(5);
        _speed = 5f;
    }

    public void ShieldBoostActive()
    {
        _shieldActive = true;
        _shieldVisualizer.SetActive(true);
    }

    //method to add 10 to score
    //communicate with UI

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }


}
