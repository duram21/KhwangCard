using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardManager : MonoBehaviour
{
  public static CardManager Inst { get; private set;}
  void Awake() => Inst = this;

  [SerializeField] ItemSO itemSO;
  [SerializeField] GameObject cardPrefab;
  [SerializeField] List<Card> myCards;
  [SerializeField] List<Card> otherCards;
  [SerializeField] Transform cardSpawnPoint;

  List<Item> itemBuffer;

  public Item PopItem()
  {
    if(itemBuffer.Count == 0)
      SetupItemBuffer();

    Item item = itemBuffer[0];
    itemBuffer.RemoveAt(0);
    return item;
  }
  
  void SetupItemBuffer() {
    itemBuffer = new List<Item>();
    for(int i = 0 ; i < itemSO.items.Length; i++) 
    {
      Item item = itemSO.items[i];
      for(int j = 0 ; j < item.percent; j++)
      {
        itemBuffer.Add(item);
      }
    }
    for(int i = 0 ; i < itemBuffer.Count; i++)
    {
      int rand = Random.Range(i, itemBuffer.Count);
      Item temp = itemBuffer[i];
      itemBuffer[i] = itemBuffer[rand];
      itemBuffer[rand] = temp;
    }
  }

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
    {
        SetupItemBuffer();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Keypad1))
          AddCard(true);
        
        if(Input.GetKeyDown(KeyCode.Keypad2)){
          AddCard(false);
        }
    }

    void AddCard(bool isMine)
    {
      var cardObject = Instantiate(cardPrefab, cardSpawnPoint.position, Utils.QI);
      var card = cardObject.GetComponent<Card>();
      card.Setup(PopItem(), isMine);
      (isMine ? myCards : otherCards).Add(card);

      SetOriginOrder(isMine);
      CardAlignment(isMine);
    }

    void SetOriginOrder(bool isMine){
      int count = isMine ? myCards.Count : otherCards.Count;
      for(int i = 0 ; i < count; i++)
      {
        var targetCard = isMine ? myCards[i] : otherCards[i];
        targetCard?.GetComponent<Order>().SetOriginOrder(i);
      }
    }

    void CardAlignment(bool isMine)
    {
      var targetCards = isMine ? myCards : otherCards;
      for(int i = 0 ; i < targetCards.Count; i++)
      {
        var targetCard = targetCards[i];

        targetCard.originPRS = new PRS(Vector3.zero, Utils.QI, Vector3.one * 1.9f);
        targetCard.MoveTransform(targetCard.originPRS, true, 0.7f);
      }
    }
}
