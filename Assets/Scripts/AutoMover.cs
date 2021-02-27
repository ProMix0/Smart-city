using Game;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Transform))]
    public class AutoMover : MonoBehaviour
    {
        public float Speed { get; set; } = 1;
        public Vector3 Destination { get; private set; }
        private Transform objectTransform;

        public PathController pathController;

        void Start()
        {
            objectTransform = GetComponent<Transform>();
        }

        void FixedUpdate()
        {
            //Получить новую точку при достижении текущей
            if (Destination == objectTransform.position)
                Destination = pathController.GetNextPoint(gameObject);

            //Рассчёт расстояния до точки и расстояния, которое должно быть пройдено с текущей скоростью
            Vector3 distance = Destination - transform.position;
            Vector3 path = distance.normalized * Speed * Time.fixedDeltaTime;

            //Выбор пути - либо значение, соответствующее скорости, либо оставшееся до точки расстояние
            Vector3 finalMovement = path.magnitude < distance.magnitude ? path : distance;

            objectTransform.Translate(finalMovement);
        }

        public abstract class PathController
        {
            public abstract Vector3 GetNextPoint(GameObject gameObject);
        }
    }

    public class RobotPathController : AutoMover.PathController
    {
        private Graph<Sidewalk>.Node current, previous;

        public RobotPathController(Graph<Sidewalk>.Node entry)
        {
            current = entry;
        }

        public override Vector3 GetNextPoint(GameObject gameObject)
        {
            System.Random random = new System.Random();
            Graph<Sidewalk>.Node newWayPoint= current.Edges.Select(edge => edge.Node1 == current ? edge.Node2 : edge.Node1)
                .Where(node => node != previous).OrderBy(node => random.Next()).First();
            previous = current;
            current = newWayPoint;
            return current.Item.transform.position;
        }
    }
}