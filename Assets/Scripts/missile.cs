using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missile : MonoBehaviour
{
    public GameObject explosionArea;
    public Rigidbody2D rb2d;
    TerrainDestroyer td;
    public float radius = 0.5f;
    bool hasHit = false;
    // Start is called before the first frame update
    void Start()
    {
        td = FindObjectOfType<TerrainDestroyer>();
        rb2d = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (hasHit == false)
        {
            float angle = Mathf.Atan2(rb2d.velocity.y, rb2d.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void OnDestroy()
    {
        //CameraFollow.target = GameObject.FindWithTag("Player").transform;
        Detonate();
        CameraFollow cm = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
        cm.CameraReset();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.transform.CompareTag("Ground"))
        {
            explosionArea.SetActive(true);
            Destroy(this.gameObject, 0.02f);
        }
        if(other.transform.CompareTag("Player") || other.transform.CompareTag("Enemy"))
        {
            return;
        }
        hasHit = true;
        rb2d.velocity = Vector2.zero;
        rb2d.isKinematic = true;
    }

    void Detonate()
    {
        td.DestroyTerrain(transform.position, radius);
    }
}
