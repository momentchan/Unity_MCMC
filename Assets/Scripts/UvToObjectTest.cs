using mj.gist;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UvToObjectTest : MonoBehaviour {
    [SerializeField] UvToObjectWrapper wraper;
    [SerializeField] private GameObject prefab;
    [SerializeField, Range(0, 1)] private float u;
    [SerializeField, Range(0, 1)] private float v;

    private GameObject go;
    void Start() {
        wraper = GetComponent<UvToObjectWrapper>();
        go = Instantiate(prefab);
    }

    void Update() {
        Vector3 lpos, normal;
        if (wraper.GetObjectPos(new Vector2(u, v), out lpos, out normal)) {
            go.transform.position = lpos;
        }

    }
}
