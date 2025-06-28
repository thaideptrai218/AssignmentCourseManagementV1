using AssignmentCourseManagementV1.Models;
using AssignmentCourseManagementV1.ViewModels;
using AssignmentCourseManagementV1.Views;
using System.ComponentModel;
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

namespace AssignmentCourseManagementV1.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {

        }
        private void cbCourses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ScheduleGrid.Items.Filter = obj =>
            {
                Course selected = (Course)cbCourses.SelectedItem;
                CourseSchedule target = (CourseSchedule)obj;
                return target.CourseId == selected.CourseId;
            };
        }
    }
}