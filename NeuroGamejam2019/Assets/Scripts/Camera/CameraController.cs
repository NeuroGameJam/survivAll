using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private ProceduralGrid grid;

    public float xOff;
    public float yOff;
    public float zOff;
   
    void Start()
    {
        grid = FindObjectOfType<ProceduralGrid>();
        Vector2 newPos = grid.GetCenter();
        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z) + new Vector3(xOff,yOff,zOff);
    }
}
