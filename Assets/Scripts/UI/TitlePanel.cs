using UnityEngine;

public class TitlePanel : MonoBehaviour
{
    public void StartGameClick()
    {
        GameManager.Inst.StartGame();
        Active(false);
    }

    public void Active(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
