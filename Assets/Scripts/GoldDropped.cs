using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeoneer
{
    public class GoldDropped : MonoBehaviour
    {
        public GameObject player;
        public int goldAmount = 1;
        public bool isActive;

        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        // Update is called once per frame
        void Update()
        {
            if (isActive)
                transform.position =
                    Vector3.MoveTowards(transform.position, player.transform.position, 6 * Time.deltaTime);

            if (Vector3.Distance(transform.position, player.transform.position) < .5f)
                PickUp();

            if (Vector3.Distance(transform.position, player.transform.position) < 3.5f)
                isActive = true;
        }

        public void PickUp()
        {
            player.GetComponent<Inventory>().AddGold(goldAmount);
            GameObject.FindWithTag("Canvas").GetComponent<UIManager>().UpdateGold();
            Destroy(gameObject);
        }
    }
}
