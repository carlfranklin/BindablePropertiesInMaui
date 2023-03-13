namespace BindablePropertiesInMaui;

public class PersonEditor : BindableObject
{
    public static readonly BindableProperty PersonProperty = BindableProperty.Create(
        nameof(Person),
        typeof(Person),
        typeof(PersonEditor));

    public PersonEditor(Person person)
    {
        Person = person;
    }

    public Person Person
    {
        get { return (Person)GetValue(PersonProperty); }
        set { SetValue(PersonProperty, value); }
    }
}