using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New level", menuName = "ScriptableObject/Level")]
public class Level : ScriptableObject
{
    [SerializeField] private Vector2Int itemSize;

    public Vector2Int ItemSize { get => itemSize; }
}
