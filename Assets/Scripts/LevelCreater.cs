using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreater : MonoBehaviour
{
    public Chess curChess = null;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("°´ÏÂ¿Õ¸ñ¼ü");
            curChess = AnimalSpawner.Instance.ReleaseObj();
            print(curChess.transform.position);
        }
        if (curChess != null)
        {
            curChess.transform.localPosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonDown(0))
        {
            curChess = null;
        }
    }


}
