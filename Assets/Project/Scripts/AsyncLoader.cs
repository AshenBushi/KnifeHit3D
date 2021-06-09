using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsyncLoader : MonoBehaviour
{
    private static AsyncOperation _operation;

    public static void PrepareScene()
    {
        _operation = SceneManager.LoadSceneAsync(1);
        _operation.allowSceneActivation = false;
    }

    public static void LoadScene()
    {
        _operation.allowSceneActivation = true;
    }
}
