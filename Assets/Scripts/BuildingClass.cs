using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Building
    {
        private GameObject model;
        public City CityParent { get; private set; }

        internal Building(string path, City city)
        {
            CityParent = city;
            //TODO добавление 3D модели
        }
        public void SetCoordinates(int x, int y, int z)
        {
            model.transform.position = new Vector3(x, y, z);
        }
        public void Show()
        {
            //TODO метод, который выводит модель на экран и делает её статичной (запрет на перемещение)
        }
    }
    public interface IBuildingPart{}
    public interface IHouse:IBuildingPart
    {
        int MaxCitizenCount { get; }
        int CitizenCount { get; }
        List<Citizen> Citizens { get; }
    }
    public interface IPollute:IBuildingPart
    {
        int BasePollution { get; }
        int FinalPollution { set; }
        void Pollute();
    }
    public interface IResourceStorage
    {
        Dictionary<IResource,int> MaxCapacity { get; }
        Dictionary<IResource, int> InStorage { get; }
    }
    public interface IConsumer:IBuildingPart, IResourceStorage
    {
        Dictionary<IResource,int> Consuming { get; }
        bool TryConsume();
        void Consume();
    }
    public interface IProducter : IBuildingPart, IResourceStorage
    {
        Dictionary<IResource, int> Producting { get; }
        bool TryProduct();
        void Product();
    }
    public interface IUseCitizen:IBuildingPart
    {
        int NeedCount { get; }
        List<Citizen> Citizens { get; }
        bool HaveNeededCount();
    }
    public interface IRestPoint:IBuildingPart
    {
        int MaxCitizenCount { get; }
        int CitizenCount { get; }
        List<Citizen> Citizens { get; }
        void Rest();
    }
    public interface ITransportNet:IBuildingPart
    {
        //TODO... All...
    }
}