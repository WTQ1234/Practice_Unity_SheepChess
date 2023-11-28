using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoSingleton<InputManager>
{
    public bool canClick = true;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(world, Vector2.zero);
            if (hit.collider != null && canClick)
            {
                var obj = hit.transform.gameObject;
                if (obj.GetComponent<SpriteRenderer>().color == Color.white)
                {
                    Chess chess = obj.GetComponent<Chess>();
                    if (chess == null || chess.InProcession)
                    {
                        return;
                    }
                    PoolManager.Instance.Recycle<Chess>(chess);
                    Procession.Instance.AddToList(AnimalSpawner.Instance.SpawnAnimalByID(chess.ID, chess.transform.position));
                }
            }
        }
    }
}
