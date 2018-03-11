using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour {

    NoiseEmitter noiseEmitter;
    public float bulletNoiseValue = 6f;

    private void Awake()
    {
        noiseEmitter = gameObject.GetComponentInChildren<NoiseEmitter>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        noiseEmitter.EmitNoise(bulletNoiseValue);
        Invoke("DestroyRock", noiseEmitter.noiseTime + 0.1f);
    }

    private void DestroyRock()
    {
        Destroy(gameObject);
    }
}
