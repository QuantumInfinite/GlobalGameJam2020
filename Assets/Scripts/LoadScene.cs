using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string stringLevelName;
    public float timeDelay;

    public void LoadLevel()
    {
        StartCoroutine(StartLevel());
    }

    public IEnumerator StartLevel()
    {
        yield return new WaitForSeconds(timeDelay);

        SceneManager.LoadScene(stringLevelName);
    }
}