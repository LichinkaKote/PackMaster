using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "ScriptableObject/Item")]
public class Item : ScriptableObject
{
    [SerializeField] private Vector2Int size;
    [SerializeField] private Sprite sprite;

    public Vector2Int Size { get => size; }
    public Sprite Sprite { get => sprite;}
}
