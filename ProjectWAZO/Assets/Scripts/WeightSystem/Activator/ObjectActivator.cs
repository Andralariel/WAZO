namespace WeightSystem.Activator
{
    public class ObjectActivator : Activator
    {
        public override void Activate()
        {
            gameObject.SetActive(true);
        }

        public override void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}
