using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnnemyManager : MonoBehaviour
{
    private List<GameObject> mAllEnnemies;
    private float mMinDistanceFromPlayer = 8f;

    public void Initialize()
    {
        mAllEnnemies = new List<GameObject>();
        GameManager.mInstance.mEnnemyManager.SpawnEnnemies( 2, 4 );
    } 

    public void SpawnEnnemies( int level, int count )
    {
        GameObject area = GameObject.Find( "SpawnAreas/" + level );
        Debug.Assert( area != null );

        Rect areaBBox = Utilities.GetBBoxFromTransform( area );
        Vector3 playerPosition = GameManager.mInstance.mThePlayer.transform.position;
        Tilemap tileMapPoison = GameObject.Find( "Grid/Tilemap_Poison" ).GetComponent<Tilemap>();
 
        for( int i = 0; i < count; ++i )
        { 
            Vector3 randomPoint = new Vector3( 0, 0, 0 );
            bool isOk = false;
            while( !isOk )
            {
                randomPoint = GetRandomPoint( areaBBox );
                isOk = (playerPosition - randomPoint).magnitude > mMinDistanceFromPlayer;
                isOk = isOk && tileMapPoison.GetTile( new Vector3Int( (int)randomPoint.x, (int)randomPoint.y, 0 ) ) == null;
            }

            GameObject ennemyPrefab = Resources.Load<GameObject>("Prefabs/Ennemies/EnemyBasic");
            GameObject ennemy = GameObject.Instantiate( ennemyPrefab, randomPoint, Quaternion.Euler(0, 0, 0) );

            mAllEnnemies.Add( ennemy.gameObject );
        }
    }


    public void DestroyAllEnnemies()
    {
        foreach ( GameObject item in mAllEnnemies )
        {
            GameObject.Destroy( item );
        }
    }


    private Vector3 GetRandomPoint( Rect inside )
    {
        float randomX = Random.Range( inside.xMin, inside.xMax );
        float randomY = Random.Range( inside.yMin, inside.yMax );
        return  new Vector3( randomX, randomY, -1 );
    }
}
