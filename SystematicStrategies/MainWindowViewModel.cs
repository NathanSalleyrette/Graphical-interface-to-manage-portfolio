using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Commands;
using System.Windows.Controls;

namespace SystematicStrategies
{
    internal class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
            //FirstDate = new DatePicker();
            //FirstDate.SelectedDate = new DateTime(2009, 12, 12);
            FirstDate = new DateTime(2009, 12, 12);
        }

        public DateTime FirstDate { get; set; }
        
    }

}
