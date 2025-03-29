using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public float mouseSensivity = 100f;

    float xRotation = 0f;
    float yRotation = 0f;

    public float topClamp = -90f;
    public float bottomClamp = 90f;
    void Start()
    {
        // Делаем курсор неподвижным и невидимым:
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Движение мышки:
        float mouseX = Input.GetAxis("Mouse X") * mouseSensivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensivity * Time.deltaTime;
        // вращение вокруг оси x (смотрим вверх и вниз):
        xRotation -= mouseY;
        // зафиксировать вращение:
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);
        // вращение вокруг оси y (смотрим влево и вправо):
        yRotation += mouseX;
        // применяем вращения к нашему преобразованию:
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}