using UnityEngine;

[CreateAssetMenu(fileName = "DungeonSettings", menuName = "Dungeon/CreateSettings")]
public class DungeonSettings : ScriptableObject
{
    public int seed;
    public int dungeonNumber;
}
