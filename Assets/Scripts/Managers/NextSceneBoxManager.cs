using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneBoxManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Collider entered");
            StartCoroutine(waitfortransition());
            //Go to next town scene.
            SceneManager.LoadScene("AidanTestScene");
        }
    }

    IEnumerator waitfortransition()
    {
        yield return new WaitForSeconds(1);
    }
}
