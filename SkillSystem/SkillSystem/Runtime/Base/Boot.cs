namespace SkillSystem.Runtime
{
    public class Boot
    {
        public ILoader loader;
        public ILogger logger;
        public IPhysics physics;
        public ActionHandler actionHandler;

        private Boot() { }
        public Boot(ILoader loader, IPhysics physics, ILogger logger)
        {
            this.loader = loader;
            this.physics = physics;
            this.logger = logger;
            this.actionHandler = new ActionHandler();
        }

        public AbilityManager CreateAbilityManager(IUnit unit)
        {
            var mgr = new AbilityManager();
            mgr.Init(this, unit);
            return mgr;
        }

        public ModifierManager CreateModifierManager(IUnit unit)
        {
            var mgr = new ModifierManager();
            mgr.Init(this, unit);
            return mgr;
        }
    }
}
