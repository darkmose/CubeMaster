using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCUBE : MonoBehaviour
{
    private bool motion_flag;  //Метка движения: 1- Куб движется, 0- Куб стоит на месте. Для блокировки движений во все стороны.
    public float rotate_speed = 5; //Скорость вращения куба.

    private GameObject Cube1;           //Два составляющих куба 
    private GameObject Cube2;


    private Quaternion Move_setp;

    private void Awake()
    {
        Move_setp = new Quaternion();
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
            if (isVertical())
            {
                Move_setp = Quaternion.AngleAxis(transform.rotation.x - 90.0f, Vector3.right);
            }
            else
            {
                Move_setp = Quaternion.AngleAxis(transform.rotation.x - 90.0f, Vector3.right);
            }

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
            if (isVertical())
            {
                Move_setp = Quaternion.AngleAxis(transform.rotation.x + 90.0f, Vector3.right);
            }
            else
            {
                Move_setp = Quaternion.AngleAxis(transform.rotation.x + 90.0f, Vector3.right);
            }

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
            Quaternion q_result = Quaternion.RotateTowards(transform.rotation, Move_setp, Time.deltaTime * 50 * rotate_speed);       //Quaternion.RotateTowards(transform.rotation, Move_setp, Time.deltaTime * 50 * rotate_speed);
            transform.rotation = q_result;

            if (transform.rotation.eulerAngles == Move_setp.eulerAngles) //Заданный угол поворота равен текущему
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

        if (Physics.Raycast(new Ray(Cube1.transform.position, Cube1.transform.position + Vector3.up), out hitinfo, 1f))
        {

            if (Cube2.CompareTag(hitinfo.collider.tag))
            {
                return true;
            }

        }

        if (Physics.Raycast(new Ray(Cube1.transform.position, Cube1.transform.position + Vector3.down), out hitinfo, 1f))
        {

            if (Cube2.CompareTag(hitinfo.collider.tag))
            {
                return true;
            }

        }

        return false;
    }

    
    void Update()
    {
        Move();
    }
}
