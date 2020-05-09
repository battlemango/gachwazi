using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class CircleController : MonoBehaviour
{
    [SerializeField] RectTransform FxHolder;
    [SerializeField] Image CircleImg;
    [SerializeField] Image CircleCenterImg;

    [SerializeField] [Range(0, 1)] float progress = 0f;

    public ParticleSystem ps;
    
    private const float MAX_SCALE = 1.9f;
    private float currentScale = 1;

    private float runningTime = 0;
    private float speed = 10f;

    private void Start()
    {
        //initCircle();
    }
    // Update is called once per frame
    void Update()
    {
        updateCircle();
    }

    public void initCircle()
    {
        progress = 0;
        Color color = getRandomColor();
        CircleImg.color = color;
        CircleCenterImg.color = color;

        MainModule mainModule = ps.main;
        ParticleSystem.MinMaxGradient grad = new ParticleSystem.MinMaxGradient(color, color);
        mainModule.startColor = grad;

        this.gameObject.transform.localScale = new Vector3(1, 1, 1);
        currentScale = 1;
        runningTime = 0;

        updateCircle();
    }

    private Color32 getRandomColor()
    {
        int r = Random.Range(0, 256);
        int g = Random.Range(0, 256);
        int b = Random.Range(0, 256);

        return new Color32((byte)r, (byte)g, (byte)b, 255);
    }

    private void updateCircle()
    {
        CircleImg.fillAmount = progress;
        if(progress < 1)
        {
            progress += 0.5f * Time.deltaTime;
            if (progress > 1)
                progress = 1;
        }
        FxHolder.rotation = Quaternion.Euler(new Vector3(0f, 0f, -progress * 360));

        updateContainer();
    }

    private void updateContainer()
    {
        if (currentScale < MAX_SCALE)
        {
            currentScale += 2f * Time.deltaTime;
            if (currentScale > MAX_SCALE)
                currentScale = MAX_SCALE;
        }

        runningTime += Time.deltaTime * speed;
        float addtionalScale = Mathf.Sin(runningTime) * 0.07f;

        this.gameObject.transform.Rotate(0, 0, -250f * Time.deltaTime);
        this.gameObject.transform.localScale = new Vector3(currentScale + addtionalScale, currentScale + addtionalScale, 1);
    }

    public Color32 getColor32()
    {
        return CircleImg.color;
    }
}
