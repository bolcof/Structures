using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Cube : MonoBehaviour
{
    [SerializeField] public int id, level;
    [SerializeField] private List<bool> isCovered;

    public List<bool> IsCovered => isCovered;

    public void level0Init (int id, int length) {
        this.id = id;
        for (int i = 0; i < length; i++)
        {
            isCovered.Add(false);
            if(i == id){
                isCovered[i] = true;
            }
        }
    }

    public void set (int level, Vector3 pos, Vector3 size) {
        this.level = level;
        this.gameObject.transform.localPosition = pos;
        this.gameObject.transform.localScale = size;
    }

    public void setIsCoverd (List<bool> parentStats, int otherID)
    {
        for (int i = 0; i < parentStats.Count; i++) {
            this.isCovered.Add(parentStats[i]);
        }
        this.isCovered[otherID] = true;
    }

    public Cube col (Cube other) {

        Vector3 thisSize = this.gameObject.transform.localScale;
        Vector3 thisPos = this.gameObject.transform.localPosition;
        Vector3 thisPosMin = this.gameObject.transform.localPosition - thisSize / 2;
        Vector3 thisPosMax = this.gameObject.transform.localPosition + thisSize / 2;

        Vector3 otherSize = other.gameObject.transform.localScale;
        Vector3 otherPos = other.gameObject.transform.localPosition;
        Vector3 otherPosMin = other.gameObject.transform.localPosition - otherSize / 2;
        Vector3 otherPosMax = other.gameObject.transform.localPosition + otherSize / 2;

        bool hitX = Mathf.Abs(otherPos.x - thisPos.x) < (otherSize.x + thisSize.x) / 2;
        bool hitY = Mathf.Abs(otherPos.y - thisPos.y) < (otherSize.y + thisSize.y) / 2;
        bool hitZ = Mathf.Abs(otherPos.z - thisPos.z) < (otherSize.z + thisSize.z) / 2;

        bool isHit = hitX && hitY && hitZ;

        if(isHit){
            Vector3 newCubeSize = new Vector3(
                Mathf.Min(thisPosMax.x, otherPosMax.x) - Mathf.Max(thisPosMin.x, otherPosMin.x),
                Mathf.Min(thisPosMax.y, otherPosMax.y) - Mathf.Max(thisPosMin.y, otherPosMin.y),
                Mathf.Min(thisPosMax.z, otherPosMax.z) - Mathf.Max(thisPosMin.z, otherPosMin.z)
            );

            Vector3 newCubePos = new Vector3(
                Mathf.Max(thisPosMin.x, otherPosMin.x) + newCubeSize.x / 2,
                Mathf.Max(thisPosMin.y, otherPosMin.y) + newCubeSize.y / 2,
                Mathf.Max(thisPosMin.z, otherPosMin.z) + newCubeSize.z / 2
                );
            

            GameObject newCube = Instantiate(Resources.Load("CubeOrigin"), this.gameObject.transform.parent) as GameObject;
            newCube.GetComponent<Cube>().set(this.level+1, newCubePos, newCubeSize);
            return newCube.GetComponent<Cube>();
        }else{
            return null;
        }
    }

    public float returnSurface()
    {
        Vector3 thisScale = this.gameObject.transform.localScale;

        return
            (thisScale.x * thisScale.y * 2) +
            (thisScale.y * thisScale.z * 2) +
            (thisScale.z * thisScale.x * 2);
    }

    public float returnVolume()
    {
        Vector3 thisScale = this.gameObject.transform.localScale;

        return thisScale.x * thisScale.y * thisScale.z;
    }
}
