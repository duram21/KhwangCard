using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Animations;

public class EntityManager : MonoBehaviour
{
    public static EntityManager Inst {get; private set;}
    void Awake() => Inst = this;

    [SerializeField] GameObject entityPrefab;
    [SerializeField] List<Entity> myEntities;
    [SerializeField] List<Entity> otherEntities;
    [SerializeField] GameObject TargetPicker;
    [SerializeField] Entity myEmptyEntity;
    [SerializeField] Entity myBossEntity;

    [SerializeField] Entity otherBossEntity;

    const int MAX_ENTITY_COUNT = 6;
    public bool IsFullMyEntities => myEntities.Count >= MAX_ENTITY_COUNT && !ExistMyEmptyEntity;
    bool IsFullOtherEntities => otherEntities.Count >= MAX_ENTITY_COUNT;
    bool ExistTargetPickEntity => targetPickEntity != null;
    bool ExistMyEmptyEntity => myEntities.Exists(x => x == myEmptyEntity);
    int MyEmptyEntityIndex => myEntities.FindIndex(x => x == myEmptyEntity);
    bool CanMouseInput => TurnManager.Inst.myTurn && !TurnManager.Inst.isLoading;

    Entity selectEntity;
    Entity targetPickEntity;
    WaitForSeconds delay1 = new WaitForSeconds(1);


    void EntityAlignment(bool isMine)
    {
        float targetY = isMine ? -4.35f : 4.15f;
        var targetEntities = isMine ? myEntities : otherEntities;

        for(int i = 0; i < targetEntities.Count; i++)
        {
            float targetX = (targetEntities.Count - 1) * -3.4f + i * 6.8f;
            var targetEntity = targetEntities[i];
            targetEntity.originPos = new Vector3(targetX, targetY, 0);
            targetEntity.MoveTransform(targetEntity.originPos, true, 0.5f);
            targetEntity.GetComponent<Order>()?.SetOriginOrder(i);
        }
    }

    public void InsertMyEmptyEntity(float xPos)
    {
        if (IsFullMyEntities)
            return;
        if(!ExistMyEmptyEntity)
            myEntities.Add(myEmptyEntity);

        Vector3 emptyEntityPos = myEmptyEntity.transform.position;
        emptyEntityPos.x = xPos;
        myEmptyEntity.transform.position = emptyEntityPos;

        int _emptyEntityIndex = MyEmptyEntityIndex;
        myEntities.Sort((entity1, entity2) => entity1.transform.position.x.CompareTo(entity2.transform.position.x));
        if(MyEmptyEntityIndex != _emptyEntityIndex)
            EntityAlignment(true);
    }

    public void RemoveMyEmptyEntity()
    {
        if(!ExistMyEmptyEntity)
            return;
        myEntities.RemoveAt(MyEmptyEntityIndex);
        EntityAlignment(true);
    }

    public bool SpawnEntity(bool isMine, Item item, Vector3 spawnPos)
    {
        if(isMine)
        {
            if(IsFullMyEntities || !ExistMyEmptyEntity)
                return false;
        }
        else
        {
            if(IsFullOtherEntities)
                return false;
        }

        var entityObject = Instantiate(entityPrefab, spawnPos, Utils.QI);
        var entity = entityObject.GetComponent<Entity>();

        if(isMine)
            myEntities[MyEmptyEntityIndex] = entity;
        else
            otherEntities.Insert(Random.Range(0, otherEntities.Count), entity);

        entity.isMine = isMine;
        entity.Setup(item);
        EntityAlignment(isMine);

        return true;
    }

    public void EntityMouseDown(Entity entity)
    {
        if(!CanMouseInput)
            return;

        selectEntity = entity;
    }

    public void EntityMouseUp()
    {
        if(!CanMouseInput)
            return;
        
        selectEntity = null;
        targetPickEntity = null;
    }

    public void EntityMouseDrag()
    {
        if(!CanMouseInput || selectEntity == null)
            return;

        // other 타겟엔티티 찾기
        bool existTarget = false;
        foreach ( var hit in Physics2D.RaycastAll(Utils.MousePos, Vector3.forward))
        {
            Entity entity = hit.collider?.GetComponent<Entity>();
            if(entity != null && !entity.isMine && selectEntity.attackable)
            {
                targetPickEntity = entity;
                existTarget= true;
                break;
            }
        }
        if(!existTarget)
            targetPickEntity = null;
    }

    public void AttackableReset(bool isMine)
    {
        var targetEntities = isMine  ? myEntities : otherEntities;
        targetEntities.ForEach(x => x.attackable = true);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TurnManager.OnTurnStarted += OnTurnStarted;
    }
    void OnDestroy()
    {
        TurnManager.OnTurnStarted -= OnTurnStarted;
    }

    // Update is called once per frame
    void Update()
    {
        ShowTargetPicker(ExistTargetPickEntity);
    }

    void ShowTargetPicker(bool isShow)
    {
        TargetPicker.SetActive(isShow);
        if(ExistTargetPickEntity)
            TargetPicker.transform.position = targetPickEntity.transform.position;
    }

    void OnTurnStarted(bool myTurn)
    {
        AttackableReset(myTurn);

        if(!myTurn)
            StartCoroutine(AICo());
    }

    IEnumerator AICo()
    {
        CardManager.Inst.TryPutCard(false);
        yield return delay1;

        // 공격 로직
        TurnManager.Inst.EndTurn();
    }

}
