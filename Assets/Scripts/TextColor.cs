using TMPro;
using UnityEngine;

public class TextColor : MonoBehaviour
{
    void Start()
    {
        // 获取TextMeshPro组件
        TextMeshProUGUI textMeshPro = GetComponent<TextMeshProUGUI>();

        // 设置字体颜色为红色
        textMeshPro.color = Color.blue;
    }
}
