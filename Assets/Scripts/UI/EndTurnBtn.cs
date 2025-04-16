using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class EndTurnBtn : MonoBehaviour
{
    [SerializeField] Sprite active;
    [SerializeField] Sprite inactive;
    [SerializeField] TMP_Text btnText;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Setup(false);
        TurnManager.OnTurnStarted += Setup;
    }

    void OnDestroy()
    {
        TurnManager.OnTurnStarted -= Setup;
    }

    public void Setup(bool isActive)
    {
        GetComponent<Image>().sprite = isActive ? active : inactive;
        GetComponent<Button>().interactable = isActive;
        btnText.color = isActive ? new Color32(255, 195, 90, 255) : new Color32(55, 55, 55, 255);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
