using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class Rotater : MonoBehaviour
{
    [SerializeField] OVRInput.RawAxis2D rawAxis2D;

    void Start()
    {
        var t = this.transform;
        this.UpdateAsObservable()
            .Where(_ => Mathf.Abs(OVRInput.Get(rawAxis2D).x) > 0.1f)
            .Subscribe(_ =>
            {
                t.Rotate(Vector3.up * OVRInput.Get(rawAxis2D).x * 5f);
            });
    }
}
