using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public enum Tile { floor }

    [Header("Prefabs")]
    public GameObject FloorPrefab;
    public GameObject WallPrefab;

    [Header("DungeonSettings")]
    public int amountRooms;
    public int width = 100;
    public int height = 100;
    public int minRoomSize = 3;
    public int maxRoomSize = 7;

    private Dictionary<Vector2Int, Tile> dungeonDictionary = new Dictionary<Vector2Int, Tile>();
    private List<Room> roomList = new List<Room>();
    private List<GameObject> allSpawnedObjects = new List<GameObject>();

    void Start()
    {
        
    }

    private void AllocateRooms()
    {
        for (int i = 0; i < amountRooms; i++)
        {
            Room room = new Room()
            {
                Position = new Vector2Int(Random.Range(0, width), Random.Range(0, height)),
                size = new Vector2Int(Random.Range(minRoomSize, maxRoomSize), Random.Range(minRoomSize, maxRoomSize))
            };

            if (CheckIfRoomFitsDungeon(room))
            {
                AddRoomToDungeon(room);
            }
            else
            {
                i--;
            }
        }
    }

    private void AddRoomToDungeon(Room room)
    {
        for (int xx = room.position.x; xx < room.position.x + room.size.x; xx++)
        {
            for (int yy = room.position.y; yy < room.position.y + room.size.y; yy++)
            {
                Vector2Int pos = new Vector2Int(xx, yy);
                dungeonDictionary.Add(pos, Tile.floor);
            }
        }
        roomList.Add(room);
    }

    private bool CheckIfRoomFitsDungeon(Room room)
    {
        for (int xx = room.position.x; xx < room.position.x + room.size.x; xx++)
        {
            for (int yy = room.position.y; yy < room.position.y + room.size.y; yy++)
            {
                Vector2Int pos = new Vector2Int(xx, yy);
                if (dungeonDictionary.ContainsKey(pos))
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void AllocateCorridors()
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            Room startRoom = roomList[i];
            Room otherRoom = roomList[(i + Random.Range(1, roomList.Count - 1)) % roomList.Count];

            int dirX = Mathf.RoundToInt(Mathf.Sign(otherRoom.position.x - startRoom.position.x));
            for (int x = startRoom.position.x; x != otherRoom.position.x; x += dirX)
            {
                Vector2Int pos = new Vector2Int(x, startRoom.position.y);
                if (!dungeonDictionary.ContainsKey(pos))
                {
                    dungeonDictionary.Add(pos, Tile.floor);
                }
            }
            
            int dirY = Mathf.RoundToInt(Mathf.Sign(otherRoom.position.y - startRoom.position.y));
            for (int y = startRoom.position.y; y != otherRoom.position.y; y += dirY)
            {
                Vector2Int pos = new Vector2Int(otherRoom.position.x, y);
                if (!dungeonDictionary.ContainsKey(pos))
                {
                    dungeonDictionary.Add(pos, Tile.floor);
                }
            }
        }
    }

    private void BuildDungeon()
    {

    }

    private void SpawnWallsForTile(Vector2Int Position)
    {

    }

    public void GenerateDungeon()
    {

    }
}

public class Room
{
    public Vector2Int position;
    public Vector2Int size;
}
