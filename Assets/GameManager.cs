using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameManager : MonoBehaviour
{
    [SerializeField, Header("Prefabs")]
    private Explosion explosionPrefab_;
    [SerializeField]
    private GameObject reticlePrefab_;
    [SerializeField]
    private Missile missilePrefab_;
    [SerializeField]
    private Meteor meteorPrefab_;

    private Camera mainCamera_;

    [SerializeField, Header("MeteorSpawner")]
    private BoxCollider2D ground_;
    [SerializeField]
    private float meteorInterval_ = 1.0f;
    private float meteorTimer_ = 0.0f;
    [SerializeField]
    private List<Transform> spawnPositions_;

    [SerializeField, Header("ScoreUISettings")]
    private ScoreText scoreText_;
    private int score_;

    [SerializeField, Header("LifeUISettings")]
    private LifeBar lifeBar_;
    [SerializeField]
    private float maxLife_ = 10f;
    [SerializeField]
    private float life_;

    // 登録されたアイテムのプレハブ一覧
    [SerializeField, Header("Prefabs")]
    private List<ItemBase> items_; 

    // アイテム生成位置
    [SerializeField, Header("ItemSettings")]
    private Transform itemSpawnPoint_; 

    // アイテム生成間隔（秒
    [SerializeField]
    private float itemSpawnInterval_ = 10f; 

    // アイテム生成用タイマー
    private float itemTimer_ = 0f;



    void Start()
    {
        GameObject mainCameraObject = GameObject.FindGameObjectWithTag("MainCamera");
        bool isGetComponent = mainCameraObject.TryGetComponent(out mainCamera_);
        Assert.IsTrue(isGetComponent, "MainCamera に Camera コンポーネントがありません");

        Assert.IsTrue(spawnPositions_.Count > 0, "spawnPositions_ に要素がありません");
        foreach (Transform t in spawnPositions_)
        {
            Assert.IsNotNull(t, "spawnPositions_ に Null が含まれています");
        }

        ResetLife();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GenerateMissile();
        }

        UpdateMeteorTimer();
        UpdateLifeBar();
        UpdateItemTimer();
    }

    /// <summary>
    /// ミサイルとレティクルを生成する
    /// </summary>
    private void GenerateMissile()
    {
        Vector3 clickPosition = mainCamera_.ScreenToWorldPoint(Input.mousePosition);
        clickPosition.z = 0.0f;

        GameObject reticle = Instantiate(reticlePrefab_, clickPosition, Quaternion.identity);

        Vector3 launchPosition = new Vector3(0, -3, 0); // 固定発射位置
        Missile missile = Instantiate(missilePrefab_, launchPosition, Quaternion.identity);
        missile.Setup(reticle);
    }

    /// <summary>
    /// スコア加算
    /// </summary>
    public void AddScore(int point)
    {
        score_ += point;
        scoreText_.SetScore(score_);
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    public void Daange(int point)
    {
        life_ -= point;
        UpdateLifeBar();
    }

    /// <summary>
    /// 流星生成タイマー更新
    /// </summary>
    private void UpdateMeteorTimer()
    {
        meteorTimer_ -= Time.deltaTime;
        if (meteorTimer_ > 0.0f) return;

        meteorTimer_ += meteorInterval_;
        GenerateMeteor();
    }

    /// <summary>
    /// 流星生成
    /// </summary>
    private void GenerateMeteor()
    {
        int max = spawnPositions_.Count;
        int posIndex = Random.Range(0, max);
        Vector3 spawnPosition = spawnPositions_[posIndex].position;

        Meteor meteor = Instantiate(meteorPrefab_, spawnPosition, Quaternion.identity);
        meteor.Setup(ground_, this, explosionPrefab_);
    }

    /// <summary>
    /// ライフ初期化
    /// </summary>
    private void ResetLife()
    {
        life_ = maxLife_;
        UpdateLifeBar();
    }

    /// <summary>
    /// ライフゲージ更新
    /// </summary>
    private void UpdateLifeBar()
    {
        float lifeRatio = Mathf.Clamp01(life_ / maxLife_);
        lifeBar_.SetGaugeRatio(lifeRatio);
    }

    private ItemBase PickupItem()
    {
        int itemPrefabNum = items_.Count;
        Assert.IsTrue(itemPrefabNum > 0); // アイテムが1つ以上登録されていることを確認

        int pickedupIndex = Random.Range(0, itemPrefabNum); // ランダムインデックス取得
        ItemBase pickedupItem = items_[pickedupIndex];
        return pickedupItem;
    }

    private void UpdateItemTimer()
    {
        itemTimer_ -= Time.deltaTime;
        if (itemTimer_ > 0) return;

        itemTimer_ += itemSpawnInterval_; // 次回生成までのタイマーリセット
        ItemBase pickedUpItem = PickupItem(); // ランダムアイテム取得
        Instantiate(pickedUpItem, itemSpawnPoint_.position, Quaternion.identity); // 生成
    }

}
