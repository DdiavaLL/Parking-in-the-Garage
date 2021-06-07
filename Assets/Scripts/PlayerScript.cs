using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Threading;
using System;
using System.Linq;
//описывает поведение нашей машинки
public class PlayerScript : MonoBehaviour
{
    // Start is called before the first frame update

    Rigidbody2D rb2;
    Vector3 rotate;
    Vector2 finish;
    [SerializeField]
    private float rotationSpeed;
    Sensor rays;
    public float RotationSpeed { get => rotationSpeed; set => rotationSpeed = value; }
    [SerializeField]
    bool isBack;
    bool CanRotate;
    [HideInInspector]
    bool rotationforward;
    public Vector3 rotateBack;
    public Vector3 rotateForward;
    public bool isFinish;

    bool CenterSensor, LeftSensor, RightSensor;

    [SerializeField]
    private float speed = 2f;
    public float Speed { get => speed; set => speed = value; }
    //Класс для крыши гаража, чтобы она красиво появлялась и пропадала.
    static class RoofOfGarage
    {
        static public SpriteRenderer Roof;
        static public int ring = 0; //счетчик кругов-коллайдеров
    }
    void Start()
    {
        CanRotate = true;
        rb2 = GetComponent<Rigidbody2D>();
        finish = GameObject.FindGameObjectWithTag("Finish").transform.position;
        rays = new Sensor(transform.position);
        isBack = true;
        isFinish = CenterSensor = LeftSensor = RightSensor = false;
    }

    // Update is called once per frame
    void Update()
    {
        MovingCar();
    }

    void MovingCar()
    {
        /* float totalMovementTime = 100f; //the amount of time you want the movement to take
         float currentMovementTime = 0;//The amount of time that has passed
         if (Mathf.Abs(transform.position.x - finish.x) > 0.001f || Mathf.Abs(transform.position.y - finish.y) > 0.001f)
         {

             currentMovementTime += Speed;
             transform.localPosition = Vector3.Lerp(transform.position, finish, currentMovementTime / totalMovementTime);
             rays.MovementRays(transform.position);
         }
         if (Mathf.Abs(Mathf.Tan((90 + transform.rotation.eulerAngles.z) * Mathf.Deg2Rad) - (finish.y - transform.position.y) / (finish.x - transform.position.x)) > 0.1f)
         {
             transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
             rays.RotateRays(transform);
         }*/


        if (isBack)
            LogicScriptBack();
        else LogicScriptForward();
        rays.MovementRays(transform.position);
        rays.RotateRays(transform);
        DebugRays();
    }

