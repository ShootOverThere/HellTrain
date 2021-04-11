using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    Vector3 MousePosition;
    public LayerMask whatIsGround;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D overCollider2D = Physics2D.OverlapCircle(MousePosition, 0.01f, whatIsGround);
            if(overCollider2D != null)
            {
                overCollider2D.transform.GetComponent<Ground>().MakeDot(MousePosition);
            }
        }
    }
}
