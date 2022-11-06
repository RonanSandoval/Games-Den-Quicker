using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{

    public enum GameState {
        Playing,
        Info,
        Dead,
        Win,
        Menu
    }

    public GameState currentState;

    public AudioSource clicker;

    private static GameController gameInstance;
    // Start is called before the first frame update
    void Start()
    {
        if (gameInstance == null) {
            gameInstance = this;
        } else {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(transform.gameObject);
        currentState = GameState.Menu;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == GameState.Win || currentState == GameState.Menu || currentState == GameState.Dead) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                clicker.Play();
                StartCoroutine(reload());
            }
        }
    }
    
    IEnumerator reload() {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(1);
        currentState = GameState.Info;
    }
}
