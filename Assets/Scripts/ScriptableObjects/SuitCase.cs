using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New suitcase", menuName = "ScriptableObject/Suitcase")]
public class SuitCase : ScriptableObject
{
    [SerializeField] private Vector2Int size;
    [SerializeField] private Vector2Int cellGridSize;
    [SerializeField] private Vector2 cellSize;

    public Vector2Int Size { get => size; }
    public Vector2Int CellGridSize { get => cellGridSize;}
    public Vector2 CellSize { get => cellSize; }
}
