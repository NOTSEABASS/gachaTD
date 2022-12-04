using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuConsts.ScriptableObject +"Enemy Spawn")]
public class EnemySpawnResources : ScriptableObject
{
   public List<EnemySpawnBatch> batches = new List<EnemySpawnBatch>();

}
