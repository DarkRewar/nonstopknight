using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region SINGLETON

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    public uint Floor = 1;

    public uint Gold = 0;

    public BoxCollider SpawnZone;

    public PlayerBehaviour Player;

    public List<GameObject> EnemiesPrefab = new List<GameObject>();
    public List<EnemyBehaviour> EnemiesInGame = new List<EnemyBehaviour>();

    public List<ItemObject> ItemObjects;

    private int _rooms = 0;
    private float _waitEnemies = 2f;

    public static event Action<ItemObject> OnItemAdded;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 2f;    
        Player = FindObjectOfType<PlayerBehaviour>();

        EnemiesInGame = new List<EnemyBehaviour>(FindObjectsOfType<EnemyBehaviour>());
    }

    // Update is called once per frame
    void Update()
    {
        if(EnemiesInGame.Count == 0)
        {
            if (_waitEnemies <= 0f)
            {
                SpawnEnemies();
                _waitEnemies = 2f;
            }
            else
            {
                _waitEnemies -= Time.deltaTime;
            }
        }
    }

    internal void EnemyDie(EnemyBehaviour enemyBehaviour)
    {
        EnemiesInGame.Remove(enemyBehaviour);
        Gold += enemyBehaviour.Gold;

        DropItem();

        Destroy(enemyBehaviour.gameObject);
    }

    private void DropItem()
    {
        int rand = UnityEngine.Random.Range(0, 5);
        if(rand == 0)
        {
            ItemObject obj = Instantiate(ItemObjects.GetRandom()) as ItemObject;
            Player.AddItem(obj);
            OnItemAdded?.Invoke(obj);
        }
    }

    private void SpawnEnemies()
    {
        if (DungeonGenerator.Instance.CurrentRooms.Count < _rooms) return;

        for(int i = 0; i < (5 + Mathf.Floor((float)Floor/30)); ++i)
        {
            GameObject go = EnemiesPrefab.GetRandom();
            Transform module = DungeonGenerator.Instance.CurrentRooms[_rooms].transform;
            //Vector3 randomPoint = new Vector3(
            //    UnityEngine.Random.Range(SpawnZone.bounds.min.x, SpawnZone.bounds.max.x),
            //    0.5f,
            //    UnityEngine.Random.Range(SpawnZone.bounds.min.z, SpawnZone.bounds.max.z)
            //);
            Vector3 randomPoint = new Vector3(
                UnityEngine.Random.Range(module.position.x - 2f, module.position.x + 2f),
                0.5f,
                UnityEngine.Random.Range(module.position.z - 2f, module.position.z + 2f)
            );
            var enemy = Instantiate(go, randomPoint, Quaternion.identity);
            EnemyBehaviour behaviour = enemy.GetComponent<EnemyBehaviour>();

            behaviour.Datas.Lifepoints = Formula(behaviour.Datas.Lifepoints);
            behaviour.Datas.MaxLifepoints = behaviour.Datas.Lifepoints;
            behaviour.Datas.Damages = Formula(behaviour.Datas.Damages);
            behaviour.Gold = Formula(behaviour.Gold);

            EnemiesInGame.Add(behaviour);
        }

        ++_rooms;

        if (_rooms >= DungeonGenerator.Instance.CurrentRooms.Count)
        {
            _rooms = 0;
            ++Floor;
            DungeonGenerator.Instance.GenerateMap();
        }
    }

    internal EnemyBehaviour GetNearestEnemy()
    {
        EnemiesInGame.Sort((a, b) => {
            return Vector3.Distance(Player.transform.position, a.transform.position) >
                Vector3.Distance(Player.transform.position, b.transform.position) ? 1 : -1;
        });

        return EnemiesInGame[0];
    }

    internal uint Formula(uint value)
    {
        //=ARRONDI($C$2 + (EXP($C$2 * LOG(A3) *  0,4)); 0)
        float v = Mathf.Exp(value * Mathf.Log10(Floor) * 0.4f);
        return value + (uint)Mathf.RoundToInt(v);
    }

    internal static void EquipItem(ItemObject itemRef)
    {
        Instance.Player.Equip(itemRef);
    }
}
