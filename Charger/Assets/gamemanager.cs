using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gamemanager : MonoBehaviour
{
    private const int STATE_GAME = 0;
    private const int STATE_END = 1;
    private int mState = STATE_GAME;

    public List<touchlocation> touches = new List<touchlocation>();
    public GameObject[] circles;
    private Canvas cameraCanvas;
    public GameObject mImgClear;
    private ClearImageController mClearImageController;

    const float MAX_END_TIME = 3f;
    float mEndTime = 2f;

    const float MAX_END_IMGAE_SHOW_TIME = 4f;
    float mCurEndImgShowTime = 0;

    bool mIsClicked = false;
    bool mIsTimeout = false;

    int mCurrentTurnForCircle = 0;

    private void Start()
    {
        cameraCanvas = GameObject.Find("CameraCanvas").GetComponent<Canvas>();
        mClearImageController = mImgClear.GetComponent<ClearImageController>();
    }
    // Update is called once per frame
    void Update()
    {
        switch (mState)
        {
            case STATE_GAME:
                updateGameState();
                break;
            case STATE_END:
                updateEndState();
                break;
        }
       
    }

    private void updateGameState()
    {
        int i = 0;
        if (Application.isMobilePlatform)
        {
            while (i < Input.touchCount)
            {
                Touch t = Input.GetTouch(i);
                if (t.phase == TouchPhase.Began)
                {
                    Debug.Log("touch began");
                    touches.Add(new touchlocation(t.fingerId, getCircle(touches.Count), t.position));
                    resetEndTime();
                }
                else if (t.phase == TouchPhase.Ended)
                {
                    Debug.Log("touch ended");
                    touchlocation thisTouch = touches.Find(touchLocation => touchLocation.touchId == t.fingerId);
                    thisTouch.dismissCircle();
                    touches.RemoveAt(touches.IndexOf(thisTouch));
                    resetEndTime();
                }
                else if (t.phase == TouchPhase.Moved)
                {
                    Debug.Log("touch is moving");
                    touchlocation thisTouch = touches.Find(touchLocation => touchLocation.touchId == t.fingerId);
                    thisTouch.moveCircle(t.position);
                    //thisTouch.circle.transform.position = getTouchPosition(t.position);
                }
                ++i;
            }
        }
        else
        {

            if (Input.GetButtonDown("Fire1"))
            {
                showCircle(circles[0], Input.mousePosition);
                mIsClicked = true;
                resetEndTime();
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                dismissCircle(circles[0]);
                mIsClicked = false;
                resetEndTime();
            }
            else if (Input.GetButton("Fire1"))
            {
                moveCircle(circles[0], Input.mousePosition);
                //circles[0].SetActive(true);
            }
        }

        checkGameEnd();
    }

    private void updateEndState()
    {
        mCurEndImgShowTime += Time.deltaTime;
        if(mCurEndImgShowTime > MAX_END_IMGAE_SHOW_TIME)
        {
            if (Input.touchCount == 0|| !mIsClicked)
            {
                stopEndAnimaition();
                allClear();
            }
        }        
    }

    private void allClear()
    {
        mIsClicked = false;
        mIsTimeout = false;
        for (int i=0; i< circles.Length; i++)
        {
            dismissCircle(circles[i]);
        }
        touches.Clear();
    }

    private void checkGameEnd()
    {
        if(touches.Count >1 || mIsClicked)
        {
            mEndTime -= Time.deltaTime;
            if(mEndTime < 0)
            {
                startEndAnimaition();
            }else if(mEndTime < 1 && mIsTimeout == false)
            {
                mIsTimeout = true;
                SoundManager.sInstance.playOneShot(AudioClipManager.sinstance.AC_TIME_OUT);
            }
        }
        
    }

    private void startEndAnimaition()
    {
        mCurEndImgShowTime = 0;
        mState = STATE_END;

        if (mIsClicked)
        {
            CircleController circleController = circles[0].GetComponent<CircleController>();

            mClearImageController.startAnimaition(circles[0].transform.position, circleController.getColor32());
        }
        else
        {
            // right case
            //int onePerson = Random.Range(0, touches.Count);

            // wrong case
            int onePerson = Random.Range(0, touches.Count - 1);


            touchlocation thisTouch = touches[onePerson];

            foreach(touchlocation tl in touches)
            {
                if (tl == thisTouch)
                    continue;

                tl.dismissCircle();
            }

            mClearImageController.startAnimaition(thisTouch.getPos(), thisTouch.getColor32());
        }
    }

    private void stopEndAnimaition()
    {
        mClearImageController.stopAnimation();
        mState = STATE_GAME;
    }

    private void resetEndTime()
    {
        mEndTime = MAX_END_TIME;
        mIsTimeout = false;
    }

    private GameObject getCircle(int index)
    {
        GameObject circle = circles[mCurrentTurnForCircle];
        mCurrentTurnForCircle++;
        if(mCurrentTurnForCircle >= circles.Length)
        {
            mCurrentTurnForCircle = 0;
        }
        return circle;
    }

    private void showCircle(GameObject go, Vector3 pos)
    {
        CircleController controller = go.GetComponentInChildren<CircleController>();
        controller.initCircle();
        moveCircle(go, pos);
        go.SetActive(true);
        SoundManager.sInstance.playOneShot(AudioClipManager.sinstance.AC_GEM_GET);
    }

    private void moveCircle(GameObject go, Vector3 pos)
    {
        pos.z = cameraCanvas.planeDistance;
        Camera renderCamera = cameraCanvas.worldCamera;
        Vector3 canvasPos = renderCamera.ScreenToWorldPoint(pos);

        go.transform.position = canvasPos;
    }

    private void dismissCircle(GameObject go)
    {
        go.SetActive(false);
    }

    Vector2 getTouchPosition(Vector2 touchPosition)
    {
        return GetComponent<Camera>().ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, transform.position.z));
    }

}

