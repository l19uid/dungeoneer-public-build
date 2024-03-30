using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Dungeoneer
{
    public class Bank : MonoBehaviour, IDataPersistence
    {
        private int _gold = 0;
        private int _goldCap = 1000000;
        private int _goldCapUpgradeCost = 250000;
        private int _goldCapUpgradeCostMultiplier = 2;
        private Inventory _inventory;

        private void Start()
        {
            _inventory = GetComponent<Inventory>();
        }

        public void DepositGold(string input)
        {
            Debug.Log(input);
            if (input == "custom")
            {
                DepositGold(GameObject.Find("CustomInput").GetComponent<TextMeshProUGUI>().text);
            }
            else if (input == "half")
            {
                DepositGold((GetGold() / 2).ToString());
            }
            else if (input == "all")
            {
                DepositGold(_inventory.GetGold().ToString());
            }
            else if (input == "fifth")
            {
                DepositGold((GetGold() / 5).ToString());
            }
            else if (Int32.TryParse(input, out int gold))
            {
                if (gold + GetGold() > GetGoldCap())
                {
                    UIManager.Instance.DisplayMessage("Not enough space in bank.", UIManager.PopUpType.Error);
                }
                else if (_inventory.GetGold() < gold)
                {
                    UIManager.Instance.DisplayMessage("Not enough gold in inventory.", UIManager.PopUpType.Error);
                }
                else
                {
                    _inventory.RemoveGold(gold);
                    AddGold(gold);
                    UIManager.Instance.DisplayMessage("Deposited " + gold + " gold.", UIManager.PopUpType.Success);
                }
            }
            else
                UIManager.Instance.DisplayMessage("Error while depositing.", UIManager.PopUpType.Error);
        }

        public void WithdrawGold(string input)
        {
            if (input == "half")
            {
                WithdrawGold((GetGold() / 2).ToString());
            }
            else if (input == "all")
            {
                WithdrawGold(GetGold().ToString());
            }
            else if (input == "fifth")
            {
                WithdrawGold((GetGold() / 5).ToString());
            }
            //if can parse then instantiate success if not error
            else if (Int32.TryParse(input, out int gold))
            {
                if (gold > GetGold())
                {
                    UIManager.Instance.DisplayMessage("Not enough gold in bank.", UIManager.PopUpType.Error);
                    return;
                }
                else
                {
                    RemoveGold(gold);
                    _inventory.AddGold(gold);
                    UIManager.Instance.DisplayMessage("Withdrew " + gold + " gold.", UIManager.PopUpType.Success);
                    return;
                }
            }
            else
                UIManager.Instance.DisplayMessage("Error while withdrawing.", UIManager.PopUpType.Error);
        }

        private void RemoveGold(int gold)
        {
            _gold -= gold;
        }

        private void AddGold(int gold)
        {
            _gold += gold;
        }

        public int GetGold()
        {
            return _gold;
        }

        public int GetGoldCap()
        {
            return _goldCap;
        }

        public int GetGoldCapUpgradeCost()
        {
            return _goldCapUpgradeCost;
        }

        public int GetGoldCapUpgradeCostMultiplier()
        {
            return _goldCapUpgradeCostMultiplier;
        }

        public void UpgradeGoldCap()
        {
            _goldCap *= _goldCapUpgradeCostMultiplier;
            _goldCapUpgradeCost *= _goldCapUpgradeCostMultiplier;
        }

        public void SetGold(int gold)
        {
            _gold = gold;
        }

        public void SetGoldCap(int goldCap)
        {
            _goldCap = goldCap;
        }

        public void SetGoldCapUpgradeCost(int goldCapUpgradeCost)
        {
            _goldCapUpgradeCost = goldCapUpgradeCost;
        }

        public void LoadData(GameData data)
        {
            _gold = data.playerData.bankGold;
            _goldCap = data.playerData.bankGoldCap;
            _goldCapUpgradeCost = data.playerData.bankGoldCapUpgradeCost;
            _goldCapUpgradeCostMultiplier = data.playerData.bankGoldCapUpgradeCostMultiplier;
        }

        public void SaveData(GameData data)
        {
            data.playerData.SetBank(GetGold(), GetGoldCap(), GetGoldCapUpgradeCost(), GetGoldCapUpgradeCostMultiplier());
        }
    }
}