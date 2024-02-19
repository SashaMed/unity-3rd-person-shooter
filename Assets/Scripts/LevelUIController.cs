using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelUIController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuUI;

    [Header("Gun UI")]
    [SerializeField] private TextMeshProUGUI gunNameText;
    [SerializeField] private TextMeshProUGUI ammoAmountText;
    [SerializeField] private Image reloadImage;

    [Header("Player Health UI")]
    [SerializeField] private TextMeshProUGUI playerHealthText;
    [SerializeField] private Slider playerHealthSlider;
    [SerializeField] private Image healthImage;

    [Header("Level End UI")]
    [SerializeField] private GameObject leveEndUI;
    [SerializeField] private TextMeshProUGUI headerLevelEndUI;
    [SerializeField] private TextMeshProUGUI victoriesLevelEndUI;
    [SerializeField] private TextMeshProUGUI defeadsLevelEndUI;

    public static LevelUIController Instance { get; private set; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Time.timeScale = 0;
        leveEndUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }

    public void ShowLevelEndUI(string header, int victories, int defeats)
    {
        leveEndUI.SetActive(true);
        headerLevelEndUI.text= header;
        victoriesLevelEndUI.text = "victories " + victories;
        defeadsLevelEndUI.text = "defeats " + defeats;
        Cursor.lockState = CursorLockMode.None;

    }

    public void SetFillForReloadImage(float amount)
    {
        reloadImage.fillAmount = amount;
    }

    public void SetGunName(string text)
    {
        gunNameText.text = text;
    }

    public void SetAmmoAmountText(string text)
    {
        ammoAmountText.text = text;
    }


    public void SetPlayerHealth(float value, string text)
    {
        healthImage.fillAmount = value;
        playerHealthText.text = text;
        playerHealthSlider.value = value;
    }

    public void OnExitClick()
    {
        SoundPool.SoundInstance.PlayButtonSound();
        Application.Quit();
    }

    public void OnRestartClick()
    {
        SoundPool.SoundInstance.PlayButtonSound();
        var sceneNum = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneNum);
    }

    public void OnStartClick()
    {
        SoundPool.SoundInstance.PlayButtonSound();
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        mainMenuUI.SetActive(false);
    }
}
