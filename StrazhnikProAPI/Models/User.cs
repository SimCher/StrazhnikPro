namespace StrazhnikProAPI.Models;

public class User
{
    public int Id { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    //!!!ОБЯЗАТЕЛЬНО СОЗДАЁМ СВОЙСТВО С ВНЕШНИМ КЛЮЧОМ ПРИ НАЛИЧИИ СВЯЗИ!!!
    public int GroupId { get; set; }
    
    //т.к. отношение одна группа - ко многим пользователям, то мы храним один связанный
    //экземпляр группы
    public Group Group { get; set; }
}