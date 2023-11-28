using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chess : BasicPoolObj
{
    [SerializeField] List<GameObject> overlapList;
    [SerializeField] int id;
    public int ID => id;
    public bool Detect = true;
    public bool beOverlapped = false;
    public bool InProcession = false;
    public SpriteRenderer mSpriteRenderer;
    public SpriteRenderer mChildSpriteRender;

    public void SetID(int _id, bool _detect=true)
    {
        id = _id;
        Detect = _detect;
        mSpriteRenderer = GetComponent<SpriteRenderer>();
        mChildSpriteRender = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
    }    

    protected override void _SetActive(bool active = true)
    {
        if (active)
        {
            Detect = true;
            beOverlapped = false;
            InProcession = false;
            overlapList.Clear();
            mSpriteRenderer.color = Color.white;
            mChildSpriteRender.color = Color.white;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Detect)
            return;
        if (collision.gameObject == null)
            return;
        Chess chess = collision.gameObject.GetComponent<Chess>();
        if (chess == null)
            return;
        if (!chess.Detect)
            return;
        SpriteRenderer spriteRenderer = collision.gameObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            return;
        if (spriteRenderer.sortingOrder > mSpriteRenderer.sortingOrder)
        {
            if (!beOverlapped)//如果是第一次进入
            {
                mSpriteRenderer.color = Color.grey;
                mChildSpriteRender.color = Color.gray;
            }
            overlapList.Add(collision.gameObject);
            beOverlapped = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!Detect)
            return;
        if (collision.gameObject == null)
            return;
        Chess chess = collision.gameObject.GetComponent<Chess>();
        if (chess == null)
            return;
        if (!chess.Detect)
            return;
        SpriteRenderer spriteRenderer = collision.gameObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            return;
        if (spriteRenderer.sortingOrder > mSpriteRenderer.sortingOrder)
        {
            overlapList.Remove(collision.gameObject);
            if (beOverlapped && overlapList.Count == 0)
            {
                mSpriteRenderer.color = Color.white;
                mChildSpriteRender.color = Color.white;
            }
        }
    }
}
