using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureObjectLayout : MonoBehaviour
{
    public GameObject ObjTemplate;
    private GameObject[] objArr;

    private int screenWidth;
    private int screenHeight;

    private int objWidth;
    private int objHeight;

    // Use this for initialization
    void Awake ()
    {
        screenWidth = 640;
        screenHeight = 480;

        objWidth = 100;
        objHeight = 100;

        var rowCount = screenWidth / objWidth;
        var colCount = screenHeight / objHeight;

        var rowCountHalf = rowCount / 2;
        var colCountHalf = colCount / 2;

        var objList = new List<GameObject>();

        for (int i = -colCountHalf; i < colCountHalf; i++)
        {
            for (int j = -rowCountHalf; j < rowCountHalf; j++)
            {
                var obj = Instantiate(ObjTemplate, ObjTemplate.transform.parent);
                obj.transform.localScale = new Vector3(objWidth, objHeight, 1);
                obj.transform.localPosition = new Vector3(objWidth * j, objHeight * i, 0);

                objList.Add(obj);
            }
        }

        objArr = objList.ToArray();
    }
}
