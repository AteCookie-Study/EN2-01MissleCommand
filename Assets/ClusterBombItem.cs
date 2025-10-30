using UnityEngine;

// ItemBase を継承
public class ClusterBombItem : ItemBase
{
    // 爆発プレハブ（小爆発用）
    [SerializeField]
    private Explosion explosionPrefab_;

    // 取得済みフラグ
    private bool isGet = false;

    // 爆発持続時間（秒）
    private float explosionEmmitionTimer_ = 3f;

    // 爆発間隔（秒）
    private float explosionInterval_ = 0.2f;

    // 爆発タイマー
    private float explosionTimer_ = 0.0f;

    // 見た目制御用
    private Renderer renderer_;

    // アイテム取得時の処理
    public override void Get()
    {
        // Renderer を取得して非表示にする
        if (TryGetComponent(out renderer_))
        {
            renderer_.enabled = false;
        }

        // 当たり判定を無効化
        collider_.enabled = false;

        // 子オブジェクト（TextMeshなど）を非表示に
        transform.GetChild(0).gameObject.SetActive(false);

        // 取得済みフラグを立てる
        isGet = true;
    }

    // 毎フレームの処理
    protected override void Update()
    {
        // 未取得なら通常の移動処理（基底クラスの Update を呼ぶ）
        if (!isGet)
        {
            base.Update();
            return;
        }

        // 爆発持続タイマーを減算
        explosionEmmitionTimer_ -= Time.deltaTime;

        // 時間切れで自身を削除
        if (explosionEmmitionTimer_ <= 0)
        {
            Destroy(gameObject);
            return;
        }

        // クラスター爆発処理
        UpdateClusterExplosion();
    }

    // 小爆発の生成処理
    private void UpdateClusterExplosion()
    {
        // 爆発間隔タイマーを減算
        explosionTimer_ -= Time.deltaTime;

        // まだ間隔に達していなければ何もしない
        if (explosionTimer_ > 0)
        {
            return;
        }

        // ランダムなオフセット位置を決定
        float randomWidth = 2f;
        Vector3 offset = new Vector3(
            Random.Range(-randomWidth, randomWidth),
            Random.Range(-randomWidth, randomWidth),
            0f
        );

        // 爆発を生成
        Instantiate(explosionPrefab_, transform.position + offset, Quaternion.identity);

        // 次の爆発までのタイマーをリセット
        explosionTimer_ = explosionInterval_;
    }
}
