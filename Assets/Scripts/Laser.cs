using UnityEngine;

public class Laser : MonoBehaviour
{
    //speed variable of 8 m/s
    [SerializeField]
    private float _speed = 8f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //translate laser upwards
        CalculateMovement();
        
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y >= 8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
}
