#pragma warning disable 649

using UnityEngine;

public class DefaultConsoleDrag : MonoBehaviour
{
    [SerializeField] private Transform _container;

    private Vector3 _dragOffsetAtBegin;

    // Set offset at the start of drag
    public void BeginDrag() => _dragOffsetAtBegin = _container.position - Input.mousePosition;

    // Perform drag
    public void Drag()
    {
        // Check screen bounds
        if (Input.mousePosition.x >= Screen.width || Input.mousePosition.x <= 0f || Input.mousePosition.y >= Screen.height || Input.mousePosition.y <= 0f)
            return;

        var x = Input.mousePosition.x + _dragOffsetAtBegin.x;
        var y = Input.mousePosition.y + _dragOffsetAtBegin.y;
        _container.position = new Vector3(x, y, 0f);
    }
}
