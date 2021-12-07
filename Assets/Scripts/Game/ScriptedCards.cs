using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Card", menuName = "ScriptedCards", order = 1)]
public class ScriptedCards : ScriptableObject
{
    [EnumPaging]
    public CardType _cardType;

    [PreviewField(60)]
    public GameObject _cardModel; 

    public int _amount; //Amount of coins it provides
    public int _cardID;

    [Space]
    [TextArea(10,14)] public string _description;
}

