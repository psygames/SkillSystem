namespace SkillSystem
{
    public abstract class NamedValue : Value
    {
        public string name;

        public NamedValue() { }
        public NamedValue(string name)
        {
            this.name = name;
        }
    }
}
