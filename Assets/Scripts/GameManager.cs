using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && _isGameOver)
        {
            SceneManager.LoadScene(1);//Current Game Scene
        }

        //if esc key is pressed
        //quit app
        if (Input.GetKeyDown("escape")){
            Debug.Log("Exit");
            Application.Quit();
        }
    }
    public void GameOver()
    {
        _isGameOver = true;
    }
}
