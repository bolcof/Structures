using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Structure : MonoBehaviour
{
    [SerializeField] int id, cubeNum;
    [SerializeField] private Vector3
        area,
        rootPostion;
    [SerializeField] private float
        setVolume,
        setSurface,
        nowVolume,
        nowSurface,
        minSize,
        maxSize;
    [SerializeField] private List<Cube> cubes;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {

            int cubeID = 1;
            int tryCount = 0;
            cubeNum = 0;
            nowSurface = nowVolume = 0;

            foreach (Transform n in this.gameObject.transform)
            {
                GameObject.Destroy(n.gameObject);
            }

            Cube.ganarateRoot(0, area, minSize, maxSize);
            cubes.Add(GameObject.Find("rootCube").GetComponent<Cube>());


            while (cubeID <= 9)
            {
                Debug.Log("makeCube:" + cubeID.ToString());

                int i = Random.Range(0, cubes.Count);
                if(Cube.AddCube(cubeID, 0, cubes[i], area, minSize, maxSize, (setVolume-nowVolume), (setSurface - nowSurface))) { cubeID++; }
                else { tryCount++; }

                if (tryCount > 100) { cubeNum = cubeID; break; }
            }
        }
    }

    private float checkSurface ()
    {
        return 0.0f;
    }

    private float checkVolume()
    {
        return 0.0f;
    }

    private void output ()
    {

    }
}
