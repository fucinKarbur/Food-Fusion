using System.Collections;
using UnityEngine;
using Zenject;

namespace FoodFusion
{
    public class Dropper : MonoBehaviour
    {
        [SerializeField] private float _dropForce = 5f;

        private WaitForSeconds _createDelay = new(.5f);
        private InputHandler _input;
        private NextFoodHolder _foodHolder;
        private FoodObject _currentObject;

        [Inject]
        private void Construct(InputHandler input, NextFoodHolder foodHolder)
        {
            _input = input;
            _foodHolder = foodHolder;
        }

        private void Start()
        {
            GetObject();
            _input.OnMouseUp += Drop;
        }

        private void OnEnable()
        {
            _currentObject?.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            _currentObject?.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _input.OnMouseUp -= Drop;
        }

        private void Drop()
        {
            if (gameObject.activeSelf == false || _currentObject == null)
                return;

            _currentObject.Drop(AddForce());
            _currentObject = null;
            StartCoroutine(UpdateCurrentObject());
        }

        private void GetObject()
        {
            if (_currentObject != null)
                return;

            _currentObject = _foodHolder.GetObject();

            _currentObject.gameObject.SetActive(true);
            _currentObject.transform.position = transform.position;
        }

        private IEnumerator UpdateCurrentObject()
        {
            yield return _createDelay;
            GetObject();
        }

        private Vector3 AddForce()
        {
            var direction = (_input.MousePosition - transform.position).normalized;
            return direction * _dropForce;
        }
    }
}
