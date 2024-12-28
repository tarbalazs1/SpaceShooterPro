using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;
    [SerializeField]
    private float _maxYPosition = 8.0f;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y >= _maxYPosition)
        {
            //tripleshot ha van parent akkor destroy a parentet is
            if (transform.parent != null) { 
                Destroy(transform.parent.gameObject);
            }
            Destroy(gameObject);
        }

    }
}
