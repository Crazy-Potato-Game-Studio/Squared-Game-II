using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LevelBuilder
{
    public class CameraEdit : MonoBehaviour
    {
        [SerializeField] Camera _camera;
        [SerializeField] float zoomStep, minZoom, maxZoom;
        Vector3 dragOrigin;
        EventSystem eventSystem;
        private void Awake()
        {
            eventSystem = EventSystem.current;
        }
        private void Update()
        {
            if (Input.mouseScrollDelta.y != 0 && !eventSystem.IsPointerOverGameObject())
                _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize -= Input.mouseScrollDelta.y * zoomStep, minZoom, maxZoom);
        }



        private void LateUpdate() => PanCamera();
        private void PanCamera()
        {
            if (Input.GetMouseButtonDown(1)) { dragOrigin = _camera.ScreenToWorldPoint(Input.mousePosition); }

            if (Input.GetMouseButton(1))
            {
                Vector3 distance = dragOrigin - _camera.ScreenToWorldPoint(Input.mousePosition);
                _camera.transform.position += distance;
            }
        }
    }
}

