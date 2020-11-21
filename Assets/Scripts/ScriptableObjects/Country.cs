using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New country", menuName ="ScriptableObject/Country")]
public class Country : ScriptableObject
{
    [SerializeField] private List<Item> posibleItems;
    [SerializeField] private List<Client> posibleClients;

    public List<Item> PosibleItems { get => posibleItems; }
}
