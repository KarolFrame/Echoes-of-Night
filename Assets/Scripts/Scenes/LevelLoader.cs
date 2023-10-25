
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static string nextLevel;

    public static void LoadLevel(string name)
    {
        nextLevel= name;

        SceneManager.LoadScene("PantallaDeCarga");
    }
}
