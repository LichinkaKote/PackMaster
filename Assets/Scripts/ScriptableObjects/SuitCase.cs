using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New suitcase", menuName = "ScriptableObject/Suitcase")]
public class SuitCase : ScriptableObject
{
    [SerializeField] private Vector2Int size;
    [SerializeField] private Vector2Int cellGridSize;
    /*public SuitCase(Vector2Int size)
    {
        var sizeX = Mathf.Clamp(size.x, 1, int.MaxValue);
        var sizeY = Mathf.Clamp(size.y, 1, int.MaxValue);
        this.size = new Vector2Int(sizeX, sizeY);
        cellGridSize = new Vector2Int(2, 2);
    }*/

    public Vector2Int Size { get => size; }
    public Vector2Int CellGridSize { get => cellGridSize;}
}
