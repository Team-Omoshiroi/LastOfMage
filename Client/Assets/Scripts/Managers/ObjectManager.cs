using Google.Protobuf.Protocol;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectManager : CustomSingleton<ObjectManager>
{
    public PilotSync pilotSync { get; set; }
    Dictionary<int, GameObject> _objects = new Dictionary<int, GameObject>();

    public static GameObjectType GetObjectTypeById(int id)
    {
        int type = (id >> 24) & 0x7F;
        return (GameObjectType)type;
    }

    public void Update()
    {
        Debug.Log($"_objects.Count : {_objects.Count}");
    }

    public void Add(ObjectInfo info, bool pilotPlayer = false)
    {
        GameObjectType objectType = GetObjectTypeById(info.ObjectId);

        if (objectType == GameObjectType.Player)
        {
            Vector3 S_Position = new Vector3(
                info.PosInfo.PosX,
                info.PosInfo.PosY,
                info.PosInfo.PosZ
            );

            if (pilotPlayer)
            {
                GameObject gameObject = Instantiate(
                    Resources.Load<GameObject>("Prefabs/PilotPlayer")
                );

                gameObject.name = info.Name;
                _objects.Add(info.ObjectId, gameObject);

                pilotSync = gameObject.GetComponent<PilotSync>();
                pilotSync.Id = info.ObjectId;
                pilotSync.PosInfo = info.PosInfo;
                pilotSync.StatInfo = info.StatInfo;
                pilotSync.State = info.State;
            }
            else
            {
                Vector3 SpawnPos = S_Position;

                GameObject gameObject = Instantiate(
                    Resources.Load<GameObject>("Prefabs/ClonePlayer"),
                    SpawnPos,
                    Quaternion.identity
                );

                _objects.Add(info.ObjectId, gameObject);

                CloneSync cloneSync = gameObject.GetComponent<CloneSync>();
                cloneSync.Id = info.ObjectId;
                cloneSync.PosInfo = info.PosInfo;
                cloneSync.StatInfo = info.StatInfo;
                cloneSync.State = info.State;
                cloneSync.CallMoveEvent(cloneSync.State, cloneSync.PosInfo, cloneSync.VelInfo);
            }
        }
    }

    public void Remove(int id)
    {
        GameObject gameObject = FindById(id);
        if (gameObject == null)
            return;

        _objects.Remove(id);
        Destroy(gameObject);
    }

    public GameObject FindById(int id)
    {
        _objects.TryGetValue(id, out GameObject gameObject);
        return gameObject;
    }

    public void Clear()
    {
        foreach (GameObject gameObject in _objects.Values)
        {
            Destroy(gameObject);
        }

        _objects.Clear();
        pilotSync = null;
    }
}
