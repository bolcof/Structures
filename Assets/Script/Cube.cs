using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Cube : MonoBehaviour
{
    [SerializeField] public int
        _id,
        _structId;
    [SerializeField] public float _top;
    [SerializeField] public Vector3 
        _size,
        _position;

    public void set (int id, int structId, Vector3 size, Vector3 position)
    {
        Debug.Log("set" + id.ToString());
        this._id = id;
        this._structId = structId;
        this._size = size;
        this._position = position;

        this.gameObject.transform.localScale = size;
        this.gameObject.transform.localPosition = position;

        this._top = this._position.y + this._size.y;
    }



    public static void ganarateRoot(int structId, Vector3 Area, float minSize, float maxSize) {

        Vector3 size = new Vector3(Random.Range(minSize, maxSize), Random.Range(minSize, maxSize), Random.Range(minSize, maxSize));
        Vector3 position = new Vector3(
            Random.Range(size.x / 2, Area.x - size.x / 2),
            Random.Range(size.y / 2, Area.y - size.y / 2),
            Random.Range(size.z / 2, Area.z - size.z / 2)
            );

        Cube root = Instantiate((GameObject)Resources.Load("CubeOrigin"), GameObject.Find("Root_" + structId.ToString()).transform).GetComponent<Cube>();

        root.set(0, structId, size, position);

        root.gameObject.name = "rootCube";
    }

    public static bool AddCube (int Id, int structId, Cube rootCube, Vector3 Area, float minSize, float maxSize, float volume, float surface)
    {
        bool hitOne = false;
        int tryCount = 0;
        Cube Added;
        Vector3 size = new Vector3(-1, -1, -1);
        Vector3 position = new Vector3(-1, -1, -1);

        while (!hitOne)
        {
            size = new Vector3(Random.Range(minSize, maxSize), Random.Range(minSize, maxSize), Random.Range(minSize, maxSize));
            position = new Vector3(
                Random.Range(Mathf.Max(size.x / 2, rootCube._position.x - ((rootCube._size.x + size.x) / 2)), Mathf.Min((Area.x - size.x / 2), rootCube._position.x + ((rootCube._size.x + size.x) / 2))),
                Random.Range(Mathf.Max(size.y / 2, rootCube._position.y - ((rootCube._size.y + size.y) / 2)), Mathf.Min((Area.y - size.y / 2), rootCube._position.y + ((rootCube._size.y + size.y) / 2))),
                Random.Range(Mathf.Max(size.z / 2, rootCube._position.z - ((rootCube._size.z + size.z) / 2)), Mathf.Min((Area.z - size.z / 2), rootCube._position.z + ((rootCube._size.z + size.z) / 2)))
                );

            int hitNum = 0;

            GameObject[] children = GetChildren("Root_" + structId.ToString());
            for (int i = 1; i < children.Length; i++)
            {
                hitNum += col(size, position, children[i].GetComponent<Cube>());
            }

            if (hitNum == 1)
            {
                hitOne = true;
            }
            else
            {
                hitOne = false;
            }
            
            tryCount++;
            if(tryCount >= 50) {
                Debug.Log("give up");
                return false;
            }
        }

        Debug.Log("try" + tryCount.ToString());
        Added = Instantiate((GameObject)Resources.Load("CubeOrigin"), GameObject.Find("Root_" + structId.ToString()).transform).GetComponent<Cube>();
        Added.set(Id, structId, size, position);
        return true;
    }

    private static GameObject[] GetChildren(string parentName)
    {
        // 検索し、GameObject型に変換
        var parent = GameObject.Find(parentName) as GameObject;
        // 見つからなかったらreturn
        if (parent == null) return null;
        // 子のTransform[]を取り出す
        var transforms = parent.GetComponentsInChildren<Transform>();
        // 使いやすいようにtransformsからgameObjectを取り出す
        var gameObjects = from t in transforms select t.gameObject;
        // 配列に変換してreturn
        return gameObjects.ToArray();
    }

    public static int col (Vector3 targetSize, Vector3 targetPos, Cube other)
    {
        float difX = targetPos.x - other._position.x;
        float difY = targetPos.y - other._position.y;
        float difZ = targetPos.z - other._position.z;

        if (
            Mathf.Abs(difX) < (targetSize.x + other._size.x) / 2 &&
            Mathf.Abs(difY) < (targetSize.y + other._size.y) / 2 &&
            Mathf.Abs(difZ) < (targetSize.z + other._size.z) / 2)
        { return 1; }
        else
        { return 0; }
    }

    public static void AddLastCube (float volume, float surface)
    {

    }

    public static int returnHighest (int structId) {
        return 0;
    }
}
