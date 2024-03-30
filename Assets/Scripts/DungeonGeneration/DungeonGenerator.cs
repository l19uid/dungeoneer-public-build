using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Dungeoneer
{
    public class DungeonGenerator : MonoBehaviour
    {
        [SerializeField] [Header("Dungeon Info")]
        public string dungeonName;

        public enum DungeonType
        {
            Default,
            Jungle,
            Desert,
            Tundra,
            Lava
        }

        public DungeonType dungeonType;

        [Range(6, 16)] [SerializeField] private int size;

        [Range(1, 5)] [SerializeField] private int _maxChestCount;
        [Range(0, 3)] [SerializeField] private int _maxMiniBossCount;
        [Range(1, 2)] [SerializeField] private int _maxTrapCount;
        [SerializeField] private int _maxBranchCount;

        [Range(0, 100)] [SerializeField] private float _branchChance;
        [SerializeField] private BossManager _bossManager;
        [SerializeField] private GameObject _bossRoom;

        private int _trapCount;
        private int _miniBossCount;
        private int _chestCount;
        private int _branchCount = 0;
        private List<Vector2> _branches = new List<Vector2>();

        private Vector2 _spawn;
        private Vector2 _exit;
        [SerializeField] private int pathLengthToExit = 7;
        [SerializeField] private Vector2 branchSizes;
        private GradingSystem _gs;

        [Header("Room")] public GameObject[] rooms;

        private int _floor;

        public GameObject entranceIcon;
        public GameObject exitIcon;
        public GameObject chestIcon;
        public GameObject regularIcon;
        public GameObject trapIcon;
        public GameObject miniBossIcon;
        public GameObject bossIcon;

        public class RoomVar
        {
            private Vector2 pos;
            private Vector2 parent;
            private int pathDistance;
            private bool[] directions = new bool[4];
            private Enums.RoomType roomType;

            public RoomVar(Vector2 pos, Vector2 parent, bool[] directions, int pathDistance, Enums.RoomType roomType)
            {
                this.pos = pos;
                this.parent = parent;
                this.pathDistance = pathDistance;
                this.roomType = roomType;
                this.directions = directions;
            }

            public Vector2 GetPos()
            {
                return pos;
            }

            public Vector2 GetParent()
            {
                return parent;
            }

            public int GetPathDistance()
            {
                return pathDistance;
            }

            public bool[] GetDirections()
            {
                return directions;
            }

            public Enums.RoomType GetRoomType()
            {
                return roomType;
            }

            public void SetDirections(bool[] directions)
            {
                this.directions = directions;
            }
        }

        private List<RoomVar> roomVars = new List<RoomVar>();

        void Start()
        {
            _gs = GetComponent<GradingSystem>();
            GenerateFloor();
        }

        public void GenerateFloor(int floor = 1)
        {
            GenerateSpawn();
            GenerateRooms();

            TeleportPlayer();
        }

        private void GenerateRooms()
        {
            GeneratePath(pathLengthToExit, _spawn);
            for (int i = 0; i < _branches.Count; i++)
            {
                GeneratePath(Random.Range((int)branchSizes.x, (int)branchSizes.y), _branches[i], true);
            }

            for (int i = 0; i < roomVars.Count; i++)
            {
                GenerateConnections(roomVars[i]);
            }

            GenerateGameObjects();
        }

        private void GenerateGameObjects()
        {
            for (int i = 0; i < roomVars.Count; i++)
            {
                //StartCoroutine(GenerateRoom(i));
                GameObject room = new GameObject();
                if (roomVars[i].GetRoomType() == Enums.RoomType.Start ||
                    roomVars[i].GetRoomType() == Enums.RoomType.End ||
                    roomVars[i].GetRoomType() == Enums.RoomType.Chest)
                {
                    room = Instantiate(rooms[0],
                        new Vector3(roomVars[i].GetPos().x * 32, roomVars[i].GetPos().y * 32, 0), Quaternion.identity,
                        transform);
                }
                else
                {
                    room = Instantiate(rooms[Random.Range(0, rooms.Length)],
                        new Vector3(roomVars[i].GetPos().x * 32, roomVars[i].GetPos().y * 32, 0), Quaternion.identity,
                        transform);
                }

                room.GetComponent<Room>().CreateRoom(roomVars[i].GetDirections(), roomVars[i].GetRoomType());
                room.GetComponent<Room>().SpawnRoom();
                room.name = "Room: " + roomVars[i].GetPos() + " | " + roomVars[i].GetRoomType();
            }
        }

        private IEnumerator GenerateRoom(int i)
        {
            GameObject room = new GameObject();
            if (roomVars[i].GetRoomType() == Enums.RoomType.Start ||
                roomVars[i].GetRoomType() == Enums.RoomType.End ||
                roomVars[i].GetRoomType() == Enums.RoomType.Chest)
            {
                room = Instantiate(rooms[0],
                    new Vector3(roomVars[i].GetPos().x * 32, roomVars[i].GetPos().y * 32, 0), Quaternion.identity,
                    transform);
            }
            else
            {
                room = Instantiate(rooms[Random.Range(0, rooms.Length)],
                    new Vector3(roomVars[i].GetPos().x * 32, roomVars[i].GetPos().y * 32, 0), Quaternion.identity,
                    transform);
            }

            room.GetComponent<Room>().CreateRoom(roomVars[i].GetDirections(), roomVars[i].GetRoomType());
            room.GetComponent<Room>().SpawnRoom();
            room.name = "Room: " + roomVars[i].GetPos() + " | " + roomVars[i].GetRoomType();
            yield return new WaitForSeconds(.1f);
        }

        private void GenerateConnections(RoomVar roomVar)
        {
            bool[] directions = new bool[4];
            if (roomVar.GetParent() == new Vector2(roomVar.GetPos().x, roomVar.GetPos().y + 1))
            {
                directions[0] = true;
                AddDirectionToParent(new Vector2(roomVar.GetPos().x, roomVar.GetPos().y + 1), 1);
            }

            if (roomVar.GetParent() == new Vector2(roomVar.GetPos().x, roomVar.GetPos().y - 1))
            {
                directions[1] = true;
                AddDirectionToParent(new Vector2(roomVar.GetPos().x, roomVar.GetPos().y - 1), 0);
            }

            if (roomVar.GetParent() == new Vector2(roomVar.GetPos().x - 1, roomVar.GetPos().y))
            {
                directions[2] = true;
                AddDirectionToParent(new Vector2(roomVar.GetPos().x - 1, roomVar.GetPos().y), 3);
            }

            if (roomVar.GetParent() == new Vector2(roomVar.GetPos().x + 1, roomVar.GetPos().y))
            {
                directions[3] = true;
                AddDirectionToParent(new Vector2(roomVar.GetPos().x + 1, roomVar.GetPos().y), 2);
            }

            roomVar.SetDirections(directions);
        }

        private void AddDirectionToParent(Vector2 pos, int dir)
        {
            for (int i = 0; i < roomVars.Count; i++)
            {
                if (roomVars[i].GetPos() == pos)
                {
                    bool[] directions = roomVars[i].GetDirections();
                    directions[dir] = true;
                    roomVars[i].SetDirections(directions);
                }
            }
        }

        private RoomVar GetRoomVarFromParent(Vector2 parent)
        {
            for (int i = 0; i < roomVars.Count; i++)
            {
                if (roomVars[i].GetPos() == parent)
                    return roomVars[i];
            }

            return null;
        }

        private void TeleportPlayer()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = new Vector3(_spawn.x * 32, _spawn.y * 32, 0);
        }

        private void GenerateSpawn()
        {
            int x = size / 2;
            int y = size / 2;

            _spawn = new Vector2(x, y);
            roomVars.Add(new RoomVar(_spawn, _spawn, new bool[4], 0, Enums.RoomType.Start));
        }

        private void GeneratePath(int pathLengthMax, Vector2 startPos, bool isBranch = false)
        {
            int pathLength = 0;
            Vector2 lastPos = startPos;
            Vector2 currentPos = startPos;

            while (pathLength < pathLengthMax)
            {
                int direction = Random.Range(0, 4);
                bool[] directions = new bool[4];

                //Check for possible direction
                if (currentPos.y + 1 <= size && !RoomExists(new Vector2(currentPos.x, currentPos.y + 1)))
                {
                    directions[0] = true;
                }

                if (currentPos.y - 1 > 0 && !RoomExists(new Vector2(currentPos.x, currentPos.y - 1)))
                {
                    directions[1] = true;
                }

                if (currentPos.x - 1 > 0 && !RoomExists(new Vector2(currentPos.x - 1, currentPos.y)))
                {
                    directions[2] = true;
                }

                if (currentPos.x + 1 <= size && !RoomExists(new Vector2(currentPos.x + 1, currentPos.y)))
                {
                    directions[3] = true;
                }

                //Check if there is a possible direction
                if (!directions[0] && !directions[1] && !directions[2] && !directions[3])
                {
                    Debug.Log("No possible direction");
                    currentPos = GetRoomParentPos(currentPos);
                    continue;
                }

                //Generate random directions from possible directions
                while (!directions[direction])
                {
                    direction = Random.Range(0, 4);
                }

                //Set current position to the new position
                switch (direction)
                {
                    case 0:
                        currentPos = new Vector2(currentPos.x, currentPos.y + 1);
                        pathLength++;
                        break;
                    case 1:
                        currentPos = new Vector2(currentPos.x, currentPos.y - 1);
                        pathLength++;
                        break;
                    case 2:
                        currentPos = new Vector2(currentPos.x - 1, currentPos.y);
                        pathLength++;
                        break;
                    case 3:
                        currentPos = new Vector2(currentPos.x + 1, currentPos.y);
                        pathLength++;
                        break;  
                }

                if (currentPos != lastPos && !isBranch && pathLength == pathLengthMax)
                    roomVars.Add(new RoomVar(currentPos, lastPos, new bool[4], pathLength, Enums.RoomType.End));
                else if (currentPos != lastPos && isBranch && pathLength == pathLengthMax)
                    roomVars.Add(new RoomVar(currentPos, lastPos, new bool[4], pathLength, Enums.RoomType.Chest));
                else if (currentPos != lastPos && !isBranch)
                    roomVars.Add(new RoomVar(currentPos, lastPos, new bool[4], pathLength, GenerateRandomRoomType()));
                else if (currentPos != lastPos && isBranch)
                    roomVars.Add(new RoomVar(currentPos, lastPos, new bool[4], pathLength, GenerateRandomRoomType()));

                if (!isBranch && Random.Range(0, 100) < _branchChance && _branchCount < _maxBranchCount &&
                    pathLength != pathLengthMax)
                {
                    _branches.Add(currentPos);
                    _branchCount++;
                }

                lastPos = currentPos;
            }
        }

        private Vector2 GetRoomParentPos(Vector2 currentPos)
        {
            for (int i = 0; i < roomVars.Count; i++)
            {
                if (roomVars[i].GetPos() == currentPos)
                    return roomVars[i].GetParent();
            }

            return Vector2.zero;
        }

        private Vector2 GetRoomChildPos(Vector2 currentPos)
        {
            for (int i = 0; i < roomVars.Count; i++)
            {
                if (roomVars[i].GetParent() == currentPos)
                    return roomVars[i].GetPos();
            }

            return Vector2.zero;
        }

        private bool RoomExists(Vector2 pos)
        {
            for (int i = 0; i < roomVars.Count; i++)
            {
                if (roomVars[i].GetPos() == pos)
                {
                    return true;
                }
            }

            return false;
        }

        private Enums.RoomType GenerateRandomRoomType()
        {
            int random = Random.Range(0, 100);
            if (random < 5 && _chestCount < _maxChestCount)
            {
                _chestCount++;
                return Enums.RoomType.Chest;
            }
            else if (random < 15 && _trapCount < _maxTrapCount)
            {
                _trapCount++;
                return Enums.RoomType.Trap;
            }
            else if (random < 30 && _miniBossCount < _maxMiniBossCount)
            {
                _miniBossCount++;
                return Enums.RoomType.MiniBoss;
            }
            else
                return Enums.RoomType.Regular;
        }

        //create an instance that can be called from anywhere that contains functions to return icons

        public class Instance
        {
            public static Object GetIcon(Enums.RoomType roomType)
            {
                switch (roomType)
                {
                    case Enums.RoomType.Chest:
                        return Resources.Load("UI/IconChest");
                    case Enums.RoomType.Regular:
                        return Resources.Load("UI/IconRegular");
                    case Enums.RoomType.MiniBoss:
                        return Resources.Load("UI/IconMiniBoss");
                    case Enums.RoomType.Boss:
                        return Resources.Load("UI/IconBoss");
                    case Enums.RoomType.Start:
                        return Resources.Load("UI/IconStart");
                    case Enums.RoomType.End:
                        return Resources.Load("UI/IconExit");
                    case Enums.RoomType.Trap:
                        return Resources.Load("UI/IconTrap");
                    default:
                        return null;
                }
            }
        }

        public void GenerateBossRoom()
        {
            GameObject room = Instantiate(_bossRoom, new Vector3(-100, -100, 0), Quaternion.identity,
                transform);
            room.name = "Boss Room";
        }
    }
}