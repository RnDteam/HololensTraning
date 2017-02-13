public enum Weapon  {
   None, Missile, Rocket
}

public static class Extensions
{
    public static string GetHebrewString(this Weapon weapon)
    {
        switch (weapon)
        {
            case Weapon.Missile:
                return "טיל";
            case Weapon.Rocket:
                return "רקטה";
            default:
                return "אין";
        }
    }
}

