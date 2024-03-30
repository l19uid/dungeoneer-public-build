using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeoneer
{
    public class NPC : MonoBehaviour
    {
        [SerializeField] protected string name;
        [SerializeField] protected Sprite sprite;
        [TextArea] [SerializeField] protected string welcomeText;
        [TextArea] [SerializeField] protected List<string> voiceLines;
        [SerializeField] protected string npcType;
        [Tooltip("If the sprite is facing right, check this box.")]
        [SerializeField] private bool isSpriteInverted;
        
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
        
        public virtual string GetName()
        {
            return name;
        }
        
        public virtual Sprite GetSprite()
        {
            return sprite;
        }
        
        public virtual string GetWelcomeText()
        {
            return welcomeText;
        }
        
        public virtual List<string> GetVoiceLines()
        {
            return voiceLines;
        }
        
        public virtual string GetNPCType()
        {
            return npcType;
        }

        public virtual bool IsSpriteInverted()
        {
            return isSpriteInverted;
        }
    }
}   
