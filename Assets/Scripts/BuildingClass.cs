using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.InGame
{
    public class Building
    {
        private GameObject model;
        public List<BuildingPart> Parts { get; private set; } = new List<BuildingPart>();
        public City CityParent { get; private set; }

        internal Building(string path, City city)
        {
            CityParent = city;
            //TODO добавление 3D модели
        }

        public bool Is<T>()
        {
            foreach (var item in Parts)
            {
                if (item is T)
                    return true;
            }
            return false;
        }
        public T As<T>()
            where T : BuildingPart
        {
            foreach (var item in Parts)
            {
                if (item is T)
                    return (T)item;
            }
            return null;
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

    #region Interfaces

    public abstract class BuildingPart
    {
        protected Building parent;
    }
    public class House : BuildingPart
    {
        public int MaxResidentsCount { get; private set; }
        public int ResidentsCount { get => Residents.Count; }
        public List<Citizen> Residents { get; private set; } = new List<Citizen>();

        public void AddResident(Citizen citizen)
        {
            if (ResidentsCount < MaxResidentsCount)
            {
                Residents.Add(citizen);
            }
            else
            {
                throw new InvalidOperationException("Can't add citizen");
            }
        }

        public void RemoveResident(Citizen citizen)
        {
            Residents.Remove(citizen);
        }
    }
    public class Pollute : BuildingPart
    {
        public int BasePollution { get; private set; }
        public int PollutionMultiplier { get; set; }
        void ThrowPollution()
        {
            //TODO pollution
        }
    }
    public class ResourceStorage : BuildingPart
    {
        private readonly Dictionary<Resource, int> maxCapacity = new Dictionary<Resource, int>();
        private readonly Dictionary<Resource, int> inStorage = new Dictionary<Resource, int>();

        public int InStorage(Resource resource)
        {
            return inStorage[resource];
        }
        public int sMaxCapacity(Resource resource)
        {
            return maxCapacity[resource];
        }
        public void Add(Resource resource, int count)
        {
            if (maxCapacity[resource] > inStorage[resource])
            {
                inStorage[resource] += count;
            }
            else
            {
                throw new InvalidOperationException("Can't add resources");
            }
        }

        public void Add(Dictionary<Resource, int> toAdd)
        {
            foreach (var item in toAdd)
            {
                if (maxCapacity[item.Key] > inStorage[item.Key])
                {
                    throw new InvalidOperationException("Can't add resources");
                }
            }
            foreach (var item in toAdd)
            {
                inStorage[item.Key] += item.Value;
            }
        }

        public void Substract(Resource resource, int count)
        {
            if (inStorage[resource] - count >= 0)
            {
                inStorage[resource] -= count;
            }
            else
            {
                throw new InvalidOperationException("Can't substract resources");
            }
        }

        public void Substract(Dictionary<Resource, int> toSub)
        {
            foreach (var item in toSub)
            {
                if (inStorage[item.Key] - item.Value >= 0)
                {
                    throw new InvalidOperationException("Can't substract resources");
                }
            }
            foreach (var item in toSub)
            {
                inStorage[item.Key] -= item.Value;
            }
        }
    }
    public class Consumer : BuildingPart
    {
        private ResourceStorage Storage
        {
            get
            {
                return parent.As<ResourceStorage>();
            }
        }

        public Dictionary<Resource, int> Consuming { get; private set; }
        public void Consume()
        {
            if (TryConsume())
            {
                Storage.Substract(Consuming);
            }
            else
            {
                throw new InvalidOperationException("Can't consume resources");
            }
        }

        public bool TryConsume()
        {
            foreach (var item in Consuming)
            {
                if (Storage.InStorage(item.Key) < item.Value)
                    return false;
            }
            return true;
        }
    }
    public class Producter : BuildingPart
    {
        private ResourceStorage Storage
        {
            get
            {
                return parent.As<ResourceStorage>();
            }
        }
        public Dictionary<Resource, int> Producting { get; private set; }
        public void Product()
        {
            if (TryProduct())
            {
                Storage.Add(Producting);
            }
            else
            {
                throw new InvalidOperationException("Can't product resources");
            }
        }

        public bool TryProduct()
        {
            foreach (var item in Producting)
            {
                if (Storage.InStorage(item.Key) >= Storage.MaxCapacity(item.Key))
                    return false;
            }
            return true;
        }
    }
    public class UseCitizen : BuildingPart
    {
        public int NeedCitizens { get; private set; }
        public List<Citizen> Citizens { get; private set; }
        public bool HaveNeededCount()
        {
            return Citizens.Count >= NeedCitizens;
        }
    }
    public class RestPoint : BuildingPart
    {
        public int MaxCitizenCount { get; private set; }
        public int CitizenCount { get => Citizens.Count; }
        public List<Citizen> Citizens { get; private set; }
        void Rest()
        {
            //TODO rest
        }
    }
    public class TransportNet : BuildingPart
    {
        //TODO... All...
    }

    #endregion

}