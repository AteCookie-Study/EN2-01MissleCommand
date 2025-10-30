using UnityEngine;
using UnityEngine.Assertions;

public class Missile : MonoBehaviour
{
    // 爆炸效果的预制体（由 GameManager 赋值）
    [SerializeField] private Explosion explosionPrefab_;

    // 移动速度（由 Inspector 设置）
    [SerializeField] private float speed_;

    // 移动向量（由 SetupVelocity 计算）
    private Vector3 velocity_;

    // 目标 Reticle（由 GameManager 生成并传入）
    private GameObject reticle_;

    /// <summary>
    /// 初始化导弹，接收目标 Reticle 并计算移动参数
    /// </summary>
    public void Setup(GameObject reticle)
    {
        reticle_ = reticle;

        if (reticle.transform.position != transform.position)
        {
            SetupVelocity();   // 计算移动量
            LookAtReticle();   // 计算朝向
        }
        else
        {
            Explosion();       // 如果位置重合，立即爆炸
        }
    }

    /// <summary>
    /// 计算导弹的移动向量
    /// </summary>
    private void SetupVelocity()
    {
        Vector3 direction = reticle_.transform.position - transform.position;
        Assert.IsTrue(direction != Vector3.zero); // 防止除以零
        direction = direction.normalized;
        velocity_ = direction * speed_;
    }

    /// <summary>
    /// 使导弹朝向目标 Reticle
    /// </summary>
    private void LookAtReticle()
    {
        float angle = Mathf.Atan2(velocity_.y, velocity_.x) * Mathf.Rad2Deg;
        angle -= 90; // Capsule 默认朝右，需旋转 90 度
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    /// <summary>
    /// 生成爆炸效果并销毁导弹与目标 Reticle
    /// </summary>
    private void Explosion()
    {
        Instantiate(explosionPrefab_, transform.position, Quaternion.identity);
        Destroy(reticle_);
        Destroy(gameObject);
    }

    /// <summary>
    /// 每帧更新导弹位置，判断是否到达目标
    /// </summary>
    private void Update()
    {
        float distanceSqr = Vector3.SqrMagnitude(reticle_.transform.position - transform.position);
        Vector3 velocityDeltaTime = velocity_ * Time.deltaTime;
        float velocityDistanceSqr = Vector3.SqrMagnitude(velocityDeltaTime);

        if (distanceSqr >= velocityDistanceSqr)
        {
            transform.position += velocityDeltaTime;
            return;
        }

        transform.position = reticle_.transform.position;
        Explosion();
    }
}
