using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private int levelSceneIndex = 1;
    public void OnStartClick()
    {
        SoundPool.SoundInstance.PlayButtonSound();
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        SceneManager.LoadScene(levelSceneIndex);

    }


    public void OnExitClick()
    {
        SoundPool.SoundInstance.PlayButtonSound();
        Application.Quit();
    }
}
