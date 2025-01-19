using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    [SerializeField] int sceneIndex;

    public void OnClick()
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
