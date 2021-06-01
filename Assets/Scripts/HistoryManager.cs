using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistoryManager
{
    public static int storedActions = 10;
    public static List<Action> actionHistory = new List<Action>();
    public static List<Action> actionRedo = new List<Action>();

    public enum ActionType { CreateRoom, CreateDoor, CreateStairs, Delete };
    public struct Action
    {
        public Action(ActionType _type, GameObject _object) // For Create Actions
        {
            actionType = _type;
            createdObject = _object;
        }

        public ActionType actionType;
        public GameObject createdObject;
    }

    public static void HandleUndo()
    {
        Action lastAction = actionHistory[actionHistory.Count - 1];

        switch (lastAction.actionType)
        {
            case ActionType.CreateRoom:
            case ActionType.CreateDoor:
            case ActionType.CreateStairs:
                actionHistory.RemoveAt(actionHistory.Count - 1);
                lastAction.createdObject.SetActive(false);
                actionRedo.Add(lastAction);
                break;
            case ActionType.Delete:
                actionHistory.RemoveAt(actionHistory.Count - 1);
                lastAction.createdObject.SetActive(true);
                actionRedo.Add(lastAction);
                break;
            default: break;
        }
    }

    public static void HandleRedo()
    {
        Action lastAction = actionRedo[actionRedo.Count - 1];

        switch (lastAction.actionType)
        {
            case ActionType.CreateRoom:
            case ActionType.CreateDoor:
            case ActionType.CreateStairs:
                actionRedo.RemoveAt(actionRedo.Count - 1);
                lastAction.createdObject.SetActive(true);
                actionHistory.Add(lastAction);
                break;
            case ActionType.Delete:
                actionRedo.RemoveAt(actionRedo.Count - 1);
                lastAction.createdObject.SetActive(false);
                actionHistory.Add(lastAction);
                break;
            default: break;
        }
    }
}
