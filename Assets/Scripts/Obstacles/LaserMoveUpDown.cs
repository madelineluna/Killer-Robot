// Written by Madeline Luna

using UnityEngine;

public class LaserMove : MonoBehaviour
{
    public float moveDistance = 3f;
    public float moveSpeed = 2f;

    public float startOffset = 0f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float movement = Mathf.Sin((Time.time + startOffset) * moveSpeed) * moveDistance;

        transform.position = startPosition + new Vector3(0, movement, 0);
    }
}