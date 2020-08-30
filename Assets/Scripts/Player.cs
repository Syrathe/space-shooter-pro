using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private GameObject _nukePrefab;
    [SerializeField]
    private float _fireRate = 0.15f;
    [SerializeField]
    private int _lives = 3;
    private float _canFire = -1f;
    private int _ammo = 15;
    private SpawnManager _spawnManager;
    [SerializeField]
    private bool _isTripleShotActive = false;
    [SerializeField]
    private int _shieldActive = 0;
    [SerializeField]
    GameObject[] _shieldVisualizer = new GameObject[3];

    [SerializeField]
    private int _score;
    private UIManager _uiManager;
    [SerializeField]
    private GameObject _rightEngineDamage;
    [SerializeField]
    private GameObject _leftEngineDamage;

    [SerializeField]
    private AudioClip _laserClip;
    [SerializeField]
    private AudioClip _laserFail;
    [SerializeField]
    private AudioClip _explosionClip;

    [SerializeField]
    private bool _nuke = false;
    public CameraShake cameraShake;
    public int _gas = 100;

    public ThrustBar _thrustBar;


//variable to store audio clip

    void Start(){
        _thrustBar = GameObject.Find("Thrust_Bar").GetComponent<ThrustBar>();
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_spawnManager == null){
            Debug.LogError("Spawn Manager is null");
        }

        if (_uiManager == null){
            Debug.LogError("UI Manager is null");
        }

        if (_thrustBar == null){
            Debug.LogError("Thrust Bar is empty");
        }
        StartCoroutine("GasRefill");
    }

    void Update(){
        CalculateMovement();

        //upon hitting SPACE key, spawn game object
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire){
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift)){
            StartCoroutine("Thrust");
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)){
            if ((_speed == 8) || (_speed == 13)){
                _speed -= 3;
            }
            StopCoroutine("Thrust");
        }
    }

    IEnumerator Thrust(){
        if (_gas >= 3){
            _speed += 3;
            while (_gas >= 3){
                _gas -= 3;
                _thrustBar.SetThrustBar(_gas);
                yield return new WaitForSeconds(0.1f);
            }
        }
        if (_gas < 3){
            if ((_speed == 8)||(_speed == 13)){
                _speed -= 3;
            }
            yield break;
        }

    }
    void Shoot(){
        if (_ammo <= 0){
            AudioSource.PlayClipAtPoint(_laserFail, transform.position);
        } else if (_ammo > 0){
            if (_nuke == true){
                Instantiate(_nukePrefab, new Vector3(transform.position.x - 0.83f, transform.position.y + 2.0f, 0), Quaternion.identity);
            } else {
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
                AudioSource.PlayClipAtPoint(_laserClip, transform.position);
            }
        }
        _ammo--;   
    }

    void CalculateMovement(){
        //These will get input from default keys WASD/D-Pad
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        //When WASD/D-Pad are pressed, these will be performed
        transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
        transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);
        //limit vertical movement by clamping.
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);
        //Scroll back around from/to left/right
        if (transform.position.x > 10) {
            transform.position = new Vector3(-10, transform.position.y, 0);
        } else if (transform.position.x < -10) {
            transform.position = new Vector3(10, transform.position.y, 0);
        }
    }

    public void Damage (){
        if (_shieldActive > 0){
            //_shieldVisualizer.SetActive(false);
            _shieldActive -= 1;
            CheckShield();
            return;
        } else {
            _lives--;
            Shake();
            CheckDamage();
        }
        
    }

    private void Shake(){
        StartCoroutine(cameraShake.Shake(.15f, .1f));
    }
    public void TrishotActive(){
        _isTripleShotActive = true;
        StartCoroutine(TrishotOff());
    }

    private IEnumerator TrishotOff(){
        yield return new WaitForSeconds(5);
        _isTripleShotActive = false;
    }

    public void AmmoReload(){
        Debug.Log("Ammo reloading");
        _ammo = 15;
    }

    public void SpeedBoostActive(){
        _speed += 5f;
        StartCoroutine(SpeedBoostOff());
    }

    private IEnumerator SpeedBoostOff(){
        yield return new WaitForSeconds(5);
        _speed -= 5f;
    }

    private IEnumerator NukeUp(){
        _nuke = true;
        yield return new WaitForSeconds(5);
        _nuke = false;
    }

    public void Nuke(){
        StartCoroutine("NukeUp");
    }

    public void ShieldBoostActive(){
        if (_shieldActive >= 3){
            return;
        } else {
            _shieldActive += 1;
        }
        CheckShield();
        //_shieldVisualizer.SetActive(true);
    }

    public void Heal(){
        if (_lives < 3){
            _lives ++;
            CheckDamage();
        }
        
    }

    private void CheckShield(){
        if (_shieldActive == 0){
            _shieldVisualizer[0].SetActive(false);
        }
        if (_shieldActive == 1){
            _shieldVisualizer[1].SetActive(false);
            _shieldVisualizer[0].SetActive(true);
        } else {
            if (_shieldActive == 2){
                _shieldVisualizer[0].SetActive(false);
                _shieldVisualizer[2].SetActive(false);
                _shieldVisualizer[1].SetActive(true);
            } else {
                if (_shieldActive == 3){
                    _shieldVisualizer[1].SetActive(false);
                    _shieldVisualizer[2].SetActive(true);
                }
            }
        }
    }

    private void CheckDamage(){
        if (_lives == 3){
            _leftEngineDamage.SetActive(false);
            _rightEngineDamage.SetActive(false);
        } if (_lives == 2){
            _leftEngineDamage.SetActive(true);
            _rightEngineDamage.SetActive(false);
        } else if (_lives == 1){
            _rightEngineDamage.SetActive(true);
            _leftEngineDamage.SetActive(true);
        } else if (_lives == 0){
            _leftEngineDamage.SetActive(false);
            _rightEngineDamage.SetActive(false);
        }

        _uiManager.UpdateLives(_lives);
        if (_lives < 1){
            _spawnManager.OnPlayerDeath();
            AudioSource.PlayClipAtPoint(_explosionClip, transform.position);
            Destroy(this.gameObject);
        }
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    IEnumerator GasRefill(){
        while (_gas <= 100){
            if (_gas <= 97){
                _gas += 2;
                _thrustBar.SetThrustBar(_gas);
            }
            yield return new WaitForSeconds(1);
        }
    }
}
