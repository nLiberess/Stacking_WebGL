using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    private Text txt;

    private void Start()
    {
        txt = GetComponent<Text>();
    }

    private void Update()
    {
        if(txt.color.a <= 0)
            Destroy(gameObject);
    }
}
