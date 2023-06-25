using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static UIManager Instance => _instance;
    private static UIManager _instance;

    public RectTransform Panel;
    public Button LobbyBtn;


    private void Awake() {
        if (_instance == null)
            _instance = this;
    }

    private void Start() {
        LobbyBtn.onClick.AddListener(() => ToLobbyBtn());

        HidePanel();
    }

    public void ShowPanel() {
        Panel.gameObject.SetActive(true);
    }

    public void HidePanel() {
        Panel.gameObject.SetActive(false);
    }

    public void ToLobbyBtn() {
        SceneManager.LoadScene("Lobby");
    }
}
