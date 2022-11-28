using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerData")]
public class PlayerDataSO : ScriptableObject
{
    public float speed = 50;
    public float runSpeed = 200;
    
    public float cameraSpeed = 40;
    public float cameraMaxAngle = 60;
    public float cameraMinAngle = -60;

    public float jumpStrength = 50;
}
