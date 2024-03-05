using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourcePanel : MonoBehaviour
{
    [SerializeField] private TMP_Text[] textA;
    private PlayerManager playerManager;
    void Start()
    {
        playerManager = PlayerManager.instance;
    }
    void Update()
    {
        UpdateResourceText();
    }
    private void UpdateResourceText()
    {
        textA[0].text = playerManager.GetResourceAmount("Stone").ToString();
        textA[1].text = playerManager.GetResourceAmount("Iron").ToString();
        textA[2].text = playerManager.GetResourceAmount("Wood").ToString();
        textA[3].text = playerManager.GetResourceAmount("Egg Food").ToString();
    }
}
