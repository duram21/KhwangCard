using UnityEngine;
using TMPro;
using System;
using DG.Tweening;
public class Entity : MonoBehaviour
{
    [SerializeField] Item item;
    [SerializeField] SpriteRenderer entity;
    [SerializeField] SpriteRenderer character;
    [SerializeField] TMP_Text nameTMP;
    [SerializeField] TMP_Text attackTMP;
    [SerializeField] TMP_Text healthTMP;
    [SerializeField] GameObject sleepParticle;

    public int attack;
    public int health;
    public bool isMine;
    public bool isBossOrEmpty;
    public bool attackable;
    public Vector3 originPos;
    int liveCount;

    public void Setup(Item item)
    {
        attack = item.attack;
        health = item.health;

        this.item = item;
        character.sprite = this.item.sprite;
        nameTMP.text = this.item.name;
        attackTMP.text = attack.ToString();
        healthTMP.text = health.ToString();
    }

    void OnMouseDown()
    {
        if(isMine)
            EntityManager.Inst.EntityMouseDown(this);
    }

    void OnMouseUp()
    {
        if (isMine)
            EntityManager.Inst.EntityMouseUp();
    }

    void OnMouseDrag()
    {
        if (isMine)
            EntityManager.Inst.EntityMouseDrag();
    }

    public void MoveTransform(Vector3 pos, bool useDotween, float dotweenTime = 0)
    {
        if(useDotween)
            transform.DOMove(pos, dotweenTime);
        else   
            transform.position = pos;
    }
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TurnManager.OnTurnStarted += OnTurnStarted;
    }

    void Oestroy()
    {
        TurnManager.OnTurnStarted -= OnTurnStarted;        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTurnStarted(bool myTurn)
    {
        if(isBossOrEmpty)
            return;
        if(isMine == myTurn)
            liveCount++;

        sleepParticle.SetActive(liveCount < 1);
    }
}