    //Прорисовывает лучи и двигает их
    void DebugRays()
    {

        Debug.DrawRay(rays.CenterForward.origin, rays.CenterForward.direction * 5f);
        Debug.DrawRay(rays.LeftForward.origin, rays.LeftForward.direction * 5f);
        Debug.DrawRay(rays.RightForward.origin, rays.RightForward.direction * 5f);
        Debug.DrawRay(rays.RightBack.origin, rays.RightBack.direction * 5f);
        Debug.DrawRay(rays.LeftBack.origin, rays.LeftBack.direction * 5f);
        Debug.DrawRay(rays.CenterBack.origin, rays.CenterBack.direction * 5f);
        RaycastHit2D[] hit = Physics2D.RaycastAll(rays.CenterForward.origin, rays.CenterForward.direction, 5f);

        // Если что-то ударит ...
        if (hit.Length > 1 && hit[1].collider != null)
        {
            Debug.Log("Попадание центрального переднего в " + hit[1].collider.gameObject.name);
            Debug.Log("Расстояние: " + hit[1].distance);
        }

        hit = Physics2D.RaycastAll(rays.LeftForward.origin, rays.LeftForward.direction * 3f, 3f);
        if (hit.Length > 1 && hit[1].collider != null)
        {
            Debug.Log("Попадание левого переднего в " + hit[1].collider.gameObject.name);
            Debug.Log("Расстояние: " + hit[1].distance);
        }

        hit = Physics2D.RaycastAll(rays.RightForward.origin, rays.RightForward.direction * 3f, 3f);
        if (hit.Length > 1 && hit[1].collider != null)
        {
            Debug.Log("Попадание правого переднего в " + hit[1].collider.gameObject.name);
            Debug.Log("Расстояние: " + hit[1].distance);
        }

        hit = Physics2D.RaycastAll(rays.CenterBack.origin, rays.CenterBack.direction * 3f, 3f);
        if (hit.Length > 1 && hit[1].collider != null)
        {
            Debug.Log("Попадание центрального заднего в " + hit[1].collider.gameObject.name);
            Debug.Log("Расстояние: " + hit[1].distance);
        }

        hit = Physics2D.RaycastAll(rays.LeftBack.origin, rays.LeftBack.direction * 3f, 3f);
        if (hit.Length > 1 && hit[1].collider != null)
        {
            Debug.Log("Попадание левого заднего в " + hit[1].collider.gameObject.name);
            Debug.Log("Расстояние: " + hit[1].distance);
        }

        hit = Physics2D.RaycastAll(rays.RightBack.origin, rays.RightBack.direction * 3f, 3f);
        if (hit.Length > 1 && hit[1].collider != null)
        {
            Debug.Log("Попадание правого заднего в " + hit[1].collider.gameObject.name);
            Debug.Log("Расстояние: " + hit[1].distance);
        }


    }

    //Расстояние от точки до гаража
    float Distance(Vector2 point)
    {
        return Vector2.Distance(point, finish);
    }

