using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class MyUtilities
{
    public static Vector2 GetWorldPosition(this Vector2 screenPosition)
    {
        return Camera.main.ScreenToWorldPoint(screenPosition);
    }

    public static Vector2 ConvertToVector2(this Vector3 position)
    {
        return new Vector2(position.x, position.y);
    }

    public static List<T> MixMe<T>(this List<T> collection)
    {
        System.Random rnd = new System.Random(UnityEngine.Random.Range(60, 100));
        List<T> rndList = collection.OrderBy(x => rnd.Next(0, collection.Count)).ToList();
        collection = rndList;

        return collection;
    }

    public static List<Fruit> GetFruitsViaTypes(this List<FruitType> fruitsTypeList)
    {
        List<Fruit> list = new List<Fruit>();
        foreach (var item in fruitsTypeList)
        {
            list.Add(item.GetFruit());
        }
        return list;
    }

    public static Fruit GetFruit(this FruitType fruitType)
    {
        string name = "";
		name = fruitType.ToString();
		/*
        switch (fruitType)
        {
            case FruitType.Ananas:
                name = "Ananas";
                break;
            case FruitType.Anar:
                name = "Anar";
                break;
            case FruitType.Gilas:
                name = "Gilas";
                break;
            case FruitType.Watermelon:
                name = "Watermelon";
                break;
            case FruitType.Lemon:
                name = "Lemon";
                break;
            default:
                break;
        }
		*/
        return Resources.Load<Fruit>("Fruits/" + name);
    }
}