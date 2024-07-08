using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingEffect : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float height = 1f;
    [SerializeField] private AnimationCurve curve;

    private Vector3 startPos;

    private void Awake()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        float time = Time.time * speed;
        float curveValue = curve.Evaluate(Mathf.PingPong(time, 1f));
        float newY = startPos.y + (curveValue - 0.5f) * 2 * height;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
