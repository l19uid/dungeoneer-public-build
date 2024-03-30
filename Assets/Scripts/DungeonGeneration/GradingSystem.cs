using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dungeoneer
{
    public class GradingSystem : MonoBehaviour
    {
        [SerializeField] private string _floorName = "Floor 1";
        [SerializeField] private string[] _grades = new string[] { "S++","S+", "S", "A", "B", "C", "D", "F" };
        [SerializeField] private float[] _gradeThresholds = new float[] {1150, 1050, 1000, 900, 800, 600, 400, 200 };

        [SerializeField] private float _score = 0f;
        [SerializeField] private float _time = 0f;
        [SerializeField] private float _roomClearTime = 0f;
        [SerializeField] private float _roomClearTimeMax = 0f;
        [SerializeField] private GameObject[] _chests;
        [SerializeField] private Sprite[] _letterSprites;
        private PlayerAttack _playerAttack;
        private UIManager _uiManager;

        private bool isFinished = false;

        private void Start()
        {
            _score = 0f;
            _time = 0f;
            _playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
            _uiManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>();
        }

        private void Update()
        {
            if(isFinished)
                return;
            _uiManager.UpdateGradeText("Score : " + GetScore() + "\nGrade : " + GetGrade());
            _time += Time.deltaTime;
            _roomClearTime += Time.deltaTime;
        }

        public void AddScore(float score)
        {
            _score += score * _playerAttack.GetCombo();
        }

        public string GetGrade()
        {
            for (int i = 0; i < _gradeThresholds.Length; i++)
            {
                if (GetScore() >= _gradeThresholds[i])
                {
                    return _grades[i];
                }
            }

            return _grades[_grades.Length - 1];
        }

        public int GetScore()
        {
            return (int)(_score - (int)(_time / 30f) * 25);
        }

        public void StartClearingRoom(float roomClearTimeMax)
        {
            _roomClearTime = 0f;
            _roomClearTimeMax = roomClearTimeMax;
        }

        public void FinishClearingRoom()
        {
            AddScore(100f - (_roomClearTimeMax - _roomClearTime));
        }

        public void FinishDungeon()
        {
            isFinished = true;
        }
        
        public string GetInfo()
        {
            if(GetScore() < _gradeThresholds[4])
                return "You didnt beat the :" + _floorName + " floor \nWith a score of :" + GetScore() + 
                       ".\nTime to clear :" + _time.ToString("F2");
            else
                return "You beat the :" + _floorName + " floor.\nWith a score of :" + GetScore() + 
                       ".\nTime to clear :" + _time.ToString("F2");
        }
        
        public Sprite GetLetterSprite()
        {
            for (int i = 0; i < _gradeThresholds.Length; i++)
            {
                if (GetScore() >= _gradeThresholds[i])
                {
                    return _letterSprites[i];
                }
            }

            return _letterSprites[_letterSprites.Length - 1];
        }
        
        public void Reset()
        {
            _score = 0f;
            _time = 0f;
            isFinished = false;
        }
        
        public float GetGradeThreshold(int index)
        {
            return _gradeThresholds[index];
        }

        public GameObject GenerateRandomChest()
        {
            GameObject chest = null;
            switch(GameObject.Find("DropPool").GetComponent<DropPool>().GetRandomRarity())
            {
                case Enums.ItemRarity.Demonic:
                    chest = _chests[0];
                    break;
                case Enums.ItemRarity.Celestial:
                    chest = _chests[1];
                    break;
                case Enums.ItemRarity.Fabled:
                    chest = _chests[2];
                    break;
                case Enums.ItemRarity.Legendary:
                    chest = _chests[3];
                    break;
                case Enums.ItemRarity.Epic:
                    chest = _chests[4];
                    break;
                case Enums.ItemRarity.Rare:
                    chest = _chests[5];
                    break;
                case Enums.ItemRarity.Uncommon:
                    chest = _chests[6];
                    break;
                case Enums.ItemRarity.Common:
                    chest = _chests[7];
                    break;
            }

            return chest;
        }
    }
}