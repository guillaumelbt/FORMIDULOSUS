using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform camera;
    [SerializeField] private PlayerInput input;
    [SerializeField] private PlayerDataSO playerData;
    private float rotationX = 0;
    void Update()
    {
        var axis = input.actions["Look"].ReadValue<Vector2>().normalized;
        
        transform.Rotate(new Vector3(0,axis.x * playerData.cameraSpeed * Time.deltaTime,0));
        
        rotationX += -axis.y * playerData.cameraSpeed * Time.deltaTime;
        rotationX= Mathf.Clamp(rotationX, playerData.cameraMinAngle, playerData.cameraMaxAngle);
        Vector3 rotation = camera.eulerAngles;
        rotation.x = rotationX;
        camera.rotation = Quaternion.Euler(rotation);
    }
}
