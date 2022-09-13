using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BarrelController : MonoBehaviour
{
    public GameObject expEffect;
    public Texture[] textures;
    public float radius = 10.0f;

    private new MeshRenderer renderer;

    Rigidbody rigid;
    Transform tr;

    int hitCount = 0;


    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        renderer = GetComponentInChildren<MeshRenderer>();

        int index = Random.Range(0, textures.Length);

        renderer.material.mainTexture = textures[index];
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag(AllString.Bullet))
        {
            if (++hitCount == 3)
            {
                ExpBarrel();
            }
        }
    }

    void ExpBarrel()
    {
        GameObject exp = Instantiate(expEffect, tr.position, Quaternion.identity);
        Destroy(exp, 5.0f);
        IndirectDamage(tr.position);
        Destroy(gameObject, 3.0f);
    }

    void IndirectDamage(Vector3 pos)
    {
        Collider[] colls = Physics.OverlapSphere(position: pos, radius: radius, layerMask: 1 << 3);

        foreach (var coll in colls)
        {
            rigid = coll.GetComponent<Rigidbody>();
            rigid.mass = 1.0f;
            rigid.constraints = RigidbodyConstraints.None;

            rigid.AddExplosionForce(explosionForce: 1500.0f, explosionPosition: pos, explosionRadius: radius, upwardsModifier: 1200.0f);
        }
    }
}
