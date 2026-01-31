using UnityEngine;

public class S_CameraLocalisation : MonoBehaviour
{
    //[Header("Settings")]

    //[Header("References")]

    //[Header("Inputs")]

    [Header("Outputs")]
    [SerializeField] RSO_CameraRotation _cameraRotation;


    void Update()
    {
        _cameraRotation.Value = transform.rotation;
    }
}