using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class nextLevel : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int level = 0;
    public GameObject scoreText;

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Win"))
        {
            Destroy(scoreText);
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(level);
        }
    }

}
