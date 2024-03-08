using UnityEngine;

public class Ribbon : MonoBehaviour
{
    /// <summary>
    /// 파괴될 때의 파티클 효과
    /// </summary>
    [SerializeField] private GameObject destroyEft;
    
    /// <summary>
    /// 축소되는 속도
    /// </summary>
    [SerializeField, Range(0.1f, 2f)] private float speed = 0.5f;
    
    /// <summary>
    /// 쌓여있는 DropEntity에 닿았는지
    /// </summary>
    private bool isCol;
    
    private void Start()
    {
        speed = 0.5f;
    }

    private void Update()
    {
        if(GameManager.Inst.IsGameOver)
            Destroy(gameObject);
        
        if (isCol)
        {
            transform.localScale -= Time.deltaTime * speed * Vector3.one;
            if (transform.localScale.y <= 0f)
            {
                // 파티클생성
                Instantiate(destroyEft, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isCol)
        {
            transform.localScale -= Time.deltaTime * speed * Vector3.up;
            return;
        }
        
        if (other.TryGetComponent(out DropEntity dropEntity))
        {
            // 쌓여있는게 닿아야함
            if (dropEntity.isLanding)
            {
                isCol = true;
                GameManager.Inst.AddGiftBox();
            }
        }
    }
}
