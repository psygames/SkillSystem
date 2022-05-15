using System.Collections.Generic;

namespace SkillSystem
{
    public class AbilityGroup
    {
        public string name;
        public string image;

        public List<Ability> abilities;
        public List<float> continueIntervals;

        public void Merge(AbilityGroup group)
        {
            if (abilities != null && group.abilities != null)
            {
                foreach (var ab in group.abilities)
                {
                    var sameAb = abilities.Find(a => a.name == ab.name);
                    if (sameAb != null)
                    {
                        sameAb.Merge(ab);
                    }
                    else
                    {
                        abilities.Add(ab);
                    }
                }
            }
            else if (abilities == null)
            {
                abilities = group.abilities;
            }
        }

        public void Merge(Ability ability)
        {
            if (abilities != null && ability != null)
            {
                foreach (var ab in abilities)
                {
                    ab.Merge(ability);
                }
            }
            else if (abilities == null && ability != null)
            {
                abilities = new List<Ability>() { ability };
            }
        }

        public static AbilityGroup Load(string name)
        {
            //Log.Debug($"Load Ability Group: {name}");
            //var bytes = ResUtil.Load<TextAsset>($"AbilitySystem/{name}").bytes;
            //var ability = Serializer.Deserialize(bytes, typeof(AbilityGroup));
            ////TODO: REMOVE CODE HERE
            //var table = TablePlayerAbility.Get(name);
            //if (table != null && !string.IsNullOrEmpty(table.icon))
            //{
            //    ((AbilityGroup)ability).image = table.icon;
            //}
            //return (AbilityGroup)ability;
            return null;
        }
    }
}
