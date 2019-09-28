using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public TMP_Text GoldText;

    public TMP_Text FloorText;

    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // C#4
        //GoldText.text = string.Format("Gold : {0}", _gameManager.Gold);

        // C#6
        GoldText.text = $"Gold : {_gameManager.Gold}";
        FloorText.text = $"Etage : {_gameManager.Floor}";
    }
}
