using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUIManager : MonoBehaviour {

    public static LobbyUIManager Instance => _instance;
    private static LobbyUIManager _instance;

    public Button MainBtn;

    private void Awake() {
        if (_instance == null)
            _instance = this;
    }

    private void Start() {
        MainBtn.onClick.AddListener(() => ToMainBtn());
    }

    public void ToMainBtn() {
        SceneManager.LoadScene("Main");
    }
}
