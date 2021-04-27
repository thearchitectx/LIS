using UnityEngine;

public class TransformNoise : MonoBehaviour
{
        public float PerlinScale = 4.56f;
        public float WaveSpeed = 1f;
        public float WaveHeight = 2f;

        public Vector3 AxisEffect = Vector3.one;

        void LateUpdate()
        {
            Vector3 pos = transform.position;
        
            float pX = ( pos.x * PerlinScale ) + ( Time.timeSinceLevelLoad * WaveSpeed );
            float pZ = ( pos.z * PerlinScale ) + ( Time.timeSinceLevelLoad * WaveSpeed );
            float pY = ( pos.y * PerlinScale ) + ( Time.timeSinceLevelLoad * WaveSpeed );
        

            pos.x += ( Mathf.PerlinNoise( pY, pZ ) - 0.5f ) * WaveHeight * AxisEffect.x;
            pos.y += ( Mathf.PerlinNoise( pX, pZ ) - 0.5f ) * WaveHeight * AxisEffect.y;
            pos.z += ( Mathf.PerlinNoise( pX, pY ) - 0.5f ) * WaveHeight * AxisEffect.z;
        
            transform.position = pos;
        }
}