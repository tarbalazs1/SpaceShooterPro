using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;

    private void Update()
    {
        //if R key was pressed restart game
    }

    public void GameOver() { 
        _isGameOver = true;
    }
}
