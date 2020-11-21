using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New client", menuName = "ScriptableObject/Client")]
public class Client : ScriptableObject
{
    [SerializeField] private Sprite sprite;

    public Sprite Sprite { get => sprite; }
}
