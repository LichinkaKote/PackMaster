using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuUI : MonoBehaviour
{
    [SerializeField] private Button newGameBtn;

    private void Awake()
    {
        newGameBtn.onClick.AddListener(() => GameManager.NewGame());
    }

}
