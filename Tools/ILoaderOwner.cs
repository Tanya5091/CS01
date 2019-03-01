
using System.ComponentModel;

using System.Windows;

namespace Birth.Tools

{

	internal interface ILoaderOwner : INotifyPropertyChanged

	{

		Visibility LoaderVisibility { get; set; }

		bool IsControlEnabled { get; set; }

	}

}
