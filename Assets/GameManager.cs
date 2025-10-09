
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField, Header("Prefabs")]
    private Explosion explosionPrefab_;
    private Camera mainCamera_;
    [SerializeField]
    private Meteor meteorPrefab_;

    [SerializeField, Header("MeteorSpawner")]
    private BoxCollider2D ground_;

    [SerializeField]
    private float meteorInterval_ = 1.0f;
    private float meteorTimer_ = 0.0f;

    [SerializeField]
    private List<Transform> spawnPositions_;

    // スコア関係
    [SerializeField, Header("ScoreUISettings")]
    private ScoreText scoreText_; // スコア表示用テキスト

    private int score_; // スコア本体

    // ライフ関係
    [SerializeField, Header("LifeUISettings")]
    private LifeBar lifeBar_; // ライフゲージ

    [SerializeField]
    private float maxLife_ = 10f; // 最大体力
    [SerializeField]
    private float life_; // 現在体力




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject mainCameraObject = GameObject.FindGameObjectWithTag("MainCamera");
        bool isGetComponent = mainCameraObject.TryGetComponent<Camera>(out mainCamera_);
        Assert.IsTrue(isGetComponent,"MainCamera にCameraコンポーネントがありません");

        Assert.IsTrue(spawnPositions_.Count > 0, "spawnPositions_に要素が一つもありません");
        foreach(Transform t in spawnPositions_)
        {
            Assert.IsNotNull(t, "spawnPositions_にNullが含まれています");
        }
        ResetLife();
    }
    private void GenerateExplosion()
    {
        Vector3 clickPosition = mainCamera_.ScreenToWorldPoint(Input.mousePosition);
        clickPosition.z = 0.0f;

        Explosion explosion = Instantiate(explosionPrefab_, clickPosition, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GenerateExplosion();
        }
        UpdateMeteorTimer();
        UpdateLifeBar();
    }
    public void AddScore(int point)
    {
        score_ += point;
        scoreText_.SetScore(score_);
    }



    public void Daange(int point)
    {
        life_ -= point;
        UpdateLifeBar();
    }

    private void UpdateMeteorTimer()
    {
        meteorTimer_ -= Time.deltaTime;
        if (meteorTimer_ > 0.0f) { return; }
        meteorTimer_ += meteorInterval_;
        GenerateMeteor();
    }

    private void GenerateMeteor()
    {
        int max = spawnPositions_.Count;
        int posIndex = Random.Range(0, max);
        Vector3 spawnPosition = spawnPositions_[posIndex].position;
        Meteor meteor = Instantiate(meteorPrefab_, spawnPosition, Quaternion.identity);
        meteor.Setup(ground_, this, explosionPrefab_);
    }

    /// <summary>
    /// ライフの初期化
    /// </summary>
    private void ResetLife()
    {
        life_ = maxLife_;
        UpdateLifeBar();
    }

    /// <summary>
    /// ライフUIの更新
    /// </summary>
    private void UpdateLifeBar()
    {
        float lifeRatio = Mathf.Clamp01(life_ / maxLife_);
        lifeBar_.SetGaugeRatio(lifeRatio);
    }


}

