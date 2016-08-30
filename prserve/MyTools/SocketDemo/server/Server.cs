using UnityEngine;
using System.Collections;

public class Server : MonoBehaviour
{
    void Start()
    {
        Loger.LogWarning(Application.persistentDataPath);
        Game.SocketManager.Instance.Start();
    }
}