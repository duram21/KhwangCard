using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
  public static CardManager Inst { get; private set;}
  void Awake() => Inst = this;

  [SerializeField] ItemSO itemSO;

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
          print(PopItem().name);
    }
}
