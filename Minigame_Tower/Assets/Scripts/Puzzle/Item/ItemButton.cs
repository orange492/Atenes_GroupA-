using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;
using UnityEngine.UI;
using static BlockController;

public class ItemButton : MonoBehaviour
{
    bool isClicked = false;
    public bool IsClicked => isClicked;
    Button bombButton;
    Button resetButton;
    Button feverButton;
    MeshRenderer meshRenderer;
    [SerializeField]
    Material[] material;
    int materialIndex = 0;
    Image image;
    TextMeshProUGUI bombRemainText;
    TextMeshProUGUI resetRemainText;
    TextMeshProUGUI feverRemainText;
    int bombRemain = 0;
    int resetRemain = 0;
    int feverRemain = 0;
    Fever fever;

    public int BombRemain
    {
        get => bombRemain;
        set => bombRemain = value;
    }


    BlockController blockController;

    private void Awake()
    {
        bombButton = transform.GetChild(0).GetComponent<Button>();
        bombRemainText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        resetButton = transform.GetChild(2).GetComponent<Button>();
        resetRemainText = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        feverButton = transform.GetChild(4).GetComponent<Button>();
        feverRemainText = transform.GetChild(5).GetComponent<TextMeshProUGUI>();

        bombButton.onClick.AddListener(OnClick_Bomb);
        resetButton.onClick.AddListener(OnClick_Reset);
        feverButton.onClick.AddListener(OnClick_Fever);



        image = transform.GetChild(0).GetComponent<Image>();
        image.material = material[materialIndex];
    }


    void Start()
    {
        blockController = FindObjectOfType<BlockController>();
        fever = FindObjectOfType<Fever>();
        if (TowerManager.Inst.GetDifficulty() == 0)
        {
            bombRemain = 3;
            resetRemain = 3;
            feverRemain = 3;
        }
        if (TowerManager.Inst.GetDifficulty() == 1)
        {
            bombRemain = 2;
            resetRemain = 2;
            feverRemain = 2;
        }
        if (TowerManager.Inst.GetDifficulty() == 2)
        {
            bombRemain = 1;
            resetRemain = 1;
            feverRemain = 1;
        }
        bombRemainText.text = bombRemain.ToString();
        resetRemainText.text = resetRemain.ToString();
        feverRemainText.text = feverRemain.ToString();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnClick_Bomb()
    {
        if (bombRemain > 0 && blockController.Mode == BlockController.GameMode.Normal)
        {
            isClicked = !isClicked;
            materialIndex++;
            materialIndex %= 2;
            image.material = material[materialIndex];
        }
    }

    private void OnClick_Reset()
    {

        if (resetRemain > 0 && blockController.Mode == BlockController.GameMode.Normal)
        {
            blockController.ResetAllBlock();
            blockController.Mode = GameMode.Checking;
            Debug.Log("리셋체킹");
            blockController.CreateAllBlock();
            if (!blockController.AllBlockCheck())
            {
                blockController.Mode = GameMode.Normal;
                Debug.Log("리셋후 노멀");
            }
            else
            {
                //blockController.AllBlockAction();
            }
            resetRemain--;
            resetRemainText.text = resetRemain.ToString();

        }
    }
    private void OnClick_Fever()
    {
        if (!fever.IsFevering)
        {
            blockController.onFeverChange?.Invoke(1000);
            feverRemain--;
            feverRemainText.text = feverRemain.ToString();
        }
    }

    public void BombRemainToText()
    {
        bombRemainText.text = bombRemain.ToString();
        if (bombRemain <= 0)
        {
            image.material = material[0];
        }
    }

}
