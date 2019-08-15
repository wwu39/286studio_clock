﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Moving { None, Left, Right }

public class Swipable : MonoBehaviour
{
    public GameObject Left;
    public GameObject Right;
    public GameObject GameSpace;

    RectTransform rt;
    public Moving moving;
    public Moving mv_pos;

    float startTime = -1;
    public float speed = 3000F;


    float EndPoint;
    Vector3 GS_EndPoint;

    // Start is called before the first frame update
    void Start()
    {
        EndPoint = -AppManager.AdjustedWidth;
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(-Screen.width, 0, 0));
        pos.y = 0;
        pos.z = 0;
        GS_EndPoint = pos;

        rt = GetComponent<RectTransform>();
        moving = Moving.None;
        mv_pos = Moving.Right;
        Right.GetComponent<RectTransform>().anchoredPosition = new Vector3(AppManager.AdjustedWidth, 0, 0);
        Left.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (moving == Moving.None)
        {
            if (swipeRight() && mv_pos == Moving.Left)
            {
                moving = Moving.Right;
                startTime = Time.time;
            }

            if (swipeLeft() && mv_pos == Moving.Right)
            {
                moving = Moving.Left;
                startTime = Time.time;
            }
        }
    }

    private void FixedUpdate()
    {
        if (startTime > 0)
        {
            if (moving==Moving.Left)
            {
                moveLeft();
            }

            if (moving==Moving.Right)
            {
                moveRight();
            }
        }
    }

    bool swipeLeft()
    {

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.A)) return true;
#endif

        foreach (Touch touch in Input.touches)
        {
            return touch.deltaPosition.x / touch.deltaTime < -2000;
        }
        return false;
    }

    bool swipeRight()
    {

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.S)) return true;
#endif
        foreach (Touch touch in Input.touches)
        {
            return touch.deltaPosition.x / touch.deltaTime > 2000;
        }
        return false;
    }

    void moveLeft()
    {
        // deal with canvas space
        // Distance moved = time * speed.
        float distCovered = (Time.time - startTime) * speed;

        // Fraction of journey completed = current distance divided by total distance.
        float fracJourney = distCovered / AppManager.AdjustedWidth;
        rt.anchoredPosition = Vector3.Lerp(Vector3.zero, new Vector3(EndPoint, 0, 0), fracJourney);


        if (System.Math.Abs(EndPoint - rt.anchoredPosition.x) < double.Epsilon)
        {
            startTime = -1;
            moving = Moving.None;
            mv_pos = Moving.Left;
        }

        // deal with game space
        GameSpace.transform.position = Vector3.Lerp(Vector3.zero, GS_EndPoint, fracJourney);
    }

    void moveRight()
    {
        // Distance moved = time * speed.
        float distCovered = (Time.time - startTime) * speed;

        // Fraction of journey completed = current distance divided by total distance.
        float fracJourney = distCovered / AppManager.AdjustedWidth;

        rt.anchoredPosition = Vector3.Lerp(new Vector3(EndPoint, 0, 0), Vector3.zero, fracJourney);

        if (System.Math.Abs(rt.anchoredPosition.x) < double.Epsilon)
        {
            startTime = -1;
            moving = Moving.None;
            mv_pos = Moving.Right;
        }

        // deal with game space
        GameSpace.transform.position = Vector3.Lerp(GS_EndPoint, Vector3.zero, fracJourney);
    }
}