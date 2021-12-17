using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    public CanvasManager currentCanvas;
    bool isWinConditionMet = false;
    static GameManager _instance = null;
    public static GameManager instance
    {
        get { return _instance; }
        set { _instance = value; }
    }

    int _score = 0;
    int _lives = 1;

    public int maxLives = 3;
    public GameObject playerPrefab;

    [HideInInspector] public GameObject playerInstance;
    [HideInInspector] public LevelManager currentLevel;

    public int score
    {
        get { return _score; }
        set
        {
            _score = value;
            Debug.Log("Score changed to " + _score);
        }
    }

    public int lives
    {
        get { return _lives; }
        set
        {
            if (!currentCanvas)
                currentCanvas = FindObjectOfType<CanvasManager>();

            if (_lives < value)
            { 
                _lives = value;
                return;
            }

            _lives = value;
            if (_lives > maxLives)
            {
                _lives = maxLives;
                return;
            }

            if (_lives < 0)
            {
                //gameover stuff can go here
                return;
            }

            //if execution reaches here - we need to respawn
            if (currentLevel)
            {
                Destroy(playerInstance);
                SpawnPlayer(currentLevel.spawnPoint);
            }

            if (currentCanvas)
                currentCanvas.SetLivesText(_lives);

            Debug.Log("Lives changed to " + _lives);

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        if (!currentCanvas)
            currentCanvas = FindObjectOfType<CanvasManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name == "MainMenu")
                SceneManager.LoadScene("SampleScene");
            else if (SceneManager.GetActiveScene().name == "SampleScene")
                SceneManager.LoadScene("EndScene");
            else if (SceneManager.GetActiveScene().name == "EmdScene")
                SceneManager.LoadScene("MainMenu");
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
            QuitGame();

    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }


    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ReturnToTitle()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void EndGame(bool isComplete)
    {
        isWinConditionMet = isComplete;
        SceneManager.LoadScene("EndScene");       
    }
    public void SpawnPlayer(Transform spawnLocation)
    {
        playerInstance = Instantiate(playerPrefab, spawnLocation.position, spawnLocation.rotation);
    }
}
