using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    public GameObject StartGame;
    public GameObject EndGame;

    public LayerMask hexLayerMask;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Проверяем пересечение с объектами слоя hexLayerMask
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, hexLayerMask))
            {
                if (hit.collider.gameObject == StartGame.gameObject)
                {
                    SceneManager.LoadScene("Game");
                    return;
                }

                if (hit.collider.gameObject == EndGame.gameObject)
                {
                    Application.Quit();
                    return;
                }
            }
        }
    }
}
