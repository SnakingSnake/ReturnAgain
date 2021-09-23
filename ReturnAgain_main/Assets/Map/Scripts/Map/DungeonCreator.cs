using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using NavMeshBuilder = UnityEngine.AI.NavMeshBuilder;

public class DungeonCreator : MonoBehaviour
{
    public int dungeonWidth, dungeonLength;
    public int roomWidthMin, roomLengthMin;
    public int maxIterations;
    public int corridorWidth;
    [Range(0.0f, 0.3f)]
    public float roomBottomCornerModifier;
    [Range(0.7f, 1.0f)]
    public float roomTopCornerModifier;
    [Range(0,2)]
    public int roomOffset;
    public GameObject DungeonWall;
    public GameObject DungeonFloor;

    List<Vector3Int> possibleDoorVerticalPosition;
    List<Vector3Int> possibleDoorHorizontalPosition;
    List<Vector3Int> possibleWallVerticalPosition;
    List<Vector3Int> possibleWallHorizontalPosition;
    List<Vector3Int> possibleWallUpVerticalPosition;
    List<Vector3Int> possibleWallDownVerticalPosition;
    List<Vector3Int> possibleWallUpHorizontalPosition;
    List<Vector3Int> possibleWallDownHorizontalPosition;
    public List<GameObject> enemys = new List<GameObject>();
    public List<GameObject> decoration = new List<GameObject>();
    List<Vector3Int> spawnPosition;

    public Material dungeonTile;
    public GameObject startPortal;
    public GameObject endPortal;

    bool seCreate;

    Quaternion soRotation;
    Quaternion v3Rotation = Quaternion.Euler(0f, -45f, 0f);  // 회전각
    NavMeshSurface NavMesh;

    void Awake()
    {
        CreateDungeon();
    }

