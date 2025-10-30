// ItemBase.cs
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
abstract public class ItemBase : MonoBehaviour
{
    //  移动速度（派生类可访问）
    [SerializeField] protected float speed_ = 3;

    //  摄像机引用（用于屏幕边界判断）
    protected Camera camera_;

    //  碰撞体引用（用于尺寸判断）
    protected Collider2D collider_;

    // 初始化：获取主摄像机和自身碰撞体
    private void Awake()
    {
        camera_ = Camera.main;
        collider_ = GetComponent<Collider2D>();
    }

    //  更新：右移 + 屏幕外销毁（可被派生类 override）
    protected virtual void Update()
    {
        // 移动
        transform.Translate(Vector3.right * speed_ * Time.deltaTime);

        // 屏幕右边界计算
        float worldScreenRight = camera_.orthographicSize * camera_.aspect;

        // 自身宽度（用于完全离开屏幕判断）
        float boundsSize = collider_.bounds.size.x;

        // 超出屏幕则销毁
        if (transform.position.x > worldScreenRight + boundsSize)
        {
            Destroy(gameObject);
        }
    }

    //  碰撞检测：与 Explosion 标签接触时触发 Get()
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Explosion"))
        {
            Get();
        }
    }

    // 抽象方法：必须由派生类实现
    public abstract void Get();
}
