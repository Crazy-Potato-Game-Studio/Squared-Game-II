using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace LevelBuilder
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(FloorHandler))]
    [RequireComponent(typeof(PlatformHandler))]
    [RequireComponent(typeof(ClimbableHandler))]
    [RequireComponent(typeof(DecorationHandler))]
    [RequireComponent(typeof(EnemyHandler))]
    [RequireComponent(typeof(InteractableHandler))]
    [RequireComponent(typeof(ItemHandler))]
    [RequireComponent(typeof(OtherHandler))]
    public class LevelCreateManager : MonoBehaviour
    {
        public static Action EscButtonEvent;
        public static Action<LevelEditorItem> ItemSelectEvent;
        public static Action OverUiEvent;
        public static LevelCreateManager Singleton;
        public PlatformHandler PlatformHandler { get; private set; }
        public FloorHandler FloorHandler { get; private set; }
        public ClimbableHandler ClimbableHandler { get; private set; }
        public DecorationHandler DecorationHandler  { get; private set; }
        public EnemyHandler EnemyHandler  { get; private set; }
        public InteractableHandler InteractableHandler  { get; private set; }
        public ItemHandler ItemHandler  { get; private set; }
        public OtherHandler OtherHandler  { get; private set; }
        [field:SerializeField]public LevelEditorItem selectedItem { get; private set; }
        [Header("Grid")]
        public Camera editCamera;
        public Grid grid;
        public bool itemPlaceable { get; private set; }
        EventSystem eventSystem;
        public Vector3Int gridPos { get; private set; }
        public  UnityEvent escButtonEvent;

        public UiDoorPopUp popUp;
        private void Awake()
        {
            Singleton = this;
            InitializeScene();
            eventSystem = FindObjectOfType<EventSystem>();
        }
        private void InitializeScene()
        {
            PlatformHandler = GetComponent<PlatformHandler>();
            FloorHandler = GetComponent<FloorHandler>();
            ClimbableHandler = GetComponent<ClimbableHandler>();
            DecorationHandler = GetComponent<DecorationHandler>();
            EnemyHandler = GetComponent<EnemyHandler>();
            InteractableHandler = GetComponent<InteractableHandler>();
            ItemHandler = GetComponent<ItemHandler>();
            OtherHandler = GetComponent<OtherHandler>();
        }
        private void Start()
        {
            if (FindObjectOfType<SceneManagement>())
            {
                Destroy(FindObjectOfType<SceneManagement>().gameObject);
            }
        }
        public void SelectItem(LevelEditorItem sandBoxItem)
        {
            selectedItem = sandBoxItem;
            GridCursor.Singleton.DisplayCursor(selectedItem);
            ItemSelectEvent?.Invoke(selectedItem);
        }

        private void Update()
        {
            gridPos = grid.WorldToCell(editCamera.ScreenToWorldPoint(Input.mousePosition));
            if (Input.GetKeyDown(KeyCode.Escape)) { EscButtonEvent?.Invoke(); escButtonEvent?.Invoke(); }
            if (eventSystem.IsPointerOverGameObject()) { GridCursor.Singleton.gridCursorImage.enabled = false; OverUiEvent?.Invoke(); return; }

            if (InteractableHandler.childDictionary.TryGetValue(TileHandler.GetTileKey(gridPos.x, gridPos.y), out var parentKey))
            {
                InteractableHandler.interactableDictionary.TryGetValue(parentKey, out var property);
                LevelEditorItem item = ItemManager.Singleton.GetItemDetails(property.id, ItemCategory.Interactable);
                if (item.extra.propertyType == PropertyType.Door)
                {
                    popUp.ShowDoorName("Door [" + property.key+ "]");
                }
                else if (item.extra.propertyType == PropertyType.Portal)
                {
                    popUp.ShowDoorName("Portal [" + property.key + "]");
                }
                else
                {
                    popUp.gameObject.SetActive(false);
                }

                if (item.extra.propertyType == PropertyType.Trigger)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        InteractableHandler.powerTriggerManager.ShowTrigger(InteractableHandler.interactableDictionary, property);
                    }
                }
            }
            else
            {
                popUp.gameObject.SetActive(false);
            }


            if (selectedItem != null)
            {
                GridCursor.Singleton.gridCursorImage.enabled = true;
                itemPlaceable = CheckValidTile();
            }
        }
        private bool CheckValidTile()
        {
            switch (selectedItem.validationType)
            {
                case ValidationType.Floor:
                    return FloorHandler.ValidationCheckForFloor(selectedItem, gridPos);
                case ValidationType.Platform:
                    return PlatformHandler.ValidationCheckForPlatform(selectedItem, gridPos);
                case ValidationType.Climbable:
                    return ClimbableHandler.ValidationCheckForClimbable(selectedItem, gridPos);
                case ValidationType.Decoration:
                    return DecorationHandler.ValidationCheckForDecoration(selectedItem, gridPos);
                case ValidationType.Enemy:
                    return EnemyHandler.ValidationCheckForEnemy(selectedItem, gridPos);
                case ValidationType.Interactable:
                    return InteractableHandler.ValidationCheckForInteractableObject(selectedItem, gridPos);
                case ValidationType.Items:
                    return ItemHandler.ValidationCheckForIItem(selectedItem, gridPos);
                case ValidationType.Other:
                    return OtherHandler.ValidationCheckForOthers(selectedItem, gridPos);
                default: return false;
            }
        }

        public static string GetTileKey(Vector3Int pos) => "X" + pos.x + "Y" + pos.y;
        public static string GetTileKey(int x,int y) => "X" + x + "Y" + y;
    }

}