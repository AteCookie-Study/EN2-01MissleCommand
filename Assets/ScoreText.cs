using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class ScoreText : MonoBehaviour
{
    /// <summary>
    /// 表示用のスコア
    /// </summary>
    private int score_;

    /// <summary>
    /// テキスト本体
    /// </summary>
    private TMP_Text scoreText_;

    private void Start()
    {
        score_ = 0;
        scoreText_ = GetComponent<TMP_Text>();
        Debug.Assert(scoreText_ != null, "TMP_Text コンポーネントが見つかりません。ScoreText スクリプトが正しいオブジェクトにアタッチされているか確認してください。");
    }

    /// <summary>
    /// スコアを更新とテキストへの適用
    /// </summary>
    /// <param name="score">新しいスコア値</param>
    public void SetScore(int score)
    {
        score_ = score;
        UpdateScoreText();
    }

    /// <summary>
    /// テキストの更新
    /// </summary>
    private void UpdateScoreText()
    {
        // 数値は8桁0詰め
        if (scoreText_ != null)
        {
            scoreText_.text = $"SCORE:{score_:00000000}";
        }
        else
        {
            Debug.LogError("scoreText_ が null です。TMP_Text コンポーネントが正しく設定されているか確認してください。");
        }
    }
}