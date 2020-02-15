using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickDect_sprint2 : MonoBehaviour
{
    public LayerMask clickMask;

    private int shape = 0;

    private GameObject primitive;
    private float red = 1.0f, blue = 1.0f, green = 1.0f, destroyTime = 3f, timeToDestroy = 3f, Xcutoff;
    public Text mousePosition, blueAm, redEm, greenAm, sizeAm, timeAm, animAm;

    [SerializeField]
    private float distance = 5f, distanceChange;
    private bool timeDestroyIsOn = true, isAmimTyleRandom, isSpawnTypeRandom, isSpawnTimeRandom, isAnimSpeedRandom;
    private float size = 1.0f;


    private Vector3 lastClickPos = Vector3.zero;
    public Text lifeTime;


    public GameObject paintedObj00, paintedObj01, paintedObj02, explosion;
    private Color paintObjColor, painedObjEm;

    [SerializeField]
    [Range(0.0f, 2f)]


    private float emissionStr = 1f;
    private float opacityStr = 0.7f;

    private int animationState = 0;
    private float animationSpeed = 1f;
    public Dropdown aminDropDown, shapeDropDown;
    private Vector3 clickPosition;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) aminDropDown.value = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) aminDropDown.value = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) aminDropDown.value = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4)) ChangeSpawnObj(0);
        if (Input.GetKeyDown(KeyCode.Alpha5)) ChangeSpawnObj(1);
        if (Input.GetKeyDown(KeyCode.Alpha6)) ChangeSpawnObj(2);



        if (Input.GetMouseButtonDown(0)) //left click
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.layer == 9)  // clock face
                {
                    hit.transform.parent.GetComponent<clock>().UpdateTime(hit.transform.localEulerAngles.y);

                }

            }
        }
        if (Input.GetMouseButton(0) && (EventSystem.current.currentSelectedGameObject == null)) // right hold
        {
            //clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0f, 0f, distance));
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.layer == 10)  //painted object
                {
                    Destroy(hit.transform.gameObject);
                    primitive = Instantiate(explosion, hit.transform.position, Quaternion.identity);
                    Destroy(primitive, 1f);

                }

            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            lastClickPos = Vector3.zero;
        }




        if (Input.GetMouseButton(0) && (EventSystem.current.currentSelectedGameObject == null) && Input.mousePosition.x > Xcutoff)
        {
            clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0f, 0f, distance));

            if (isSpawnTypeRandom)
            {
                changeShape((int)Random.Range(0.0f, 2.99999f));
            }


            switch (shape)
            {
                case 0:
                    primitive = Instantiate(paintedObj00, clickPosition, Quaternion.identity);
                    break;
                case 1:
                    //primitive = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                    primitive = Instantiate(paintedObj01, clickPosition, Quaternion.identity);
                    break;
                case 2:
                    primitive = Instantiate(paintedObj02, clickPosition, Quaternion.identity);

                    //primitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    break;
                default:
                    break;

            }

            if (lastClickPos == Vector3.zero) primitive.transform.localScale = new Vector3(Random.Range(0.1f, 1f) * size, Random.Range(0.1f, 1f) * size, Random.Range(0.1f, 1f * size));
            else
            {
                float x = Mathf.Clamp(Random.Range(size, size * 6f) * Mathf.Abs(lastClickPos.x - clickPosition.x), .1f, size * 10f);
                float y = Mathf.Clamp(Random.Range(size, size * 6f) * Mathf.Abs(lastClickPos.y - clickPosition.y), .1f, size * 10f);
                float z = (x + y) / 2f;
                primitive.transform.localScale = new Vector3(x, y, z);
            }

            //randomize color

            if (primitive.GetComponent<Renderer>() != null)
            {
                paintObjColor = new Color(Random.Range(0.0f, red), Random.Range(0.0f, green), Random.Range(0.0f, blue), opacityStr);
                primitive.GetComponent<Renderer>().material.color = paintObjColor;
                painedObjEm = new Color(paintObjColor.r * emissionStr, paintObjColor.g * emissionStr, paintObjColor.b * emissionStr);
                primitive.GetComponent<Renderer>().material.SetColor("_EmissionColor", painedObjEm);

            }

            else { }

            foreach (Transform child in primitive.transform)
            {
                if (child.gameObject.GetComponent<Renderer>() != null)
                {
                    paintObjColor = new Color(Random.Range(0.0f, red), Random.Range(0.0f, blue), Random.Range(0.0f, green), opacityStr);
                    primitive.GetComponent<Renderer>().material.color = paintObjColor;
                    primitive.gameObject.GetComponent<PrefabData>().initialColorInfo.Add(paintObjColor);


                    painedObjEm = new Color(paintObjColor.r * emissionStr, paintObjColor.g * emissionStr, paintObjColor.b * emissionStr);
                    primitive.GetComponent<Renderer>().material.SetColor("_EmissionColor", painedObjEm);
                }
            }

            if (primitive.GetComponent<Animator>() != null)
            {
                if (isAmimTyleRandom)
                {
                    animationState = (int)Random.Range(0f, 2.99f);
                    aminDropDown.value = animationState;
                }
                primitive.GetComponent<Animator>().SetInteger("state", animationState);

                if (isAnimSpeedRandom) primitive.GetComponent<Animator>().speed = Random.Range(0f, animationSpeed);
                primitive.gameObject.GetComponent<PrefabData>().initialAnimSpeed = primitive.GetComponent<Animator>().speed;

            }

            primitive.transform.parent = this.transform;
            if (timeDestroyIsOn)
            {
                if (isSpawnTimeRandom) Destroy(primitive, Random.Range(Mathf.Clamp(timeToDestroy - 1f, 0f, timeToDestroy), timeToDestroy + 1f));
                else Destroy(primitive, timeToDestroy);
            }

            lastClickPos = clickPosition;
        }
        mousePosition.text = "Mouse Pos of X: " + Input.mousePosition.x.ToString("F0") + ", Y: " + Input.mousePosition.y.ToString("F0");
    }






    public void changeShape(int temp)
    {
        shape = temp;
        shapeDropDown.value = temp;
    }

    public void changePrefabTypeRandom(bool temp)
    {
        isSpawnTypeRandom = temp;
    }

    public void changePrefabTimeRandom(bool temp)
    {
        isSpawnTimeRandom = temp;
    }

    public void changeAnimTypeRandom(bool temp)
    {
        isAmimTyleRandom = temp;
    }

    public void changeAnimSpeedRandom(bool temp)
    {
        isAnimSpeedRandom = temp;
    }


    public void ChangeAnimState(int temp)
    {
        animationState = temp;

        if (!isAmimTyleRandom)
        {
            foreach (Transform child in transform)
            {
                if (child.gameObject.GetComponent<Animator>() != null)
                {
                    child.gameObject.GetComponent<Animator>().SetInteger("state", animationState);

                }
            }
        }
    }

    public void ChangeAnimSpeed(float temp)
    {
        animationSpeed = temp;
        animAm.text = animationSpeed.ToString("F1") + "X";

        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Animator>() != null)
            {
                child.gameObject.GetComponent<Animator>().speed = child.GetComponent<PrefabData>().initialAnimSpeed * temp;

            }
        }

    }


    public void ChangeSpawnObj(int temp)
    {
        shape = temp;
        shapeDropDown.value = temp;

    }


    public void changeToRed(float temp)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Renderer>() != null)
            {
                paintObjColor = child.GetComponent<Renderer>().material.GetColor("_Color");
                paintObjColor.r = temp;
                child.GetComponent<Renderer>().material.SetColor("_Color", paintObjColor);
            }

            int childCount = 0;
            foreach (Transform grandchild in child.transform)
            {
                if (grandchild.gameObject.GetComponent<Renderer>() != null)
                {
                    paintObjColor = grandchild.GetComponent<Renderer>().material.GetColor("_Color");
                    paintObjColor.r = child.GetComponent<PrefabData>().initialColorInfo[childCount].r * temp;
                    grandchild.GetComponent<Renderer>().material.SetColor("_EmissionColor", painedObjEm);

                    painedObjEm = new Color(paintObjColor.r * emissionStr, paintObjColor.g * emissionStr, paintObjColor.b * emissionStr);
                    grandchild.GetComponent<Renderer>().material.SetColor("_EmissionColor", painedObjEm);
                }
                childCount++;

            }
        }
        red = temp;
        redEm.text = (red * 100f).ToString("F0");
    }



    public void changeToGreen(float temp)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Renderer>() != null)
            {
                paintObjColor = child.GetComponent<Renderer>().material.GetColor("_Color");
                paintObjColor = new Color(paintObjColor.r, temp, paintObjColor.b, paintObjColor.a);

                child.GetComponent<Renderer>().material.SetColor("_Color", paintObjColor);
            }

            int childCount = 0;
            foreach (Transform grandchild in child.transform)
            {
                if (grandchild.gameObject.GetComponent<Renderer>() != null)
                {
                    paintObjColor = grandchild.GetComponent<Renderer>().material.GetColor("_Color");
                    paintObjColor.r = child.GetComponent<PrefabData>().initialColorInfo[childCount].g * temp;
                    grandchild.GetComponent<Renderer>().material.SetColor("_EmissionColor", painedObjEm);

                    painedObjEm = new Color(paintObjColor.r * emissionStr, paintObjColor.g * emissionStr, paintObjColor.b * emissionStr);
                    grandchild.GetComponent<Renderer>().material.SetColor("_EmissionColor", painedObjEm);
                }
                childCount++;

            }
        }
        green = temp;
        greenAm.text = (green * 100f).ToString("F0");
    }


    public void changeToBlue(float temp)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Renderer>() != null)
            {
                paintObjColor = child.GetComponent<Renderer>().material.GetColor("_Color");
                paintObjColor = new Color(paintObjColor.r, temp, paintObjColor.b, paintObjColor.a);

                child.GetComponent<Renderer>().material.SetColor("_Color", paintObjColor);
            }

            int childCount = 0;
            foreach (Transform grandchild in child.transform)
            {
                if (grandchild.gameObject.GetComponent<Renderer>() != null)
                {
                    paintObjColor = grandchild.GetComponent<Renderer>().material.GetColor("_Color");
                    paintObjColor.r = child.GetComponent<PrefabData>().initialColorInfo[childCount].b * temp;
                    grandchild.GetComponent<Renderer>().material.SetColor("_EmissionColor", painedObjEm);

                    painedObjEm = new Color(paintObjColor.r * emissionStr, paintObjColor.g * emissionStr, paintObjColor.b * emissionStr);
                    grandchild.GetComponent<Renderer>().material.SetColor("_EmissionColor", painedObjEm);
                }
                childCount++;

            }
        }
        blue = temp;
        blueAm.text = (blue * 100f).ToString("F0");
    }





    public void ChangeEmStr(float temp)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Renderer>() != null)

            {
                paintObjColor = child.GetComponent<Renderer>().material.GetColor("_Color");
                painedObjEm = new Color(paintObjColor.r* temp, paintObjColor.g * temp, paintObjColor.b * temp);
                child.GetComponent<Renderer>().material.SetColor("_EmissionColor", painedObjEm);

            }
            foreach (Transform grandchild in child.transform)
            {
                if (grandchild.gameObject.GetComponent<Renderer>() != null)

                {
                    paintObjColor = grandchild.GetComponent<Renderer>().material.GetColor("_Color");
                    painedObjEm = new Color(paintObjColor.a * temp, paintObjColor.a * temp, paintObjColor.a * temp);
                    grandchild.GetComponent<Renderer>().material.SetColor("_EmissionColor", painedObjEm);

                }

            }


        }
        emissionStr = temp;
    }

    public void ChangeOpacityStr(float temp)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Renderer>() != null)

            {
                paintObjColor = child.GetComponent<Renderer>().material.GetColor("_Color");
                paintObjColor.a = temp;
                child.GetComponent<Renderer>().material.SetColor("_Color", paintObjColor);

            }
            foreach (Transform grandchild in child.transform)
            {
                if (grandchild.gameObject.GetComponent<Renderer>() != null)

                {
                    paintObjColor = grandchild.GetComponent<Renderer>().material.GetColor("_Color");
                    paintObjColor.a = temp;
                    grandchild.GetComponent<Renderer>().material.SetColor("_Color", paintObjColor);

                }

            }


        }
        opacityStr = temp;
    }



    public void DestroyObjects()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
            primitive = Instantiate(explosion, child.position, Quaternion.identity);
            Destroy(primitive, 1f);
        }
    }

    public void ToggleTimeDestroy(bool timer)
    {
        timeDestroyIsOn = timer;
    }


    public void ChangeSize(float temp)
    {
        foreach (Transform child in transform)
        {
            child.localScale = child.localScale * temp / size;
        }
        size = temp;
    }

    public void ChangeTimeToDestroy(float temp)
    {
        timeToDestroy = temp;
        timeAm.text = timeToDestroy.ToString("F0" + " Sec");
    }


}







