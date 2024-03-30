using UnityEngine;

namespace Dungeoneer
{
    public class NPCBanker : NPC
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>().SetNearNPC(true, gameObject, "bank");
            }
        }
    
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>().SetNearNPC(false, gameObject, "bank");
            }
        }
    }
}