using Manager.Classes.Utils;
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

namespace Manager.Windows.Dialog
{
    /// <summary>
    /// FirebaseAuthInputDialog.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class FirebaseAuthInputDialog : Window
    {
        public FirebaseAuthInputDialog()
        {
            InitializeComponent();
        }



        /// <summary>
        /// 확인 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            string inputRealTimeDatabasrURL = realtimeDatbaseURL.Text;
            string inpuFirebasePassword = firebasePassword.Text;

            if (inputRealTimeDatabasrURL.Replace(" ", "").Length <= 0)
            {
                result.Text = "Firebase realtime database URL을 입력해 주세요. 해당 항목은 필수로 입력해야 합니다.";
                realtimeDatbaseURL.Focus();

                return;
            }

            if (inpuFirebasePassword.Replace(" ", "").Length <= 0)
            {
                result.Text = "Firebase admin password를 입력해 주세요. 해당 항목은 필수로 입력해야 합니다.";
                firebasePassword.Focus();

                return;
            }

            AdminLoginManager.getInstance().setAdminInfoForInput(inputRealTimeDatabasrURL, inpuFirebasePassword);
            AdminLoginManager.getInstance().saveAdminInfoToFile();

            this.Close();
        }



        /// <summary>
        /// 프로그램 종료 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            ExceptionManager.getInstance().exitProgram(0);
        }
    }
}
