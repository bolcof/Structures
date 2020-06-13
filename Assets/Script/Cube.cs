using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Cube : MonoBehaviour
{
    [SerializeField] public int id, level;
    [SerializeField] List<bool> isCovered;

    public void init (int id, int length) {
        this.id = id;
        for (int i = 0; i < length; i++)
        {
            isCovered.Add(false);
            if(i == id){
                isCovered[i] = true;
            }
        }
    }

    public void set (Vector3 pos, Vector3 size) {
        this.gameObject.transform.localPosition = pos;
        this.gameObject.transform.localScale = size;
    }

    public Cube col (Cube other) {

        Vector3 thisPos = this.gameObject.transform.localPosition;
        Vector3 thisSize = this.gameObject.transform.localScale;
        Vector3 otherPos = other.gameObject.transform.localPosition;
        Vector3 otherSize = other.gameObject.transform.localScale;

        bool hitX = Mathf.Abs(otherPos.x - thisPos.x) < (otherSize.x + thisSize.x) / 2;
        bool hitY = Mathf.Abs(otherPos.y - thisPos.y) < (otherSize.y + thisSize.y) / 2;
        bool hitZ = Mathf.Abs(otherPos.z - thisPos.z) < (otherSize.z + thisSize.z) / 2;

        bool isHit = hitX && hitY && hitZ;

        if(isHit){
            Vector3 newCubeSize = new Vector3(
                ((otherSize.x + thisSize.x) / 2) - Mathf.Abs(otherPos.x - thisPos.x),
                ((otherSize.y + thisSize.y) / 2) - Mathf.Abs(otherPos.y - thisPos.y),
                ((otherSize.z + thisSize.z) / 2) - Mathf.Abs(otherPos.z - thisPos.z)
            );

            Vector3 newCubePos;
            if(otherPos.x - thisPos.x >= 0){
                newCubePos.x = (thisSize.x - newCubeSize.x) / 2;
            }else{
                newCubePos.x = - (thisSize.x - newCubeSize.x) / 2;
            }

            if(otherPos.y - thisPos.y >= 0){
                newCubePos.y = (thisSize.y - newCubeSize.y) / 2;
            }else{
                newCubePos.y = - (thisSize.y - newCubeSize.y) / 2;
            }

            if(otherPos.z - thisPos.z >= 0){
                newCubePos.z = (thisSize.z - newCubeSize.z) / 2;
            }else{
                newCubePos.z = - (thisSize.z - newCubeSize.z) / 2;
            }

            GameObject newCube = Instantiate(Resources.Load("CubeOrigin"), this.gameObject.transform.parent) as GameObject;
            newCube.GetComponent<Cube>().set(newCubePos, newCubeSize);
            return newCube.GetComponent<Cube>();
        }else{
            return null;
        }
    }
}
