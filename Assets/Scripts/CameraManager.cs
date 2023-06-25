using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {
    public static CameraManager Instance => _instance;
    private static CameraManager _instance;

    private CinemachineVirtualCamera _vCam;

    private void Awake() {
        if(_instance == null)   
            _instance = this;

        _vCam = GetComponent<CinemachineVirtualCamera>();
    }

    public void SetFollow(Transform ts) {
        _vCam.m_Follow = ts;
    }


}
