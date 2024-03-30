using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeoneer
{
    public class DungeonEntrance : MonoBehaviour
    {
        private bool _playerClose;

        public GameObject entranceOverlay;
        // Update is called once per frame

        public PlayerMovement playerMovement;

        private void Start()
        {
            playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        }

        void Update()
        {
            if (_playerClose && Input.GetKeyDown(KeyCode.F))
            {
                OpenMenu();
            }

            if (entranceOverlay.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            {
                CloseMenu();
            }
        }

        public void OpenMenu()
        {
            playerMovement.SetOpenUI(true);
            entranceOverlay.SetActive(true);
        }

        public void CloseMenu()
        {
            playerMovement.SetOpenUI(false);
            entranceOverlay.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Player>(out var player))
            {
                _playerClose = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<Player>(out var player))
            {
                _playerClose = false;
            }
        }
    }
}
