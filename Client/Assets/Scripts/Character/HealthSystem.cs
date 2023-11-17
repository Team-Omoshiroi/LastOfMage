using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event Action OnDead;
    private CharacterStats Stats;
    private PilotSync Sync;

    private void Awake()
    {
        // stats = dataContainer.Stats;
        //Sync = GetComponent<PilotSync>();
    }

    private void Start()
    {
        Stats = gameObject.GetComponent<DataContainer>().Stats;
        Sync = gameObject.GetComponent<PilotSync>();
    }

    public void TakeDamage(int changeAmount)
    {
        int remain = Stats.Hp - changeAmount;

        if (remain <= 0)
        {
            // 플레이어 사망 또는 무언가를 처리하는 부분
            CallOnDead();
        }
        else if (Stats.MaxHp < remain)
        {
            // 최대 체력을 초과한 치유 시 처리 부분
            // 채이환 => 이는 Stats.Hp의 Setter에서 처리하는 것으로 보임.
        }
        else { }

        Stats.Hp = remain;
        //UIController.Instance.HandlerHp(Stats.MaxHp, Stats.Hp);
        Sync?.SendC_ChangeHpPacket(Stats.Hp);
    }

    public void TakeRecovery(int changeAmount)
    {
        int remain = Stats.Hp + changeAmount;

        if (remain <= 0)
        {
            //플레이어 사망 또는 무언가를 처리하는 부분
        }
        else if (Stats.MaxHp < remain)
        {
            //최대 체력을 초과한 치유 시 처리 부분
        }
        else { }

        //UIController.Instance.HandlerHp(Stats.MaxHp, Stats.Hp);
        Stats.Hp = remain;
        Sync?.SendC_ChangeHpPacket(Stats.Hp);
    }

    private void CallOnDead()
    {
        OnDead?.Invoke();
        gameObject.SetActive(false);
    }
}
