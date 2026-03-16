using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneBoxManager : MonoBehaviour
{
    public string sceneName;
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
        if (other.gameObject.tag == "Player" && sceneName == "map2")
        {
            Debug.Log("Collider entered");
            StartCoroutine(waitfortransitiontohub());
            //Go to next town scene.
            SceneManager.LoadScene("AidanTestScene");
        }
        else if(other.gameObject.tag == "Player" && sceneName == "Hub" || sceneName == "Tutorial")
        {
            Debug.Log("Collider entered");
            StartCoroutine(waitfortransitiontomap2());
            //Go to next map2 scene.
            SceneManager.LoadScene("map2");
        }
       

    }

    IEnumerator waitfortransitiontohub()
    {
        yield return new WaitForSeconds(1);
    }

    IEnumerator waitfortransitiontomap2()
    {
        yield return new WaitForSeconds(1);
    }
}
