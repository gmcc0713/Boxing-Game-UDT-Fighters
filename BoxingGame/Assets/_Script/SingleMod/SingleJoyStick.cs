using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SingleJoyStick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform leverSing;
    private RectTransform rectTransformSing;

    [SerializeField, Range(10, 250)]
    private float leverRangeSing;

    private Vector2 inputDirectionSing;
    private bool isInputSing;

    public SinglePlayer controllerSing;

    private void Awake()
    {
        rectTransformSing = GetComponent<RectTransform>();
    }
    public void OnBeginDrag(PointerEventData eventDataSing)
    {
        ControllJoystickLever(eventDataSing);
        isInputSing = true;
    }

    public void OnDrag(PointerEventData eventDataSing)
    {
        ControllJoystickLever(eventDataSing);
    }

    public void OnEndDrag(PointerEventData eventDataSing)
    {
        leverSing.anchoredPosition = Vector2.zero;
        isInputSing = false;
        controllerSing.MobieMoveSing(Vector2.zero);
    }

    private void ControllJoystickLever(PointerEventData eventDataSing)
    {
        var inputPosSing = eventDataSing.position - rectTransformSing.anchoredPosition;
        var inputVectorSing = inputPosSing.magnitude < leverRangeSing ? inputPosSing : inputPosSing.normalized * leverRangeSing;
        leverSing.anchoredPosition = inputVectorSing;
        inputDirectionSing = inputVectorSing / leverRangeSing;
    }

    private void InputControlVectorSing()
    {
        controllerSing.MobieMoveSing(inputDirectionSing);
    }

    private void Update()
    {
        if (isInputSing)
        {
            InputControlVectorSing();
        }
    }
}
