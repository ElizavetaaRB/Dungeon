using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    [SerializeField] private InputManagerS0 inputManager;
    [SerializeField] private GameObject pauseMenu;
    private void Awake()
    {
        if (pauseMenu != null)
            pauseMenu.SetActive(false);
    }


    private void OnEnable()
    {
        if (inputManager != null)
            inputManager.OnPause += Pause;
    }
    private void OnDisable()
    {
        if (inputManager != null)
            inputManager.OnPause -= Pause;
    }

    private void OnDestroy()
    {
        // Limpieza
        if (inputManager != null)
            inputManager.OnPause -= Pause;
    }
    private void Update()
    {

    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        if (pauseMenu != null)
            pauseMenu.SetActive(false);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    private void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }
}