    //Логика для задних датчиков
    void LogicScriptBack()
    {
        //Машина сжирает 2.5f
        RaycastHit2D[] hitsCenter = Physics2D.RaycastAll(rays.CenterBack.origin, rays.CenterBack.direction, 5f);
        RaycastHit2D[] hitsLeft = Physics2D.RaycastAll(rays.LeftBack.origin, rays.LeftBack.direction, 5f);
        RaycastHit2D[] hitsRight = Physics2D.RaycastAll(rays.RightBack.origin, rays.RightBack.direction, 5f);
        float angle = transform.position.z % 360;
        if (angle < 0)
            angle += 360;
        int countRoofCenter = hitsCenter.Where(x => x.collider.gameObject.tag == "Roof").Count();
        int countRooLeft = hitsLeft.Select(x => x.collider.gameObject.tag == "Roof").Count();
        int countRooRight = hitsRight.Select(x => x.collider.gameObject.tag == "Roof").Count();
        if (!isFinish)
        {
            if (RoofOfGarage.ring == 1)
            {
                Speed = 20f;
                isFinish = true;
                NearGarage();
            }
            //Если справа и по центру преграда, то движемся влево
            if ((hitsCenter.Length > 1) && (hitsLeft.Length == 1) && (hitsRight.Length > 1))
            {
                if (angle >= 0 && angle < 90 || angle >= 270 && angle < 360)
                    rotate = Vector3.back;
                else rotate = Vector3.forward;
                rb2.transform.Rotate(rotate * 30 * Time.deltaTime);
            }
            //Если слева и по центру преграда, то движемся вправо 
            if ((hitsCenter.Length > 1) && (hitsLeft.Length > 1) && (hitsRight.Length == 1))
            {
                if (angle >= 0 && angle < 90 || angle >= 270 && angle < 360)
                    rotate = Vector3.forward;
                else rotate = Vector3.back;
                rb2.transform.Rotate(rotate * 30 * Time.deltaTime);
            }
            //Если преграда слева
            if (hitsLeft.Length > 1)
            {
                if (angle >= 0 && angle < 90 || angle >= 270 && angle < 360)
                    rotate = Vector3.forward;
                else rotate = Vector3.back;
                rb2.transform.Rotate(rotate * 30 * Time.deltaTime);
            }
            //Если преграда справа
            if (hitsRight.Length > 1)
            {
                if (angle >= 0 && angle < 90 || angle >= 270 && angle < 360)
                    rotate = Vector3.back;
                else rotate = Vector3.forward;
                rb2.transform.Rotate(rotate * 30 * Time.deltaTime);
            }
            //Если везде, то снижаем скорость, переключаемся на перед и там делаем поворот в сторону гаража для въезда задним ходом
            if ((hitsLeft.Length > 1) && (hitsRight.Length > 1))
            {
                Speed = 50f;   //скорость изменить        
                isBack = false;
                rotationforward = true;
            }

            //Левый поворот
            if (Distance(rays.LeftBack.direction) < Distance(rays.RightBack.direction) && hitsLeft.Length == 1)
            {
                if (angle >= 0 && angle < 90 || angle >= 270 && angle < 360)
                    rotate = Vector3.back;
                else rotate = Vector3.forward;
                rb2.transform.Rotate(rotate * 20 * Time.deltaTime);

            }
            else if (Distance(rays.LeftBack.direction) > Distance(rays.RightBack.direction) && hitsRight.Length == 1)
            {
                if (angle >= 0 && angle < 90 || angle >= 270 && angle < 360)
                    rotate = Vector3.forward;
                else rotate = Vector3.back;//Правый поворот
                rb2.transform.Rotate(rotate * 20 * Time.deltaTime);

            }
            if (hitsCenter.Length == 1)
            {
                Speed = 150; //Исправить на максимлаьную
                rotate = Vector3.zero;     //не поворачиваем, просто едем прямо
                rb2.transform.Rotate(rotate * 30 * Time.deltaTime);
            }

        }
        else
        {
            //Если координаты нужные - останавливаемся
            if (Distance(rb2.transform.up) > 0.1f && Distance(rb2.transform.up) < 0.3f && Distance(-rb2.transform.up) > 2.2f)
            {
                Speed = 0;
            }
            if (CenterSensor && !LeftSensor && !RightSensor || (CenterSensor && LeftSensor && RightSensor))
            {
                rotate = Vector3.zero;
                rb2.transform.Rotate(rotate * 30 * Time.deltaTime);
            }
            //Если передний и левый датчики в гараже, а правый нет, то сворачиваем левее
            if (CenterSensor && LeftSensor && !RightSensor)
            {
                if (angle >= 0 && angle < 90 || angle >= 270 && angle < 360)
                    rotate = Vector3.forward;
                else
                    rotate = Vector3.back;
                rb2.transform.Rotate(rotate * 30 * Time.deltaTime);
            }
            //Если передний и правый датчики в гараже, а левый нет, то сворачиваем правее
            if (CenterSensor && !LeftSensor && RightSensor)
            {
                if (angle >= 0 && angle < 90 || angle >= 270 && angle < 360)
                    rotate = Vector3.back;
                else
                    rotate = Vector3.forward;
                rb2.transform.Rotate(rotate * 30 * Time.deltaTime);
            }
            //Если в гараже только правый датчик, то сворачиваем правее
            if (!CenterSensor && !LeftSensor && RightSensor)
            {
                if (angle >= 0 && angle < 90 || angle >= 270 && angle < 360)
                    rotate = Vector3.back;
                else
                    rotate = Vector3.forward;
                rb2.transform.Rotate(rotate * 30 * Time.deltaTime);
            }
            //Если в гараже только левый датчик, то сворачиваем левее
            if (!CenterSensor && LeftSensor && !RightSensor)
            {
                if (angle >= 0 && angle < 90 || angle >= 270 && angle < 360)
                    rotate = Vector3.forward;
                else
                    rotate = Vector3.back;
                rb2.transform.Rotate(rotate * 30 * Time.deltaTime);
            }
        }
        rb2.transform.Rotate(rotate * 30 * Time.deltaTime);
        rb2.transform.Translate(new Vector3(0, -0.01f, 0) * Speed * Time.deltaTime);
    }


