using UnityEngine;
using System.Collections;
using Assets.Scripts.Game;
using System.Linq;

namespace Assets.Scripts.ShapeBasket
{
    public class BasketBuilder : MonoBehaviour
    {

        [System.Serializable]
        public class Blocks
        {
            public GameObject tile;
            public GameObject indicator;
        }

        [System.Serializable]
        public class TilesRow
        {
            [Range(1,5)]
            public int tiles = 1;
            public TilesRow(int tiles)
            {
                this.tiles = tiles;
            }
        }

        [System.Serializable]
        public class BaseLine
        {
            public float xStart, xEnd, y, z;

            public Vector3 Start
            {
                get { return new Vector3(xStart, y, z); }
            }

            public Vector3 End
            {
                get { return new Vector3(xEnd, y, z); }
            }
        }

        [System.Serializable]
        public struct BuildSpace
        {
            [SerializeField]
            private BaseLine bottom;
            [SerializeField]
            private float height;
            [SerializeField]
            private float indicatorHeight;
            [SerializeField]
            private Vector3 indicatorOffset;
            [SerializeField]
            private Vector3 shapeOffset;

            public Vector3 BottomStart
            {
                get { return bottom.Start; }
            }
            public Vector3 BottomEnd
            {
                get { return bottom.End; }
            }

            public Vector3 IndicatorOffset
            {
                get { return indicatorOffset; }
            }

            public Vector3 IndicatorStart
            {
                get
                {
                    var p = bottom.Start;
                    p.y += height + indicatorHeight;
                    return p;
                }
            }

            public Vector3 IndicatorEnd
            {
                get
                {
                    var p = bottom.End;
                    p.y += height + indicatorHeight;
                    return p;
                }
            }

            public Vector3 TopStart
            {
                get
                {
                    var p = bottom.Start;
                    p.y += height;
                    return p;
                }
            }

            public Vector3 TopEnd
            {
                get
                {
                    var p = bottom.End;
                    p.y += height;
                    return p;
                }
            }

            public Vector3 Center
            {
                get
                {
                    var center = Vector3.Lerp(BottomStart, BottomEnd, 0.5f);
                    center.y = bottom.y + height / 2;
                    return center;
                }
            }

            public float Height
            {
                get { return height; }
            }

            public float IndicatorHeight
            {
                get { return height + indicatorHeight; }
            }

            public Vector3 IndicatorScale
            {
                get
                {
                    return new Vector3((bottom.xEnd - bottom.xStart)*2, height + 2 * indicatorHeight, bottom.z);
                }
            }

            public Vector3 Scale
            {
                get
                {
                    return new Vector3(bottom.xEnd - bottom.xStart, height, bottom.z);
                }
            }
        }

        [SerializeField]
        private Blocks blocks;
        public BuildSpace buildSpace;
        public TilesRow[] rows = new TilesRow[0];
        


        private GameObject[] tiles = new GameObject[0];
        private GameObject[] tilesIndicators = new GameObject[0];
        private bool isReady = false;
        private Coroutine rebuildCoroutine;

        private Vector3 PointToLossyScale(Vector3 p)
        {
            p.Scale(transform.lossyScale);
            return p;
        }
        
        private Vector3 LocalToGlobal(Vector3 p)
        {
            return transform.position + PointToLossyScale(p);
        }

        private Vector3 GetBaseLineStartPosition()
        {
            return LocalToGlobal(buildSpace.BottomStart);
        }

        private Vector3 GetBaseLineEndPosition()
        {
            return LocalToGlobal(buildSpace.BottomEnd);
        }

        private void OnDrawGizmos()
        {


            Gizmos.color = Color.white;
            Vector3 bottomS = GetBaseLineStartPosition();
            Vector3 bottomE = GetBaseLineEndPosition();
            Vector3 topS = LocalToGlobal(buildSpace.TopStart);
            Vector3 topE = LocalToGlobal(buildSpace.TopEnd);
            Vector3 iTopS = LocalToGlobal(buildSpace.IndicatorStart);
            Vector3 iTopE = LocalToGlobal(buildSpace.IndicatorEnd);
            Vector3 center = LocalToGlobal(buildSpace.Center);
            Gizmos.DrawWireSphere(bottomS, 0.1f);
            Gizmos.DrawWireSphere(bottomE, 0.1f);
            Gizmos.DrawWireSphere(topS, 0.1f);
            Gizmos.DrawWireSphere(topE, 0.1f);
            Gizmos.DrawWireSphere(iTopS, 0.1f);
            Gizmos.DrawWireSphere(iTopE, 0.1f);
            Gizmos.DrawWireSphere(center, 0.1f);
            Gizmos.DrawLine(iTopS, iTopE);
            Gizmos.DrawLine(iTopE, topE);
            Gizmos.DrawLine(iTopS, topS);

            Vector3 startingPosition = bottomS;
            Vector3[][] tilesPositions = new Vector3[rows.Length][];
            for (int i = 0; i < tilesPositions.Length; i++)
            {
                tilesPositions[i] = CreateRowPositions(startingPosition, rows[i]);
                startingPosition.y += GetTileLossyScale(rows[i]).y;
            }

            Gizmos.color = Color.blue;
            for (int i = 0; i < tilesPositions.Length; i++)
            {
                var tiles = tilesPositions[i];
                var scale = GetTileLossyScale(rows[i]);
                foreach (var tile in tiles)
                {
                    Gizmos.DrawWireCube(tile, scale);
                }
            }

        }

