using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StylingData : MonoBehaviour
{
    public static StylingData Instance;

    [SerializeField] public Color colourA;
    [SerializeField] public Color colourB;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
}
