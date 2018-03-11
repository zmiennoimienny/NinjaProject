using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hidening : MonoBehaviour {

    [Range(0.0f, 1.0f)]
    public float hiddenLevel; // from 0.0f to 1.0f
    [SerializeField] private float defaultHiddenLevel = 0.0f;

    public void SetHiddenLevel(float value)
    {
        hiddenLevel = value;
    }
    public float GetHiddenLevel()
    {
        return hiddenLevel;
    }

    public void SetDefaultHiddenLevel()
    {
        hiddenLevel = defaultHiddenLevel;
    }
}
