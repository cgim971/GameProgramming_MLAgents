using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {
    public static CameraManager Instance => _instance;
    private static CameraManager _instance;

    private CinemachineVirtualCamera _vCam;

    private float _shakeTimer = 0f;
    private float _shakeIntensity = 0f;

    private void Awake() {
        if (_instance == null)
            _instance = this;

        _vCam = GetComponent<CinemachineVirtualCamera>();
    }

    private void Update() {
        if (_shakeTimer > 0f) {
            _shakeTimer -= Time.deltaTime;
            if (_shakeTimer <= 0f) {
                _vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f;
            }
        }
    }

    public void SetFollow(Transform ts) {
        _vCam.m_Follow = ts;
    }

    public void ShakeCamera(float shakeDuration, float shakeAmplitude) {
        _shakeTimer = shakeDuration;
        _shakeIntensity = shakeAmplitude;

        _vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = _shakeIntensity;
    }
}
