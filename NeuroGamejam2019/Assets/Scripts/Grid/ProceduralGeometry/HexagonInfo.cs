using UnityEngine;


public class HexagonInfo : MonoBehaviour
{
    private Vector2 position;
    private MeshRenderer mesh;
    private ExploitSoundScript sound;
    internal float maxR1;
    internal float R1;
    internal float resourceCount; //TODO change name
    internal float resourceProbability;
    internal float resourceValue = 100;


    public Vector2 Position => position;

    public void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        CalculateCenter();
    }

    internal Vector2 CalculateCenter()
    {
        position = mesh.bounds.center + transform.parent.transform.position;
        return position;
    }

    internal void ExploitResource()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
            {
                if (transform.GetChild(i).GetComponent<ExploitSoundScript>() != null)
                {
                    sound = transform.GetChild(i).GetComponent<ExploitSoundScript>();
                    sound.source.PlayOneShot(sound.clip);
                    transform.GetChild(i).GetComponent<MeshRenderer>().enabled = false;
                }
                else
                    transform.GetChild(i).gameObject.SetActive(false);

                resourceCount -= 1;
                R1 -= 100;
                break;
            }
        }
    }
    float timer = 0;
    public void Update()
    {
        if (sound && sound.source.isPlaying)
            timer += 0.1f;

        if(timer > 2.5f)
        {
            timer = 0;
            sound.gameObject.SetActive(false);
        }

    }
    public void RespawnResource()
    {
        if(resourceCount < maxR1)
        {
            R1 += 10;
            if (R1 % 100 == 0)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (!transform.GetChild(i).gameObject.activeSelf)
                    {
                        transform.GetChild(i).gameObject.SetActive(true);
                        resourceCount += 1;
                        break;
                    }
                }
                R1 = 0;
            }
        }

    }
    public void SpawnResources()
    {
        maxR1 = transform.childCount;
        R1 = (maxR1 * resourceValue);
    }

}
