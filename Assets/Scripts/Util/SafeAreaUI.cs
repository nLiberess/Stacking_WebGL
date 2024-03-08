using UnityEngine;

public class SafeAreaUI : MonoBehaviour
{
    public RectTransform RectTs { get; private set; }

    /// <summary>
    /// Screen의 safeArea에 맞춰, Anchor를 재설정하는지
    /// </summary>
    [SerializeField] 
    private bool isReAnchor = true;
    
    private Rect safeArea;
    private Vector2 minAnchor;
    private Vector2 maxAnchor;

    private void Awake()
    {
        RectTs = GetComponent<RectTransform>();
        
        safeArea = Screen.safeArea;

        if (!isReAnchor)
            return;
        
        minAnchor = safeArea.position;
        maxAnchor = minAnchor + safeArea.size;

        minAnchor.x /= Screen.width;
        minAnchor.y /= Screen.height;
        
        maxAnchor.x /= Screen.width;
        maxAnchor.y /= Screen.height;

        RectTs.anchorMin = minAnchor;
        RectTs.anchorMax = maxAnchor;
    }

    public Vector2 GetSafeAreaSize()
    {
        return new Vector2(RectTs.rect.width, RectTs.rect.height);
    }
}