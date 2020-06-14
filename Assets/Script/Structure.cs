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
    [SerializeField] public List<Cube> level0Cubes;

    [SerializeField] private int maxLevel = 0;
    [SerializeField] private int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in this.gameObject.transform)
        {
            cubes.Add(child.gameObject.GetComponent<Cube>());
            level0Cubes.Add(child.gameObject.GetComponent<Cube>());
        }
        Debug.Log("cube count:" + cubes.Count.ToString());

        for (count = 0; count < cubes.Count; count++) {
            cubes[count].level0Init(count, cubes.Count);
        }

        makeLevel1();

        bool continued = true;
        for(int i = 1; continued; i++)
        {
            continued = makeNextLevel(i);
        }

        nowVolume = nowSurface = 0.0f;
        for (int i = 0; i <= maxLevel; i++)
        {
            List<Cube> matchCubes = cubes.Where(C => C.level == i).ToList();
            for (int j = 0; j < matchCubes.Count; j++)
            {
                int seed = i % 2 == 0 ? 1 : -1;
                nowSurface += matchCubes[j].returnSurface() * seed;
                nowVolume += matchCubes[j].returnVolume() * seed;
            }
        }
    }

    bool makeNextLevel(int level){
        List<Cube> matchCubes = cubes.Where(C => C.level == level).ToList();
        Debug.Log("level:" + level.ToString() + " : " + matchCubes.Count.ToString());
        if (matchCubes.Count != 0)
        {
            for (int i = 0; i < matchCubes.Count; i++)
            {
                for (int j = searchMax(matchCubes[i].IsCovered); j < level0Cubes.Count; j++)
                {
                    if (!matchCubes[i].IsCovered[j])
                    {
                        Cube newCube = matchCubes[i].col(level0Cubes[j]);
                        if (newCube != null)
                        {
                            cubes.Add(newCube);
                            newCube.setIsCoverd(matchCubes[i].IsCovered, j);
                        }
                    }
                }
            }
            return true;
        }else{
            Debug.Log("generate complete : " + level.ToString());
            maxLevel = level - 1;
            return false;
        }
    }

    void makeLevel1()
    {
        for (int i = 0; i < level0Cubes.Count; i++)
        {
            for (int j = i + 1; j < level0Cubes.Count; j++)
            {
                Cube newCube = level0Cubes[i].col(level0Cubes[j]);
                if (newCube != null)
                {
                    cubes.Add(newCube);
                    newCube.setIsCoverd(level0Cubes[i].IsCovered, j);
                }
            }
        }
    }

    int searchMax (List<bool> source)
    {
        int max = -1;
        for(int i = 0; i < source.Count; i++)
        {
            if (source[i]) { max = i; }
        }
        return max;
    }

    private void outputBoth () {

        nowSurface = 0.0f;
        nowVolume = 0.0f;

    }
}
