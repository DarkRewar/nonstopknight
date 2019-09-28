using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class Datas : ICloneable
{
    public uint Lifepoints = 20;

    public uint MaxLifepoints = 20;

    public uint Damages = 1;

    public float SpeedAttack = 1;

    public float MoveSpeed = 1f;

    public float AttackRange = 1f;

    public float LifeRegenCooldown = 0.5f;

    public uint LifeRegen = 2;

    public float CritRate = 0.1f;

    public uint CritDamages = 3;

    public bool IsAlive => Lifepoints > 0;

    public bool IsDead => Lifepoints <= 0;

    public object Clone()
    {
        return new Datas
        {
            Lifepoints = Lifepoints,
            MaxLifepoints = MaxLifepoints,
            Damages = Damages,
            SpeedAttack = SpeedAttack,
            MoveSpeed = MoveSpeed,
            AttackRange = AttackRange,
            LifeRegenCooldown = LifeRegenCooldown,
            LifeRegen = LifeRegen,
            CritRate = CritRate,
            CritDamages = CritDamages
        };
    }

    public static Datas operator +(Datas a, Datas b)
    {
        return new Datas
        {
            Lifepoints = a.Lifepoints + b.Lifepoints,
            MaxLifepoints = a.MaxLifepoints + b.MaxLifepoints,
            Damages = a.Damages + b.Damages,
            SpeedAttack = a.SpeedAttack + b.SpeedAttack,
            MoveSpeed = a.MoveSpeed + b.MoveSpeed,
            AttackRange = a.AttackRange + b.AttackRange,
            LifeRegenCooldown = a.LifeRegenCooldown + b.LifeRegenCooldown,
            LifeRegen = a.LifeRegen + b.LifeRegen,
            CritRate = a.CritRate + b.CritRate,
            CritDamages = a.CritDamages + b.CritDamages,
        };
    }

    public static Datas operator *(Datas a, Datas b)
    {
        return new Datas
        {
            Lifepoints = a.Lifepoints * b.Lifepoints,
            MaxLifepoints = a.MaxLifepoints * b.MaxLifepoints,
            Damages = a.Damages * b.Damages,
            SpeedAttack = a.SpeedAttack * b.SpeedAttack,
            MoveSpeed = a.MoveSpeed * b.MoveSpeed,
            AttackRange = a.AttackRange * b.AttackRange,
            LifeRegenCooldown = a.LifeRegenCooldown * b.LifeRegenCooldown,
            LifeRegen = a.LifeRegen * b.LifeRegen,
            CritRate = a.CritRate * b.CritRate,
            CritDamages = a.CritDamages * b.CritDamages,
        };
    }
}