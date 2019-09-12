using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "To Do", menuName = "To Do List", order = 1)]
public class ToDo : ScriptableObject
{
    public List<string> toDoList = new List<string>() { "To Do Item #1 Goes Here" };
}