using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ResultPanel : MonoBehaviour
{
    [SerializeField] TMP_Text resultTMP;

    public void Show(string message)
    {
        resultTMP.text = message;
        transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutQuad);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() => ScaleZero();

    [ContextMenu("ScaleOne")]
    void ScaleOne() => transform.localScale = Vector3.one;

    [ContextMenu("ScaleZero")]
    public void ScaleZero() => transform.localScale = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        
    }
}
