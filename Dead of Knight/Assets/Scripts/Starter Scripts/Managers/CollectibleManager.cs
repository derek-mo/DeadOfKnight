using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleManager : MonoBehaviour
{
    [SerializeField] private Text collectibles;
    [SerializeField] private int collected;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        collectibles.text = "Donuts " + collected;
    }

    public void Collected(int value)
    {
        collected += value;
    }

}
