using UnityEngine;

public class RingEffect : MonoBehaviour
{
    private SpriteRenderer spriteRd;
    
    private void Start()
    {
        spriteRd = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(spriteRd.color.a <= 0f)
            Destroy(gameObject);
    }
}
