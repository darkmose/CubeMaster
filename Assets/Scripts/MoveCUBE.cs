using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCUBE : MonoBehaviour
{
    private bool motion_flag;  //Метка движения: 1- Куб движется, 0- Куб стоит на месте. Для блокировки движений во все стороны.
    public float rotate_speed = 5; //Скорость вращения куба.
    public float move_speed = 5; //Скорость вращения куба.

    private GameObject Cube1;           //Два составляющих куба 
    private GameObject Cube2;


    private Quaternion Rotate_setp;
    private Vector3 Move_setp;

    private void Awake()
    {
        Rotate_setp = new Quaternion();
        Move_setp = new Vector3();
    }

    private void Start()
    {
        Cube1 = transform.GetChild(0).gameObject;
        Cube2 = transform.GetChild(1).gameObject;
    }

    /// <summary>
    /// Функция движения куба (Управление) (Вызов в Update()).
    /// </summary>
    private void Move()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            MoveLeft();
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            MoveRight();
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            MoveForward();
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            MoveBackward();
        }
    }
    /// <summary>
    /// Движение вправо
    /// </summary>
    private void MoveRight() 
    {
        if (!motion_flag)
        {
            Rotate_setp = Quaternion.AngleAxis(90, Vector3Int.back) * transform.rotation;            
            Move_setp = (!isVertical()) ? transform.position + Vector3Int.right : transform.position + new Vector3(1.5f, 0, 0); // В лежачем положении двигаем на 1 ячейку, в вертикальном на 1.5.
            

            motion_flag = true;
            StartCoroutine(MotionControl());
        }        
    }
    /// <summary>
    /// Движение влево
    /// </summary>
    private void MoveLeft() 
    {
        if (!motion_flag)
        {
            Rotate_setp = Quaternion.AngleAxis(90, Vector3Int.forward) * transform.rotation;
            Move_setp = (!isVertical()) ? transform.position + Vector3Int.left : transform.position + new Vector3(-1.5f, 0, 0);

            motion_flag = true;
            StartCoroutine(MotionControl());
        }      
    }

    /// <summary>
    /// Движение назад
    /// </summary>
    private void MoveBackward() 
    {
        if (!motion_flag)
        {
            Rotate_setp = Quaternion.AngleAxis(90, Vector3Int.left) * transform.rotation;
            Move_setp = (!isVertical()) ? transform.position + Vector3Int.back : transform.position + new Vector3(0, 0, -1.5f);

            motion_flag = true;
            StartCoroutine(MotionControl());
        }
    }                       
    /// <summary>
    /// Движение вперед
    /// </summary>
    private void MoveForward() 
    {
        if (!motion_flag)
        {
            Rotate_setp = Quaternion.AngleAxis(90, Vector3Int.right) * transform.rotation;
            Move_setp = (!isVertical()) ? transform.position + Vector3Int.forward : transform.position + new Vector3(0, 0, 1.5f);

            motion_flag = true;
            StartCoroutine(MotionControl());
        }
    }


    /// <summary>
    /// Корутина, непосредственно управляющая движением куба.
    /// </summary>
    IEnumerator MotionControl()
    {
        while (true)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Rotate_setp, Time.deltaTime * 50 * rotate_speed);
            transform.position = Vector3.MoveTowards(transform.position, Move_setp, Time.deltaTime * move_speed);
                   

            if (transform.rotation.eulerAngles == Rotate_setp.eulerAngles  &&  transform.position == Move_setp ) //Заданный угол поворота равен текущему
            {
                motion_flag = false;
                break;
            }
            yield return null;
        }         
        yield return null;              
    }


    /// <summary>
    /// Функция определяющая вертикально ли стоит куб или горизонтально лежит
    /// </summary>
    /// <returns>Если вертикально стоит, то TRUE</returns>
    private bool isVertical()  
    {
        RaycastHit hitinfo = new RaycastHit();

        if (Physics.Raycast(new Ray(Cube1.transform.position, Vector3.up), out hitinfo, 1f))
        {
            Debug.Log(hitinfo.collider.name);

            if (Cube2.CompareTag(hitinfo.collider.tag))
            {
            Debug.Log("Vertical");
                return true;
            }
        }


        if (Physics.Raycast(new Ray(Cube1.transform.position, Vector3.down), out hitinfo, 1f))
        {
            Debug.Log(hitinfo.collider.name);

            if (Cube2.CompareTag(hitinfo.collider.tag))
            {
                return true;
            }
        }

        Debug.Log("Horizontal");

        return false;
    }

    
    void Update()
    {
        Move();
    }
}
