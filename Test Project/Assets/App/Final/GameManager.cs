using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private void OnEnable()
    {
        GameEvents.instance.OnGameOver += ChangeScene;
    }

    private void OnDisable()
    {
        GameEvents.instance.OnGameOver -= ChangeScene;
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene("Game Over");
    }
}
