using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hideout : MonoBehaviour {

    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float hiddenLevel; // from 0.0f to 1.0f

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Hidening>())
        {
            collision.gameObject.GetComponent<Hidening>().SetHiddenLevel(hiddenLevel);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Hidening>())
        {
            collision.gameObject.GetComponent<Hidening>().SetDefaultHiddenLevel();
        }
    }
}
