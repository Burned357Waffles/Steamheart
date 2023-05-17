using System;
using Hex;
using UnityEngine;

namespace UI.HUD
{
    public class HexHighlight : MonoBehaviour
    {
        [SerializeField] public GameObject highlighter;

        private HexGrid _hexGrid;
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
            _hexGrid = FindObjectOfType<HexGrid>();
            highlighter.SetActive(true);
        }

        private void Update()
        {
            Ray ray = _camera!.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit)) return;
            if (!hit.transform.CompareTag("Hex")) 
                if (!hit.transform.CompareTag("City") )
                    if (!hit.transform.CompareTag("Unit"))
                    {
                        highlighter.SetActive(false);
                        if (hit.transform.gameObject.layer == LayerMask.NameToLayer("UI"))
                        {
                            Debug.Log("inner if");
                            highlighter.SetActive(false);
                            return;
                        }

                        return;
                    }
            if (hit.transform.CompareTag("Unit") || hit.transform.CompareTag("City"))
            {
                Debug.Log(hit.transform.position);
            }
            
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("UI"))
            {
                highlighter.SetActive(false);
                return;
            }
            highlighter.SetActive(true);
            
            int currentHexIndex = _hexGrid.GetHexIndexAtWorldPos(hit.transform.position);
            

            Hex.Hex hex = _hexGrid.GetHexList()[currentHexIndex];

            highlighter.transform.position = hex.WorldPosition;
        }
    }
}