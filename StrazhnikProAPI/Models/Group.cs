namespace StrazhnikProAPI.Models;

public class Group
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    //если столбец Name в базе данных может иметь NULL, то мы указываем его тип в классе с вопросительным знаком
    //public string? Name {get; set; }
    
    //так можно указывать любые типы
    //int? double? float? bool? object?
    
    //кстати, если ты будешь хранить фотографии, то их нужно указывать как массив байтов с вопросом или без
    //byte[] или byte[]?
    
    //Т.к. связь одна группа - ко многим пользователям, то здесь мы храним коллекцию экземпляров студентов
    //привязанных к группе
    public List<User> Users { get; set; }
    
    //Если связь между группой и пользователями была бы необязательной, то тип списка был бы указан с вопросом
    //public List<User>? Users {get; set;}
}