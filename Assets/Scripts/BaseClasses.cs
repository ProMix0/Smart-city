using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building
{
    private GameObject model;
    private City cityParent;

    public Building(string path, City city)
    {
        cityParent = city;
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

public class City
{
    public List<Building> Buildings { get; } = new List<Building>();
}
