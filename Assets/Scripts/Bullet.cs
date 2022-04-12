using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] GameObject destroyVfx;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Tank tank = collision.GetComponent<Tank>();

        if (tank != null)
        {
            tank.Damage(damage);
            Explode();
        }
        else if(collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Explode();
        }    
    }

    private void Explode()
    {
        GameObject vfx = Instantiate(destroyVfx);
        vfx.transform.position = transform.position;
        vfx.transform.up = transform.right;

        Destroy(vfx, 2f);

        Destroy(gameObject);
    }
}
