using UnityEngine;
using System.Collections;
using Assets.Scripts.Shape;
using Assets.Scripts.Indicator;

namespace Assets.Scripts.ShapeBasket
{
    
    public class ShapeCatcher : MonoBehaviour
    {



        public GameMode mode = GameMode.None;

        public GameObject particleEffect = null;

        public System.Action<bool> OnCatch;

        public Vector3 GetParticleEffectPosition(Collider shape, Collider basket)
        {
            return new Vector3(shape.bounds.center.x, basket.bounds.center.y, shape.bounds.center.z);
        }

        public void OnTriggerEnter(Collider shape)
        {
            //Debug.Log(shape.tag);

            if (shape.tag != "Shape" && shape.tag != "Bonus")
                return;

            GameObject tile = GetComponent<TileSwitcher>().Tile;
            Collider shapeBasket = GetComponent<Collider>();

            //Debug.Log("TILE:" + tile.name +" ID:"+tile.GetInstanceID());
            ShapeType tileShapeType = GetComponentInChildren<ShapeMixer>().tileToShape[tile].GetComponent<ShapeToTile>().shapeType;
            ShapeType shapeShapeType = shape.gameObject.GetComponentInParent<RandomRotation>().CurrentRotation.type;

            Color shapeColor = shape.GetComponentInParent<ColorPicker>().GetColor();
            Color tileColor = tile.GetComponent<MeshRenderer>().sharedMaterial.color;

            //Debug.Log("shape color:" + shapeColor + " tile color:" + tileColor);

            bool match = mode == GameMode.Color && tileColor == shapeColor || mode == GameMode.Shape && tileShapeType == shapeShapeType;

            if(shape.tag == "Shape")
            {
                OnCatch(match);
            }

            shape.GetComponentInParent<Destruction>().StartDestruction();
            CatcherParticleEffect catcherParticleEffect = particleEffect.GetComponent<CatcherParticleEffect>();
            catcherParticleEffect.shapeShapeType = shapeShapeType;
            catcherParticleEffect.tileShapeType = tileShapeType;
            catcherParticleEffect.shapeColor = shapeColor;
            catcherParticleEffect.tileColor = tileColor;
            Instantiate(catcherParticleEffect.gameObject, GetParticleEffectPosition(shape, shapeBasket), Quaternion.identity).SetActive(true);

        }
    }
}


