using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp_net_Framework_
{
    class Person
    {
        public string Name;
        public int Age;
        public List<string> Languages;
        public Person(string Name, int Age, List<string> Languages)
        {
            this.Name = Name; this.Age = Age; this.Languages = Languages;
        }
        public override bool Equals(object obj)
        {
            if (obj is Person person)
                return Name == person.Name;
            return false;
        }
        public override int GetHashCode() => Name.GetHashCode();
    }

    class CustomStringComparer : IComparer<String>
    {
        public int Compare(string x, string y)
        {
            int xLength = x == null ? 0 : x.Length; // если x равно null, то длина 0
            int yLength = y == null ? 0 : x.Length;
            return xLength - yLength;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string[] men = { "Александр", "Борис", "Владимир", "Евгений", "Алексей", "Михаил", "Сергей", "Анатолий" };
            string[] men_two = { "Артем", "Степан", "Олег", "Алексей", "Никита",  "Борис", "Иван", "Анатолий" };
            //________________________________________________________________________ФИЛЬТРАЦИЯ КОЛЛЕКЦИИ ___________________________________________________________
            var selectedPeople = 
                men.Where(p => p.ToUpper().StartsWith("А")).Distinct().OrderBy(p => p);
            int countPeople = men.Count();
            foreach (string person in selectedPeople)
                            Console.WriteLine(person);
                        Console.ReadLine();            
            // методы расширения
            var pLength3 = men.Where(p => p.Length == 3);
            // операторы запросов
            pLength3 = from p in men 
            where p.Length == 3 
            select p;            
            int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }; 
            var num1 = numbers.Where(i => i % 2 == 0 && i > 5);
            var num2 = from i in numbers
                         where i % 2 == 0 && i > 5
                         select i;            
            //ФИЛЬТРАЦИЯ СЛОЖНЫХ ТИПОВ
            List<Person> people = new List<Person> { 
                new Person ("Tom", 23, new List<string> {"english", "german"}), new Person ("Bob", 27, new List<string> {"english", "french" }), new Person ("Sam", 29, new List<string>  { "english", "spanish" }),
                new Person ("Sam", 27, new List<string>  { "english", "french" }), new Person ("Alice", 24, new List<string> {"spanish", "german" })
            };
            List<Person> people_2 = new List<Person> {
                new Person ("Tom", 23, new List<string> {"english", "german"}), new Person ("Bob", 27, new List<string> {"english", "french" }), new Person ("Sam", 29, new List<string>  { "english", "spanish" }),
                new Person ("Sam", 27, new List<string>  { "english", "french" }), new Person ("Alice", 24, new List<string> {"spanish", "german" })
            };
            var selectedPeople_new = from p in people
                                 where p.Age > 25
                                 select p;

            selectedPeople_new = from person in people
                                 from lang in person.Languages
                                 where person.Age < 28
                                 where lang == "english"
                                 select person;

            selectedPeople_new = people.SelectMany(u => u.Languages,
                            (u, l) => new { Person = u, Lang = l })
                          .Where(u => u.Lang == "english" && u.Person.Age < 28)
                          .Select(u => u.Person);

            selectedPeople_new = people.Where(u => u.Age < 28 && u.Languages.Exists(s => s == "английский"));//++
            //_______________________________________________________________________СОРТИРОВКА КОЛЛЕКЦИИ ___________________________________________________________
            var sortedPeople = from p in people
                                orderby p 
                                select p;

            sortedPeople = people.OrderBy(p => p);
            //СОРТИРОВКА СЛОЖНЫХ ОБЪЕКТОВ
            sortedPeople = from p in people
                           orderby p.Name //orderby p.Name descending
                           select p;

            sortedPeople = people.OrderBy(p => p.Name); // OrderByDescending (p => p.Name)
            //МНОЖЕСТВЕННЫЕ КРИТЕРИИ СОРТИРОВКИ
            sortedPeople = from p in people
                           orderby p.Name, p.Age
                           select p;

            sortedPeople = people.OrderBy(p => p.Name).ThenBy(p => p.Age);
            //ПЕРЕНОПРЕДЕЛЕНИЕ КРИТЕРИЯ СОРТИРОВКИ
            sortedPeople = people.OrderBy(p => p.Name, new CustomStringComparer());
            //__________________________________________________________________ОБЪЕДИНЕНИЕ, ПЕРЕСЕКЧЕНЕИЕ И РАЗНОСТЬ КОЛЛЕКЦИИ ___________________________________________
            string[] soft = { "Microsoft", "Google", "Apple" };
            string[] hard = { "Apple", "IBM", "Samsung" };

            // разность последовательностей
            var result = soft.Except(hard);
            // пересечение последовательностей
            result = soft.Intersect(hard);
            // удаление дублей
            result = soft.Distinct();
            // объединение последовательностей
            result = soft.Union(hard);    //повторы добавляются 1 раз!
            result = soft.Concat(hard);   //добавляются все записи
            //СЛОЖНЫЕ ОБЪЕКТЫ
            Person[] students = { new Person("Tom", 28, new List<string> { "english", "german" }), new Person("Bob", 32, new List<string> { "english", "german" }), new Person("Sam", 33, new List<string> { "english", "german" }) };
            Person[] employees = { new Person("Tom", 25, new List<string> { "english", "german" }), new Person("Bob", 34, new List<string> { "english", "german" }), new Person("Mike", 24, new List<string> { "english", "german" }) };
            // объединение последовательностей
            var sortedPeople_new  = students.Union(employees);
            //_______________________________________________________________________АГРЕГАТНЫЕ ОПЕРАЦИИ ___________________________________________________________
            //операции над выборкой, например, получение числа элементов, получение минимального, максимального и среднего значения в выборке, а также суммирование значений
            int[] numbers_new = { 1, 2, 3, 4, 5 };

            int query = numbers_new.Aggregate((x, y) => x - y);// int query = 1 - 2 - 3 - 4 - 5
            Console.WriteLine(query);   // -13
            
            //Еще одна версия метода позволяет задать начальное значение, с которого начинается цепь агрегатных операций
            string[] words = { "Gaudeamus", "igitur", "Juvenes", "dum", "sumus" };
            var sentence = words.Aggregate("Text:", (first, next) => $"{first} {next}");
            Console.WriteLine(sentence);  // Text: Gaudeamus igitur Juvenes dum sumus

            int size = numbers_new.Count();  // 5
                size = numbers.Count(i => i % 2 == 0 && i > 3); //1
            int sum = numbers.Sum(); //15
            int min = numbers.Min(); //1
            int max = numbers.Max(); //5

            //Если мы работаем со сложными объектами, то в эти методы передается делегат, который принимает свойство, применяемое в вычислениях
            int minAge = people.Min(p => p.Age); // минимальный возраст
            int maxAge = people.Max(p => p.Age); // максимальный возраст
            var selectedP = people.Where(x => x.Age == maxAge);// people.Where(x => x.Age == people.Max(n => n.Age));
            double averageAge = people.Average(p => p.Age); //средний возраст

            //_______________________________________________________________________ПОЛУЧЕНИЕ ЧАСТИ КОЛЛЕКЦИИ ________________________________________________________
            var resultSkip = people.Skip(2);    // пропускает определенное количество элементов
            //resultSkip = people.SkipLast(2);  // с конца коллекции
            resultSkip = people.SkipWhile(p => p.Name.Length == 3); //пропускает элементы, длина которых равна 3 символам

            var resultTake = people.Take(3);    // извлекает определенное число элементов
            //resultTake = people.TakeLast(3);    // с конца коллекции
            resultTake = people.TakeWhile(p=> p.Name.Length == 3); // выбирает цепочку элементов, начиная с первого элемента, пока они удовлетворяют определенному условию
            resultTake = people.Skip(3).Take(2);    // пропускаем 3 элемента и выбираем 2 элемента

            //______________________________________________________________________ГРУППИРОВКА КОЛЛЕКЦИИ ___________________________________________________________
            //Если в выражении LINQ последним ОПЕРАТОРОМ, выполняющим операции над выборкой, является group, то оператор select не применяется.
            var names = from person in people
                            group person by person.Name;

            foreach (var name in names)
            {
                Console.WriteLine(name.Key); // Каждая группа имеет ключ, который мы можем получить через свойство Key: name.Key
                foreach (var person in name) 
                { 
                    Console.WriteLine(person.Name); 
                }
                Console.WriteLine(); // для разделения между группами
            }
            //метод расширения
            names = people.GroupBy(p => p.Name);

            //создание нового объекта при группировке
            var Names = from person in people
                    group person by person.Name into g
                    select new { Name = g.Key, Count = g.Count()};// Александр : 2 ; Евгений : 3 ; создается объект для каждой группы

            foreach (var name in Names)
            {
                Console.WriteLine($"{name.Name} : {name.Count}");
            }
            //метод расширения
            Names = people.GroupBy(p => p.Name).Select(g => new { Name = g.Key, Count = g.Count() }); // var antype = new { User = "Usr", Age = 21 };
            
            //вложенные запросы
            //создание нового объекта при группировке
            var Names_gr = from person in people
                         group person by person.Name into g
                         select new { Name = g.Key, Count = g.Count(), Age = from a in g select a };// Александр : 2 >> 32 25 ; Евгений : 3 >> 45 31; создается объект для каждой группы
            //метод расширения
            Names_gr = people.GroupBy(p => p.Name).Select(g => new { Name = g.Key, Count = g.Count(), Age = g.Select(p=>p) }); //

            //______________________________________________________________________ СОЕДИНЕНИЕ КОЛЛЕКЦИИ ________________________________________________________
            //Соединение в LINQ используется для объединения двух разнотипных наборов в один
            var men_all = from p in people
                            join p2 in people_2 on p.Name equals p2.Name
                            select new { Name = p.Name, Name2 = p2.Name, Language = p2.Languages };

             men_all = people.Join(people_2, // второй набор
             p => p.Name, // свойство-селектор объекта из первого набора
             p2 => p2.Name , // свойство-селектор объекта из второго набора
             (p, p2) => new { Name = p.Name, Name2 = p2.Name, Language = p2.Languages }); // результат

            //Метод GroupJoin() кроме соединения последовательностей также выполняет и группировку.
            //Метод Zip() последовательно объединяет соответствующие элементы текущей последовательности со второй последовательностью, которая передается в метод в качестве параметра.

            //___________________________________________________________ ПРОВЕРКА НАЛИЧИЯ И ПОЛУЧЕНИЕ ЭЛЕМЕНТОВ КОЛЛЕКЦИИ _______________________________________________
            bool allHas6Chars = people.All(s => s.Name.Length == 6);         // все ли имена из 6 символов
            bool allStartsWithT = people.All(s => s.Name.StartsWith("T"));   // все ли имена начинаются с Т символа
            bool allHasMore3Chars = people.Any(s => s.Name.Length > 3);      // хотя бы один соответствует условию            
            //Стоит отметить, что для сравнения объектов применяется реализация метода Equals. Cоответственно, если мы работаем с объектами своих типов, то мы можем реализовать данный метод
            bool hasTom = people.Contains(new Person("Tom", 23, new List<string> { "english", "german" }));     //содержит ли коллекция определенный элемент
            var first_ = people.First();  // Стоит учитывать, что если коллекция пуста или в коллекции нет элементов, который соответствуют условию, то будет сгенерировано исключение.                                    
            var firstWith4Chars = people.First(p => p.Name.Length == 4);  // первая строка, длина которой равна 4 символам
            var first_def = people.FirstOrDefault(); // если в коллекции не окажется элементов, которые соответствуют условию, то метод возвращает значение по умолчанию для этого типа элемента
            var firstWith4Chars_def = people.FirstOrDefault(s => s.Name.Length == 4);
            // Last и LastOrDefault аналогичен по работе методу First

            //___________________________________________________________ ОТЛОРЖЕННОЕ И НЕМЕДЛЕННОЕ ВЫПОЛНЕНИЕ LINQ ____________________________________________________
            //Есть два способа выполнения запроса LINQ: отложенное(deferred) и немедленное(immediate) выполнение.
            //Отложенное выражение не выполняется, до итерация или перебора по выборке, например, в цикле foreach. 
            //Обычно подобные операции возвращают объект IEnumerable<T> или IOrderedEnumerable<T>.
            
            //СПИСОК ОТЛОЖЕННЫХ ОПЕРАЦИЙ LINQ:
            // AsEnumerable, Cast, Concat, DefaultIfEmpty, Distinct, Except, GroupBy, GroupJoin, Intersect, Join, OfType, OrderBy, OrderByDescending, Range,
            // Repeat, Reverse, Select, SelectMany, Skip, SkipWhile, Take, TakeWhile, ThenBy, ThenByDescending, Union, Where
            
            //можно сказать, что это переменная запроса
            selectedP = people.Where(s => s.Name.Length == 3).OrderBy(s => s);
            // до выполнения запроса источник данных может изменяться
            people[2].Name = "Сергей";
            // непосредственно выполнение LINQ-запроса
            foreach (string s in selectedPeople)
                Console.WriteLine(s);

            //______________________________________________________________________ ДЕЛЕГАТЫ В ЗАПРОСАХ LINQ ____________________________________________________
            // Многие методы расширения LINQ в качестве параметра принимают делегаты например, Func<TSource, bool>
            // В качестве делегата в подобные методы удобно передавать лямбда-выражения, но мы также можем передать полноценные методы
            var result_ = people.Where(LenghtIs3);
            foreach (var person in result)
                Console.WriteLine(person);
            bool LenghtIs3(Person p) => p.Name.Length == 3;

            int[] numbers_ = { -2, -1, 0, 1, 2, 3, 4, 5, 6, 7 };
            var result_d = numbers_.Where(i => i > 0).Select(Square);
            foreach (int i in result_d)
                Console.WriteLine(i);
            int Square(int n) => n * n;


            //______________________________________________________________МЕТОДЫ РАСШИРИНИЯ_________________________________________________________________________
            //Select: определяет проекцию выбранных значений
            //Where: определяет фильтр выборки
            //OrderBy: упорядочивает элементы по возрастанию
            //OrderByDescending: упорядочивает элементы по убыванию
            //ThenBy: задает дополнительные критерии для упорядочивания элементов возрастанию
            //ThenByDescending: задает дополнительные критерии для упорядочивания элементов по убыванию
            //Join: соединяет две коллекции по определенному признаку
            //Aggregate: применяет к элементам последовательности агрегатную функцию, которая сводит их к одному объекту
            //GroupBy: группирует элементы по ключу
            //ToLookup: группирует элементы по ключу, при этом все элементы добавляются в словарь
            //GroupJoin: выполняет одновременно соединение коллекций и группировку элементов по ключу
            //Reverse: располагает элементы в обратном порядке
            //All: определяет, все ли элементы коллекции удовлятворяют определенному условию
            //Any: определяет, удовлетворяет хотя бы один элемент коллекции определенному условию
            //Contains: определяет, содержит ли коллекция определенный элемент
            //Distinct: удаляет дублирующиеся элементы из коллекции
            //Except: возвращает разность двух коллекцию, то есть те элементы, которые создаются только в одной коллекции
            //Union: объединяет две однородные коллекции
            //Intersect: возвращает пересечение двух коллекций, то есть те элементы, которые встречаются в обоих коллекциях
            //Count: подсчитывает количество элементов коллекции, которые удовлетворяют определенному условию
            //Sum: подсчитывает сумму числовых значений в коллекции
            //Average: подсчитывает cреднее значение числовых значений в коллекции
            //Min: находит минимальное значение
            //Max: находит максимальное значение
            //Take: выбирает определенное количество элементов
            //Skip: пропускает определенное количество элементов
            //TakeWhile: возвращает цепочку элементов последовательности, до тех пор, пока условие истинно
            //SkipWhile: пропускает элементы в последовательности, пока они удовлетворяют заданному условию, и затем возвращает оставшиеся элементы
            //Concat: объединяет две коллекции
            //Zip: объединяет две коллекции в соответствии с определенным условием
            //First: выбирает первый элемент коллекции
            //FirstOrDefault: выбирает первый элемент коллекции или возвращает значение по умолчанию
            //Single: выбирает единственный элемент коллекции, если коллекция содержит больше или меньше одного элемента, то генерируется исключение
            //SingleOrDefault: выбирает единственный элемент коллекции. Если коллекция пуста, возвращает значение по умолчанию. Если в коллекции больше одного элемента, генерирует исключение
            //ElementAt: выбирает элемент последовательности по определенному индексу
            //ElementAtOrDefault: выбирает элемент коллекции по определенному индексу или возвращает значение по умолчанию, если индекс вне допустимого диапазона
            //Last: выбирает последний элемент коллекции
            //LastOrDefault: выбирает последний элемент коллекции или возвращает значение по умолчанию
        }
    }
}
