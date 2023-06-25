using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Policies;
using UnityEngine;

public class StageManager : MonoBehaviour {

    public static StageManager Instance => _instance;
    private static StageManager _instance;

    [SerializeField] private List<AI> _aiList = new List<AI>();
    [SerializeField] private Dictionary<int, AI> _aiDictionary = new Dictionary<int, AI>();
    private List<AI> _currentAiList = new List<AI>();

    public bool IsDie = false;


    private void Awake() {
        if (_instance == null)
            _instance = this;

        List<int> aiList = new List<int>();
        aiList.Add(1);
        aiList.Add(2);
        aiList.Add(1);
        Init(0, aiList);
    }

    public void Init(int characterIndex, List<int> aiList) {
        int index = 0;

        // 캐릭터 생성
        AI ai = Instantiate(_aiList[characterIndex], transform);
        ai.IsPlayer = true;
        ai.GetComponent<BehaviorParameters>().BehaviorType = BehaviorType.HeuristicOnly;
        CameraManager.Instance.SetFollow(ai.transform);

        _aiDictionary[index++] = ai;
        _currentAiList.Add(ai);
        ai = null;

        for (int i = 0; i < aiList.Count; i++) {
            for (int j = 0; j < aiList[i]; j++) {
                ai = Instantiate(_aiList[i], transform);
                _aiDictionary[index++] = ai;
                _currentAiList.Add(ai);
            }
        }

        for (int i = 0; i < _aiDictionary.Count; i++) {
            for (int j = 0; j < _aiDictionary.Count; j++) {
                if (i == j)
                    continue;

                _aiDictionary[i].TargetList.Add(_aiDictionary[j].gameObject);
            }
        }
    }

    public void Die(AI ai, bool isPlayer) {
        if (isPlayer) {
            IsDie = true;
        }

        foreach (int i in _aiDictionary.Keys) {
            if (ai == _aiDictionary[i]) {
                _aiDictionary.Remove(i);

                if (_currentAiList[_index] == ai)
                    SetFollow();

                _currentAiList.Remove(ai);
                break;
            }
        }

        if (_aiDictionary.Count == 1) {
            // 게임 종료
        }
    }

    private void Update() {
        if (!IsDie)
            return;

        if (Input.GetMouseButtonDown(0)) {
            SetFollow();
        }
    }

    int _index = 0;
    void SetFollow() {
        _index++;

        if (_index >= _currentAiList.Count)
            _index = 0;

        while (_currentAiList[_index] == null) {
            _index++;
            if (_index >= _currentAiList.Count)
                _index = 0;
        }

        CameraManager.Instance.SetFollow(_currentAiList[_index].transform);
    }
}
