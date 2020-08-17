using TMPro;
using UnityEngine;

public sealed class TitleBehaviour : MonoBehaviour
{
    [SerializeField] private float _titleTimeout;
    [SerializeField] private Camera _camera;

    private TextMeshProUGUI _textTitle;

    private float _currentDisplayTime;

    private void Start()
    {
        _textTitle = this.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (_textTitle.enabled == true)
        {
            ControlVisibility();
        }
    }

    private void LateUpdate()
    {
        ControlRotationLikeBilbord();
    }

    private void ControlRotationLikeBilbord()
    {
        if (_camera != null)
        {
            transform.LookAt(transform.position + _camera.transform.forward);
        }
        
    }

    private void ControlVisibility()
    {
        _currentDisplayTime += Time.deltaTime;

        if (_currentDisplayTime >= _titleTimeout)
        {
            _textTitle.enabled = false;
            _currentDisplayTime = 0.0f;
        }
    }
}
