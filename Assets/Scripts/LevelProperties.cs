using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Level")]
public class LevelProperties : ScriptableObject
{
    public int numberOfShapes;
    public string[] shapeNames;
}
