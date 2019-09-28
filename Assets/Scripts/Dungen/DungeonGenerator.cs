using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class DungeonGenerator : MonoBehaviour
{
    public static DungeonGenerator Instance;

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField]
    private int _seed = 0;

    [Range(1, 15)]
    public int Iterations;

    public List<Module> Rooms;
    public List<Module> Halls;

    private System.Random _random;

    public List<Module> CurrentRooms = new List<Module>();

    private NavMeshSurface _surface;

    // Start is called before the first frame update
    void Start()
    {
        if(_seed == 0)
            _seed = UnityEngine.Random.Range(1, 99999999);

        _random = new System.Random(_seed);
        _surface = GetComponent<NavMeshSurface>();

        GenerateMap();
    }

    public void GenerateMap()
    {
        //_surface.UpdateNavMesh(null);

        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }

        CurrentRooms.Clear();

        Module baseRoom = Instantiate(Rooms.GetRandom(), transform);
        baseRoom.transform.localPosition = Vector3.zero;
        CurrentRooms.Add(baseRoom);

        List<Module> currentModules = new List<Module>();
        List<Module> nextModules = new List<Module>();
        currentModules.Add(baseRoom);

        for(int i = 0; i < Iterations; ++i)
        {
            foreach(Module module in currentModules)
            {
                foreach (ModuleConnector connector in module.Connectors)
                {
                    if (connector.IsAlreadyUsed) continue;

                    Module tempModule;

                    connector.IsAlreadyUsed = true;

                    if (connector.CanConnectTo == ModuleType.Room)
                    {
                        tempModule = Rooms.GetRandom();
                    }
                    else
                    {
                        tempModule = Halls.GetRandom();
                    }

                    tempModule = Instantiate(tempModule, transform);

                    ModuleConnector tempConnector = tempModule.Connectors.Where(x => !x.IsAlreadyUsed).First();
                    if(tempConnector != null)
                    {
                        tempConnector.IsAlreadyUsed = true;

                        //tempModule.transform.rotation = connector.EntryPoint.rotation;
                        tempModule.transform.position = connector.EntryPoint.position;
                        tempModule.transform.position += connector.EntryPoint.position - tempConnector.EntryPoint.position;
                        float angle = Vector3.SignedAngle(
                            connector.EntryPoint.forward, 
                            tempConnector.EntryPoint.forward, 
                            Vector3.up
                        );
                        tempModule.transform.RotateAround(tempConnector.EntryPoint.position, Vector3.up, 180 - angle);

                    }
                    nextModules.Add(tempModule);
                }
            }

            currentModules = new List<Module>(nextModules);
            var query = currentModules.Where(x => x.ModuleType == ModuleType.Room).ToList();
            CurrentRooms.AddRange(query);
            nextModules.Clear();
        }

        StartCoroutine(WaitToRebuild());
    }

    private IEnumerator WaitToRebuild()
    {
        yield return new WaitForEndOfFrame();
        _surface.BuildNavMesh();
    }
}
