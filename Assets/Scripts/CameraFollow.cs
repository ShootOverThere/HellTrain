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

    void Start()
    {
        GameObject player = gc.currentPlayer;
        target = player.transform;
    }
    void Update()
    {
        if (target != null)
        {
            point = Camera.main.WorldToViewportPoint(target.position);
            delta = target.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
        }
        else
        {
            point = Camera.main.WorldToViewportPoint(lastVec);
            delta = lastVec - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
        }
        
        destination = transform.position + delta;
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);

        transform.position = new Vector3
            (
                Mathf.Clamp(transform.position.x, leftLimit, rightLimit),
                Mathf.Clamp(transform.position.y, bottomLimit, topLimit),
                transform.position.z
            );
        //Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, cameraHeight * target.localScale.x, zoomTime);
    }

    void CameraToPlayer()
    {
        GameObject player = gc.currentPlayer;
        target = player.transform;
        gc.ChangePlayer();
        //target = gc.SetPlayer().transform;
    }

    public void CameraReset()
    {
        lastVec = target.position;
        Invoke("CameraToPlayer", 1.5f);
        playerAiming.missileCount--;
    }
}
