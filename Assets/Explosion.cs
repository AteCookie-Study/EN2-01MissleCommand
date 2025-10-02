using UnityEngine;

public class Explosion : MonoBehaviour
{
    private float maxLifeTime = 1.0f; // seconds
    private float time_ = 0.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        time_ = maxLifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        time_ -= Time.deltaTime;
        if (time_ > 0) { return; }
        Destroy(gameObject);
    }
}
