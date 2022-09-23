using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LevelEditor
{
    public class InGameEditor : MonoBehaviour
    {
        public Tilemap frontMap;

        public TileBase currentTile;   

        public TileBase[] tiles;

        public GameObject HighLight;

        public Sprite HighLightSprite;

        private Vector3Int MousePosition
        {
            get
            {
                // Get camera world point.
                var clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
               
                // Convert world point to tilemap point.
                var tilemapPos = new Vector3Int(Mathf.FloorToInt(clickPos.x), Mathf.FloorToInt(clickPos.y), 0);
              
                return tilemapPos;
            }
        }

        private void Start()
        {
            if (HighLight == null)
            {
                HighLight = new GameObject("HighLightObject", typeof(SpriteRenderer));
            }

            var spriteRenderer = HighLight.GetComponent<SpriteRenderer>() ?? HighLight.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = HighLightSprite;
            spriteRenderer.sortingOrder = 100;
        }

        void Update()
        {
            // Place tiles.
            if (Input.GetButtonDown("Fire1"))         
                InvokeRepeating(nameof(PlaceTile), 0, Time.deltaTime);             
            if (Input.GetButtonUp("Fire1"))
                CancelInvoke(nameof(PlaceTile));

            // Remove tiles.
            if (Input.GetButtonDown("Fire2"))
                InvokeRepeating(nameof(RemoveTile), 0, Time.deltaTime);
            if (Input.GetButtonUp("Fire2"))
                CancelInvoke(nameof(RemoveTile));


            if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.LeftControl)) 
                SaveLevel();

            if (Input.GetKey(KeyCode.L) && Input.GetKey(KeyCode.LeftControl))
                LoadLevel();

            HighLight.transform.position = MousePosition;
        }

        private void SaveLevel()
        {
            var TileData = new FileManagement.TileData();
            TileData.tileName = currentTile.name;

            var tiles = new List<Vector3Int>();

            // Get the width of the camera.
            var screenMax = new Vector2(12, 12);

            for (int x = -(int)screenMax.x; x < (int)screenMax.x; x++)
            {
                for (int y = -(int)screenMax.y; y < (int)screenMax.y; y++)
                {
                    var pos = (new Vector3Int(x, y, 0));
                    var tile = frontMap.GetTile(pos);

                    if (tile != null)                   
                        tiles.Add(pos);                    
                }
            }

            TileData.positions = new Vector2Int[tiles.Count];
            for (int i = 0; i < TileData.positions.Length; i++)
            {
                TileData.positions[i] = (Vector2Int)tiles[i];
            }

            FileManagement.SaveManager.SaveLevel(TileData);
        }

        private void LoadLevel()
        {
            var Data = FileManagement.SaveManager.LoadLevel();

            var tileToPlace = currentTile;
            foreach (var tile in tiles)
            {
                if (tile.name.ToLower() == Data.tileName.ToLower())
                {
                    tileToPlace = tile;
                }
            }

            foreach (var pos in Data.positions)
            {
                frontMap.SetTile(new Vector3Int(pos.x, pos.y, 0), tileToPlace);
            }
        }

        private void PlaceTile()
        {         
            // Place tile at world point.
            frontMap.SetTile(MousePosition, currentTile);
        }

        private void RemoveTile()
        {
            // Remove a tile from the world point.
            frontMap.SetTile(MousePosition, null);
        }
    }
}