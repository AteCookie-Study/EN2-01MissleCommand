using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Meteor : MonoBehaviour
{
    /// <summary>
    /// 最低下落速度
    /// </summary>
    [SerializeField] private float fallSpeedMin_= 1.0f;

    /// <summary>
    /// 最高下落速度
    /// </summary>
    [SerializeField] private float fallSpeedMax_ = 3.0f;

    // スコアエフェクトプレハブ
    [SerializeField] ScoreEffect scoreEffectPrefab_;

    /// <summary>
    /// 爆発プレハブ。生成元から受け取る
    /// </summary>
    private Explosion explosionPrefab_;

    /// <summary>
    /// 地面プレハブ。生成元から受け取る
    /// </summary>
    private BoxCollider2D groundCollider_;
    private Rigidbody2D rb_;
    private GameManager gameManager_;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{
        
    //}

    private void Start() 
    { 
    rb_ = GetComponent<Rigidbody2D>();
        SetupVlocity();
    }
    public void Setup(BoxCollider2D ground, GameManager gameManager, Explosion explosionPrefab) 
    { 
        gameManager_ = gameManager;
        groundCollider_ = ground;
        explosionPrefab_ = explosionPrefab;
    }

    private void SetupVlocity() 
    { 
    float left = groundCollider_.bounds.center.x - groundCollider_.bounds.extents.x / 2;
    float right = groundCollider_.bounds.center.x + groundCollider_.bounds.extents.x / 2;
    float top = groundCollider_.bounds.center.y + groundCollider_.bounds.extents.y / 2;
    float bottom = groundCollider_.bounds.center.y - groundCollider_.bounds.extents.y / 2;

        float targetX = Mathf.Lerp(left, right, Random.Range(0.0f, 1.0f));

        Vector3 target = new Vector3(targetX, top, 0.0f);
        Vector3 direction = (target - transform.position).normalized;
        float fallSpeed = Random.Range(fallSpeedMin_, fallSpeedMax_);
        rb_.linearVelocity = direction * fallSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Explosion explosion;
    if(collision.gameObject.CompareTag("Explosion")&&
            collision.TryGetComponent(out explosion))
        { Explosion(explosion); }
    if(collision.gameObject.CompareTag("Ground"))
        { Fall(); }

    }



    private void Explosion(Explosion otherExplosion)
    {
        int chainNum = otherExplosion.chainNum + 1;

        int score = chainNum * 100;

        // GameManager に score 加算を通知
        gameManager_.AddScore(score);

        // スコアエフェクト生成
        ScoreEffect scoreEffect = Instantiate(scoreEffectPrefab_, transform.position, Quaternion.identity);
        scoreEffect.SetScore(score); 


        // スコアエフェクト生成
        Explosion explosion = Instantiate(explosionPrefab_, transform.position, Quaternion.identity);
       explosion.chainNum = chainNum;
        
        Destroy(gameObject);
    }

    private void Fall() 
    {
    gameManager_.Daange(1);

        Destroy(gameObject);

        Debug.Log("Fall");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
