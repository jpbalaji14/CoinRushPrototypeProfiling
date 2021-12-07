using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrolling : MonoBehaviour
{
    [SerializeField]
    private GameObject[] mImages;

    [SerializeField]
    private float mScrollSpeed;

    public float rePositionSpot;

    void Start()
    {
        for (int i = 0; i < mImages.Length; i++)
        {
            rePositionSpot += this.transform.GetChild(i).GetComponent<RectTransform>().sizeDelta.y;
        }
    }

    
    void Update()
    {
        for (int i = 0; i < mImages.Length; i++)
        {
            mImages[i].transform.Translate(Vector3.up * Time.smoothDeltaTime * mScrollSpeed, Space.World);
            if(mImages[i].transform.localPosition.y > 2160f)
            {
                mImages[i].transform.localPosition = new Vector3(mImages[i].transform.localPosition.x, mImages[i].transform.localPosition.y - rePositionSpot, mImages[i].transform.localPosition.z);
            }
            //if (_reelElements[i]._slotElementGameObject.transform.localPosition.y < -600)
            //{
            //    _reelElements[i]._slotElementGameObject.transform.localPosition = new Vector3(_reelElements[i]._slotElementGameObject.transform.localPosition.x, _reelElements[i]._slotElementGameObject.transform.localPosition.y + accumalatedY, _reelElements[i]._slotElementGameObject.transform.localPosition.z);
            //    _reelElements[i]._slotElementGameObject.transform.SetSiblingIndex(i);
            //}
        }
    }
}
