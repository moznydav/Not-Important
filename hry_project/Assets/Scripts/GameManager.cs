using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private bool isGamePaused = false; 

    void Pause()
    {
        isGamePaused = true;
        Time.timeScale = 0f;
    }

    void Resume()
    {
        isGamePaused = false;
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (isGamePaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }
    }
}
