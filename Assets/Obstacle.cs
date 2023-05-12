using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private float duration = 1;
    [SerializeField] private List<Transform> positions;

    private int index;
    // Update is called once per frame
    void Start()
    {
        Move();
    }

    private void Move()
    {
        var pos = positions[index];
        this.transform.DOMove(pos.position, duration).onComplete = Move;
        index += 1;
        if (index == positions.Count)
        {
            index = 0;
        }
    }
}
