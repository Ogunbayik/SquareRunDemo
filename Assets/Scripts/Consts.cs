using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consts 
{
    public struct InputConts
    {
        public const string HORIZONTAL_INPUT = "Horizontal";
        public const string VERTICAL_INPUT = "Vertical";
    }

    public struct PlayerAnimationParameter
    {
        public const string SAD_PARAMETER = "isSad";
        public const string WALK_PARAMETER = "isWalking";
        public const string RUN_PARAMETER = "isRunning";
        public const string TELEPORT_PARAMETER = "isTeleporting";
    }

    public struct GemAnimationParameter
    {
        public const string COLLECT_PARAMETER = "isCollected";
    }

    public struct DoorAnimationParameter
    {
        public const string OPENING_PARAMETER = "isOpening";
        public const string CLOSING_PARAMETER = "isClosing";
        public const string OPENED_PARAMETER = "isOpened";
        public const string CLOSED_PARAMETER = "isClosed";
    }

    public struct GameoverCanvasAnimationParameter
    {
        public const string GAMEOVER_PARAMETER = "isGameover";
    }
    
    public struct GamescoreCanvasAnimationParameter
    {
        public const string DECREASED_PARAMETER = "Decreased";
    }

    public struct GhostAnimationParameter
    {
        public const string MOVING_PARAMETER = "isMove";
    }

    
}
