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
using System.Windows.Shapes;
using WpfApp1.Data;

namespace WpfApp1
{
    /// <summary>
    /// Window2.xaml 的交互逻辑
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();


            DemoModel = new PropertyGridModel
            {
                String = "TestString",
                Enum = Gender.Female,
                Boolean = true,
                Integer = 98,
                VerticalAlignment = VerticalAlignment.Stretch
            };
        }

        public static readonly DependencyProperty DemoModelProperty = 
            DependencyProperty.Register("DemoModel", typeof(PropertyGridModel), typeof(Window2), 
                new PropertyMetadata(default(PropertyGridModel)));

        public PropertyGridModel DemoModel
        {
            get => (PropertyGridModel)GetValue(DemoModelProperty);
            set => SetValue(DemoModelProperty, value);
        }

    }




}
