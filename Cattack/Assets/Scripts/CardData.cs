using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Cards/Card Data")]
public class CardData : ScriptableObject
{
    public string cardName;
    public Sprite cardImage;
    public string description;
    public SkillType skillType;
    public KeyCode skillKey;
}

// SkillType.cs
public enum SkillType
{
    Q_Skill,
    W_Skill,
    E_Skill
}