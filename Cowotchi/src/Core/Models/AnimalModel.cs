public class AnimalModel : CreatureModel
{
    public void SetCreatureType(AnimalType animalType)
    {
        switch (animalType.Id)
        {
            case (int)Enumerations.AnimalTypes.Cow:
                CreatureType = Enumerations.CreatureTypes.Cow;
                break;
            case (int)Enumerations.AnimalTypes.Chicken:
                CreatureType = Enumerations.CreatureTypes.Chicken;
                break;
            default:
                break;
        }
    }
}