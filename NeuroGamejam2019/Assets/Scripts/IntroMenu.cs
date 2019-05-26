using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroMenu : MonoBehaviour
{

    public string playerName;
    public GameObject playerObject;
    public PlayerController playerController;
    public Logger logger;

    private WorldManager world;


    // Start is called before the first frame update
    void Start()
    {
        world = FindObjectOfType<WorldManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTutorial()
    {
        // here how to sart the tutorial
        Debug.Log("Tutorial Started");
        StartPlayer();
        

    }

    public void StartGame()
    {
        // here to start the game. 
        Debug.Log("Game Started");
        logger.initFile();
        StartPlayer();

    }

    public void StartPlayer()
    { 
        playerObject.SetActive(true);
        world.player = FindObjectOfType<PlayerController>();
        gameObject.transform.parent.gameObject.SetActive(false);

    }

    public void getName(string inputName)
    {
        playerName = inputName;
    }
}
