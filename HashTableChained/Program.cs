
/// <summary>
/// элемент хеш-таблицы
/// </summary>
/// <typeparam name="TKey">тип ключа</typeparam>
/// <typeparam name="TValue">тип значениея</typeparam>
class HashTableItem<TKey, TValue>
{
    public TKey key { get; private set; }               // ключ
    public TValue value { get; private set; }           // значение

    public HashTableItem(TKey key, TValue value)
    {
        this.key = key;
        this.value = value;
    }
}
/// <summary>
/// хеш-таблица
/// </summary>
/// <typeparam name="TKey">тип ключа</typeparam>
/// <typeparam name="TValue">тип значения</typeparam>
class HashTable<TKey, TValue>
{
    private LinkedList<HashTableItem<TKey, TValue>>[] table;    // массив из двухсвязных списков, типизированных HashtableItem
    private int capacity;                                       // вместимость
    private int size;                                           // размер
    private const double LOAD_FACTOR = 0.5;                     // если таблица заполнена на 50%
    private const int DEF_CAPACITY = 4;                         // вместимость по умолчанию (4)
    /// <summary>
    /// констркутор
    /// </summary>
    /// <param name="capacity">вместимость</param>
    public HashTable(int capacity)
    {
        size = 0;                                                               // размер
        this.capacity = capacity <= 0 ? DEF_CAPACITY : capacity;                // вместимость >=4
        table = new LinkedList<HashTableItem<TKey, TValue>>[capacity];          // новая хеш-таблица
    }
    /// <summary>
    /// получение хеша
    /// </summary>
    /// <param name="key">ключ</param>
    /// <returns>целочисленное значение хеша</returns>
    private int Hash(TKey key)
    {
        return Math.Abs(key.GetHashCode()) % capacity;          // хеш 
    }
    /// <summary>
    /// метод проверки загруженности хеш-таблицы
    /// </summary>
    /// <returns>значение от нуля до единицы</returns>
    public double GetLoadFactor()
    {
        return (double)size / (double)capacity;                // размер / вместимость
    }
    /// <summary>
    /// метод добавления пары 
    /// </summary>
    /// <param name="key">ключ</param>
    /// <param name="value">значение</param>
    public void Add(TKey key, TValue value)
    {
        if (GetLoadFactor() >= LOAD_FACTOR) this.Resize();                  // если загруженность => 50% увеличить вместимость 

        int index = Hash(key);                                              // получение индекса

        if (table[index] == null)                                           // если двусвязный список по индексу еще не создан
        {
            table[index] = new LinkedList<HashTableItem<TKey, TValue>>();   // создание двусвязного списка
        }

        var hashTableItem = new HashTableItem<TKey, TValue>(key, value);    // создание нового элемента таблицы

        var listNode = new LinkedListNode<HashTableItem<TKey, TValue>>(hashTableItem);  // создание узла и сообщение его с элементом

        table[index].AddFirst(listNode);    // добавление первого узла двусвязного списка в массиве по индексу 

        size++;                             // инкрементируем размер
    }
    /// <summary>
    /// метод удаления пары по ключу
    /// </summary>
    /// <param name="key">ключ</param>
    /// <returns>возвращает true при удалении существующего элемента</returns>
    public bool Remove(TKey key)
    {
        int index = Hash(key);                      // получение индекса

        if (table[index] == null) return false;     // если элемента по индексу не существует возвращаеи false

        foreach (var item in table[index])          // перебор узлов связанного списка по индексу
        {
            if (item.key.Equals(key))               // нахождение совпадения
            {
                table[index].Remove(item);          // удаление узла
                break;
            }
        }
        return true;
    }
    /// <summary>
    /// метод увеличения вместимости хеш-таблицы
    /// </summary>
    private void Resize()
    {
        capacity = (capacity * 2) + 1;            // +1 позволит избежать коллизии на тех же местах, изменив (не)четность
                                                  // так как хеш считается по остатку от деления
        var OldTable = table;                   // копируем ссылку на старый массив
        size = 0;                               // обнуление перед перехешированием

        table = new LinkedList<HashTableItem<TKey, TValue>>[capacity];      // создаем новый массив

        foreach (var item in OldTable)          // проходим все элементы в старом массиве
        {
            if (item != null)                    // если элемент не равен null
            {
                foreach (var pair in item)      // перебор узлов двусвязного списка
                {
                    if (pair != null)           // если элемент не равен null
                    {
                        //ПЕРЕХЕШИРОВАНИЕ С НОВЫМИ ЗНАЧЕНИЯМИ CAPACITY
                        this.Add(pair.key, pair.value);     // добавляем его в новый массив
                    }
                }
            }
        }
    }
    /// <summary>
    /// метод получения значения по ключу
    /// </summary>
    /// <param name="key">ключ</param>
    /// <returns>возвращает значение value</returns>
    public TValue GetValue(TKey key)
    {
        int index = Hash(key);                              // получение индекса

        if (table[index] == null) return default(TValue);   //если не существует элемента массива

        foreach (var item in table[index])                  //перебор элементов связанного списка по индексу в массиве
        {
            if (item.key.Equals(key)) return item.value;    //если ключ существует
        }

        return default(TValue);                             //если ключа не существует
    }
    /// <summary>
    /// очистка таблицы, обнуление размера, 
    /// создание новой таблицы со стандартной вместимостью 4
    /// </summary>
    public void Clear()
    {
        size = 0;                                                               // обнулить размер
        table = new LinkedList<HashTableItem<TKey, TValue>>[DEF_CAPACITY];      // создать новый пустой массив с вместимостью по умолчанию 
    }
}
