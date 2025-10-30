using UnityEngine;

// 继承自抽象基类 ItemBase
public class GiganticBombItem : ItemBase
{
    // 在编辑器中指定用于生成巨大爆炸的预制体
    [SerializeField]
    private Explosion giganticExplosionPrefab_;

    // 实现抽象方法 Get：道具被获取时触发
    public override void Get()
    {
        // 在当前坐标生成爆炸效果
        Instantiate(giganticExplosionPrefab_, transform.position, Quaternion.identity);

        // 销毁自身
        Destroy(gameObject);
    }
}
