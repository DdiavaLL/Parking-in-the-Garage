using System.Collections;
using UnityEngine;
//описывает поведение неактивных машинок
public class KinematicCars : MonoBehaviour
{
   public new Camera  camera;
    Vector3 MousePos;
    Coroutine func;
    // Start is called before the first frame update
    private void Start()
    {
        //StartCoroutine(BoxActive()); оставила на случай бага, который был
    }
    //при нажатии мышки делаем из бокс коллайдера триггер, чтобы можно было переносить машинку
    //запускаем функцию перемещения 
    private void OnMouseDown()
    {
        
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        func = StartCoroutine(Velocity());
      
    }
    //меняет позицию машины в соответствии с положением курсора
    IEnumerator Velocity()
    {
        while (true)
        {
           
            MousePos = camera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(MousePos.x, MousePos.y, 0);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    
   /* IEnumerator BoxActive()
    {
        yield return new WaitForSeconds(1f);
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }*/
    //делаем бокс коллайдер просто бокс коллайдером, чтобы наша машинка сталкивалась с другой машинкой
    //прекращаем перемещение
    private void OnMouseUp()
    {
        gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        StopCoroutine(func);
    }
}
