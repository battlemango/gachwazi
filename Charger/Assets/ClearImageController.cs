using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearImageController : MonoBehaviour
{
    const float MIN_SCALE = 1.9f;
    const float INIT_SCALE = 25f;
    const float SCALE_REDUCE_SPEED = 55f;

    float mCurScale = INIT_SCALE;

    void Start()
    {
        initImg();
    }

    // Update is called once per frame
    void Update()
    {
        if(mCurScale > MIN_SCALE)
        {
            mCurScale -= SCALE_REDUCE_SPEED * Time.deltaTime;
            if(mCurScale < MIN_SCALE)
            {
                mCurScale = MIN_SCALE;
            }
        }

        setScale();
    }

    private void initImg()
    {
        mCurScale = INIT_SCALE;
        setScale();
    }

    public void startAnimaition(Vector3 pos, Color32 color)
    {
        initImg();
        GetComponent<Image>().color = color;
        this.gameObject.transform.position = pos;
        this.gameObject.SetActive(true);
        //Handheld.Vibrate();
    }


    public void startAnimaition()
    {
        initImg();
        this.gameObject.SetActive(true);
    }
    public void stopAnimation()
    {
        this.gameObject.SetActive(false);
    }

    private void setScale()
    {
        this.gameObject.transform.localScale = new Vector3(mCurScale, mCurScale, 1);
    }
}
