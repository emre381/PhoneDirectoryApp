using SQLite;
using System.ComponentModel.DataAnnotations.Schema;

namespace Week7Ex1
{
    public class CustomerViewModel : BindableObject
    {
        private ImageSource _profileImageSource = ImageSource.FromFile("user1.png"); // Default profil fotoğrafı
        public ImageSource ProfileImageSource
        {
            get => _profileImageSource;
            set
            {
                _profileImageSource = value;
                OnPropertyChanged();
            }
        }

        // Diğer müşteri özellikleri buraya eklenebilir
    }


    [SQLite.Table("customer")]
    public class Customer
    {
        [PrimaryKey]
        [AutoIncrement]
        [SQLite.Column("id")]
        public int Id { get; set; }
        [SQLite.Column("customer_name")]
        public string CustomerName { get; set; }
        [SQLite.Column("mobile")]
        public string Mobile { get; set; }
        [SQLite.Column("email")]
        public string Email { get; set; }
    }
}
