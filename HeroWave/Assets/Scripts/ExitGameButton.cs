using UnityEngine;

public class ExitGameButton : MonoBehaviour
{
    public void ExitApplication()
    {
        Debug.Log("Exiting the application.");

        Application.Quit();
    }
}
