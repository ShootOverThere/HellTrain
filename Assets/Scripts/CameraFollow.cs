using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameControl gc;
    public float dampTime;
    public float zoomTime;
    private float cameraHeight = 5f;
    private Vector3 velocity = Vector3.zero;
    public static Transform target;
    Vector3 point;
    Vector3 delta;
    Vector3 destination;
    Vector3 lastVec;

    public float leftLimit;
    public float rightLimit;
    public float bottomLimit;
    public float topLimit;
}
