using lab9.models;
using lab9.viewModels.PhoneBookApp.Helpers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;

namespace lab9.viewModels
{
    /// ViewModel — связывает View (XAML) и Model (в нашем случае контакт)
    /// Содержит бизнес-логику, команды и состояние UI
    public class MainViewModel : INotifyPropertyChanged
    {
        /// Коллекция контактов.
        /// ObservableCollection автоматически обновляет UI при изменениях
        public ObservableCollection<Contact> Contacts { get; set; }

        private string _name;
        private string _phone;
        private Contact _selectedContact;

        /// Имя нового контакта (привязка к TextBox)
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
                AddCommand.RaiseCanExecuteChanged(); // обновление состояния кнопки
            }
        }

        /// Телефон нового контакта
        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                OnPropertyChanged();
                AddCommand.RaiseCanExecuteChanged();
            }
        }

        /// Выбранный контакт в таблице
        public Contact SelectedContact
        {
            get => _selectedContact;
            set
            {
                _selectedContact = value;
                OnPropertyChanged();
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }

        /// Команда добавления контакта
        public RelayCommand AddCommand { get; set; }

        /// Команда удаления контакта
        public RelayCommand DeleteCommand { get; set; }

        public MainViewModel()
        {
            // Инициализация коллекции контактов
            Contacts = new ObservableCollection<Contact>();

            // Привязка команд к методам логики
            AddCommand = new RelayCommand(_ => AddContact(), _ => CanAdd());
            DeleteCommand = new RelayCommand(p => DeleteContact(p as Contact), p => p is Contact);
        }

        /// Добавление нового контакта в коллекцию
        private void AddContact()
        {
            if (!IsValidName(Name))
            {
                MessageBox.Show("[Имя не должно быть пустым]");
                return;
            }

            if (!IsValidPhone(Phone))
            {
                MessageBox.Show("[Некорректный номер телефона]");
                return;
            }

            Contacts.Add(new Contact
            {
                Name = Name,
                Phone = Phone
            });

            // очистка полей после добавления
            Name = string.Empty;
            Phone = string.Empty;
        }

        /// Удаление выбранного контакта
        private void DeleteContact(Contact contact)
        {
            if (contact != null)
                Contacts.Remove(contact);
        }

        /// Проверка возможности добавления (активация кнопки)
        private bool CanAdd()
        {
            return !string.IsNullOrWhiteSpace(Name) &&
                   !string.IsNullOrWhiteSpace(Phone);
        }

        /// Проверка корректности имени
        private bool IsValidName(string name)
        {
            return !string.IsNullOrWhiteSpace(name);
        }

        /// Проверка номера телефона по формату
        /// +7XXXXXXXXXX или 8XXXXXXXXXX
        private bool IsValidPhone(string phone)
        {
            return Regex.IsMatch(phone, @"^(\+7\d{10}|8\d{10})$");
        }

        /// Событие уведомления UI об изменении свойств
        public event PropertyChangedEventHandler PropertyChanged;

        /// Метод уведомления UI (INotifyPropertyChanged)
        private void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}