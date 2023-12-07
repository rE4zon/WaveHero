using UnityEngine;

public class ExitGameButton : MonoBehaviour
{
    public void ExitApplication()
    {
        Debug.Log("Closing the app");

        Application.Quit();
    }
}
