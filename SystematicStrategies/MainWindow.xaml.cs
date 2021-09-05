using PricingLibrary.FinancialProducts;
using PricingLibrary.Utilities.MarketDataFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SystematicStrategies.Strategies;
using static SystematicStrategies.Controller;

namespace SystematicStrategies
{
	/// <summary>
	/// Logique d'interaction pour MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			//Share action = new Share("AC FP", "AC FP");
			//SemiHistoricDataFeedProvider sdf = new SemiHistoricDataFeedProvider();
			//var strike = 12;
			//VanillaCall opt = new VanillaCall("VCall", action, new DateTime(2010, 10, 30), strike);
			//var strat = new DeltaNeutralStrategy();
			//Controller controller = new Controller(opt, new DateTime(2010, 01, 05), new DateTime(2010, 10, 30), sdf, strat);
			//controller.start();
			InitializeComponent();
			this.DataContext = new MainWindowViewModel();
		}
	}
}
