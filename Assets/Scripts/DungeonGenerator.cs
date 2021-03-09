using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace roguelike.generator
{
    public class DungeonGenerator : MonoBehaviour
    {
        public enum Tile { floor }

        [Header("Prefabs")]
        public GameObject FloorPrefab;
        public GameObject WallPrefab;
        public GameObject EnemyPrefab;
        public GameObject PlanePrefab;
        
        public GameObject Player;

        [Header("DungeonSettings")]
        public int amountRooms;
        public int width = 100;
        public int height = 100;
        public int minRoomSize = 3;
        public int maxRoomSize = 7;
        public int enemiesInRoom = 2;
        public int seed;

        private Dictionary<Vector2Int, Tile> dungeonDictionary = new Dictionary<Vector2Int, Tile>();
        private List<Room> roomList = new List<Room>();
        private List<GameObject> allSpawnedObjects = new List<GameObject>();

        public NavMeshSurface surface;

        void Start()
        {
            seed = Random.Range(0, 1000);
            Random.InitState(seed);
            GenerateDungeon();

            surface.BuildNavMesh();
        }

        private void AllocateRooms()
        {
            for (int i = 0; i < amountRooms; i++)
            {
                Room room = new Room()
                {
                    position = new Vector2Int(Random.Range(0, width), Random.Range(0, height)),
                    size = new Vector2Int(Random.Range(minRoomSize, maxRoomSize), Random.Range(minRoomSize, maxRoomSize))
                };
                Room startRoom = new Room()
                {
                    position = new Vector2Int(Random.Range(0, width), Random.Range(0, height)),
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

        private void AllocateFloor()
        {

        }

        private void BuildDungeon()
        {
            GameObject plane = Instantiate(PlanePrefab, new Vector3Int((width / 2), 0, (height / 2)), Quaternion.LookRotation(new Vector3(0, 0, 90)));
            plane.transform.localScale = new Vector3Int(width, 1, height);
            
            foreach (KeyValuePair<Vector2Int, Tile> kv in dungeonDictionary)
            {
                GameObject floor = Instantiate(FloorPrefab, new Vector3Int(kv.Key.x, 0, kv.Key.y), Quaternion.identity);
                allSpawnedObjects.Add(floor);

                SpawnWallsForTile(kv.Key);
            }
        }

        private void SpawnWallsForTile(Vector2Int position)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    if (Mathf.Abs(x) == Mathf.Abs(z)) { continue; }
                    Vector2Int gridPos = position + new Vector2Int(x, z);
                    if (!dungeonDictionary.ContainsKey(gridPos))
                    {
                        Vector3 direction = new Vector3(gridPos.x, 0, gridPos.y) - new Vector3(position.x, 0, position.y);
                        GameObject wall = Instantiate(WallPrefab, new Vector3(position.x, 0, position.y), Quaternion.LookRotation(direction));
                        allSpawnedObjects.Add(wall);
                    }
                }
            }
        }

        public void GenerateDungeon()
        {
            AllocateFloor();
            AllocateRooms();
            AllocateCorridors();
            BuildDungeon();
            PoppulateDungeon();
        }

        private void PoppulateDungeon()
        {
            foreach (var startRoom in roomList)
            {
                Player.transform.position = startRoom.middlePos();
            }
            foreach (var room in roomList)
            {
                for (int i = 0; i < enemiesInRoom; i++)
                {
                    GameObject enemy = Instantiate(EnemyPrefab, room.randomPos(), Quaternion.LookRotation(new Vector3(0,0,1)));
                    allSpawnedObjects.Add(enemy);
                }
            }
        }
    }

    public class Room
    {
        public Vector2Int position;
        public Vector2Int size;

        public Vector3 randomPos()
        {
            return new Vector3(position.x + Random.Range(0, size.x), 0, position.y + Random.Range(0, size.y));
        }

        public Vector3 middlePos()
        {
            return new Vector3(position.x + (size.x / 2), 0, position.y + (size.y / 2));
        }
    }
}
