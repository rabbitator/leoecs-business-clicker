namespace BusinessClicker.Data
{
    public static class PlayerPrefsNames
    {
        public static string GetUserBalanceName()
        {
            return "UserBalance";
        }

        public static string GetBusinessBalanceName(int businessIndex)
        {
            return $"Business_{businessIndex}_Balance";
        }
        
        public static string GetBusinessLevelName(int businessIndex)
        {
            return $"Business_{businessIndex}_Level";
        }

        public static string GetImprovementName(int businessIndex, int improvementIndex)
        {
            return $"Business_{businessIndex}_HasImprovement_{improvementIndex}";
        }
    }
}