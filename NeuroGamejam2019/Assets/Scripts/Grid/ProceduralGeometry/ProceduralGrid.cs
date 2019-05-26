using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//TODO you can exploit tiles with 0 "trees"/resources including in water and desert tiles
public class ProceduralGrid : MonoBehaviour
{
    //Types of Tiles
    public Material[] materials;

    //Specific Object for each Tipe of Tile
    public GameObject[] forestPrefabs;
    public GameObject[] desertPrefabs;
    public GameObject[] waterPrefabs;

    public float seperationX = 1;
    public float seperationY = 0.5f;
    public int xSize = 5;
    public int ySize = 3;
    public GameObject hexPrefab;

    private WorldManager world;
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    public static List<Transform> hexagons = new List<Transform>();
    private static List<Transform> hexagonChilds = new List<Transform>();
    internal bool cleared;

    public List<Transform> Hexagons => hexagons;

    public int probabilityResource { get; private set; }

    public void GenerateGrid()
    {
        cleared = false;
        Vector3[] vertices = new Vector3[(xSize) * (ySize) * 6];
        int vetCount = 0;
        float offsetX = 0, offsetY = 0;
        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                vertices[vetCount] = new Vector3(x - 1 + offsetX, y - 0.5f + offsetY, 0);
                vertices[vetCount + 1] = new Vector3(x - 1 + offsetX, y + 0.5f + offsetY, 0);
                vertices[vetCount + 2] = new Vector3(x + offsetX, y + 1 + offsetY, 0);
                vertices[vetCount + 3] = new Vector3(x + 1 + offsetX, y + 0.5f + offsetY, 0);
                vertices[vetCount + 4] = new Vector3(x + 1 + offsetX, y - 0.5f + offsetY, 0);
                vertices[vetCount + 5] = new Vector3(x + offsetX, y - 1 + offsetY, 0);
                vetCount += 6;
                offsetX += seperationX;
            }
            if (y % 2 != 0)
                offsetX = 0;
            else
                offsetX = seperationX;
            offsetY += seperationY;
        }
        int[] triangles = new int[(xSize * ySize) * 12];
        int triCont = 0;
        int vertIndex = 0;
        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[triCont] = vertIndex + 1;
                triangles[triCont + 1] = vertIndex + 5;
                triangles[triCont + 2] = vertIndex;
                triangles[triCont + 3] = vertIndex + 1;
                triangles[triCont + 4] = vertIndex + 4;
                triangles[triCont + 5] = vertIndex + 5;
                triangles[triCont + 6] = vertIndex + 1;
                triangles[triCont + 7] = vertIndex + 2;
                triangles[triCont + 8] = vertIndex + 4;
                triangles[triCont + 9] = vertIndex + 2;
                triangles[triCont + 10] = vertIndex + 3;
                triangles[triCont + 11] = vertIndex + 4;

                triCont += 12;
                vertIndex += 6;
            }
        }
        int hexagonCount = 0;
        for (int i = 0; i < xSize * ySize; i++)
        {
            GameObject hexagon = Instantiate(hexPrefab, transform);
            mesh = new Mesh();
            BoxCollider2D collider = hexagon.AddComponent<BoxCollider2D>();
            HexagonInfo info = hexagon.GetComponent<HexagonInfo>();
            collider.size = new Vector2(1.5f, 1.5f);
            hexagon.AddComponent<MeshFilter>().mesh = mesh;
            mesh.name = "Hexagon" + i;
            hexagon.transform.name = mesh.name;
            int r = Random.Range(0, materials.Length);
            hexagon.AddComponent<MeshRenderer>().material = materials[r];
            info.Start();



            Vector3[] hexagonVert = new Vector3[6];
            for (int j = 0; j < hexagonVert.Length; j++)
            {
                hexagonVert[j] = vertices[j + hexagonCount];
            }
            hexagonCount += 6;

            int[] hexagonTriangles = new int[12];
            for (int k = 0; k < hexagonTriangles.Length; k++)
            {
                hexagonTriangles[k] = triangles[k];
            }

            mesh.vertices = hexagonVert;
            mesh.triangles = hexagonTriangles;
            mesh.RecalculateNormals();
            Vector2[] uv = Unwrapping.GeneratePerTriangleUV(mesh);
            List<Vector2> actualUV = new List<Vector2>();
            foreach (var v in uv)
            {
                if (!actualUV.Contains(v))
                    actualUV.Add(v);
            }

            mesh.uv = actualUV.ToArray();
            collider.offset = hexagon.GetComponent<MeshRenderer>().bounds.center + transform.position;
            hexagons.Add(hexagon.transform);
            //TODO
            if (Random.Range(0, 100) > probabilityResource)
            {
                info.resourceCount = Random.Range(0, 5);
            }

            if (r == 1)
            {
                //int mult = Random.Range(2, 5);
                for (int z = 0; z < info.resourceCount; z++)
                {
                    int v = Random.Range(0, forestPrefabs.Length);
                    GameObject hexagonChild = Instantiate(forestPrefabs[v], hexagon.transform);
                    hexagonChilds.Add(hexagonChild.transform);

                }
            }
            if (r == 0)
            {
                //int mult = Random.Range(2, 5);
                for (int z = 0; z < info.resourceCount; z++)
                {
                    int v = Random.Range(0, desertPrefabs.Length);
                    GameObject hexagonChild = Instantiate(desertPrefabs[v], hexagon.transform);
                    hexagonChilds.Add(hexagonChild.transform);

                }
            }
            if (r == 2)
            {
                //int mult = Random.Range(2, 5);
                for (int z = 0; z < info.resourceCount; z++)
                {
                    int v = Random.Range(0, waterPrefabs.Length);
                    GameObject hexagonChild = Instantiate(waterPrefabs[v], hexagon.transform);
                    hexagonChilds.Add(hexagonChild.transform);

                }
            }

        }
        DontDestroyOnLoad(this);
        Start();
    }
    public void Awake()
    {
        GetHexagons();
    }

    public void Start()
    {
        //Debug.Log("here");
        foreach(Transform t in hexagonChilds)
            t.transform.localPosition = new Vector3(t.parent.GetComponent<HexagonInfo>().Position.x + Random.Range(-0.75f, 0.75f), (t.parent.GetComponent<HexagonInfo>().Position.y + Random.Range(-0.75f, 0.75f)), t.position.z);
    }
    //Fix

    public void ClearLevel()
    {
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject g = transform.GetChild(i).gameObject;
                hexagons.Remove(g.transform);
                for(int j = 0; j < g.transform.childCount; j++)
                {
                    hexagonChilds.Remove(g.transform.GetChild(j));
                }
                DestroyImmediate(g);
            }
        }
        hexagons.Clear();
        hexagonChilds.Clear();
        cleared = true;
    }

    public Vector2 GetCenter()
    {
        return hexagons[hexagons.Count / 2].GetComponent<HexagonInfo>().Position;
    }

    public List<Transform> GetHexagons()
    {
        List<Transform> newHexagons = new List<Transform>();
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                newHexagons.Add(transform.GetChild(i));
                if(transform.GetChild(i).childCount > 0)
                {
                    transform.GetChild(i).GetComponent<HexagonInfo>().resourceCount = transform.GetChild(i).childCount;
                    for (int j = 0; j < transform.GetChild(i).childCount; j++)
                    {
                        hexagonChilds.Add(transform.GetChild(i).transform.GetChild(j));
                    }
                }

            }
        }
        hexagons = newHexagons;
        return newHexagons;
    }
}

