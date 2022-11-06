using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public enum GameState {
        Playing,
        Info,
        Dead,
        Win
    }

    public GameState currentState;

    // Start is called before the first frame update
    void Start()
    {
        currentState = GameState.Info;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
