using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

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
        public Transform cursorObjectParent;
        public GameObject[] disableGameObjectOnScreenShot;
        public static Action EscButtonEvent;
        public static Action<LevelEditorItem> ItemSelectEvent;
        public static Action OverUiEvent;
        public static LevelCreateManager Singleton;
        public PlayableMaker playableMaker { get; private set; }
        public PlatformHandler PlatformHandler { get; private set; }
        public FloorHandler FloorHandler { get; private set; }
        public ClimbableHandler ClimbableHandler { get; private set; }
        public DecorationHandler DecorationHandler  { get; private set; }
        public EnemyHandler EnemyHandler  { get; private set; }
        public InteractableHandler InteractableHandler  { get; private set; }
        public ItemHandler ItemHandler  { get; private set; }
        public OtherHandler OtherHandler  { get; private set; }
        [field:SerializeField] public LevelEditorItem selectedItem { get; private set; }
        [Header("Grid")]
        public Camera editCamera;
        public Grid grid;
        public bool itemPlaceable { get; private set; }
        EventSystem eventSystem;
        public Vector2Int gridPos { get; private set; }
        public UnityEvent escButtonEvent;

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
            playableMaker = GetComponent<PlayableMaker>();
        }
        private void Start()
        {
            if (FindObjectOfType<SceneManagement>())
            {
                Destroy(FindObjectOfType<SceneManagement>().gameObject);
            }
            SaveLoadManager.Singleton.objectToHideOnScreenshot = disableGameObjectOnScreenShot;
            SaveLoadManager.Singleton.OnLoad += OnLevelLoad;
            SaveLoadManager.Singleton.Load();
        }
        private void OnLevelLoad(SaveData saveData)
        {
            if (saveData.saveInfo.floatValues == null) { return; }

            Dictionary<string, float> floatValues = saveData.saveInfo.floatValues;
            Camera camera = Camera.main;
            floatValues.TryGetValue("camPosX", out float camPosX);
            floatValues.TryGetValue("camPosY", out float camPosY);
            camera.transform.position = new(camPosX, camPosY,-10);
        }
        public void SaveLevel()
        {
            Dictionary<string, float> floatValues = new();
            Camera camera = Camera.main;
            floatValues.Add("camPosX", camera.transform.position.x);
            floatValues.Add("camPosY", camera.transform.position.y);
            ItemSelectEvent?.Invoke(null);
            SaveLoadManager.Singleton.currentlyLoadedSaveData.saveInfo.floatValues = floatValues;
            SaveLoadManager.Singleton.Save();
            ItemSelectEvent?.Invoke(ItemManager.Singleton.selectedItem);
        }
        public void PlayLevel()
        {
            PlayableMaker.Singleton.Play();
        }
       
        public void SelectItem(LevelEditorItem sandBoxItem)
        {
            selectedItem = sandBoxItem;
            ItemSelectEvent?.Invoke(selectedItem);
        }

        private void Update()
        {
            Vector3Int cellPos = grid.WorldToCell(editCamera.ScreenToWorldPoint(Input.mousePosition));
            gridPos = new(cellPos.x, cellPos.y);

            if (Input.GetKeyDown(KeyCode.Escape)) { escButtonEvent?.Invoke(); }

            if (eventSystem.IsPointerOverGameObject())
            {
                GridCursor.Singleton.SetVisibility(false);
                OverUiEvent?.Invoke();
                return;
            }

            if(selectedItem == null) { return; }

            GridCursor.Singleton.SetVisibility(true);
            itemPlaceable = CheckValidTile();
            if (itemPlaceable)
            {
                GridCursor.Singleton.gridCursorImage.color = GridCursor.Singleton.cursorValidColor;
            }
            else
            {
                GridCursor.Singleton.gridCursorImage.color = GridCursor.Singleton.cursorInvalidColor;
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
        public static string GetTileKey(Vector2Int pos) => "X" + pos.x + "Y" + pos.y;
        public static string GetTileKey(int x,int y) => "X" + x + "Y" + y;
    }

}