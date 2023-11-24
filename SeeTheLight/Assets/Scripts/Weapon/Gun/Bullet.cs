using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet")]
    public float speed;
    public GameObject explosionPrefab;
    public float autoRemoveTime = 10;
    protected Rigidbody2D rb;
    public int atk = 1;
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public virtual void SetSpeed(Vector2 direction)
    {
        rb.velocity = direction.normalized * speed;
        autoRemoveTime = 10;
        StartCoroutine(AutoRemove());
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        // 如果接触到非发射者
        if (collision.gameObject.tag != tag)
        {
            if (collision.gameObject.CompareTag("Interact"))
            {
                // 忽略可交互物品
            }
            else
            {
                //创造爆炸特效并回收
                mPool.Inst.GetFromPool(explosionPrefab.name).transform.position = transform.position;
                mPool.Inst.AddToPool(name, gameObject);
                if (gameObject.CompareTag("Player") && collision.gameObject.CompareTag("Enemy"))
                {
                    collision.gameObject.GetComponent<Enemy>().Hurt(atk);
                }
                else if (gameObject.CompareTag("Enemy") && collision.gameObject.CompareTag("Player"))
                {
                    collision.gameObject.GetComponent<cPlayer>().Hurt(atk);
                }
            }
        }
    }
    protected virtual IEnumerator AutoRemove()
    {
        while (autoRemoveTime > 0)
        {
            yield return new WaitForSeconds(1);
            autoRemoveTime--;
        }
        mPool.Inst.AddToPool(name, gameObject);
    }
}
