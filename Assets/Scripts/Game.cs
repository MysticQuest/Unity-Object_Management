using UnityEngine;
using System.Collections.Generic;

public class Game : PersistableObject
{
    public PersistentStorage storage;

    public ShapeFactory shapeFactory;

    public KeyCode createKey = KeyCode.C;
    public KeyCode newGameKey = KeyCode.Z;
    public KeyCode saveKey = KeyCode.S;
    public KeyCode loadKey = KeyCode.F;

    List<PersistableObject> shapes;

    void Awake()
    {
        shapes = new List<PersistableObject>();
    }

    void Update()
    {
        if (Input.GetKeyDown(createKey))
        {
            CreateShape();
        }
        else if (Input.GetKey(newGameKey))
        {
            BeginNewGame();
        }
        else if (Input.GetKeyDown(saveKey))
        {
            storage.Save(this);
        }
        else if (Input.GetKeyDown(loadKey))
        {
            storage.Load(this);
        }
    }

    void CreateShape()
    {
        Shape instance = shapeFactory.GetRandom();
        Transform t = instance.transform;
        t.localPosition = Random.insideUnitSphere * 5f;
        t.localRotation = Random.rotation;
        t.localScale = Vector3.one * Random.Range(0.1f, 1f);
        shapes.Add(instance);
    }

    void BeginNewGame()
    {
        for (int i = 0; i < shapes.Count; i++)
        {
            Destroy(shapes[i].gameObject);
        }
        shapes.Clear();
    }

    public override void Save(GameDataWriter writer)
    {
        writer.Write(shapes.Count);
        for (int i = 0; i < shapes.Count; i++)
        {
            shapes[i].Save(writer);
        }
    }

    public override void Load(GameDataReader reader)
    {
        int count = reader.ReadInt();
        for (int i = 0; i < count; i++)
        {
            Shape instance = shapeFactory.Get(0);
            instance.Load(reader);
            shapes.Add(instance);
        }
    }
}

