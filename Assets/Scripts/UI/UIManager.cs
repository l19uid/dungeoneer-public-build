using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

namespace Dungeoneer
{
    public class UIManager : MonoBehaviour
    {
        [Header("Panel Elements")] 
        public GameObject inventoryPanel;
        public GameObject miniMapPanel;
        public GameObject shopPanel;
        public GameObject artifactPanel;
        public GameObject equippedPanel;
        public GameObject escapePanel;
        public GameObject creditsPanel;
        public GameObject settingsPanel;
        public GameObject skillPanel;
        [Header("Player Elements")] 
        public GameObject equippedItems;
        public GameObject playerStats;
        [Header("NPC Elements")] 
        public GameObject npcPanel;
        public GameObject bankPanel;
        public GameObject[] npcButtons;
        public TextMeshProUGUI npcName;
        public TextMeshProUGUI npcText;
        public Image npcSprite;
        public float delayBetweenLetters = .05f;
        private bool _isTyping;
        NPCShop _shop;
        private bool _isNearNpc;
        private string _nearNPC;
        private NPC _npc;

        [Header("Equipped Elements")] 
        public GameObject equippedWeapon;
        public GameObject equippedWeaponCooldown;
        public GameObject equippedWeaponChargeCooldown;
        private float equippedWeaponTime;
        private float equippedWeaponTimeMax;
        private bool isChargingWeapon;
        private float equippedWeaponChargeTime;
        private float equippedWeaponChargeTimeMax;
        public GameObject equippedAbility;
        public GameObject equippedAbilityCooldown;
        private float equippedAbilityTime;
        public GameObject[] selectedInventoryHighlight;

        [Header("Inventory Elements")] public Enums.ItemType selectedType;
        public GameObject itemPrefab;
        public GameObject weapon;
        public GameObject ability;

        [Tooltip("Necessary to keep from top to down.")]
        public GameObject[] armor;

        public GameObject[] inventorySlots;
        public GameObject[] artifactSlots;
        public GameObject[] shopSlots;
        private GameObject[] _shopItems;
        private bool _isNearChest;
        private bool _isNearItem;
        private bool _isNearPortal;
        private bool _isNearEntrance;
        private bool _isNearBank;
        public GameObject infoPanel;

        private Inventory _inventory;

        [Header("Texts")] 
        public TextMeshProUGUI playerName;
        public TextMeshProUGUI goldOverlayText;
        public TextMeshProUGUI goldInventoryText;
        public TextMeshProUGUI goldBankText;
        public TextMeshProUGUI starsText;
        public TextMeshProUGUI lvlText;
        public TextMeshProUGUI itemStats;
        public TextMeshProUGUI upgradeText;
        public Image upgradeItemImage;
        public static TextMeshProUGUI versionText;
        public TextMeshProUGUI newPlayerName;

        private static List<GameObject> transferQueue = new List<GameObject>();

        [Header("Quests")] public TextMeshProUGUI questText;
        [SerializeField] private int selectedSlot;
        public GameObject sellOverlay;
        [Header("Combo")] public Image comboImage;
        public TextMeshProUGUI comboText;
        [Header("RoomInfo")] public TextMeshProUGUI roomInfoText;
        [Header("Grading")] public TextMeshProUGUI gradeText;
        public GameObject awardPanel;
        public TextMeshProUGUI awardText;
        public Image awardSprite;
        public TextMeshProUGUI floorText;
        public TextMeshProUGUI timeText;
        public GameObject finishPannel;
        public TextMeshProUGUI finishInfo;
        [Header("Upgrader")] 
        public GameObject upgradePanel;
        public GameObject recipeItemPrefab;
        public GameObject upgradeRecipeParent;
        public List<GameObject> upgradeRecipeList;
        [Header("Portal")] 
        public GameObject portalPanel;
        public GameObject entrancePanel;
        public Button portalButton;
        [Header("Stamina")]
        public Image staminaBar;
        [Header("Death")]
        public GameObject deathPanel;

        List<GameObject> loadedInventory = new List<GameObject>();
        List<GameObject> loadedShop = new List<GameObject>();
        List<GameObject> loadedArtifacts;

        private GameObject loadedWeapon;
        private GameObject loadedAbility;
        private GameObject loadedHelmet;
        private GameObject loadedChestplate;
        private GameObject loadedLeggings;
        private GameObject loadedBoots;
        private GameObject loadedNecklace;
        private GameObject loadedGloves;
        private GameObject loadedConsumableOne;
        private GameObject loadedConsumableTwo;
        private PlayerAttack _playerAttack;
        private PlayerMovement _playerMovement;
        private Player _player;
        private GradingSystem _gradingSystem;
        private string _crntInvType;

