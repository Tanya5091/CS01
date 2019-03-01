using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Birth.Tools;
namespace Birth
{

	internal class UserViewModel : INotifyPropertyChanged, ILoaderOwner
	{ 
        #region Fields
       
		private string _age;
		private string _date;
		private string _zodiac;
		private string _chinese;
		private Visibility _loaderVisibility = Visibility.Hidden;
		private bool _isControlEnabled = true;
		private static readonly string[] Zodiacs =
		{
			"Capricorn", "Aquarius", "Pisces", "Aries", "Taurus", "Gemini", "Cancer", "Leo", "Virgo", "Libra", "Scorpio",
			"Sagittarius"
		};

		private static readonly string[] ChineseHoroscopes =
			{ "Monkey", "Rooster", "Dog", "Pig", "Rat", "Ox", "Tiger", "Rabbit", "Dragon", "Snake", "Horse", "Sheep"};
		private RelayCommand<object> _relay;
		#endregion

		public UserViewModel()
		{
			LoaderManager.Instance.Initialize(this);
		}

		public string Zodiac
		{
			get { return _zodiac; }
			set { _zodiac = value; OnPropertyChanged(); }
		}

		public string Chinese
		{
			get { return _chinese; }
			set { _chinese = value; OnPropertyChanged(); }
		}
		public string Date
		{
			get { return _date; }
			set { _date = value; OnPropertyChanged(); }
		}


		public string Age
		{
			get { return _age; }
			set{ _age = value; OnPropertyChanged(); }
		}

		public RelayCommand<object> Calculate
		{
			get
			{
				return _relay ?? (_relay = new RelayCommand<object>(
						   Calculation, o => CanExecuteCommand()));
			}
		}
		

		private async void Calculation(object obj)
		{
			LoaderManager.Instance.ShowLoader();

			await Task.Run(() =>
			{
				DateTime birthday = Convert.ToDateTime(_date);
				if (!IsValid(birthday))
				{
					LoaderManager.Instance.HideLoader();
					return;
				}

				FullAge(birthday);
				ChineseSign(birthday);
				ZodiacSign(birthday);
			});

			LoaderManager.Instance.HideLoader();
		}

		private bool IsValid(DateTime date)
		{
			if (DateTime.Today.Year < date.Year|| DateTime.Today.Year-date.Year>134 || (DateTime.Today.DayOfYear <= date.DayOfYear && DateTime.Today.Year == date.Year))
			{
				MessageBox.Show("The value is out of range");
				return false;
			}
			if (DateTime.Today.Month == date.Month && DateTime.Today.Day == date.Day)
			{
				
					MessageBox.Show("Wishing you all the best");
			}

			return true;
		}

		public bool CanExecuteCommand()
		{
			return !String.IsNullOrWhiteSpace(_date);
		}

		private void FullAge(DateTime birth)
		{
			int years = DateTime.Today.Year - birth.Year;
			if (DateTime.Today.Month < birth.Month||(DateTime.Today.Month == birth.Month && DateTime.Today.Day < birth.Day))
			{
				years--;
			}
			Age = years.ToString();
		}

		public void ZodiacSign(DateTime date)
		{
			int zodN = 0;
			var day = date.Day;
			switch (date.Month)
			{
				case 1:

					zodN = day < 21 ? 0 : 1;
					break;
				case 2:
					zodN = day < 20 ? 1 : 2;
					break;
				case 3:

					zodN = day < 22 ? 2 : 3;
					break;
				case 4:
					zodN = day < 21 ? 3 : 4;
					break;
				case 5:

					zodN = day < 22 ? 4 : 5;
					break;
				case 6:
					zodN = day < 22 ? 5 : 6;
					break;
				case 7:

					zodN = day < 24 ? 6 : 7;
					break;
				case 8:
					zodN = day < 24 ? 7 : 8;
					break;
				case 9:

					zodN = day < 24 ? 8 : 9;
					break;
				case 10:
					zodN = day < 24 ? 9 : 10;
					break;
				case 11:

					zodN = day < 23 ? 10 : 11;
					break;
				case 12:
					zodN = day < 23 ? 11 : 0;
					break;
			}
			 Zodiac= Zodiacs[zodN];
		}

		private void ChineseSign(DateTime date)
		{
			int num = date.Year % 12;
			Chinese = ChineseHoroscopes[num];
		}

		public Visibility LoaderVisibility
		{
			get { return _loaderVisibility; }
			set
			{
				_loaderVisibility = value;
				OnPropertyChanged();
			}
		}

		public bool IsControlEnabled
		{
			get { return _isControlEnabled; }
			set
			{
				_isControlEnabled = value;
				OnPropertyChanged();
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}


