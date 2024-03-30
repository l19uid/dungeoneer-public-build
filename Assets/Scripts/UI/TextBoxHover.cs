using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Dungeoneer
{
    public class TextBoxHover : MonoBehaviour
    {
        [SerializeField] private Vector2 limit = new Vector2(0, 0);
        [SerializeField] private TextMeshProUGUI text;
        private Camera cam;
        [SerializeField] private Vector2 center;

        [SerializeField] private List<Sprite> backgrounds;
        [SerializeField] private GameObject progressBar;

        // Update is called once per frame
        private Item item;

        private void Start()
        {
            cam = GameObject.Find("Camera").GetComponent<Camera>();
            gameObject.transform.parent = GameObject.FindGameObjectWithTag("Canvas").transform;
            center = GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>().position;
            transform.position = new Vector2(
                Math.Clamp(Input.mousePosition.x, center.x - limit.x / 2, (center.x + limit.x + 400) / 2),
                Math.Clamp(Input.mousePosition.y, (center.y - limit.y) / 2, (center.y + limit.y + 200) / 2));
        }

        void Update()
        {
            transform.position = new Vector2(
                Math.Clamp(Input.mousePosition.x, center.x - limit.x / 2, (center.x + limit.x + 400) / 2),
                Math.Clamp(Input.mousePosition.y, (center.y - limit.y) / 2, (center.y + limit.y + 200) / 2));

            if (Input.GetKeyDown(KeyCode.LeftShift))
                text.text = item.GetHoverStats(true);
            else if (Input.GetKeyUp(KeyCode.LeftShift))
                text.text = item.GetHoverStats();

            HandleScroll();
        }

        private void HandleScroll()
        {
            //If the player scrolls up, or down and the window is out of the screen, using its size and canvas size decide where to move it
            if (Input.mouseScrollDelta.y > 0 && transform.position.y + limit.y / 2 > Screen.height)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y - 100);
            }
            else if (Input.mouseScrollDelta.y < 0 && transform.position.y - limit.y / 2 < 0)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + 100);
            }
        }

        public void InitHoverPanel(GameObject item)
        {
            text.text = item.GetComponent<Item>().GetHoverStats();
            this.item = item.GetComponent<Item>();

            switch (item.GetComponent<Item>().GetItemRarity())
            {
                case Enums.ItemRarity.Common:
                    gameObject.GetComponentInChildren<Image>().sprite = backgrounds[0];
                    break;
                case Enums.ItemRarity.Uncommon:
                    gameObject.GetComponentInChildren<Image>().sprite = backgrounds[1];
                    break;
                case Enums.ItemRarity.Rare:
                    gameObject.GetComponentInChildren<Image>().sprite = backgrounds[2];
                    break;
                case Enums.ItemRarity.Epic:
                    gameObject.GetComponentInChildren<Image>().sprite = backgrounds[3];
                    break;
                case Enums.ItemRarity.Legendary:
                    gameObject.GetComponentInChildren<Image>().sprite = backgrounds[4];
                    break;
                case Enums.ItemRarity.Fabled:
                    gameObject.GetComponentInChildren<Image>().sprite = backgrounds[5];
                    break;
                case Enums.ItemRarity.Celestial:
                    gameObject.GetComponentInChildren<Image>().sprite = backgrounds[6];
                    break;
                case Enums.ItemRarity.Demonic:
                    gameObject.GetComponentInChildren<Image>().sprite = backgrounds[7];
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public void InitProgressBar(float fillAmount)
        {
            progressBar.SetActive(true);
            progressBar.GetComponent<Image>().fillAmount = fillAmount;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(center, new Vector3(limit.x, limit.y, 0));
        }
    }
}
