using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class NoiseEmitter : MonoBehaviour {

    public float noiseTime = 0.1f;
    CircleCollider2D circleCollider2D;

    private void Awake()
    {
        circleCollider2D = gameObject.GetComponent<CircleCollider2D>();
        circleCollider2D.isTrigger = true;

    }

    public void EmitNoise(float value)
    {
        circleCollider2D.radius = value;
        circleCollider2D.enabled = true;
        this.Invoke("DistableNoise", noiseTime);
    }

    void DistableNoise()
    {
        circleCollider2D.radius = 1f;
        circleCollider2D.enabled = false;
    }
}
