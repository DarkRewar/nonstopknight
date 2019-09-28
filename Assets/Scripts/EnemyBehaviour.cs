using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBehaviour : MonoBehaviour
{
    public Datas Datas;

    public uint Gold = 3;

    public Canvas CanvasUI;
    public Image Lifebar;

    public bool TargetIsInRange => Datas.AttackRange > Vector3.Distance(_target.transform.position, transform.position);

    private NavMeshAgent _agent;

    private PlayerBehaviour _target;

    private float _attackCooldown = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        _target = GameManager.Instance.Player;
    }

    // Update is called once per frame
    void Update()
    {
        if (Datas.Lifepoints <= 0)
        {
            Death();
            return;
        }

        _agent.SetDestination(GameManager.Instance.Player.transform.position);

        if (_attackCooldown > 0f)
        {
            _attackCooldown -= Time.deltaTime;
        }
        else if (TargetIsInRange)
        {
            BaseAttack();
        }
    }

    private void Death()
    {
        GameManager.Instance.EnemyDie(this);
    }

    private void LateUpdate()
    {
        CanvasUI.transform.LookAt(Camera.main.transform.position);
        Lifebar.fillAmount = (float)Datas.Lifepoints / Datas.MaxLifepoints;
    }

    void BaseAttack()
    {
        if (_target != null && _target.Datas.IsAlive)
        {
            _target.TakeDamage(Datas.Damages);
            //_target.Datas.Lifepoints -= Datas.Damages;
            _attackCooldown = 1f / Datas.SpeedAttack;
        }
    }

    internal void TakeDamage(uint damages)
    {
        if (Datas.Lifepoints <= damages)
        {
            damages = Datas.Lifepoints;
        }
        Datas.Lifepoints -= damages;
    }
}
