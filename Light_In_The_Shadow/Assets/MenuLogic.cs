using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLogic : MonoBehaviour {
    public PlayerController player;
    public GameObject settingsPanel, pauseMenuPanel, mainMenuPanel;
    public bool paused = false, isMainMenu = false;
    public MasterManager manager;
    public GameObject blur;

    private void Start() {
        manager = MasterManager.Instance;
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0)) {
            isMainMenu = true;
            player.FreezePlayer(true);
            blur.SetActive(true);
        }
        else mainMenuPanel.SetActive(false);
    }

    void Update() {
        if (isMainMenu) return;
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (!paused) {
                player.FreezePlayer(true);
                pauseMenuPanel.SetActive(true);
                paused = true;
                Time.timeScale = 0.0f;
                blur.SetActive(true);
            }
            else Play();
        }
    }

    public void Play() {
        Time.timeScale = 1.0f;
        player.FreezePlayer(false);
        mainMenuPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        paused = false;
        blur.SetActive(false);
        if (isMainMenu) {
            manager.StartLoadingNextScene();
        }
    }

    public void Settings() {
        mainMenuPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void Back() {
        if (isMainMenu) {
            mainMenuPanel.SetActive(true);
            pauseMenuPanel.SetActive(false);
            settingsPanel.SetActive(false);
        }
        else {
            pauseMenuPanel.SetActive(true);
            settingsPanel.SetActive(false);
            mainMenuPanel.SetActive(false);
        }
    }


   

    public void Exit() {
        if (isMainMenu) Application.Quit();
        else SceneManager.LoadScene(0);
    }
}