using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeoneer
{
    public class NPCQuester : NPC
    {
        [SerializeField] private List<GameObject> quests;

        // Update is called once per frame
        void Update()
        {
            //if (Input.GetKeyDown(KeyCode.F) && isClose && !_uiManager.questPanel.activeSelf)
            //    _uiManager.DisplayQuests(welcomeText);
            //if (Input.GetKeyDown(KeyCode.Escape) && _uiManager.questPanel.activeSelf)
            //    _uiManager.HideQuests();
            //if (Input.GetKeyDown(KeyCode.F) && _uiManager.questPanel.activeSelf)
            //{
            //    _uiManager.DisplayQuest(quests[0].GetComponent<Quest>());
            //    _inventory.AddQuest(quests[0]);
            //    quests.Remove(quests[0]);
            //}
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>().SetNearNPC(true, gameObject, npcType);
            }
        }
    
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>().SetNearNPC(false, null, npcType);
            }
        }
        
        public override string GetName()
        {
            return name;
        }
        
        public override Sprite GetSprite()
        {
            return sprite;
        }
        
        public override string GetWelcomeText()
        {
            return welcomeText;
        }
        
        public override List<string> GetVoiceLines()
        {
            return voiceLines;
        }
        
        public override string GetNPCType()
        {
            return npcType;
        }
        
        public List<GameObject> GetQuests()
        {
            return quests;
        }
    }
}