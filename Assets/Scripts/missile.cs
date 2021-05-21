using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missile : MonoBehaviour
{
    public GameObject explosionArea;
    public Rigidbody2D rb2d;
    public TankAgent parent_tank;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float angle = Mathf.Atan2(rb2d.velocity.y, rb2d.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void OnDestroy()
    {
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Ground"))
        {
            explosionArea.SetActive(true);
        }
        this.gameObject.SetActive(false);
        parent_tank.missile_destroyed();
    }

    private void OnBecameInvisible()
    {
        this.gameObject.SetActive(false);
    }

    public void Init_Missile(SpriteRenderer aimSprite)
    {
        this.transform.position = aimSprite.transform.position;
        this.transform.rotation = aimSprite.transform.rotation;
    }
}
