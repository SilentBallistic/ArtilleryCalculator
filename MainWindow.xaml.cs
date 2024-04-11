using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SilentBallisticsArtilleryCalculatorV2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double friendlyDist = 0;
        double friendlyAzim = 0;
        double enemyDist = 0;
        double enemyAzim = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            friendlyDist = Validate(txtFriendlyDistance.Text);
            if (friendlyDist == -1)
            {
                txtFriendlyDistance.Text = "Incorrect Input";
                return;
            }

            friendlyAzim = Validate(txtFriendlyAzimuth.Text);
            if (friendlyAzim < 0)
            {
                txtFriendlyAzimuth.Text = "Incorrect Input";
                return;
            }
            //----------------------------------------------------//
            enemyDist = Validate(txtEnemyDistance.Text);
            if (enemyDist == -1)
            {
                txtEnemyDistance.Text = "Incorrect Input";
                return;
            }

            enemyAzim = Validate(txtEnemyAzimuth.Text);
            if (enemyAzim < 0)
            {
                txtEnemyAzimuth.Text = "Incorrect Input";
                return;
            }

            CalculateAndOutput();
        }

        private void CalculateAndOutput()
        {
            double distance, azimuth, slope;
            //double finalAngle = 0;
            //double finalAzimuth = 0;
            double[] X = new double[2], Y = new double[2];

            if (enemyAzim > 180)
                enemyAzim -= 180;
            else enemyAzim += 180;

            if (friendlyAzim > 180)
                friendlyAzim -= 180;
            else friendlyAzim += 180;

            friendlyAzim *= (Math.PI / 180);
            enemyAzim *= (Math.PI / 180);

            if (friendlyAzim > (2 * Math.PI))
                friendlyAzim -= (2 * Math.PI);
            else friendlyAzim += (2 * Math.PI);

            if (enemyAzim > (2 * Math.PI))
                enemyAzim -= (2 * Math.PI);
            else enemyAzim += (2 * Math.PI);

            X[0] = enemyDist * Math.Cos(enemyAzim);
            Y[0] = enemyDist * Math.Sin(enemyAzim);

            X[1] = friendlyDist * Math.Cos(friendlyAzim);
            Y[1] = friendlyDist * Math.Sin(friendlyAzim);

            distance = Math.Sqrt(Math.Pow(X[0] - X[1], 2) + Math.Pow(Y[0] - Y[1], 2));

            slope = (Y[0] - Y[1]) / (X[0] - X[1]);

            azimuth = Math.Atan(slope) * (180 / Math.PI);

            if (X[0] == X[1])
                azimuth = Math.PI / 2;

            if (azimuth > 180)
                azimuth -= 180;
            else azimuth += 180;

            txtAzimuth.Text = Math.Round(azimuth).ToString();
            txtDistance.Text = Math.Round(distance).ToString();
        }

        private double Validate(string text)
        {
            double returnValue = 0;

            if (!double.TryParse(text, out returnValue))
                return -1;
            else
                return returnValue;
        }

        public void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.GotFocus -= TextBox_GotFocus;
        }
    }
}