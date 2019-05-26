using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //Controls tha player and camera
    public float playerHealth = 50;

    private WealthBar wealth;
    private Camera mainCam;
    private Vector3 playerPos;
    private Vector2 playerHex;

    private WorldManager world;
    private AudioSource moveSound;
    private AudioSource decaySound;

    public Transform grid;
    public float zoom; //mudar zoom se usar ortho cam basta variar size
    public float playerZOffset;//offset depende do Z dos tiles
    public Logger log;

    private int nplays;
    private int state;
    private Vector2 newPos;

    // Start is called before the first frame update
    public void Start()
    {
        nplays = 0;
        mainCam = Camera.main;
        state = 0;

        moveSound = mainCam.GetComponents<AudioSource>()[1]; //TODO indexes?

        world = FindObjectOfType<WorldManager>();

        //set player pos
        playerHex = grid.GetChild(Random.Range(0, grid.childCount)).GetComponent<HexagonInfo>().Position;
        transform.position = new Vector3(playerHex.x, playerHex.y, playerZOffset);

        //set cam pos
        mainCam.transform.position = new Vector3(playerHex.x, playerHex.y, zoom);
        wealth = GetComponent<WealthBar>();
        wealth.CurrValue = playerHealth;

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.mouseScrollDelta.y != 0 && zoom <= 3) //TODO bounds
        {
            zoom = zoom + Input.mouseScrollDelta.y > -3 ? -3 : zoom + Input.mouseScrollDelta.y;
            mainCam.transform.position = new Vector3(mainCam.transform.position.x, mainCam.transform.position.y, zoom);
        }

        switch (state)
        {
            case 0:
                //Click
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));

                    if (rayHit)
                    {
                        Debug.Log("lclick" + rayHit.transform.tag);

                        if (rayHit.transform.CompareTag("Hexagon"))
                        {
                            HexagonInfo info = rayHit.transform.GetComponent<HexagonInfo>();
                            if (info.Position == playerHex && info.resourceCount > 0)
                            {
                                nplays++;
                                rayHit.transform.GetComponent<HexagonInfo>().ExploitResource();
                                if (playerHealth <= 100)
                                    wealth.HealDamage(10);
                                playerHealth = wealth.CurrValue;
                                world.ExploitGrid();
                                world.RespawnResources();
                                world.CalculateHealth();

                                log.Log(nplays + " E " + playerHealth + " " + world.planetHealth + " " + world.deltaChange + " " + rayHit.transform.name.Split('n')[1]);
                            }
                            else
                            {
                                newPos = info.Position;

                                if (Vector2.Distance(newPos, playerHex) < 2.1f && newPos != playerHex)
                                {
                                    nplays++;
                                    playerHex = newPos; // n sei se guardar o transform do hex e redundante
                                    wealth.TakeDamage(5);
                                    playerHealth = wealth.CurrValue;
                                    world.CalculateHealth();
                                    world.RespawnResources();

                                    moveSound.Play();
                                    log.Log(nplays + " M " + playerHealth + " " + world.planetHealth + " " + world.deltaChange + " " + rayHit.transform.name.Split('n')[1]); ; //TODO exception on close, last move saved?
                                    state = 1;

                                }
                            }
                        }
                    }
                }
                else if (Input.GetMouseButtonDown(1))
                {

                    RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));

                    if (rayHit)
                    {
                        Debug.Log("rclick" + rayHit.transform.tag);

                        if (rayHit.transform.CompareTag("Hexagon"))
                        {
                            HexagonInfo info = rayHit.transform.GetComponent<HexagonInfo>();
                            if (info.Position == playerHex)
                            {
                                nplays++;
                                wealth.TakeDamage(5);
                                playerHealth = wealth.CurrValue;
                                world.InvestGrid();
                                world.CalculateHealth();
                                world.RespawnResources();

                                log.Log(nplays + " I " + playerHealth + " " + world.planetHealth + " " + world.deltaChange + " " + rayHit.transform.name.Split('n')[1]);
                            }

                        }
                    }

                }
                break;
            case 1:
                transform.position = Vector3.Lerp(transform.position, new Vector3(newPos.x, newPos.y, playerZOffset), 0.5f);
                mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, new Vector3(newPos.x, newPos.y, mainCam.transform.position.z), 0.5f);
                if (Vector3.Distance(transform.position, newPos) < 0.1f)
                    state = 0;
                break;
        }
    }

}

