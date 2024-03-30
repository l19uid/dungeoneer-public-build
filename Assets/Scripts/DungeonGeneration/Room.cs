using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Dungeoneer
{
    public class Room : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] _entrances;
        [SerializeField]
        private bool[] _directions;
        [SerializeField]
        [Range(0,100)]
        private float _obstructionChance = 50f;
        [SerializeField]
        private GameObject[] _obstructions;
        [SerializeField]
        private GameObject[] _walls;
        [SerializeField]
        private GameObject[] _entranceWalls;
        
        private GameObject _room;
        public bool isStarted = false;
        private bool isFinished = false;
        
        [SerializeField]
        private float pathLength;
        [SerializeField]
        private Vector2 generationSize;
        [SerializeField]
        private Vector2 roomSize;
        [SerializeField]
        private Vector2 roomCenter;
        [SerializeField]
        private Vector2 waveCount;
        [SerializeField] 
        private Vector2 waveSize;
        public Enums.RoomType roomType = Enums.RoomType.Regular;
        [SerializeField] 
        private float _timeToClear;
        private int _liveEnemyCount;

        private int _curWave = 0;
        private int _maxWaves = 0;
        public Vector2[] chestPositions;

        public LayerMask wallLayer;

        private EnemyPool _enemyPool;

        public GameObject[] minimap;
        public GameObject bossPortal;
        
        private DungeonGenerator _dungeonGenerator;
        private UIManager _uiManager;
        private GradingSystem _gradingSystem;

        public void CreateRoom(bool[] directions = null, Enums.RoomType type = Enums.RoomType.Regular)
        {
            roomType = type;
            _enemyPool = GameObject.Find("EnemyPool").GetComponent<EnemyPool>();
            _maxWaves = Random.Range((int)waveCount.x, (int)waveCount.y);
            _directions = directions;
            _uiManager = GameObject.FindWithTag("Canvas").GetComponent<UIManager>();
            _gradingSystem = GameObject.Find("Generator").GetComponent<GradingSystem>();
            _dungeonGenerator = GameObject.Find("Generator").GetComponent<DungeonGenerator>();
        }
        
        public void SpawnRoom()
        {
            SpawnEntranceWalls();
            SpawnObstructions();
        }

        private void Update()
        {
            _liveEnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
            
            if(isStarted && !isFinished && _liveEnemyCount == 0 && roomType == Enums.RoomType.Regular)
            {
                if(_curWave <= _maxWaves)
                    SpawnWave();
                else
                    FinishRoom();
            }
        }

        private void FinishRoom()
        {
            isFinished = true;
            _uiManager.UpdateRoomText("Finished");
            _gradingSystem.FinishClearingRoom();
            for (int i = 0; i < _entrances.Length; i++)
            {
                _entrances[i].SetActive(false);
            }
        }
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if(col.name == "Player" && !isStarted)
            {
                DiscoverMiniMap();
                isStarted = true;
                _curWave = 1;

                if (roomType == Enums.RoomType.Regular)
                {
                    LockDoors();
                    _uiManager.UpdateRoomText("Wave: " +(_curWave) + "/" + _maxWaves);
                    _gradingSystem.StartClearingRoom(_timeToClear);
                }
                else if (roomType == Enums.RoomType.Chest)
                {
                    Debug.Log("starting chests");
                    _uiManager.UpdateRoomText("Chest room.");
                    GenerateRandomChests();
                }
                else if (roomType == Enums.RoomType.Start)
                {
                    _uiManager.UpdateRoomText("Start");
                }
                else if (roomType == Enums.RoomType.End)
                {
                    _dungeonGenerator.GenerateBossRoom();
                    Instantiate(bossPortal, transform.position, Quaternion.identity);
                }
            }
        }
        
        private void GenerateRandomChests()
        {
            int amount = 1;
            switch(GameObject.Find("DropPool").GetComponent<DropPool>().GetRandomRarity())
            {
                case Enums.ItemRarity.Celestial:
                    amount = Random.Range(3, 4);
                    break;
                case Enums.ItemRarity.Fabled:
                    amount = Random.Range(2, 4);
                    break;
                case Enums.ItemRarity.Legendary:
                    amount = Random.Range(2, 3);
                    break;
                case Enums.ItemRarity.Epic:
                    amount = Random.Range(1, 3);
                    break;
                case Enums.ItemRarity.Rare:
                    amount = Random.Range(1, 2);
                    break;
                case Enums.ItemRarity.Uncommon:
                    amount = Random.Range(1, 2);
                    break;
                case Enums.ItemRarity.Common:
                    amount = 1;
                    break;
            }

            for (int i = 0; i < amount; i++)
            {
                GameObject chest = _gradingSystem.GenerateRandomChest();
                Instantiate(chest, transform.position + (Vector3)chestPositions[i], Quaternion.identity);
            }
        }
        
        private void OnTriggerExit2D(Collider2D col)
        {
            if(col.name == "Player")
            {
                _uiManager.UpdateRoomText();
            }
        }

        private void LockDoors()
        {
            _entrances[0].SetActive(_directions[0]);
            _entrances[1].SetActive(_directions[1]);
            _entrances[2].SetActive(_directions[2]);
            _entrances[3].SetActive(_directions[3]);
        }

        private void SpawnWave()
        {
            int enemyCount = Random.Range((int)waveSize.x, (int)waveSize.y);
            _uiManager.UpdateRoomText("Wave: " +(_curWave) + "/" + _maxWaves);
            Debug.Log("Wave: " + (_curWave) + "/" + _maxWaves);
            
            for (int i = 0; i < enemyCount; i++)
            {
                Vector3 pos;
                do
                {
                    pos = new Vector2(transform.position.x + roomCenter.x + Random.Range(-roomSize.x/2, roomSize.x/2), 
                        transform.position.y + roomCenter.y + Random.Range(-roomSize.y/2, roomSize.y/2));
                    Debug.Log(pos);
                } while (IsSpaceTaken(pos));
                if(roomType == Enums.RoomType.Regular)
                    Instantiate(_enemyPool.GetRandomEnemy(), pos, Quaternion.identity);
            }
            _curWave++;
        }
        
        private void DiscoverMiniMap()
        {
            foreach (var part in minimap)
            {
                part.SetActive(true);
            }
            
            Instantiate(DungeonGenerator.Instance.GetIcon(roomType), transform.position , Quaternion.identity);
        }

        private bool IsSpaceTaken(Vector2 pos)
        {
            return Physics2D.CircleCast(pos, 1,Vector2.zero, 999,LayerMask.NameToLayer("WallCollider"));
        }
        
        private void SpawnObstructions()
        {
            if (_obstructions.Length == 0 || roomType == Enums.RoomType.End || 
                roomType == Enums.RoomType.Start || roomType == Enums.RoomType.Chest)
                return;
            if(Random.Range(0,100) < _obstructionChance)
                _obstructions[Random.Range(0, _obstructions.Length)].SetActive(true);
        }
        
        private void SpawnEntranceWalls()
        {
            if(_directions[0])
                _entranceWalls[0].SetActive(true);
            else
                _walls[0].SetActive(true);
            if(_directions[1])
                _entranceWalls[1].SetActive(true);
            else
                _walls[1].SetActive(true);
            if(_directions[2])
                _entranceWalls[2].SetActive(true);
            else
                _walls[2].SetActive(true);
            if(_directions[3])
                _entranceWalls[3].SetActive(true);
            else
                _walls[3].SetActive(true);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(new Vector2(transform.position.x + roomCenter.x,transform.position.y + roomCenter.y),
                new Vector2(roomSize.x, roomSize.y));

            switch (roomType)
            {
                case Enums.RoomType.Start:
                    Gizmos.color = Color.blue;
                    break;
                case Enums.RoomType.Regular:
                    Gizmos.color = Color.green;
                    break;
                case Enums.RoomType.Trap:
                    Gizmos.color = Color.yellow;
                    break;
                case Enums.RoomType.End:
                    Gizmos.color = Color.red;
                    break;
                case Enums.RoomType.Chest:
                    Gizmos.color = Color.magenta;
                    break;
            }
            Gizmos.DrawWireCube(transform.position, new Vector2(4,4));
            
            Gizmos.color = Color.yellow;
            if (chestPositions.Length > 0)
            {
                for (int i = 0; i < chestPositions.Length; i++)
                {
                    Gizmos.DrawWireCube(transform.position + (Vector3)chestPositions[i], new Vector2(1,1));
                }
            }
        }
    }
}