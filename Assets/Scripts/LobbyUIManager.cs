using Google.Protobuf.WellKnownTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUIManager : MonoBehaviour {

    public static LobbyUIManager Instance => _instance;
    private static LobbyUIManager _instance;

    public Button MainBtn;

    public Slider PlayerCharacterSlider;
    public TMP_Text PlayerText;

    public List<Slider> AISliderList = new List<Slider>();
    public List<TMP_Text> AITextList = new List<TMP_Text>();
    public List<int> AIList = new List<int>();

    private void Awake() {
        if (_instance == null)
            _instance = this;
    }

    private void Start() {
        MainBtn.onClick.AddListener(() => ToMainBtn());
        PlayerCharacterSlider.onValueChanged.AddListener((value) => ChangeCharacter(value));
        AISliderList[0].onValueChanged.AddListener((value) => ChangeLongSwordAI(value));
        AISliderList[1].onValueChanged.AddListener((value) => ChangeBowAI(value));
        AISliderList[2].onValueChanged.AddListener((value) => ChangeAxeAI(value));

        AIList.Clear();
        AIList.Add(0);
        AIList.Add(0);
        AIList.Add(0);
    }

    public void ToMainBtn() {
        int cnt = AIList[0] + AIList[1] + AIList[2];
        if (cnt <= 0 || cnt > 7) {
            return;
        }

        GameManager.Instance.GameStart((int)PlayerCharacterSlider.value, AIList);
    }

    public void ChangeCharacter(float value) {
        string text = "Player: ";
        switch ((int)value) {
            case 0:
                text += "LongSword";
                break;
            case 1:
                text += "Bow";
                break;
            case 2:
                text += "Axe";
                break;
        }

        PlayerText.SetText(text);
    }

    public void ChangeLongSwordAI(float value) {
        AITextList[0].SetText("LongSword: " + (int)value);
        AIList[0] = (int)value;
    }

    public void ChangeBowAI(float value) {
        AITextList[1].SetText("Bow: " + (int)value);
        AIList[1] = (int)value;
    }
    public void ChangeAxeAI(float value) {
        AITextList[2].SetText("Axe: " + (int)value);
        AIList[2] = (int)value;
    }
}
