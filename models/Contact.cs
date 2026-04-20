using System;
using System.Collections.Generic;
using System.Text;

namespace lab9.models
{
    /// Модель данных
    /// Представляет сущность "Контакт" в телефонной книге
    /// Хранит только данные, без логики!!!
    public class Contact
    {
        public string Name { get; set; }
        public string Phone { get; set; }
    }
}
