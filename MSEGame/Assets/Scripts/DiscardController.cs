using UnityEngine;
using UnityEngine.EventSystems;

public class DiscardController : MonoBehaviour, IDropHandler
{
    public virtual void OnDrop(PointerEventData eventData)
    {
        if (GameManager.Instance.Stage != GameStage.CombatPreparations)
            return;

        GameObject draggedObj = eventData.pointerDrag;
        if (draggedObj.TryGetComponent<CardController>(out var cc))
        {
            if (cc.Card is TreasureCard treasure)
            {
                // Add consumable bonus to round bonus
                PlayerManager.Instance.CurrentPlayer.Player.Gold += treasure.value;

                Destroy(draggedObj);
                Debug.Log($"Sold {treasure.title} for {treasure.value} gold.");
            }
        }
    }
}
