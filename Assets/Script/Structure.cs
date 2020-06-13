using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Structure : MonoBehaviour
{
    [SerializeField] private float
        targetVolume,
        targetSurface,
        nowVolume,
        nowSurface;

    [SerializeField] private List<Cube> cubes;
    [SerializeField] private List<Cube> level0Cubes;

    [SerializeField] private int maxLevel = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in this.gameObject.transform)
        {
            cubes.Add(child.gameObject.GetComponent<Cube>());
            level0Cubes.Add(child.gameObject.GetComponent<Cube>());
        }
        Debug.Log("cube count:" + cubes.Count.ToString());

        for (int i = 0; i < cubes.Count; i++) {
            cubes[i].init(i, cubes.Count);
        }

        makeNextLevel(0);

        // while
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool makeNextLevel(int level){
        List<Cube> matchCubes = cubes.Where(C => C.level == level).ToList();
        Debug.Log("level:" + level.ToString() + " : " + matchCubes.Count.ToString());
        if (matchCubes.Count != 0)
        {
            for (int i = 0; i < matchCubes.Count; i++){
                for (int j = 0; j < level0Cubes.Count; j++)
                {
                    Cube newCube = matchCubes[i].col(level0Cubes[i]);
                    if(newCube != null){
                        cubes.Add(newCube);
                    }
                }
            }
            return true;
        }else{
            Debug.Log("generate complete : " + level.ToString());
            return false;
        }
    }

    private void outputBoth () {

        nowSurface = 0.0f;
        nowVolume = 0.0f;

    }
}
