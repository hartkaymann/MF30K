using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomManager : Manager<RoomManager>
{
    [SerializeField] private RoomController currentRoom;
    public RoomController CurrentRoom { get { return currentRoom; } }

    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private GameObject npcPrefab;
    [SerializeField] private GameObject slimePrefab;
    [SerializeField] private GameObject ghostPrefab;

    public void InstantiateRoom(DoorCard card)
    {
        // Destroy old room
        if (currentRoom != null)
            Destroy(currentRoom.gameObject);

        // Create new room
        GameObject obj = Instantiate(roomPrefab, new Vector3(0, 1.5f, 0), Quaternion.identity);
        currentRoom = obj.GetComponent<RoomController>();
        currentRoom.Card = card;

        Transform npcPosition = currentRoom.transform.Find("NPC");
        if (card is MonsterCard monsterCard)
        {
            GameObject prefab = monsterCard.title.ToLower().EndsWith("slime") ? slimePrefab : ghostPrefab;
            currentRoom.NPC = Instantiate(prefab, npcPosition.position, Quaternion.identity, npcPosition).GetComponent<NpcController>();
        }
        else
        {
            currentRoom.NPC = Instantiate(npcPrefab, npcPosition.position, Quaternion.identity, npcPosition).GetComponent<NpcController>();

            Race race = PlayerManager.Instance.PlayerController.Player.Race;
            Profession profession = PlayerManager.Instance.PlayerController.Player.Profession;

            if (card is RaceCard raceCard)
            {
                race = raceCard.race;
            }
            else if (card is ProfessionCard professionCard)
            {
                profession = professionCard.profession;
            }

            SpriteRenderer sr = currentRoom.NPC.GetComponent<SpriteRenderer>();
            sr.sprite = SpriteManager.Instance.GetNpcSprite(race, profession);

            sr.flipX = (profession == Profession.Knight);

        }
    }
}
