using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelegateDemo : MonoBehaviour
{
    delegate float SumHandler(float a, float b);

    SumHandler sumHandler;

    float Sum(float a, float b)
    {
        return a + b;
    }

    private void Start()
    {
        sumHandler = Sum;
        float sum = sumHandler(10.0f, 5.0f);

        Debug.Log($"Sum = {sum}");
    }
}
