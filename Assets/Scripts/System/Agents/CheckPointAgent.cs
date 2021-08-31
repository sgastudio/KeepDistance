using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointAgent : MonoBehaviour
{
    public int maxPerLine = 10;
    public int maxLines = 2;

    BoxCollider bounding;

    // Start is called before the first frame update
    void Start()
    {

    }

    public Vector2 GetOffset(int id)
    {
        return GetIdOffset(id, maxPerLine, maxLines);
    }

    Vector2 GetIdOffset(int id, int maxPerline, int maxLimit)
    {
        int lines = maxLimit % maxPerline != 0 ? maxLimit / maxPerline + 1 : maxLimit / maxPerline;

        int widthX = (int)(transform.lossyScale.x / maxPerLine);
        int widthY = (int)(transform.lossyScale.z / maxLines);

        int posX = id % maxPerline;
        int posY = id / maxPerline;


        posX -= maxPerline / 2;
        posY -= lines / 2;


        return new Vector2(widthX * posX, widthY * posY);
    }
}
