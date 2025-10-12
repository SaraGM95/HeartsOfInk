namespace Assets.Scripts.Controller.Debug
{
    public static class DebugStaticHolder
    {
        private static string firstBugMessage;

        public static string FirstBugMessage
        {
            get { return firstBugMessage; }
            set
            {
                if (string.IsNullOrWhiteSpace(firstBugMessage)) 
                { 
                    firstBugMessage = value;
                }
            }
        }
    }
}
