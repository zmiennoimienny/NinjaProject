using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Hidening))]
public class HiddenValueToColor : MonoBehaviour {

    SpriteRenderer spriteRenderer;
    Hidening hidening;

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        hidening = gameObject.GetComponent<Hidening>();
    }

    private void Update()
    {
        float hValue = hidening.GetHiddenLevel();
        spriteRenderer.color = new Color(1f-hValue, 1f-hValue, 1f-hValue);
    }
}
