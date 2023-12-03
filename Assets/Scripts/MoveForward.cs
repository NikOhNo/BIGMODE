using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveRight : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * speed);
    }
}
