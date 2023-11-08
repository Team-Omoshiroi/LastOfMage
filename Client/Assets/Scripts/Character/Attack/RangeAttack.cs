using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RangeAttack : MonoBehaviour
{
    [SerializeField] private float speed = 10f; // 공격의 속도
    [SerializeField] private float lifeTime = 5f; // 공격이 존재할 수 있는 시간

    public void Launch()
    {
        // 공격생명주기
        Invoke("Deactivate", lifeTime);
    }

    private void Update()
    {
        // 매 프레임마다 공격을 전방으로 이동시킴
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void Deactivate()
    {
        // 오브젝트를 비활성화하고 오브젝트 풀로 반환
        AttackManager.Instance.ReturnAttackToPool(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 다른 콜라이더와 충돌 시 호출됨
        // 이곳에 충돌한 대상에 대한 처리 로직을 구현

        // 충돌 후 비활성화를 위해 Deactivate 함수 호출
        Deactivate();
    }
}
