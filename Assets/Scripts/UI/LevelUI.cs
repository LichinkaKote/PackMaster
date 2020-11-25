using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LevelUI : MonoBehaviour
{
    [SerializeField] private Button closeCaseBtn;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private TMP_Text levelTMP, moneyTmp, countryTmp;
    private void Awake()
    {
        closeCaseBtn.onClick.AddListener(() => levelManager.CloseCase());
        levelManager.retryLevel += LevelManager_retryLevel;
        levelManager.updateLevelUI += LevelManager_updateLevelUI;
        levelManager.blockStartBtn += LevelManager_blockStartBtn;
    }

    private void LevelManager_blockStartBtn()
    {
        closeCaseBtn.interactable = false;
    }

    private void LevelManager_updateLevelUI()
    {
        moneyTmp.text = GameManager.money.ToString();
        countryTmp.text = levelManager.Country.name;
    }

    private void LevelManager_retryLevel(bool retry)
    {
        closeCaseBtn.interactable = true;
        closeCaseBtn.onClick.RemoveAllListeners();
        var tmpText = closeCaseBtn.transform.GetComponentInChildren<TMP_Text>();
        if (retry)
        {
            tmpText.text = "Retry";
            closeCaseBtn.onClick.AddListener(() => levelManager.LoadItems());
        }
        else
        {
            tmpText.text = "Start";
            closeCaseBtn.onClick.AddListener(() => levelManager.CloseCase());
        }
        
    }

    private void Start()
    {
        levelTMP.text = $"Level {GameManager.LevelIndex + 1}";

    }

}
