using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    //handle to text
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _LivesImg;
    [SerializeField]
    private int _score = 0;
    [SerializeField]
    private Text _ammoText;
    [SerializeField]
    private int _ammo = 15;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private GameObject _gameOverText;
    [SerializeField]
    private GameObject _restartText;
    private bool _playerDead = false;
    private GameManager _gameManager;
    void Start(){
        _scoreText.text = "Score: " + _score;//assign text component to handle
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gameManager == null){
            Debug.Log("Game Manager did not load correctly");
        }

    }

    public void UpdateScore(int playerScore){
        _scoreText.text = "Score: " + playerScore;
    }

    public void increaseScore(){
        _score += 10;
    }

    public void UpdateAmmo(int ammo){
        _ammo = ammo;
        _ammoText.text = "Ammo: " + _ammo + "/15";
    }

    public void setAmmo(int x){
        
    }

    public void UpdateLives(int currentLives){
        _LivesImg.sprite = _liveSprites[currentLives];
        if (currentLives == 0){
            GameOverSequence();
            
        }
    }

    void GameOverSequence(){
        _gameManager.GameOver();
        _playerDead = true;
        _gameOverText.SetActive(true);
        StartCoroutine(FlickrGO());
    }

    private IEnumerator FlickrGO(){
        while (_playerDead == true){
            _restartText.SetActive(false);
            _gameOverText.SetActive(true);
            yield return new WaitForSeconds(.5f);
            _restartText.SetActive(true);
            _gameOverText.SetActive(false);
            yield return new WaitForSeconds(.5f);
        }
    }
}