    void NearGarage()
    {
        RaycastHit2D[] hitsCenter = Physics2D.RaycastAll(rays.CenterBack.origin, rays.CenterBack.direction, 5f);
        RaycastHit2D[] hitsLeft = Physics2D.RaycastAll(rays.LeftBack.origin, rays.LeftBack.direction, 5f);
        RaycastHit2D[] hitsRight = Physics2D.RaycastAll(rays.RightBack.origin, rays.RightBack.direction, 5f);

        LeftSensor = Distance(rays.LeftBack.direction * 5f) <= 2f;
        CenterSensor = Distance(rays.CenterBack.direction * 5f) <= 2f;
        RightSensor = Distance(rays.RightBack.direction * 5f) <= 2f;
    }

    //Логика для передних датчиков
    void LogicScriptForward()
    {
        System.Random r = new System.Random(DateTime.Now.Millisecond);
        var x = 0;

        RaycastHit2D[] hitsCenter = Physics2D.RaycastAll(rays.CenterForward.origin, rays.CenterForward.direction, (float)(5 - x));
        RaycastHit2D[] hitsLeft = Physics2D.RaycastAll(rays.LeftForward.origin, rays.LeftForward.direction, (float)(5 - x));
        RaycastHit2D[] hitsRight = Physics2D.RaycastAll(rays.RightForward.origin, rays.RightForward.direction, (float)(5 - x));
        if (hitsCenter.Length == 1)
        {
            float angle = transform.position.z % 360;
            if (angle < 0)
                angle += 360;

            //Левый поворот
            if (Distance(rays.LeftBack.direction) < Distance(rays.RightBack.direction) && hitsLeft.Length == 1 && rotationforward)
            {
                Debug.Log("Поворот вправо, чтоб задняя часть машины шла влево");
                if (angle >= 0 && angle < 90 || angle >= 270 && angle < 360)
                {
                    rotate = Vector3.back;
                    Debug.Log("Машина смотрит вверх");
                }
                else
                {
                    rotate = Vector3.forward;
                    Debug.Log("Машина смотрит вниз");
                }
            }
            else rotationforward = false;
             if (Distance(rays.LeftBack.direction) > Distance(rays.RightBack.direction) && hitsRight.Length == 1 && !rotationforward )
            {//Правый поворот
                Debug.Log("Поворот влево, чтобы задняя часть машины шла вправо");
                if (angle >= 0 && angle < 90 || angle >= 270 && angle < 360)
                {
                    rotate = Vector3.forward;
                    Debug.Log("Машина смотрит вверх");
                }
                else
                {
                    rotate = Vector3.back;
                    Debug.Log("Машина смотрит вниз");
                }
            }
        }
        else
        {
            Speed = 70f;          
            isBack = true;
            
        }
       
        rb2.transform.Translate(new Vector3(0, 0.01f, 0) * Speed * Time.deltaTime);
        rb2.transform.Rotate(rotate * 20 * Time.deltaTime);
    }

    //Запрещает поворот при столкновении с предметами 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Other" || collision.gameObject.tag == "Garage")
        {

            CanRotate = false;
        }
    }

    //Разрешает поворот, когда нет препятствий
    private void OnCollisionExit2D(Collision2D collision)
    {
        CanRotate = true;
    }



    //При приближении к гаражу считает круговые коллайдеры, и если пройдено 2, то крыша исчезает
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag=="Roof")
        {
            RoofOfGarage.Roof = collision.gameObject.GetComponent<SpriteRenderer>();
            RoofOfGarage.ring++;
            if (RoofOfGarage.ring == 2)
            {
                
                RoofOfGarage.Roof.enabled = false;
            }
        }
    }

    //При оттдалении от гаража уменьшает количество кругов, и если машина отъехала от них, то скрывает крышу
    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "Roof" )
        {

            RoofOfGarage.ring--;
            if(RoofOfGarage.ring == 0 && RoofOfGarage.Roof.enabled==false)
                RoofOfGarage.Roof.enabled = true;
        }
    }
}
