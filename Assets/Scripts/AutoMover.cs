using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Transform))]
    public class AutoMover : MonoBehaviour
    {
        public float Speed { get; set; } = 1;
        public Vector3 Destination { get; private set; }
        private Transform objectTransform;

        //Ивент для получения новой точки
        event NewDestinationHandler OnDestinationPoint;
        delegate Vector3 NewDestinationHandler(GameObject gameObject);

        void Start()
        {
            objectTransform = GetComponent<Transform>();
        }

        private void Awake()
        {
            
        }

        void FixedUpdate()
        {
            //Получить новую точку при достижении текущей
            if (Destination == objectTransform.position)
                Destination= (Vector3)(OnDestinationPoint?.Invoke(gameObject));

            //Рассчёт расстояния до точки и расстояния, которое должно быть пройдено с текущей скоростью
            Vector3 distance = Destination - transform.position;
            Vector3 path = distance.normalized * Speed * Time.fixedDeltaTime;

            //Выбор пути - либо значение, соответствующее скорости, либо оставшееся до точки расстояние
            Vector3 finalMovement = path.magnitude < distance.magnitude ? path : distance;

            objectTransform.Translate(finalMovement);
        }
    }
}