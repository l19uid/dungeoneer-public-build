using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Dungeoneer
{
    public class UIInfoHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public string type = "stats";
        private bool isHovering = false;
        private GameObject hoverPanel;
        Player player;

        public void OnPointerEnter(PointerEventData eventData)
        {
            player = GameObject.Find("Player").GetComponent<Player>();
            isHovering = true;
            hoverPanel = (GameObject)Instantiate(Resources.Load("UI/" + "TextBoxHover"));

            if (type == "stats")
            {
                hoverPanel.GetComponentInChildren<TextMeshProUGUI>().text = player.GetPlayerStats();
            }
            else if (type == "main")
            {
                hoverPanel.GetComponentInChildren<TextMeshProUGUI>().text = player.GetPlayerSkills().ToString();
                hoverPanel.GetComponent<TextBoxHover>().InitProgressBar(
                    player.GetPlayerSkills().GetExperience() / player.GetPlayerSkills().GetExperienceToNextLevel());
            }
            else if (Enum.TryParse(type, out Enums.SkillType st))
            {
                //try to convert string into enum if not display error text
                try
                {
                    PlayerSkill skill = player.GetPlayerSkillsFromType(st);

                    hoverPanel.GetComponentInChildren<TextMeshProUGUI>().text = skill.ToString();
                    hoverPanel.GetComponent<TextBoxHover>().InitProgressBar(
                        skill.GetExperience() / skill.GetExperienceToNextLevel());
                }
                catch (ArgumentException e)
                {
                    hoverPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Error: " + e.Message;
                }
            }
            else if (Enum.TryParse(type, out Enums.EnemyFamily ef))
            {
                //try to convert string into enum if not display error text
                try
                {
                    EnemySoul skill = player.GetEnemySkillsFromFamily(ef);

                    hoverPanel.GetComponentInChildren<TextMeshProUGUI>().text = skill.ToString();
                    hoverPanel.GetComponent<TextBoxHover>().InitProgressBar(
                        skill.GetCount() / skill.GetCountToNextLevel());
                }
                catch (ArgumentException e)
                {
                    hoverPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Error: " + e.Message;
                }
            }

            hoverPanel.transform.parent = GameObject.FindGameObjectWithTag("Canvas").transform;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isHovering = false;
            Destroy(hoverPanel);
        }

        private void Update()
        {
            if (isHovering && hoverPanel != null)
                hoverPanel.transform.position = Input.mousePosition;
        }
    }
}
