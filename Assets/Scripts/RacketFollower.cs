using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class RacketFollower : MonoBehaviour
{
    [SerializeField] AudioManager audioManager = null;
    [SerializeField] AudioClip hitAudio = null;
    [SerializeField] Transform target = null;
    [SerializeField] float sensitivity = 50f;
    [SerializeField] OVRInput.Controller controllerMask;
    [SerializeField] Transform hitParticleAnchor;
    [SerializeField] ParticleSystem hitParticle;
    [SerializeField] ParticleSystem hitParticleL;

    private Transform tf;
    private Rigidbody rb;
    private System.IDisposable disposable;

    void Awake()
    {
        tf = this.transform;
        rb = this.gameObject.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            return;
        }
        Vector3 velocity = (target.position - tf.position) * sensitivity;
        rb.velocity = velocity;
        tf.rotation = target.rotation;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision == null/* || collision.gameObject.tag == "Racket" */)
        {
            return;
        }

        float impact = collision.relativeVelocity.magnitude;
        impact = Mathf.Min(Mathf.Max(impact, 5f), 20f);
        float rate = (impact - 5f) / 15f;

        // Audio
        audioManager.Play(hitAudio, rate + 0.5f, Vector3.zero);

        // Particle
        if (collision.contactCount > 0)
        {
            hitParticleAnchor.position = collision.contacts[0].point;
        }
        if (rate < 0.5f)
        {
            hitParticle.Play();
        }
        else
        {
            hitParticleL.Play();
        }

        // Vibration
        OVRInput.SetControllerVibration(1f, 1f, controllerMask);
        float counter = 0;
        float step = Mathf.Lerp(0.5f, 0.1f, rate);
        disposable?.Dispose();
        disposable = Observable.Interval(System.TimeSpan.FromSeconds(0.017f)).Subscribe(_ =>
        {
            counter += step;
            if (counter < 3.14159)
            {
                float amplitude = Mathf.Cos(counter) * 0.5f + 0.5f;
                OVRInput.SetControllerVibration(1f, amplitude, controllerMask);
            }
            else
            {
                OVRInput.SetControllerVibration(0f, 0f, controllerMask);
                disposable?.Dispose();
            }
        });
    }
}
