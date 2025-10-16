using UnityEngine;
using TMPro;

/// <summary>
/// 控制分数文本的显示、上升动画以及自动销毁。
/// </summary>
[RequireComponent(typeof(TMP_Text))]
public class ScoreEffect : MonoBehaviour
{
    // 上升速度（单位：单位/秒）
    [SerializeField] private float upSpeed_ = 1f;

    // 显示持续时间（单位：秒）
    [SerializeField] private float aliveTime_ = 1f;

    // 存活计时器
    private float alivedTimer_ = 0f;

    /// <summary>
    /// 设置分数文本内容。
    /// </summary>
    /// <param name="score">要显示的分数</param>
    public void SetScore(int score)
    {
        GetComponent<TMP_Text>().text = score.ToString();
    }

    private void Update()
    {
        // 更新时间
        alivedTimer_ += Time.deltaTime;

        // 超时销毁
        if (alivedTimer_ >= aliveTime_)
        {
            Destroy(gameObject);
            return;
        }

        // 上升动画
        transform.Translate(Vector3.up * upSpeed_ * Time.deltaTime);
    }
}
