using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{

    [SerializeField]
    private UnityEngine.Camera _Camera;

    [SerializeField]
    private Player _Player;

    [Range(0.0f, 100.0f)]
    [SerializeField]
    private float _MoveToPlayerSpeed;
    
    [Range(0.1f, 100.0f)]
    [SerializeField]
    private float _CameraFinalSize;
    
    [Range(0.1f, 100.0f)]
    [SerializeField]
    private float _CameraFinalSizeSpeed;

    void Update()
    {
        _Camera.orthographicSize = Mathf.Lerp(_Camera.orthographicSize, _CameraFinalSize, Time.deltaTime * _CameraFinalSizeSpeed);

        float toPlayer = _Player.transform.position.x - transform.position.x;
        transform.position += new Vector3(toPlayer, 0.0f, 0.0f) * Time.deltaTime * _MoveToPlayerSpeed;
    }
}