    public void CreateDungeon()
    {
        DestroyAllChildren();

        DungeonGenerator generator = new DungeonGenerator(dungeonWidth + Player_knights.dungeonLevel * 10, dungeonLength + Player_knights.dungeonLevel * 10);
        var listOfRooms = generator.CalculateDungeon(maxIterations + Player_knights.dungeonLevel * 2,
            roomWidthMin,
            roomLengthMin,
            roomBottomCornerModifier,
            roomTopCornerModifier,
            roomOffset,
            corridorWidth);
        GameObject wallParent = new GameObject("WallParent");
        wallParent.transform.parent = transform;
        possibleDoorVerticalPosition = new List<Vector3Int>();
        possibleDoorHorizontalPosition = new List<Vector3Int>();
        possibleWallVerticalPosition = new List<Vector3Int>();
        possibleWallHorizontalPosition = new List<Vector3Int>();
        possibleWallUpVerticalPosition = new List<Vector3Int>();
        possibleWallUpHorizontalPosition = new List<Vector3Int>();
        possibleWallDownVerticalPosition = new List<Vector3Int>();
        possibleWallDownHorizontalPosition = new List<Vector3Int>();

        spawnPosition = new List<Vector3Int>();


        GameObject floorParent = new GameObject("floorParent");
        floorParent.transform.parent = transform;
        for (int i=0; i<listOfRooms.Count; i++)
        {
            CreateMesh(listOfRooms[i].BottomLeftAreaCorner, listOfRooms[i].TopRightAreaCorner,floorParent);
        }
        floorParent.transform.localRotation = v3Rotation;

        GameObject seParent = new GameObject("seParent");//플레이어 입구 출구 생성
        seParent.transform.parent = transform;
        int num1;
        int num2;
        float roomdistance1;
        float roomdistance2;

        while (true)
        {
            num1 = Random.Range(0, listOfRooms.Count - 1);
            num2 = Random.Range(0, listOfRooms.Count - 1);
            if (num1 == num2)
                continue;

            roomdistance1 = Vector2.Distance(listOfRooms[num1].TopRightAreaCorner, listOfRooms[num1].BottomLeftAreaCorner);
            roomdistance2 = Vector2.Distance(listOfRooms[num2].TopRightAreaCorner, listOfRooms[num2].BottomLeftAreaCorner);

            if (!(roomdistance1 > ((roomLengthMin + roomWidthMin) / 2) && roomdistance2 > ((roomLengthMin + roomWidthMin) / 2)))
            {
                continue;
            }
            seCreate = CreateSE(listOfRooms[num1].BottomLeftAreaCorner, listOfRooms[num1].TopRightAreaCorner, listOfRooms[num2].BottomLeftAreaCorner, listOfRooms[num2].TopRightAreaCorner, seParent);
            if (seCreate == true)
                break;
        }
        seParent.transform.localRotation = v3Rotation; // 플레이어 입구 출구 생성후 돌리기

        GameObject decoParent = new GameObject("decoParent");
        decoParent.transform.parent = transform;
        for (int i = 0; i < listOfRooms.Count; i++)
        {
            float roomdistance = Vector2.Distance(listOfRooms[i].TopRightAreaCorner, listOfRooms[i].BottomLeftAreaCorner);
            if (roomdistance > (roomLengthMin + roomWidthMin) / 2)
            {
                CreateDeco(decoParent, listOfRooms[i].BottomLeftAreaCorner, listOfRooms[i].TopRightAreaCorner, roomdistance);
            }
        }
        decoParent.transform.localRotation = v3Rotation;

       
        //적 생성
        GameObject enemyParent = new GameObject("enemyParent");
        enemyParent.transform.parent = transform;
        for (int i = 0; i < listOfRooms.Count; i++)
        {
            if (i == num1)
                continue;
            float roomdistance = Vector2.Distance(listOfRooms[i].TopRightAreaCorner, listOfRooms[i].BottomLeftAreaCorner);
            if (roomdistance > (roomLengthMin + roomWidthMin) / 2)
            {
                CreateEnemys(enemyParent, listOfRooms[i].BottomLeftAreaCorner, listOfRooms[i].TopRightAreaCorner,roomdistance);
            }
        }
        enemyParent.transform.localRotation = v3Rotation;

        CreateWalls(wallParent);

        Vector3 DungeonVector = this.gameObject.transform.position;
        wallParent.transform.position = new Vector3(wallParent.transform.position.x + DungeonVector.x, 0, wallParent.transform.position.z + DungeonVector.z);
        wallParent.transform.localRotation = v3Rotation; 
        GameObject player = GameObject.FindWithTag("Player");
        GameObject start = GameObject.FindWithTag("Start");
        Vector3 vec = start.gameObject.transform.position;
        player.gameObject.transform.position = vec;


    }

    private void CreateDeco(GameObject decoParent, Vector2Int bottomLeftCorner, Vector2Int topRightCorner, float distance)
    {
        int listNum;
        int decox;
        int decoz;
        Vector3Int decoPosition;

        for (int i = 0; i < (5 + ((int)(distance / 12))); i++)
        {
            while (true)
            {
                decox = Random.Range((int)bottomLeftCorner.x, (int)topRightCorner.x);
                decoz = Random.Range((int)bottomLeftCorner.y, (int)topRightCorner.y);
                decoPosition = new Vector3Int(decox, 0, decoz);
                if (!spawnPosition.Contains(decoPosition))
                {
                    spawnPosition.Add(decoPosition);
                    break;
                }
            }
            listNum = Random.Range(0, decoration.Count);
            Instantiate(decoration[listNum], decoPosition, Quaternion.Euler(0f, Random.Range(0, 4) * 90, 0f), decoParent.transform);
        }
    }

