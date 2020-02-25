using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class EggEmitter : MonoBehaviour
{
    [SerializeField] GameObject eggPrefab = null;

    void Start()
    {
        Observable.Interval(System.TimeSpan.FromSeconds(1f)).Subscribe(_ =>
        {
            GameObject obj = Instantiate(eggPrefab);
            obj.transform.localPosition = this.transform.position;
            var rb = obj.GetComponent<Rigidbody>();
            var vec = Vector3.back * 17f + Vector3.up * 30f + Vector3.right * UnityEngine.Random.Range(0f, 3f);
            rb.AddForce(vec);
        });
    }
}
