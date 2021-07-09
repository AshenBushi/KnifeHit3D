namespace Watermelon
{
    public class IAPManagerInitModule : InitModule
    {
        public IAPSettings settings;

        public IAPManagerInitModule()
        {
            moduleName = "IAP Manager";
        }

        public override void CreateComponent(Initialiser Initialiser)
        {
            IAPManager IAPManager = new IAPManager();
            IAPManager.Init(Initialiser.gameObject, settings);
        }
    }
}

// -----------------
// IAP Manager v 0.4
// -----------------