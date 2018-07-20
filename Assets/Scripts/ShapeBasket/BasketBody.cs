using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Assets.Scripts.Shape;


namespace Assets.Scripts.ShapeBasket
{
    public class BasketBody : MonoBehaviour
    {
        private static readonly int MIN_NUMBER_OF_TILES = 2;
        private static readonly int MAX_NUMBER_OF_TILES = 10;
        private static readonly int MAX_TILES_IN_ROW = 5;
        [SerializeField]
        private ShapeMixer shapeMixer;
        [SerializeField]
        private BasketBuilder basketBuilder;
        [SerializeField]
        private TileSwitcher tileSwitcher;
        [SerializeField]
        private ColorsRandomizer colorsRandomizer;
        [SerializeField]
        private BasketBuilder.BuildSpace oneRow;
        [SerializeField]
        private BasketBuilder.BuildSpace twoRows;
        [SerializeField]
        private BoxCollider boxCollider;
        [SerializeField]
        private Vector3 oneRowColliderCenter = Vector3.zero, twoRowsColliderCenter = Vector3.zero;

        private int numberOfTiles = -1;

        public void SetShapes(ShapeType[] shapes)
        {
            var numberOfTiles = Mathf.Clamp(shapes.Length, MIN_NUMBER_OF_TILES, MAX_NUMBER_OF_TILES);

            if (numberOfTiles != this.numberOfTiles)
            {
                this.numberOfTiles = numberOfTiles;
                BasketBuilder.BuildSpace space;
                List<BasketBuilder.TilesRow> rows = new List<BasketBuilder.TilesRow>();
                switch (numberOfTiles)
                {
                    case 7:
                        rows.Add(new BasketBuilder.TilesRow(4));
                        rows.Add(new BasketBuilder.TilesRow(3));
                        space = twoRows;
                        boxCollider.center = twoRowsColliderCenter;
                        break;
                    default:
                        if (numberOfTiles > MAX_TILES_IN_ROW)
                        {

                            space = twoRows;
                            if (numberOfTiles % 2 != 0)
                            {
                                rows.Add(new BasketBuilder.TilesRow(MAX_TILES_IN_ROW));
                                rows.Add(new BasketBuilder.TilesRow(numberOfTiles - MAX_TILES_IN_ROW));
                            }
                            else
                            {
                                rows.Add(new BasketBuilder.TilesRow(numberOfTiles / 2));
                                rows.Add(new BasketBuilder.TilesRow(numberOfTiles / 2));
                            }

                            boxCollider.center = twoRowsColliderCenter;
                        }
                        else
                        {
                            space = oneRow;
                            rows.Add(new BasketBuilder.TilesRow(numberOfTiles));
                            boxCollider.center = oneRowColliderCenter;
                        }
                        break;
                }
                basketBuilder.buildSpace = space;
                basketBuilder.rows = rows.ToArray();
                basketBuilder.Rebuild();
            }

            shapeMixer.ShapeTypes = shapes;
            shapeMixer.Mix(true);
            tileSwitcher.Reset();
            colorsRandomizer.Reset();
        }

    }
}
