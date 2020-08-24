using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject[] _powerups;
    [SerializeField]
    private GameObject _enemyContainer;

    private bool _stopSpawning = false;

    // Start is called before the first frame update
    

    public void StartSpawning(){
        StartCoroutine(SpawnRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    //Spawn game obj every 5 secs
    //create a coroutine of type IEnumerator -- Yield events
    //while loop

    IEnumerator SpawnRoutine()
    {
        //infinite while loop
        //Instantiate enemy prefab
        //yield wait for 5 seconds
        while (_stopSpawning == false)
        {
            //We hold a reference to the new enemy and assign its parent to be the enemy Container. then yield
            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(randomValX(), 9, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        Debug.Log("Starting powerup coroutine");
        while (_stopSpawning == false)
        {
            Instantiate(_powerups[Random.Range(0,5)], new Vector3(randomValX(), 9, 0), Quaternion.identity);
            Debug.Log("Power up yo ass!");
            yield return new WaitForSeconds(Random.Range(5, 11));
        }
    }



    //return random x Value for respawning
    int randomValX()
    {
        return Random.Range(-9, 10);
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}
