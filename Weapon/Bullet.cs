using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject bulletImpactFX;

    private Rigidbody rb => GetComponent<Rigidbody>();

    private void OnCollisionEnter(Collision collision)
    {
        CreateImpactFX(collision);

        //rb.constraints = RigidbodyConstraints.FreezeAll;
        ObjectPool.instance.ReturnBullet(gameObject);
    }

    private void CreateImpactFX(Collision collision)
    {
        if (collision.contacts.Length > 0)
        {
            ContactPoint contactPoint = collision.contacts[0];

            GameObject newImpactFX = Instantiate(bulletImpactFX, contactPoint.point, Quaternion.LookRotation(contactPoint.normal));

            Destroy(newImpactFX, 1f);
        }
    }
}
