using System;
using UnityEngine;

public class MouseManager : MonoSingleton<MouseManager>
{
    public event Action<Vector3> OnMouseClicked;
    public event Action<GameObject> OnEnemyClicked;
    Ray ray;
    private RaycastHit hit;

    [SerializeField] private Texture2D Point, Doorway, Attack, Target, Arrow;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }


    private void Update()
    {
        if (Input.GetKey(KeyCode.M))
        {
            Debug.Log("M");
        }
        // SetCursorTexture();
        // MouseControl();
    }

    private void SetCursorTexture()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            switch (hit.collider.tag)
            {
                case "Ground":
                    Cursor.SetCursor(Target, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Enemy":
                    Cursor.SetCursor(Attack, new Vector2(16, 16), CursorMode.Auto);
                    break;
                default:
                    break;
            }
        }
    }

    private void MouseControl()
    {
        if (Input.GetMouseButtonDown(0) && hit.collider != null)
        {
            if (hit.collider.CompareTag("Ground"))
                OnMouseClicked?.Invoke(hit.point);
            if (hit.collider.CompareTag("Enemy"))
                OnEnemyClicked?.Invoke(hit.collider.gameObject);
        }
    }
}