        //BANK
        private Bank _bank;
        private string customBankAmount = "";
        private string customBankType = "withdraw";

        public enum PopUpType
        {
            Error,
            Success,
            Info,
            Drop
        }

        // Start is called before the first frame update
        void Start()
        {
            if (GameObject.Find("Generator") != null)
                _gradingSystem = GameObject.Find("Generator").GetComponent<GradingSystem>();
            transferQueue = new List<GameObject>();
            _playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
            _inventory = GameObject.Find("Player").GetComponent<Inventory>();
            _player = GameObject.Find("Player").GetComponent<Player>();
            _bank = GameObject.Find("Player").GetComponent<Bank>();
            _playerAttack = GameObject.Find("Player").GetComponent<PlayerAttack>();
            _crntInvType = "Item";
            UpdateUI();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                CloseAllPanels();

            HandleInteractText();

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (_isNearNpc)
                {
                    DisplayNPCPanel();
                }
            }

            if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Tab))
                DisplayInventory();
            if (Input.GetKeyDown(KeyCode.K))
                DisplaySkills();

            //if (Input.GetKeyDown(KeyCode.M))
            //    DisplayMiniMap();

            CooldownHandler();

            ComboHandler();
        }

        private void HandleInteractText()
        {
            if (_isNearChest || _isNearItem || _isNearNpc || _isNearPortal || _isNearEntrance)
            {
                if (_isNearItem)
                    SetInteractText("Press F to pick up item.");
                else if (_isNearChest)
                    SetInteractText("Press F to open chest.");
                else if (_isNearNpc)
                    SetInteractText("Press F to interact.");
                else if (_isNearPortal || _isNearEntrance)
                    SetInteractText("Press F to open portal.");
            }
            else
                SetInteractText();
        }

        public void CloseAllPanels()
        {
            bool isAnyPanelOpen = inventoryPanel.activeSelf || miniMapPanel.activeSelf || shopPanel.activeSelf || 
                                  upgradePanel.activeSelf || npcPanel.activeSelf || entrancePanel.activeSelf || 
                                  settingsPanel.activeSelf || skillPanel.activeSelf || bankPanel.activeSelf;

            miniMapPanel.SetActive(false);
            shopPanel.SetActive(false);
            upgradePanel.SetActive(false);
            npcPanel.SetActive(false);
            entrancePanel.SetActive(false);
            settingsPanel.SetActive(false);
            inventoryPanel.SetActive(false);
            skillPanel.SetActive(false);
            bankPanel.SetActive(false);

            DestroyHoverText();

            _playerAttack.SetOpenOverlay(false);
            _playerMovement.SetOpenUI(false);

            if (isAnyPanelOpen)
            {
                _playerMovement.SetOpenUI(false);
            }
            else
            {
                DisplayEscape();
            }
        }

        private void CooldownHandler()
        {
            equippedWeaponTime -= Time.deltaTime;
            equippedWeaponChargeTimeMax += Time.deltaTime;
            equippedWeaponCooldown.GetComponent<Image>().fillAmount =
                equippedWeaponTimeMax - (equippedWeaponTime / equippedWeaponTimeMax);

            if (isChargingWeapon)
            {
                equippedWeaponChargeCooldown.SetActive(true);
                equippedWeaponTime = Mathf.Clamp(equippedWeaponTime, 0, equippedWeaponTimeMax);
                equippedWeaponChargeTimeMax = 
                    Mathf.Clamp(equippedWeaponChargeTimeMax, 0, equippedWeaponChargeTimeMax);
                equippedWeaponChargeCooldown.GetComponent<Image>().fillAmount = equippedWeaponChargeTimeMax -
                    (equippedWeaponChargeTime / equippedWeaponChargeTimeMax);
            }
            else if (equippedWeaponChargeCooldown != null)
            {
                equippedWeaponChargeCooldown.SetActive(false);
            }
        }

        public void DisplayEscape()
        {
            escapePanel.SetActive(!escapePanel.activeSelf);
            creditsPanel.SetActive(!escapePanel.activeSelf);
            SwitchPlayerControl(escapePanel.activeSelf);
        }

        public void RenderItems(Enums.ItemType renderItemType = Enums.ItemType.Item)
        {
            // Disables all selected inventory highlights
            if (selectedInventoryHighlight.Length > 5)
            {
                foreach (var highlíght in selectedInventoryHighlight)
                {
                    highlíght.SetActive(false);
                }
                // Enables the selected inventory highlight
                switch (renderItemType)
                {
                    case Enums.ItemType.Ability:
                        selectedInventoryHighlight[0].SetActive(true);
                        break;
                    case Enums.ItemType.Weapon:
                        selectedInventoryHighlight[1].SetActive(true);
                        break;
                    case Enums.ItemType.Artifact:
                        selectedInventoryHighlight[2].SetActive(true);
                        break;
                    case Enums.ItemType.Item:
                        selectedInventoryHighlight[3].SetActive(true);
                        break;
                    case Enums.ItemType.Armor:
                        selectedInventoryHighlight[4].SetActive(true);
                        break;
                    case Enums.ItemType.Consumable:
                        selectedInventoryHighlight[5].SetActive(true);
                        break;
                }
            }
            
            DestroyLoadedInventory();

            Dictionary<int,GameObject> renderItems = new Dictionary<int,GameObject>();

            switch (renderItemType)
            {
                case Enums.ItemType.Ability:
                    renderItems = _inventory.GetAbilities();
                    break;
                case Enums.ItemType.Weapon:
                    renderItems = _inventory.GetWeapons();
                    break;
                case Enums.ItemType.Artifact:
                    renderItems = _inventory.GetArtifacts();
                    break;
                case Enums.ItemType.Item:
                    renderItems = _inventory.GetItems();
                    break;
                case Enums.ItemType.Armor:
                    renderItems = _inventory.GetArmor();
                    break;
                case Enums.ItemType.Consumable:
                    renderItems = _inventory.GetConsumables();
                    break;
            }

            // Renders items in slots according to their slot/key
            foreach (var item in renderItems)
            {
                if (item.Key == -2)
                {
                    continue;
                }
                var itemObject = Instantiate(itemPrefab, inventorySlots[item.Key].transform);
                itemObject.GetComponent<UIItem>().Init(item.Value,item.Key);
                loadedInventory.Add(itemObject);
            }
        }

        public void DestroyLoadedInventory()
        {
            foreach (var item in loadedInventory)
            {
                Destroy(item);
            }
        }

        public void DisplayMiniMap()
        {
            miniMapPanel.SetActive(true);
        }

        public void DisplayInventory()
        {
            SetInteractText();
            SetChestClose(false);
            SetPickUpClose(false);
            
            miniMapPanel.SetActive(false);
            shopPanel.SetActive(false);
            upgradePanel.SetActive(false);
            npcPanel.SetActive(false);
            entrancePanel.SetActive(false);
            settingsPanel.SetActive(false);
            bankPanel.SetActive(false);
            skillPanel.SetActive(false);
            
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            SwitchPlayerControl(inventoryPanel.activeSelf);
            DestroyShop();
            DestroyHoverText();
            UpdateUI();
        }
        
        public void DisplaySkills()
        {
            SetInteractText();
            SetChestClose(false);
            SetPickUpClose(false);
            
            inventoryPanel.SetActive(false);
            miniMapPanel.SetActive(false);
            shopPanel.SetActive(false);
            upgradePanel.SetActive(false);
            npcPanel.SetActive(false);
            entrancePanel.SetActive(false);
            settingsPanel.SetActive(false);
            bankPanel.SetActive(false);

            skillPanel.SetActive(!skillPanel.activeSelf);
            SwitchPlayerControl(skillPanel.activeSelf);
            DestroyShop();
            DestroyHoverText();
            UpdateUI();
        }

        public void RenderEquipped()
        {
            //This is so the hover element doesnt stay when you equip something
            if (GameObject.Find("TextBoxHover(Clone)"))
                Destroy(GameObject.Find("TextBoxHover(Clone)"));
            
            Destroy(loadedWeapon);
            Destroy(loadedAbility);
            Destroy(loadedHelmet);
            Destroy(loadedChestplate);
            Destroy(loadedLeggings);
            Destroy(loadedBoots);
            Destroy(loadedGloves);
            Destroy(loadedNecklace);

            loadedWeapon = null;
            loadedAbility = null;
            loadedHelmet = null;
            loadedChestplate = null;
            loadedLeggings = null;
            loadedBoots = null;
            loadedGloves = null;
            loadedNecklace = null;

            //WEAPON & ABILITY
            if (_inventory.GetEquippedWeapon() != null)
            {
                loadedWeapon = Instantiate(itemPrefab, weapon.transform);
                loadedWeapon.GetComponent<UIItem>().Init(_inventory.GetEquippedWeapon(), 0, true);
                equippedWeapon.GetComponent<Image>().sprite = _inventory.GetEquippedWeaponScript().GetItemIcon();
            }

            if (_inventory.GetEquippedAbility() != null)
            {
                loadedAbility = Instantiate(itemPrefab, ability.transform);
                loadedAbility.GetComponent<UIItem>().Init(_inventory.GetEquippedAbility(), 0, true);
                equippedAbility.GetComponent<Image>().sprite = _inventory.GetEquippedAbilityScript().GetItemIcon();
            }

            //ARMOR
            if (_inventory.GetEquippedHelmet() != null)
            {
                loadedHelmet = Instantiate(itemPrefab, armor[0].transform);
                loadedHelmet.GetComponent<UIItem>().Init(_inventory.GetEquippedHelmet(), 0, true);
            }

            if (_inventory.GetEquippedChestplate() != null)
            {
                loadedChestplate = Instantiate(itemPrefab, armor[1].transform);
                loadedChestplate.GetComponent<UIItem>().Init(_inventory.GetEquippedChestplate(), 0, true);
            }

            if (_inventory.GetEquippedLeggings() != null)
            {
                loadedLeggings = Instantiate(itemPrefab, armor[2].transform);
                loadedLeggings.GetComponent<UIItem>().Init(_inventory.GetEquippedLeggings(), 0, true);
            }

            if (_inventory.GetEquippedBoots() != null)
            {
                loadedBoots = Instantiate(itemPrefab, armor[3].transform);
                loadedBoots.GetComponent<UIItem>().Init(_inventory.GetEquippedBoots(), 0, true);
            }

            if (_inventory.GetEquippedGloves() != null)
            {
                loadedGloves = Instantiate(itemPrefab, armor[4].transform);
                loadedGloves.GetComponent<UIItem>().Init(_inventory.GetEquippedGloves(), 0, true);
            }

            if (_inventory.GetEquippedNecklace() != null)
            {
                loadedNecklace = Instantiate(itemPrefab, armor[5].transform);
                loadedNecklace.GetComponent<UIItem>().Init(_inventory.GetEquippedNecklace(), 0, true);
            }

            if (loadedArtifacts == null)
                loadedArtifacts = new List<GameObject>();
            for (int i = 0; i < loadedArtifacts.Count; i++)
            {
                var itemObject = loadedArtifacts[i];
                loadedArtifacts.Remove(itemObject);
                Destroy(itemObject);
            }

            // TODO : ARTIFACTS
            //for (int i = 0; i < _inventory.GetEquippedArtifacts().Count; i++)
            //{
            //    var itemObject = Instantiate(itemPrefab, artifactSlots[i].transform);
            //    itemObject.GetComponent<UIItem>().Init(_inventory.GetEquippedArtifacts()[i], i, true);
            //    loadedArtifacts.Add(itemObject);
            //}
        }

        public void UpdateUI()
        {
            Enums.ItemType type = (Enums.ItemType)System.Enum.Parse(typeof(Enums.ItemType), _crntInvType);

            //TODO : ARTIFACT
            //if (type == Enum.ItemType.Artifact)
            //{
            //    artifactPanel.SetActive(true);
            //    equippedPanel.SetActive(false);
            //}
            //else
            //{
            //    artifactPanel.SetActive(false);
            //    equippedPanel.SetActive(true);
            //}

            RenderItems(type);
            UpdatePlayerStats();
            RenderEquipped();
        }

        private void DestroyHoverText()
        {
            if (GameObject.Find("TextBoxHover(Clone)"))
                Destroy(GameObject.Find("TextBoxHover(Clone)"));
        }

        //QUESTS
        public void DisplayQuests(string text)
        {
            SwitchPlayerControl(true);
            StartCoroutine(TypeText(text, questText));
        }

        public void HideQuests()
        {
            _playerMovement.SetOpenUI(false);
        }

        public void DisplayQuest()
        {
            StartCoroutine(TypeText(_npc.GetComponent<NPCQuester>().GetQuests().ToString(), questText));
        }

        private IEnumerator TypeText(string text, TextMeshProUGUI textUI)
        {
            if (!_isTyping)
            {
                textUI.text = "";
                _isTyping = true;
                for (int i = 0; i < text.Length; i++)
                {
                    textUI.text += text[i];
                    yield return new WaitForSeconds(delayBetweenLetters);
                }

                _isTyping = false;
            }
        }

        public void DisplayShopItems()
        {
            DestroyShop();
            shopPanel.SetActive(!shopPanel.activeSelf);

            int i = 0;
            foreach (GameObject item in _shop.GetItemsForSale())
            {
                var itemObject = Instantiate(itemPrefab, shopSlots[i].transform);
                itemObject.GetComponent<UIItem>().Init(item, i, false, true);

                loadedShop.Add(itemObject);
                i++;
            }
        }

        public void DestroyShop()
        {
            foreach (var item in loadedShop)
            {
                Destroy(item);
            }
        }

        public void UpdatePlayerStats()
        {
            playerName.text = _player.GetName();
            UpdateGold();
            UpdateBankStats();
            if(starsText != null)
                starsText.text = "<gradient=\"StarGradient\">" + _inventory.GetStars() + " S</gradient>";
            if(_player.GetPlayerSkills() != null)
                lvlText.text = "Lvl : " + _player.GetPlayerSkills().GetLevel();
        }

        public void UpdateInventorySlots(Enums.ItemType type)
        {
            foreach (var slot in inventorySlots)
            {
                slot.GetComponent<UIInventorySlot>().UpdateSlot(type);
            }
        }

        public void DisplayShop()
        {
            DestroyShop();
            
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            SwitchPlayerControl(shopPanel.activeSelf);

            if (GameObject.Find("TextBoxHover(Clone)"))
                Destroy(GameObject.Find("TextBoxHover(Clone)"));

            if (inventoryPanel.activeSelf)
                UpdateUI();

            DisplayShopItems();
        }

        public void DisplayUpgrader()
        {
            SetChestClose(false);
            SetPickUpClose(false);
            
            miniMapPanel.SetActive(false);
            shopPanel.SetActive(false);
            upgradePanel.SetActive(false);
            npcPanel.SetActive(false);
            entrancePanel.SetActive(false);
            settingsPanel.SetActive(false);
            bankPanel.SetActive(false);

            upgradePanel.SetActive(true);
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            SwitchPlayerControl(upgradePanel.activeSelf);

            if (GameObject.Find("TextBoxHover(Clone)"))
                Destroy(GameObject.Find("TextBoxHover(Clone)"));

            if (inventoryPanel.activeSelf)
                UpdateUI();
        }

        public void SetCurrentInventoryType(string type)
        {
            if(type == "EnemyDrop")
                type = "Item";
            _crntInvType = type;
            UpdateUI();
        }

        private void ComboHandler()
        {
            if (comboImage == null || comboText == null || _playerAttack == null)
                return;
            comboImage.fillAmount = _playerAttack.GetComboTimer() / _playerAttack.GetComboTimerMax();

            switch (_playerAttack.GetCombo())
            {
                case 1:
                    comboImage.color = Color.Lerp(Color.gray, Color.white,
                        _playerAttack.GetComboTimer() / _playerAttack.GetComboTimerMax());
                    comboText.text = "<gradient=\"RarityCommonGradient\">" + _playerAttack.GetCombo() + "</gradient>";
                    break;
                case 2:
                    comboImage.color = Color.Lerp(new Color(0, 96, 16), new Color(0, 196, 32),
                        _playerAttack.GetComboTimer() / _playerAttack.GetComboTimerMax());
                    comboText.text = "<gradient=\"RarityUncommonGradient\">" + _playerAttack.GetCombo() + "</gradient>";
                    break;
                case 3:
                    comboImage.color = Color.Lerp(new Color(0, 96, 0), new Color(96, 196, 96),
                        _playerAttack.GetComboTimer() / _playerAttack.GetComboTimerMax());
                    comboText.text = "<gradient=\"RarityRareGradient\">" + _playerAttack.GetCombo() + "</gradient>";
                    break;
                case 4:
                    comboImage.color = Color.Lerp(new Color(96, 16, 16), new Color(196, 96, 96),
                        _playerAttack.GetComboTimer() / _playerAttack.GetComboTimerMax());
                    comboText.text = "<gradient=\"RarityEpicGradient\">" + _playerAttack.GetCombo() + "</gradient>";
                    break;
            }
        }

        public class Instance
        {
            public static void DisplayMessage(string message, PopUpType type = PopUpType.Info, Vector2 pos = default)
            {
                GameObject text;
                switch (type)
                {
                    case PopUpType.Error:
                        if (pos == default)
                            text = Instantiate(Resources.Load("UI/TextError"), Input.mousePosition,
                                Quaternion.identity) as GameObject;
                        else
                            text = Instantiate(Resources.Load("UI/TextError"), pos, Quaternion.identity) as GameObject;
                        break;
                    case PopUpType.Success:
                        if (pos == default)
                            text = Instantiate(Resources.Load("UI/TextSuccess"), Input.mousePosition,
                                Quaternion.identity) as GameObject;
                        else
                            text = Instantiate(Resources.Load("UI/TextSuccess"), pos,
                                Quaternion.identity) as GameObject;
                        break;
                    default:
                        if (pos == default)
                            text = Instantiate(Resources.Load("UI/TextInfo"), Input.mousePosition,
                                Quaternion.identity) as GameObject;
                        else
                            text = Instantiate(Resources.Load("UI/TextInfo"), pos, Quaternion.identity) as GameObject;
                        break;
                }

                text.GetComponent<TextMeshProUGUI>().text = message;

                text.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
            }

            public static void DisplayMessage(Item item, Vector2 pos = default)
            {
                GameObject text;
                if (pos == default)
                    text =
                        Instantiate(Resources.Load("UI/TextDrop"), Input.mousePosition, Quaternion.identity) as
                            GameObject;
                else
                    text = Instantiate(Resources.Load("UI/TextDrop"), pos, Quaternion.identity) as GameObject;

                string message = "You got ";

                switch (item.GetItemRarity())
                {
                    case Enums.ItemRarity.Common:
                        message += "<gradient=\"RarityCommonGradient\">" + item.GetName() + "</gradient>";
                        break;
                    case Enums.ItemRarity.Uncommon:
                        message += "<gradient=\"RarityUncommonGradient\">" + item.GetName() + "</gradient>";
                        break;
                    case Enums.ItemRarity.Rare:
                        message += "<gradient=\"RarityRareGradient\">" + item.GetName() + "</gradient>";
                        break;
                    case Enums.ItemRarity.Epic:
                        message += "<gradient=\"RarityEpicGradient\">" + item.GetName() + "</gradient>";
                        break;
                    case Enums.ItemRarity.Legendary:
                        message += "<gradient=\"RarityLegendaryGradient\">" + item.GetName() + "</gradient>";
                        break;
                    case Enums.ItemRarity.Fabled:
                        message += "<gradient=\"RarityFabledGradient\">" + item.GetName() + "</gradient>";
                        break;
                    case Enums.ItemRarity.Celestial:
                        message += "<gradient=\"RarityCelestialGradient\">" + item.GetName() + "</gradient>";
                        break;
                    default:
                        message += "Unknown Rarity";
                        break;
                }

                text.GetComponent<TextMeshProUGUI>().text = message;
                text.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
            }

            public static void PickUpItem(GameObject item)
            {
                bool exists = false;

                for (int i = 0; i < transferQueue.Count; i++)
                {
                    if (transferQueue[i].name == item.GetComponent<Item>().GetItemId())
                    {
                        exists = true;
                        int amount = transferQueue[i].GetComponent<TransferQueueItem>().GetAmount();
                        amount += item.GetComponent<Item>().GetAmount();
                        transferQueue[i].GetComponent<TransferQueueItem>().SetUp(item, amount);
                    }
                }

                if (!exists)
                {
                    GameObject text = Instantiate(Resources.Load("UI/PickUpMessage"),GameObject.Find("InventoryTransferOverlay").transform) as GameObject;
                    text.GetComponent<TransferQueueItem>().SetUp(item, item.GetComponent<Item>().GetAmount());
                    text.name = item.GetComponent<Item>().GetItemId();
                    transferQueue.Add(text);
                }
            }

            public static void DropItem(Item item)
            {
                GameObject text;
                text = Instantiate(Resources.Load("UI/PickUpMessage")) as GameObject;
                text.transform.SetParent(GameObject.Find("InventoryTransferOverlay").transform);

                text.GetComponent<TextMeshProUGUI>().text =
                    "<gardient=\"ColorRedGradient\">+ " + item.GetAmount() + "x</gradient> "
                    + item.GetRarityGradient() + item.GetName() + "</gradient>";
            }

            public static void RemoveTransferQueueItem(GameObject item)
            {
                transferQueue.Remove(item);
            }
        }

        public void SetWeaponCooldown(float value)
        {
            equippedWeaponTimeMax = 1 / value;
            equippedWeaponTime = 1 / value;
        }
        
        public void SetWeaponCharge(bool isCharging, float value)
        {
            isChargingWeapon = isCharging;
            equippedWeaponChargeTimeMax = 1 / value;
        }

        public void SetAbilityCooldown(float value)
        {
            equippedAbilityTime = value;
        }

        public void ShowSellOverlay(bool value, string text)
        {
            sellOverlay.SetActive(value);
            sellOverlay.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Sell for : " + text + " G";
        }

        // INTERACT OVERLAYS

        public void SetChestClose(bool b)
        {
            _isNearChest = b;
        }

        public void SetPickUpClose(bool b)
        {
            _isNearItem = b;
        }
        
        //BANK

        public void SetCustomBankAmount(string input = "custom")
        {
            customBankAmount = input;
        }

        public void SetCustomBankType(string type)
        {
            customBankType = type;
        }

        public void Deposit()
        {
            if (customBankAmount != "")
            {
                _bank.DepositGold(customBankAmount);
                customBankAmount = "";
            }

            UpdatePlayerStats();
        }

        public void Withdraw()
        {
            if (customBankAmount != "")
            {
                _bank.WithdrawGold(customBankAmount);
                customBankAmount = "";
            }

            UpdatePlayerStats();
        }

        public void BankTransaction()
        {
            Debug.Log(customBankAmount + " / " + customBankType);
            if (customBankType == "deposit")
                Deposit();
            else if (customBankType == "withdraw")
                Withdraw();
        }

        public void UpdateBankStats()
        {
            if(goldBankText != null)
                goldBankText.text = "You have : <gradient=\"GoldGradient\">" + _bank.GetGold() + " G</gradient> in the bank.";
        }

        public void SetInteractText(string text = "")
        {
            if(infoPanel != null)
                infoPanel.GetComponentInChildren<TextMeshProUGUI>().text = text;
        }

        public void UpdateRoomText(string text = "")
        {
            gradeText.text = text;
            roomInfoText.text = text;
        }

        public void UpdateGradeText(string text = "")
        {
            gradeText.text = text;
        }

        public void SetUpgradeInfo()
        {
            upgradeText.text = _inventory.GetUpgradeItem().GetUpgradeText();
            upgradeItemImage.sprite = _inventory.GetUpgradeItem().GetItemIcon();
            upgradeItemImage.color = Color.white;
            
            foreach (var clearGO in upgradeRecipeList)
            {
                Debug.Log("Destroyed " + clearGO.name);
                Destroy(clearGO);
            }
            upgradeRecipeList.Clear();
            
            Recipe recipe = _inventory.GetUpgradeItem().GetRecipe();
            var goldPrice = Instantiate(recipeItemPrefab, upgradeRecipeParent.transform);
            goldPrice.GetComponentInChildren<TextMeshProUGUI>().text = recipe.GetPrice() + "x";
            upgradeRecipeList.Add(goldPrice);

            if (recipe != null)
            {
                for (int i = 0; i < recipe.GetRequiredItems().Count; i++)
                {
                    var itemObject = Instantiate(recipeItemPrefab, upgradeRecipeParent.transform);
                    itemObject.GetComponentInChildren<TextMeshProUGUI>().text = recipe.GetRequiredAmounts()[i] + "x";
                    itemObject.GetComponentInChildren<Image>().sprite = recipe.GetRequiredItems()[i].GetComponent<Item>().GetItemIcon();
                    
                    upgradeRecipeList.Add(itemObject);
                }
            }
        }

        public void UpgradeItem(string type = "anvil")
        {
            _inventory.UpgradeItem(type);
            SetUpgradeInfo();
        }

        public void Save()
        {
            DataPersistenceManager.SaveGame();
        }

        public void Load()
        {
            DataPersistenceManager.LoadGame();
        }

        public void Quit()
        {
            DataPersistenceManager.SaveGame();
            Application.Quit();
        }

        public static void SetVersionText(string text)
        {
            versionText = GameObject.Find("VersionText").GetComponent<TextMeshProUGUI>();
            versionText.text = text;
        }

        public void ActivateFinishOverlay()
        {
            finishPannel.SetActive(!finishPannel.activeSelf);
        }

        public void SetFínishOverlayText(String text)
        {
            finishInfo.text = text;
        }

        public Button GetTeleportButton()
        {
            return portalButton;
        }

        public void SwitchPlayerControl(bool value)
        {
            _playerAttack.SetOpenOverlay(value);
            _playerMovement.SetOpenUI(value);
        }

        public void SetNearPortal(bool b)
        {
            _isNearPortal = b;
        }
        
        public void UpdateStaminaBar(float value)
        {
            staminaBar.fillAmount = value;
        }
        public void LoadScene(string sceneName)
        {
            Time.timeScale = 1;
            DataPersistenceManager.SaveGame();
            SceneManager.LoadScene(sceneName);
        }
        
        public void LoadHubFromMenu()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Hub");
        }
        
        public void LoadScene(int sceneIndex)
        {
            Time.timeScale = 1;
            DataPersistenceManager.SaveGame();
            SceneManager.LoadScene(sceneIndex);
        }
        
        public void LoadHub()
        {
            LoadScene("Hub");
        }

        public void Die()
        {
            StartCoroutine(FadeOut(3));
        }
        
        public IEnumerator FadeOut(float time)
        {
            deathPanel.SetActive(true);
            deathPanel.transform.GetChild(0).GetComponent<Animator>().Play("death_text_scale");
            yield return new WaitForSeconds(time);
            Time.timeScale = 0;
        }

        public void UpdateGold()
        {
            if (goldInventoryText != null && goldOverlayText != null)
            {
                goldInventoryText.text = "<gradient=\"GoldGradient\">" + _inventory.GetGold() + " G</gradient>";
                goldOverlayText.text = "<gradient=\"GoldGradient\">" + _inventory.GetGold() + " G</gradient>";
            }
        }

        public void FinishFloor()
        {
            awardPanel.SetActive(true); 
            SwitchPlayerControl(false);
            awardText.text = "\n" + _gradingSystem.GetInfo();
            awardSprite.sprite = _gradingSystem.GetLetterSprite();
        }

        public string GetPlayerName()
        {
            return playerName.text;
        }

        public void OpenPortalMenu(bool b, string scene = "", string buttonText = "")
        {
            SwitchPlayerControl(true);
            portalPanel.SetActive(b);
            portalButton.onClick.RemoveAllListeners();
            portalButton.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
            switch (scene)
            {
                case "hub":
                    portalButton.onClick.AddListener(() => LoadHub());
                    break;
                default:
                    portalButton.onClick.AddListener(() => LoadScene(scene));
                    break;
            }
            portalButton.onClick.AddListener(() => SwitchPlayerControl(false));
        }

        public string GetNewPlayerName()
        {
            return newPlayerName.text;
        }

        public void SetNearNPC(bool b, GameObject npc, string npcType)
        {
            _nearNPC = npcType;
            _isNearNpc = b;
            
            if(npc != null && npc.TryGetComponent<NPC>(out var npcComponent))
                _npc = npcComponent;
            if(npc != null && npc.TryGetComponent<NPCShop>(out var shopComponent))
                _shop = shopComponent;
        }
        
        private void DisplayNPCPanel()
        {
            npcPanel.SetActive(true);
            npcName.text = _npc.GetName();
            StartCoroutine(TypeText(_npc.GetWelcomeText(), npcText));
            
            if(_npc.IsSpriteInverted())
                npcSprite.transform.localScale = new Vector3(-1, 1, 1);
            else
                npcSprite.transform.localScale = new Vector3(1, 1, 1);

            npcSprite.sprite = _npc.GetSprite();

            SwitchPlayerControl(true);
            foreach (var button in npcButtons)
                button.SetActive(false);
            
            if (_nearNPC == "trader")
            {
                npcButtons[0].SetActive(true);
            }
            else if (_nearNPC == "upgrader")
            {   
                npcButtons[1].SetActive(true);
            }
            else if (_nearNPC == "quester")
            {
                npcButtons[2].SetActive(true);
            }
            else if (_nearNPC == "bank")
            {
                npcButtons[3].SetActive(true);
            }
        }
        
        public void DisplayBankPanel()
        {
            miniMapPanel.SetActive(false);
            shopPanel.SetActive(false);
            upgradePanel.SetActive(false);
            npcPanel.SetActive(false);
            entrancePanel.SetActive(false);
            settingsPanel.SetActive(false);
            
            bankPanel.SetActive(!bankPanel.activeSelf);
            SwitchPlayerControl(bankPanel);
        }
        
        public void SetNearEntrance(bool b)
        {
            _isNearEntrance = b;
        }

        public void DisplayEntrancePanel()
        {
            miniMapPanel.SetActive(false);
            shopPanel.SetActive(false);
            upgradePanel.SetActive(false);
            npcPanel.SetActive(false);
            entrancePanel.SetActive(false);
            settingsPanel.SetActive(false);
            
            entrancePanel.SetActive(true);
            SwitchPlayerControl(true);
        }
    }
}