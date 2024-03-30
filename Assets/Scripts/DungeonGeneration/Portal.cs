using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Dungeoneer
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] DungeonGenerator.DungeonType dungeonType;
        [SerializeField] private Enums.TeleportType teleportType;

        private UIManager _uiManager;
        private BossManager _bossManager;
        private DungeonGenerator _dungeonGenerator;
        private bool _isNear;

        private void Start()    
        {
            _uiManager = GameObject.FindWithTag("Canvas").GetComponent<UIManager>();
            if(GameObject.FindWithTag("Generator")!= null)
                _dungeonGenerator = GameObject.FindWithTag("Generator").GetComponent<DungeonGenerator>();
        }

        public void TeleportToDungeonFloor(int floor)
        {
            _uiManager.LoadScene("DungeonDefault");
            //GameObject.Find("DungeonGenerator").GetComponent<DungeonGenerator>().GenerateFloor(floor);
        }

        public void SetDungeonType(string type)
        {
            dungeonType = (DungeonGenerator.DungeonType)System.Enum.Parse(typeof(DungeonGenerator.DungeonType), type);
        }

        public void TeleportToHub()
        {
            _uiManager.OpenPortalMenu(true, "hub", "Teleport to hub!");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if(teleportType == Enums.TeleportType.DungeonEntrance)
                    _uiManager.SetNearEntrance(true);
                else
                {
                    _uiManager.SetNearPortal(true);
                }
                _isNear = true;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F) && _isNear && teleportType == Enums.TeleportType.DungeonExit)
            {
                TeleportToHub();
            }
            else if (Input.GetKeyDown(KeyCode.F) && _isNear && teleportType == Enums.TeleportType.DungeonEntrance)
            {
                _uiManager.DisplayEntrancePanel();
            }
            else if (Input.GetKeyDown(KeyCode.F) && _isNear && teleportType == Enums.TeleportType.BossEntrance)
            {
                _bossManager = GameObject.Find("Boss Room").GetComponent<BossManager>();
                _uiManager.OpenPortalMenu(true, "dungeon", "Teleport to boss room!");
                
                _uiManager.GetTeleportButton().onClick.RemoveAllListeners();
                _uiManager.GetTeleportButton().onClick.AddListener(() => GameObject.FindWithTag("Player").transform.position = new Vector3(-100,-110));
                _uiManager.GetTeleportButton().onClick.AddListener(() => StartCoroutine(_bossManager.StartBoss(new Vector3(-100,-100))));
                _uiManager.GetTeleportButton().onClick.AddListener(() => _uiManager.SwitchPlayerControl(false));
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if(teleportType == Enums.TeleportType.DungeonEntrance)
                    _uiManager.SetNearEntrance(false);
                else
                {
                    _uiManager.SetNearPortal(false);
                }
                _isNear = false;
            }
        }
    }
}