        private Vector3 GetScaledIndicatorOffset()
        {
            var indicatorOffset = this.buildSpace.IndicatorOffset;
            indicatorOffset.Scale(transform.lossyScale);
            return indicatorOffset;
        }

        private GameObject CreateTileIndicator(string name, Vector3 position, Vector3 enabledScale, Vector3 disabledScale)
        {
            var indicator = Instantiate(blocks.indicator, transform);
            indicator.transform.position = position;
            indicator.name = name;
            indicator.transform.localScale = disabledScale;
            var basketIndicator = indicator.GetComponent<BasketIndicator>();
            basketIndicator.enabledScale = enabledScale;
            basketIndicator.disabledScale = disabledScale;
            indicator.SetActive(true);
            return indicator;
        }

        private GameObject CreateTile(string name, Vector3 position, Vector3 localScale)
        {
            var tile = Instantiate(blocks.tile, transform);

            tile.transform.position = position;
            tile.transform.localScale = localScale;

            tile.SetActive(true);
            tile.name = name;
            return tile;
        }

        private Vector3[] CreateRowPositions(Vector3 startingPosition, TilesRow row)
        {
            Vector3[] tiles = new Vector3[row.tiles];
            Vector3 scale = GetTileLossyScale(row);

            startingPosition.x -= scale.x / 2;
            startingPosition.y += scale.y / 2;

            for (int i = 0; i < tiles.Length; i++)
            {
                startingPosition.x += scale.x;
                tiles[i] = startingPosition;
            }

            return tiles;
        }


        private Vector3 GetTileScale(TilesRow row)
        {
            var scale = buildSpace.Scale;
            scale.x /= row.tiles;
            scale.y /= rows.Length;
            return scale;
        }

        private Vector3 GetTileLossyScale(TilesRow row)
        {
            var scale = GetTileScale(row);
            scale.Scale(transform.lossyScale);
            return scale;
        }

        private Vector3 GetTopTilePosition()
        {
            return tiles[tiles.Length - 1].transform.position;
        }

        private Vector3 GetIndicatorPosition(GameObject tile)
        {
            Vector3 position = tile.transform.position;
            position.y = GetTopTilePosition().z;
            return position;
        }


        public void Rebuild()
        {
            if (rebuildCoroutine == null)
                rebuildCoroutine = StartCoroutine(RebuildCoroutine());
        }


        public IEnumerator RebuildCoroutine()
        {
            while (ColorsPresetsManager.Instance == null || !ColorsPresetsManager.Instance.IsReady)
                yield return null;

            isReady = false;

            foreach(var tile in this.tiles)
            {
                Destroy(tile);
            }

            foreach(var tileIndicator in this.tilesIndicators)
            {
                Destroy(tileIndicator);
            }

            int newTilesNumber = 0;
            foreach(var row in rows)
            {
                newTilesNumber += row.tiles;
            }

            GameObject[] tiles = new GameObject[newTilesNumber];
            GameObject[] tilesIndicators = new GameObject[newTilesNumber];

            Vector3 startingPosition = GetBaseLineStartPosition();
            Vector3 center = LocalToGlobal(buildSpace.Center);
            for(int i = 0, t = 0; i < rows.Length; i++)
            {
                Vector3[] positions = CreateRowPositions(startingPosition, rows[i]);
                Vector3 scale = GetTileScale(rows[i]);
                for (int j = 0; j < positions.Length; j++, t++)
                {
                    var mat = ColorsPresetsManager.Instance.Materials.basketMain[t];
                    var tileName = System.String.Format("Tile[{0}][{1}]", i + 1, j + 1);
                    var indicatorName = System.String.Format("Indicator[{0}][{1}]", i + 1, j + 1);
                    var indicatorPosition = positions[j] + GetScaledIndicatorOffset() * (t + 1);
                    indicatorPosition.y = center.y;

                    tiles[t] = CreateTile(tileName, positions[j], scale);
                    tilesIndicators[t] = CreateTileIndicator(indicatorName, indicatorPosition, buildSpace.IndicatorScale , scale);
                    
                    tiles[t].GetComponent<MeshRenderer>().sharedMaterial = mat;
                    tilesIndicators[t].GetComponent<MeshRenderer>().sharedMaterial = mat;

                }
                scale.Scale(transform.lossyScale);
                startingPosition.y += scale.y;
            }
            this.tiles = tiles;
            this.tilesIndicators = tilesIndicators;
            isReady = true;
            //Debug.Log("Basket SET READY!");
            rebuildCoroutine = null;
        }

        public GameObject[] Tiles
        {
            get { return tiles; }
        }

        public GameObject[] TilesIndicators
        {
            get { return tilesIndicators; }
        }

        public bool IsReady
        {
            get { return isReady; }
        }

    }
}