    private void CreateEnemys(GameObject enemyParent, Vector2 bottomLeftCorner, Vector2 topRightCorner,float distance)
    {
        int listNum;
        int enemyx;
        int enemyz;
        Vector3Int enemyPosition;

        for (int i = 0;i<((Player_knights.dungeonLevel*2)+5+((int)(distance/10))) ;i++)
        {
            while (true)
            {
                enemyx = Random.Range((int)bottomLeftCorner.x + 1, (int)topRightCorner.x - 1);
                enemyz = Random.Range((int)bottomLeftCorner.y + 1, (int)topRightCorner.y - 1);
                enemyPosition = new Vector3Int(enemyx, 0, enemyz);
                if (!spawnPosition.Contains(enemyPosition))
                {
                    spawnPosition.Add(enemyPosition);
                    break;
                }
            }
            listNum = Random.Range(0, enemys.Count);
            Instantiate(enemys[listNum], enemyPosition, Quaternion.Euler(0,Random.Range(0,360),0), enemyParent.transform);
            i += listNum;
        }
    }

    private void CreateWalls(GameObject wallParent)
    {
        foreach(var wallPosition in possibleWallUpHorizontalPosition)
        {
            CreateWall(wallParent, wallPosition + new Vector3Int(1, 0, 0), DungeonWall, Quaternion.Euler(0, 0, 0));
        }
        foreach (var wallPosition in possibleWallUpVerticalPosition)
        {
            CreateWall(wallParent, wallPosition, DungeonWall, Quaternion.Euler(0, 90, 0));
        }
        foreach (var wallPosition in possibleWallDownHorizontalPosition)
        {
            CreateWall(wallParent, wallPosition, DungeonWall, Quaternion.Euler(0, 180, 0));
        }
        foreach (var wallPosition in possibleWallDownVerticalPosition)
        {
            CreateWall(wallParent, wallPosition + new Vector3Int(0, 0, 1), DungeonWall, Quaternion.Euler(0, -90, 0));
        }
    }

    private void CreateWall(GameObject wallParent, Vector3Int wallPosition, GameObject wallPrefab, Quaternion wallRotation)
    {
        Instantiate(wallPrefab, wallPosition, wallRotation, wallParent.transform);
    }

