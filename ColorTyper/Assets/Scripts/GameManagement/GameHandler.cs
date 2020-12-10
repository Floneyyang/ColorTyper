using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static GameHandler instance;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        LoadData();
    }

    public PlayerData player = new PlayerData();


    public void SaveData()
    {
        string path = Application.persistentDataPath + "/player.json";
        string content = JsonUtility.ToJson(player, true);
        System.IO.File.WriteAllText(path, content);
    }

    public void LoadData()
    {
        string path = Application.persistentDataPath + "/player.json";
        if (System.IO.File.Exists(path))
        {
            string content = System.IO.File.ReadAllText(path);
            player = JsonUtility.FromJson<PlayerData>(content);
        }
        else
        {
            Debug.Log("Unable to read the save data: file path does not exist!");
            player = new PlayerData();
        }

    }
}
