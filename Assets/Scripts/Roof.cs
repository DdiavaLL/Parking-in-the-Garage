using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Описывает поведение черной крыши
public class Roof : MonoBehaviour
{
    public new Camera camera;
    Vector2 position;

    private void Start()
    {
        
        position = transform.position;
    }
    //Если мышка наводится на крышу, то скрываем ее, иначе показываем
    private void Update()
    {
        if (Mathf.Abs(camera.ScreenToWorldPoint(Input.mousePosition).x - position.x) < 3 && Mathf.Abs(camera.ScreenToWorldPoint(Input.mousePosition).y - position.y) < 3)
            gameObject.GetComponent<SpriteRenderer>().enabled = false;

        else
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }
  
}
