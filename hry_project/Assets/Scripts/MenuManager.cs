using UnityEngine;

public class MenuManager : Singleton<MenuManager>
{
    public void Play()
    {
        print("Play");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
