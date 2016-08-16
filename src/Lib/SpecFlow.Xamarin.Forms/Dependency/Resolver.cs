namespace SpecFlow.Xamarin.Forms.Dependency
{
    public class Resolver
    {
        static Resolver()
        {
            Instance = new AutoFacResolver();
        }

        public static void Reset()
        {
            Instance = null;
            Instance = new AutoFacResolver();
        }

        /// <summary>
        ///     Gets or sets the instance.
        /// </summary>
        public static IResolver Instance { get; set; }
    }
}
