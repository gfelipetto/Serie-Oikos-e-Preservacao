using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GearBehavior : MonoBehaviour {


    private Button btnVoltarMenu;
    private void Start ( ) {
        if (!this.name.Contains("Menu")) {
            btnVoltarMenu = transform.Find("Panel").Find("Btn_VoltarMenu").GetComponent<Button>();
            btnVoltarMenu.onClick.AddListener(( ) => VoltaMenu());
        }
    }

    private void VoltaMenu ( ) {
        try {
            string activeScene = SceneManager.GetActiveScene().name;
            int c_index = activeScene.IndexOf('G');
            activeScene = activeScene.Remove(c_index, 4);
            activeScene += "Menu";
            FindObjectOfType<LevelLoader>().LoadScene(activeScene);
        } catch (Exception ex) {
            Debug.LogError($"Erro: {ex}");
        } finally {
            FindObjectOfType<LevelLoader>().LoadScene("Inicio");
        }
    }

    public void PauseGame ( bool status ) {
        Time.timeScale = status ? 0 : 1;
    }
}
