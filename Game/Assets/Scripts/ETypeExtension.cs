public static class ETypeExtension
{
    public static string ToFriendlyString(this Item.EType rhs)
    {
        switch (rhs)
        {
            case Item.EType.Head:
            case Item.EType.Shoulder:
            case Item.EType.Chest:
            case Item.EType.Back:
            case Item.EType.Wrist:
            case Item.EType.Hands:
            case Item.EType.Waist:
            case Item.EType.Legs:
            case Item.EType.Feet:
            case Item.EType.Neck:
            case Item.EType.Trinket:
            case Item.EType.Finger:
                return rhs.ToString();
            case Item.EType.OneHand:
                return "Main Hand";
            case Item.EType.TwoHand:
                return "Two Hands";
            case Item.EType.OffHand:
                return "Off Hand";
            default:
                return "Undefined Item.EType in extension ToFriendlyString!";
        }
    }

    public static string ToSlotType(this Item.EType rhs)
    {
        switch (rhs)
        {
            case Item.EType.Head:
            case Item.EType.Shoulder:
            case Item.EType.Chest:
            case Item.EType.Back:
            case Item.EType.Wrist:
            case Item.EType.Hands:
            case Item.EType.Waist:
            case Item.EType.Legs:
            case Item.EType.Feet:
            case Item.EType.Neck:
            case Item.EType.Trinket:
            case Item.EType.Finger:
            case Item.EType.OffHand:
                return rhs.ToString();
            case Item.EType.OneHand:
            case Item.EType.TwoHand:
                return "MainHand";
            default:
                return "Undefined Item.EType in extension ToSlotType!";
        }
    }
}