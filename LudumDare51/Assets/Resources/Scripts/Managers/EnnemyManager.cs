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
    } 

    public void SpawnEnnemies( int level, int basicCount, int shooterCount )
    {
        GameObject area = GameObject.Find( "SpawnAreas/" + level + "-Ennemies" );
        Debug.Assert( area != null );

        Rect areaBBox = Utilities.GetBBoxFromTransform( area );
 
        for( int i = 0; i < basicCount; ++i )
        { 
            SpawnStandard( GetValidRandomPointInArea( areaBBox ) );
        }

        for( int i = 0; i < shooterCount; ++i )
        { 
            SpawnShooter( GetValidRandomPointInArea( areaBBox ) );
        } 
    }


    private void SpawnStandard( Vector3 position )
    {
        GameObject ennemyPrefab = Resources.Load<GameObject>("Prefabs/Ennemies/EnemyBasic");
        GameObject ennemy = GameObject.Instantiate( ennemyPrefab, position, Quaternion.Euler(0, 0, 0) );
        ennemy.GetComponent<Enemy>().Initialize();

        mAllEnnemies.Add( ennemy.gameObject );
    }


    private void SpawnShooter( Vector3 position )
    {
        GameObject ennemyPrefab = Resources.Load<GameObject>("Prefabs/Ennemies/EnemyShooter");
        GameObject ennemy = GameObject.Instantiate( ennemyPrefab, position, Quaternion.Euler(0, 0, 0) );
        ennemy.GetComponent<EnnemyShooterIA>().Initialize();
        ennemy.GetComponent<Enemy>().Initialize();

        mAllEnnemies.Add( ennemy.gameObject );
    }


    private Vector3 GetValidRandomPointInArea( Rect area )
    {
        Vector3 playerPosition = GameManager.mInstance.mThePlayer.transform.position;
        Tilemap tileMapPoison = GameObject.Find( "Grid/Tilemap_Poison" ).GetComponent<Tilemap>();

        Vector3 randomPoint = new Vector3( 0, 0, 0 );
        bool isOk = false;
        while( !isOk )
        {
            randomPoint = GetRandomPoint( area );
            isOk = (playerPosition - randomPoint).magnitude > mMinDistanceFromPlayer;
            isOk = isOk && tileMapPoison.GetTile( new Vector3Int( (int)randomPoint.x, (int)randomPoint.y, 0 ) ) == null;
        }

        return  randomPoint;
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
