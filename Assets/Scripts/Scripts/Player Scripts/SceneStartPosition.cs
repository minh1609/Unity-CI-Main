using UnityEngine;

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
    public StringValue currentScene;

    [Header("Player's starting direction to face")]
    public StartDirection startDirection;

    public void Start()
    {
        currentScene.element = this.gameObject.scene.name;
    }

    public void movePlayer()
    {
        switch (currentScene.element)
        {
            case "Auburn":

                if (lastScene.element == "Dungeon")
                {
                    playerStorage.defaultValue = auburnDungeon;
                    playerStorage.initialValue = auburnDungeon;
                    startDirection.startX = 0;
                    startDirection.startY = -1;
                }
                else if (lastScene.element == "Home")
                {
                    playerStorage.defaultValue = auburnHouse;
                    playerStorage.initialValue = auburnHouse;
                    startDirection.startX = 0;
                    startDirection.startY = -1;
                }
                else if (lastScene.element == "Rosewood_Forest")
                {
                    playerStorage.defaultValue = auburnRosewood;
                    playerStorage.initialValue = auburnRosewood;
                    startDirection.startX = -1;
                    startDirection.startY = 0;
                }
                if (lastScene.element == "Auburn")
                {
                    playerStorage.defaultValue = auburnHouse;
                    playerStorage.initialValue = auburnHouse;
                    startDirection.startX = 0;
                    startDirection.startY = -1;
                }


                break;

            case "Home":
                playerStorage.defaultValue = houseInterior;
                playerStorage.initialValue = houseInterior;
                startDirection.startX = 0;
                startDirection.startY = 1;
                break;

            case "Dungeon":
                playerStorage.defaultValue = dungeon;
                playerStorage.initialValue = dungeon;
                startDirection.startX = 0;
                startDirection.startY = 1;
                break;

            case "Rosewood_Forest":
                if (lastScene.element == "Auburn")
                {
                    playerStorage.defaultValue = rosewoodAuburn;
                    playerStorage.initialValue = rosewoodAuburn;
                    startDirection.startX = 1;
                    startDirection.startY = 0;
                }
                if (lastScene.element == "Rosewood_Forest")
                {
                    playerStorage.defaultValue = rosewoodAuburn;
                    playerStorage.initialValue = rosewoodAuburn;
                    startDirection.startX = 1;
                    startDirection.startY = 0;
                }

                break;

            default:
                break;
        }
    }
}
