using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStartPosition : MonoBehaviour
{
    [Header("Game Objects' Starting Positions")]
    public VectorValue playerStorage;

    [Header("Default starting positions")]
    public Vector2 auburnHouse;
    public Vector2 auburnDungeon;
    public Vector2 auburnRosewood;
    public Vector2 houseInterior;
    public Vector2 dungeon;
    public Vector2 rosewoodAuburn;
    public StringValue lastScene;

    [Header("Player's starting direction to face")]
    public StartDirection startDirection;

    public void movePlayer()
    {
        switch (this.gameObject.scene.name)
        {
            case "Auburn":

                if (lastScene.element == "Dungeon")
                {
                    playerStorage.defaultValue = auburnDungeon;
                    playerStorage.initialValue = auburnDungeon;
                    startDirection.startX = 0;
                    startDirection.startY = -1;
                    lastScene.element = "Auburn";
                }
                else if (lastScene.element == "Home")
                {
                    playerStorage.defaultValue = auburnHouse;
                    playerStorage.initialValue = auburnHouse;
                    startDirection.startX = 0;
                    startDirection.startY = -1;
                    lastScene.element = "Auburn";
                }
                else if (lastScene.element == "Rosewood Forest")
                {
                    playerStorage.defaultValue = auburnRosewood;
                    playerStorage.initialValue = auburnRosewood;
                    startDirection.startX = -1;
                    startDirection.startY = 0;
                    lastScene.element = "Auburn";
                }


                break;

            case "Home":
                playerStorage.defaultValue = houseInterior;
                playerStorage.initialValue = houseInterior;
                startDirection.startX = 0;
                startDirection.startY = 1;
                lastScene.element = "Home";
                break;

            case "Dungeon":
                playerStorage.defaultValue = dungeon;
                playerStorage.initialValue = dungeon;
                startDirection.startX = 0;
                startDirection.startY = 1;
                lastScene.element = "Dungeon";
                break;

            case "Rosewood Forest":
                if (lastScene.element == "Auburn")
                {
                    playerStorage.defaultValue = rosewoodAuburn;
                    playerStorage.initialValue = rosewoodAuburn;
                    startDirection.startX = 1;
                    startDirection.startY = 0;
                    lastScene.element = "Rosewood Forest";
                }

                break;

            case "StartMenu":
                lastScene.element = "Auburn";
                break;

            default:
                break;
        }
    }
}
