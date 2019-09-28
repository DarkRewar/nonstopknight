using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Flags]
public enum ModuleType
{
    Room = 1,
    Hall = 2,
}

[Serializable]
public class ModuleConnector
{
    public Transform EntryPoint;

    public ModuleType CanConnectTo;

    public bool IsAlreadyUsed = false;
}
public class Module : MonoBehaviour
{
    [SerializeField]
    public ModuleType _type;

    public ModuleType ModuleType => _type;

    public List<ModuleConnector> Connectors;

    private void Start()
    {
        //foreach(NavMeshSurface surface in GetComponentsInChildren<NavMeshSurface>())
        //{
        //    surface.BuildNavMesh();
        //}
    }

    private void OnDrawGizmos()
    {
        foreach(ModuleConnector module in Connectors)
        {
            var t = module.EntryPoint;

            Gizmos.color = Color.white;
            Gizmos.DrawSphere(t.position, 0.25f);

            float length = 0.75f;

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(t.position + t.forward * length, t.position);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(t.position + t.right * length, t.position - t.right * length);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(t.position + t.up * length, t.position);
        }
    }
}
