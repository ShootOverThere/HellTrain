using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public Texture2D srcTexture;
    Texture2D newTexture;
    SpriteRenderer sr;
    PolygonCollider2D temp_poly2d;
    List<PolygonCollider2D> polyList = new List<PolygonCollider2D>();

    float worldWidth, worldHeight;
    int pixelWidth, pixelHeight;

    // Start is called before the first frame update
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        newTexture = Instantiate(srcTexture);
        
        newTexture.Apply();
        MakeSprite();

        worldWidth = sr.bounds.size.x;
        worldHeight = sr.bounds.size.y;
        pixelWidth = sr.sprite.texture.width;
        pixelHeight = sr.sprite.texture.height;

        gameObject.AddComponent<PolygonCollider2D>();
    }

    public void resetTerrain()
    {
        Debug.Log("sibal");
        newTexture = Instantiate(srcTexture);
        srcTexture.Apply();
        sr.sprite = Sprite.Create(srcTexture, new Rect(0, 0, newTexture.width, newTexture.height), Vector2.one * 0.5f, 100f);

        worldWidth = sr.bounds.size.x;
        worldHeight = sr.bounds.size.y;
        pixelWidth = sr.sprite.texture.width;
        pixelHeight = sr.sprite.texture.height;
        for(int i = 0; i<polyList.Count; i++)
        {
            Debug.Log(i + "번째 삭제");
            Destroy(polyList[i]);
        }
        polyList.Clear();
        PolygonCollider2D temp = gameObject.AddComponent<PolygonCollider2D>();
        polyList.Add(temp);
    }

    public void MakeDot(Vector3 pos)
    {
        Vector2Int pixelPosition = WorldToPixel(pos);

        newTexture.SetPixel(pixelPosition.x, pixelPosition.y, Color.clear);
        newTexture.Apply();
        MakeSprite();
    }

    public void MakeAHole(CircleCollider2D c2d)
    {
        Vector2Int colliderCenter = WorldToPixel(c2d.bounds.center);
        int radius = Mathf.RoundToInt(c2d.bounds.size.x / 2 * pixelWidth / worldWidth);
        int px, nx, py, ny, distance;
        for(int i = 0; i < radius; i++)
        {
            distance = Mathf.RoundToInt(Mathf.Sqrt(radius * radius - i * i));
            for(int j = 0; j < distance; j++)
            {
                px = colliderCenter.x + i;
                nx = colliderCenter.x - i;
                py = colliderCenter.y + j;
                ny = colliderCenter.y - j;

                newTexture.SetPixel(px, py, Color.clear);
                newTexture.SetPixel(nx, py, Color.clear);
                newTexture.SetPixel(px, ny, Color.clear);
                newTexture.SetPixel(nx, ny, Color.clear);
            } 
        }

        newTexture.Apply();
        MakeSprite();
        if (gameObject.GetComponent<PolygonCollider2D>())
        {
            temp_poly2d = gameObject.GetComponent<PolygonCollider2D>();
            Destroy(gameObject.GetComponent<PolygonCollider2D>());
            PolygonCollider2D temp = gameObject.AddComponent<PolygonCollider2D>();
            polyList.Add(temp_poly2d);
            polyList.Add(temp);
        }
        /*if(!gameObject.GetComponent<PolygonCollider2D>())
            gameObject.AddComponent<PolygonCollider2D>();*/

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.GetComponent<CircleCollider2D>())
            return;
        if(col.CompareTag("Missile"))
            MakeAHole(col.GetComponent<CircleCollider2D>());
    }

    void MakeSprite()
    {
        sr.sprite = Sprite.Create(newTexture, new Rect(0, 0, newTexture.width, newTexture.height), Vector2.one * 0.5f, 100f);
    }

    private Vector2Int WorldToPixel(Vector3 pos)
    {
        Vector2Int pixelPosition = Vector2Int.zero;

        var dx = pos.x - transform.position.x;
        var dy = pos.y - transform.position.y;

        pixelPosition.x = Mathf.RoundToInt(0.5f * pixelWidth + dx * (pixelWidth / worldWidth));
        pixelPosition.y = Mathf.RoundToInt(0.5f * pixelHeight + dy * (pixelHeight / worldHeight));

        return pixelPosition;
    }
}
