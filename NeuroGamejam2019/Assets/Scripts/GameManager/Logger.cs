using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;


public class Logger : MonoBehaviour
{

    //public InputField nameField;
    private StreamWriter file;
    public IntroMenu introMenu;
    public ProceduralGrid grid;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void initFile()
    {
        System.DateTime theTime = System.DateTime.Now;
        string date = theTime.ToString("yyyy-MM-dd");
        string time = theTime.ToString("HH:mm:ss");
        string datetime = theTime.ToString("yyyy_MM_dd\\;HH_mm_ss");

        int i = 0;
        while (File.Exists("./Logs/Log_" + i))
        {
            i++;
        }

        FileStream filePtr = File.Open("./Logs/Log_" + i, FileMode.OpenOrCreate, FileAccess.Write);  //Log written to project folder
        file = new StreamWriter(filePtr);

        file.WriteLine(datetime + " ; " + "PLAYER_NAME" + ";" + grid.Hexagons.Count); //TODO replace por inputfield.text ir buscar o nome a scene da vitoria
        // file.WriteLine(datetime + " ; " + "PLAYER_NAME"); //TODO replace por inputfield.text ir buscar o nome a scene da vitoria
        file.WriteLine("Play_number Play_Type Player_Wealth World_Health World_Delta Current_Tile");
    }

    public void Log(string s)
    {
        file.WriteLine(s);
    }

    public void CloseLog()
    {
        file.Close();
    }
}
