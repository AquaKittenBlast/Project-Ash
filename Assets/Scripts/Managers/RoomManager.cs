using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    [Header("spawnPositions")]
    public Vector2 spawn0;
    public Vector2 spawn1;
    public Vector2 spawn2;
    public Vector2 spawn3;
    [Header("Variables")]
    [SerializeField] public static int currentRoom = 0;
    [SerializeField] public int currentRoomDisplay; 
    [Header("External")]
    public GameObject boundingBox;
    [SerializeField] public PlayerMovement ash;
    [SerializeField] public TransitionScript trans;

    public static List<Vector2> playerSpawns = new List<Vector2>();


    void Start()
    {
        playerSpawns.AddRange(new Vector2[] { spawn0, spawn1, spawn2, spawn3 });
        ash.ToSpawn();
    }

    public void AdvanceToNextRoom()
    {
        if (currentRoom >= playerSpawns.Count)
        {
            return;
        }
        Debug.Log(currentRoom);
        PlayerMovement.spawnPos = playerSpawns[currentRoom];
        trans.Transition(false);
        ash.ToSpawn();
    }

    public void Update()
    {
        currentRoomDisplay = currentRoom;
    }
}
