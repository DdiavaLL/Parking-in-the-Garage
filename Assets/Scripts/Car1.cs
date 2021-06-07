using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//управление машинкой, выезжающей на дорогу сверху
public class Car1 : MonoBehaviour
{
    float SpawnTime;
    public GameObject spawnpoint;

    Rigidbody2D rb;
    Sprites sprite;
    [SerializeField]
    private float speed;
    public float Speed { get => speed; set => speed = value; }

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<Sprites>();   //массив спрайтов-типов машинок
        rb = GetComponent<Rigidbody2D>();
        Speed = Random.Range(3f, 10f);
        rb.transform.position = spawnpoint.transform.position;
        StartCoroutine(Spawn());
        
    }

    // Update is called once per frame
    //двигает машинку
    void Update()
    {
        rb.velocity = new Vector3(0, -Speed, 0);
    }
    //срабатывает на триггер, которым является бокс-коллайдер у спавна второй точки. нужна, чтобы менять машинку и возвращать ее обратно в начало
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Car2")
        {
            StartCoroutine(Spawn());
        }
    }
    //ждет некоторое рандомное время, возвращает обратно наверх машину, меняет ее и ее скорость для реализма
    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(SpawnTime);
        SpawnTime = Random.Range(0f, 10f);
        this.transform.position = spawnpoint.transform.position;
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite.sprites[(int)Random.Range(0f, sprite.sprites.Length - 1)] ;
        Speed = Random.Range(3f, 10f);
      
    }
}
