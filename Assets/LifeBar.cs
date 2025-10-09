using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class LifeBar : MonoBehaviour
{
    // Slider の割合
    private float ratio_;

    // Slider コンポーネント
    private Slider slider_;

    private void Awake()
    {
        // Slider コンポーネントの取得
        slider_ = GetComponent<Slider>();
    }

    /// <summary>
    /// Slider の割合を設定
    /// </summary>
    /// <param name="ratio">0〜1の範囲の割合</param>
    public void SetGaugeRatio(float ratio)
    {
        // 0〜1の範囲で切り詰める
        ratio_ = Mathf.Clamp01(ratio);

        // UI に反映
        slider_.value = ratio_;
    }
}
