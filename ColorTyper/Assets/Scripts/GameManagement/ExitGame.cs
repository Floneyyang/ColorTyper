using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitGame : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            GameHandler.instance.SaveData();
            Application.Quit();
        }
    }

    public void OnButtonClick()
    {
        GameHandler.instance.SaveData();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name, LoadSceneMode.Single);
    }
}