    private void CreateMesh(Vector2 bottomLeftCorner, Vector2 topRightCorner,GameObject FloorParent)
    {
        Vector3 bottomLeftV = new Vector3(bottomLeftCorner.x, 0, bottomLeftCorner.y);
        Vector3 bottomRightV = new Vector3(topRightCorner.x, 0, bottomLeftCorner.y);
        Vector3 topLeftV = new Vector3(bottomLeftCorner.x, 0, topRightCorner.y);
        Vector3 topRightV = new Vector3(topRightCorner.x, 0, topRightCorner.y);



        Vector3[] vertices = new Vector3[]
        {
            topLeftV,
            topRightV,
            bottomLeftV,
            bottomRightV
        };
        Vector2[] uvs = new Vector2[vertices.Length];
        for(int i =0; i<uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        int[] triangles = new int[]
        {
            0,
            1,
            2,
            2,
            1,
            3
        };
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        GameObject dungeonFloor = new GameObject("Floor", typeof(MeshFilter), typeof(MeshRenderer));
        dungeonFloor.gameObject.isStatic = true;

        dungeonFloor.transform.position = this.gameObject.transform.position;
        dungeonFloor.transform.localScale = Vector3.one;
        dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
        dungeonFloor.GetComponent<MeshRenderer>().material = dungeonTile;
        dungeonFloor.transform.parent = FloorParent.transform;
        dungeonFloor.transform.gameObject.layer = LayerMask.NameToLayer("Minimap");

        for (int i = 0;i< (int)topRightCorner.y - (int)bottomLeftCorner.y; i++)
        {
            for (int j = 1; j < (int)topRightCorner.x - (int)bottomLeftCorner.x +1; j++)
            {
                
                Vector3 floorPosition = new Vector3(bottomLeftCorner.x + j, 0, bottomLeftCorner.y + i);
                Instantiate(DungeonFloor,floorPosition,Quaternion.Euler(0,0,0),FloorParent.transform);
            }
        }



        for (int row = (int)bottomLeftV.x; row < (int)bottomRightV.x; row++)
        {
            var wallPosition = new Vector3(row, 0, bottomLeftV.z);
            AddWallPositionToList(wallPosition, possibleWallUpHorizontalPosition, possibleDoorHorizontalPosition,possibleWallHorizontalPosition);
        }
        for(int row = (int)topLeftV.x; row < (int)topRightCorner.x; row++)
        {
            var wallPosition = new Vector3(row, 0, topRightV.z);
            AddWallPositionToList(wallPosition, possibleWallDownHorizontalPosition, possibleDoorHorizontalPosition, possibleWallHorizontalPosition);
        }
        for(int col = (int)bottomLeftV.z; col < (int)topLeftV.z; col++)
        {
            var wallPosition = new Vector3(bottomLeftV.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallUpVerticalPosition, possibleDoorVerticalPosition,possibleWallVerticalPosition);
        }
        for (int col = (int)bottomRightV.z; col < (int)topRightV.z; col++)
        {
            var wallPosition = new Vector3(bottomRightV.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallDownVerticalPosition, possibleDoorVerticalPosition, possibleWallVerticalPosition);
        }
        foreach(var v in possibleDoorVerticalPosition)
        {
            if(possibleWallUpVerticalPosition.Contains(v))
            {
                possibleWallUpVerticalPosition.Remove(v);
            }
            if (possibleWallDownVerticalPosition.Contains(v))
            {
                possibleWallDownVerticalPosition.Remove(v);
            }
        }
        foreach (var v in possibleDoorHorizontalPosition)
        {
            if (possibleWallUpHorizontalPosition.Contains(v))
            {
                possibleWallUpHorizontalPosition.Remove(v);
            }
            if (possibleWallDownHorizontalPosition.Contains(v))
            {
                possibleWallDownHorizontalPosition.Remove(v);
            }
        }
    }

    private bool CreateSE(Vector2 bottomLeftCorner1, Vector2 topRightCorner1, Vector2 bottomLeftCorner2, Vector2 topRightCorner2, GameObject seParent)
    {
        Vector3Int startPortalVector = new Vector3Int(Random.Range((int)bottomLeftCorner1.x+4, (int)topRightCorner1.x-4), 0, Random.Range((int)bottomLeftCorner1.y+4, (int)topRightCorner1.y-4));
        Vector3Int endPortalVector = new Vector3Int(Random.Range((int)bottomLeftCorner2.x + 6, (int)topRightCorner2.x - 6), 0, Random.Range((int)bottomLeftCorner2.y + 6, (int)topRightCorner2.y - 6));

        float portaldistance = Vector3.Distance(startPortalVector,endPortalVector);

        if (portaldistance >= 100)
        {
            soRotation = Quaternion.Euler(0f, Random.Range(0, 4) * 90, 0f);
            Instantiate(startPortal, startPortalVector, soRotation, seParent.transform);
            soRotation = Quaternion.Euler(0f, Random.Range(0, 360), 0f);
            Instantiate(endPortal, endPortalVector, soRotation, seParent.transform);
            return true;
        }
        return false;
    }

    private void AddWallPositionToList(Vector3 wallPosition, List<Vector3Int> wallUDList, List<Vector3Int> doorList, List<Vector3Int> wallList)
    {
        Vector3Int point = Vector3Int.CeilToInt(wallPosition);
        if (wallList.Contains(point))
        {
            doorList.Add(point);
            wallList.Remove(point);
            wallUDList.Remove(point);
        }
        else
        {
            wallList.Add(point);
            wallUDList.Add(point);
        }
    }


    private void DestroyAllChildren()
    {
        while (transform.childCount !=0)
        {
            foreach (Transform item in transform)
            {
                DestroyImmediate(item.gameObject);
            }
        }
    }
}
