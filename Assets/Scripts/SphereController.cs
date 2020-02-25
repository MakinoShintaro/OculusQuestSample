using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class SphereController : MonoBehaviour
{
    [SerializeField] AudioManager audioManager = null;
    [SerializeField] AudioClip hitAudio = null;

    Transform tf;
    Rigidbody rb;

    void Start()
    {
        tf = this.transform;
        rb = this.gameObject.GetComponent<Rigidbody>();

        this.UpdateAsObservable()
            .Where(_ => OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
            .Subscribe(_ =>
            {
                rb.velocity = Vector3.zero;
                var vec = -tf.position.normalized;
                vec.y = 0;
                rb.AddForce(vec * 20f + Vector3.up * 20f);
            })
            .AddTo(this);
    }

    void Update()
    {
        Vector3 vec = Vector3.zero;
        vec.x = rb.velocity.x;
        vec.z = rb.velocity.z;
        float magnitude = vec.magnitude;
        if (magnitude < 2f)
        {
            vec *= Mathf.Max(2f / (magnitude + 0.001f), 1f); // 0.001 はゼロ割を防ぐため
            rb.velocity = Vector3.right * vec.x + Vector3.up * rb.velocity.y + Vector3.forward * vec.z;
            Debug.Log(vec);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision == null || collision.gameObject.tag == "Racket")
        {
            return;
        }

        // Audio
        audioManager.Play(hitAudio, 1f, tf.position);
    }
}
