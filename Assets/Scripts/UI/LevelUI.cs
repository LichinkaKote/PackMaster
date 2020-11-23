using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private Button closeCaseBtn;
    [SerializeField] private LevelManager levelManager;

    private void Awake()
    {
        //closeCaseBtn.onClick.AddListener(() => levelManager.CloseCase());
    }
}
