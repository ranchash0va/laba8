namespace lab8
{
    using System;
    using System.IO;

    class Program
    {
        static Item[] items;

        static void Main()
        {
            // Использование универсального класса для сохранения и загрузки данных.
            DataHandler<Item[]> dataHandler = new DataHandler<Item[]>();

            // Загрузка данных из бинарного файла, если файл существует.
            if (File.Exists("binary_data.dat"))
            {
                items = dataHandler.LoadFromBinaryFile("binary_data.dat");
                if (items == null)
                {
                    Console.WriteLine("Ошибка при загрузке данных из бинарного файла. Создан новый массив.");
                    items = new Item[0];
                }
            }
            else
            {
                Console.WriteLine("Бинарный файл не существует. Создан новый массив.");
                items = new Item[0];
            }

            while (true)
            {
                Console.WriteLine("\nВыберите действие:");
                Console.WriteLine("1. Добавить элемент");
                Console.WriteLine("2. Удалить элемент");
                Console.WriteLine("3. Просмотр инвентаря");
                Console.WriteLine("4. Закрыть");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddItem();
                        break;
                    case "2":
                        RemoveItem();
                        break;
                    case "3":
                        ViewInventory();
                        break;
                    case "4":
                        // Сохранение данных перед закрытием.
                        dataHandler.SaveToBinaryFile(items, "binary_data.dat");
                        dataHandler.SaveToJsonFile(items, "json_data.json");
                        return;
                    default:
                        Console.WriteLine("Некорректный выбор. Пожалуйста, выберите 1, 2, 3 или 4.");
                        break;
                }
            }
        }

        static void AddItem()
        {
            Console.WriteLine("Введите данные для нового предмета:");
            Console.Write("Имя предмета: ");
            string name = Console.ReadLine();

            Console.Write("Количество: ");
            int quantity;
            while (!int.TryParse(Console.ReadLine(), out quantity) || quantity <= 0)
            {
                Console.WriteLine("Введите корректное положительное число.");
                Console.Write("Количество: ");
            }

            // Проверка наличия предмета с таким же именем в инвентаре.
            int existingItemIndex = Array.FindIndex(items, item => item?.Name == name);
            if (existingItemIndex != -1)
            {
                // Если предмет существует, увеличиваем количество.
                items[existingItemIndex].Quantity += quantity;
            }
            else
            {
                // Создание нового массива с увеличенным размером.
                Item[] newItems = new Item[items.Length + 1];

                // Копирование существующих элементов в новый массив.
                for (int i = 0; i < items.Length; i++)
                {
                    newItems[i] = items[i];
                }

                // Добавление нового элемента.
                newItems[newItems.Length - 1] = new Item { Name = name, Quantity = quantity };

                // Обновление массива.
                items = newItems;
            }

            // Сохранение данных после добавления элемента.
            SaveData();
        }

        static void RemoveItem()
        {
            if (items.Length == 0)
            {
                Console.WriteLine("Нет элементов для удаления.");
                return;
            }

            Console.WriteLine("Выберите номер элемента для удаления:");

            for (int i = 0; i < items.Length; i++)
            {
                Console.WriteLine($"{i + 1}. Name: {items[i].Name}, Quantity: {items[i].Quantity}");
            }

            int indexToRemove;
            while (!int.TryParse(Console.ReadLine(), out indexToRemove) || indexToRemove < 1 || indexToRemove > items.Length)
            {
                Console.WriteLine("Введите корректный номер элемента.");
                Console.Write("Введите номер элемента: ");
            }

            // Выбор количества элементов для удаления.
            Console.Write($"Введите количество элементов для удаления (от 1 до {items[indexToRemove - 1].Quantity}): ");
            int quantityToRemove;
            while (!int.TryParse(Console.ReadLine(), out quantityToRemove) || quantityToRemove < 1 || quantityToRemove > items[indexToRemove - 1].Quantity)
            {
                Console.WriteLine($"Введите корректное число от 1 до {items[indexToRemove - 1].Quantity}.");
                Console.Write("Введите количество элементов для удаления: ");
            }

            // Уменьшение количества выбранного элемента.
            items[indexToRemove - 1].Quantity -= quantityToRemove;

            // Если количество стало равно нулю, удаляем элемент из массива.
            if (items[indexToRemove - 1].Quantity == 0)
            {
                // Создание нового массива с уменьшенным размером.
                Item[] newItems = new Item[items.Length - 1];

                // Копирование элементов до удаляемого.
                for (int i = 0; i < indexToRemove - 1; i++)
                {
                    newItems[i] = items[i];
                }

                // Копирование элементов после удаленного.
                for (int i = indexToRemove; i < items.Length; i++)
                {
                    newItems[i - 1] = items[i];
                }

                // Обновление массива.
                items = newItems;
            }

            // Сохранение данных после удаления элемента.
            SaveData();
        }

        static void ViewInventory()
        {
            if (items.Length == 0)
            {
                Console.WriteLine("Инвентарь пуст.");
            }
            else
            {
                Console.WriteLine("Информация о предметах в инвентаре:");
                for (int i = 0; i < items.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. Name: {items[i].Name}, Quantity: {items[i].Quantity}");
                }
            }
        }

        static void SaveData()
        {
            // Сохранение данных в бинарный файл.
            DataHandler<Item[]> dataHandler = new DataHandler<Item[]>();
            dataHandler.SaveToBinaryFile(items, "binary_data.dat");

            // Сохранение данных в JSON файл.
            dataHandler.SaveToJsonFile(items, "json_data.json");

            Console.WriteLine("Данные успешно сохранены.");
        }
    }
}