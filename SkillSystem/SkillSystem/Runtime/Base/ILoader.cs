namespace SkillSystem.Runtime
{
    public interface ILoader
    {
        Ability LoadAbility(string path);
        Modifier LoadModifier(string path);
        AbilityGroup LoadAbilityGroup(string path);
    }
}
