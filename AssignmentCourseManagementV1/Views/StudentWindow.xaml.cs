using AssignmentCourseManagementV1.ViewModels;
using System.Windows;

namespace AssignmentCourseManagementV1.Views
{
    public partial class StudentWindow : Window
    {
        public StudentWindow()
        {
            InitializeComponent();
            DataContext = new StudentViewModel();
        }
    }
}
