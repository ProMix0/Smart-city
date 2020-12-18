using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.InGame
{
    public class Building
    {
        protected GameObject model;
        protected List<BuildingPart> Parts { get; private set; } = new List<BuildingPart>();
        public City CityParent { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Building(City city,string path, int width, int height)
        {
            CityParent = city;
            Width = width;
            Height = height;
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
                if (item is T t)
                    return t;
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

        public House(Building parent, int maxResidentsCount)
        {
            this.parent = parent;
            MaxResidentsCount = maxResidentsCount;
        }

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

        public Pollute(Building parent, int basePollution)
        {
            this.parent = parent;
            BasePollution = basePollution;
        }

        public void ThrowPollution()
        {
            //TODO pollution
        }
    }
    public class ResourceStorage : BuildingPart
    {
        private readonly Dictionary<Resource, int> maxCapacity = new Dictionary<Resource, int>();
        private readonly Dictionary<Resource, int> inStorage = new Dictionary<Resource, int>();

        public ResourceStorage(Building parent, Dictionary<Resource, int> maxCapacity)
        {
            this.parent = parent;
            foreach (var str in Enum.GetNames(typeof(Resource)))
            {
                this.maxCapacity[(Resource)Enum.Parse(typeof(Resource), str)] = maxCapacity.ContainsKey((Resource)Enum.Parse(typeof(Resource), str)) ?
                    maxCapacity[(Resource)Enum.Parse(typeof(Resource), str)] : 0;
                inStorage[(Resource)Enum.Parse(typeof(Resource), str)] = 0;
            }
        }

        public int InStorage(Resource resource)
        {
            return inStorage[resource];
        }
        public int MaxCapacity(Resource resource)
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

        public Consumer(Building parent, Dictionary<Resource,int> consuming)
        {
            this.parent = parent;
            Consuming = consuming;
        }

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

        public Producter(Building parent, Dictionary<Resource,int> producting)
        {
            this.parent = parent;
            Producting = producting;
        }

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
        public List<Citizen> Citizens { get; private set; } = new List<Citizen>();

        public UseCitizen(Building parent, int needCitizens)
        {
            this.parent = parent;
            NeedCitizens = needCitizens;
        }

        public bool HaveNeededCount()
        {
            return Citizens.Count >= NeedCitizens;
        }
    }
    public class RestPoint : BuildingPart
    {
        public int MaxCitizenCount { get; private set; }
        public int RestValue { get; private set; }
        public int CitizenCount { get => Citizens.Count; }
        public List<Citizen> Citizens { get; private set; }

        public RestPoint(Building parent, int maxCitizensCount,int restValue)
        {
            this.parent = parent;
            MaxCitizenCount = maxCitizensCount;
            RestValue = restValue;
        }

        public void Rest()
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