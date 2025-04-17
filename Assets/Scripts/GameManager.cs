using UnityEngine;
using System.Collections;

// 치트, UI, 랭킹, 게임오버
public class GameManager : MonoBehaviour
{
    public static GameManager Inst { get; private set; }
    void Awake() => Inst = this;

    [SerializeField] NotificationPanel notificationPanel;
    [SerializeField] ResultPanel resultPanel;
    [SerializeField] TitlePanel titlePanel;
    [SerializeField] GameObject endTurnBtn;
    WaitForSeconds delay2 = new WaitForSeconds(2);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UISetup();
    }

    void UISetup()
    {
        notificationPanel.ScaleZero();
        resultPanel.ScaleZero();
        titlePanel.Active(true);
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        InputCheatKey();
#endif
    }

    void InputCheatKey()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
            TurnManager.OnAddCard?.Invoke(true);

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            TurnManager.OnAddCard?.Invoke(false);
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            TurnManager.Inst.EndTurn();
        }
        if(Input.GetKeyDown(KeyCode.Keypad4))
            CardManager.Inst.TryPutCard(false);

        if(Input.GetKeyDown(KeyCode.Keypad5))
            EntityManager.Inst.DamageBoss(true, 19);
        
        if(Input.GetKeyDown(KeyCode.Keypad6))
            EntityManager.Inst.DamageBoss(false, 19);
    }

    public void StartGame()
    {
        StartCoroutine(TurnManager.Inst.StartGameCo());
    }

    public void Notification(string message)
    {
        notificationPanel.Show(message);
    }

    public IEnumerator GameOver(bool isMyWin)
    {
        TurnManager.Inst.isLoading = true;
        endTurnBtn.SetActive(false);
        yield return delay2;

        TurnManager.Inst.isLoading =true;

        resultPanel.Show(isMyWin ? "승리" : "패배");
    }
}
