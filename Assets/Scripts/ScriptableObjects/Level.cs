using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New level", menuName = "ScriptableObject/Level")]
public class Level : ScriptableObject
{
    [SerializeField] private Vector2Int itemSize;
    [SerializeField] private List<SuitCase> possibleSuitCases;
    public Vector2Int ItemSize { get => itemSize; }
    public List<SuitCase> PossibleSuitCases { get => possibleSuitCases;}
}
