using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WorldManager : MonoBehaviour
{

    public float planetHealth;
    public float deltaChange = 0;
    public Transform gameOverPanel;
    public Transform startMenu;
    public Logger log;
    public Light envLight;
    public float decaySoundThreshold; //decay only plays when Whp < threshold
    public GameObject audioObj;

    private AudioSource decaySound;

    private ProceduralGrid grid;
    internal PlayerController player;
    private List<HexagonInfo> hexagonsinfo;
    internal float maxHealth;

    public void Start()
    {
        decaySound = Camera.main.GetComponents<AudioSource>()[2];

        grid = GetComponent<ProceduralGrid>();
        hexagonsinfo = new List<HexagonInfo>();
        CalculateMaxHealth();
        GenerateResources();

    }

    public void CalculateMaxHealth()
    {
        deltaChange = 0;
        float count = 0;
        foreach (Transform t in grid.Hexagons)
        {
            hexagonsinfo.Add(t.GetComponent<HexagonInfo>());
            count += t.GetComponent<HexagonInfo>().maxR1;
        }
        maxHealth = count;
        planetHealth = 100;
    }

    public void GenerateResources()
    {
        foreach(HexagonInfo h in hexagonsinfo)
        {
            h.SpawnResources();
        }
    }

    public void RespawnResources()
    {
        foreach (HexagonInfo h in hexagonsinfo)
        {
            h.RespawnResource();
        }
    }
    private void Update()
    {
        if(gameOverPanel.gameObject.activeSelf) //TODO restart fica com resources mal distribuidos e outros buggs, compativel com logger?
        {
            if (Input.GetMouseButtonDown(0)) {
                grid.Awake();
                grid.Start();
                Start();
                player.playerHealth = 50;
                player.Start();
                CalculateMaxHealth();
                envLight.intensity = 1;

                Text txt = gameOverPanel.GetChild(0).GetComponent<Text>();
                txt.text = "Game Over";

                gameOverPanel.gameObject.SetActive(false);
                player.gameObject.SetActive(false);
                startMenu.gameObject.SetActive(true);
                
            }//TODO pus em comment p gerar logs mais rapido, o bug tava a atrasar

        }

    }

    public void CalculateHealth()
    {
        planetHealth = planetHealth + deltaChange;
        
        envLight.intensity = Mathf.Clamp(planetHealth,40f,100f)/100f; //TODO decay substituir 0.40 e 1 por vars no editor?

        if(planetHealth < decaySoundThreshold)
        {

            if (decaySound.isPlaying)
            {
                decaySound.volume = 1f - Mathf.Clamp(planetHealth, 0f,80f)/100f;
            } else
            {
                decaySound.Play();
            }

        }
       
        if(planetHealth <= 0)
        {
            GameOver(true);
        }
        if (player.playerHealth <= 0)
        {
            GameOver(false);
        }

    }

    public void ExploitGrid()
    {
        deltaChange += -1;
    }
    public void InvestGrid()
    {
        deltaChange += 0.5f;
    }

    public void GameOver(bool planetDeath)
    {

     //   transform.gameObject.SetActive(false);
      //  player.gameObject.SetActive(false);

        if (planetDeath)
        {
            Text txt = gameOverPanel.GetChild(0).GetComponent<Text>();
            txt.text += "\nThe planet is dead\nIt´s your fault...";
        }
        else if(player.playerHealth <= 0) //TODO redundante?
        {
            Text txt = gameOverPanel.GetChild(0).GetComponent<Text>();
            txt.text += "\nYou died.";
        }

        gameOverPanel.gameObject.SetActive(true);
        decaySound.Stop();
        decaySound.volume = 0;

        log.CloseLog();
        log.initFile();

    }

}
