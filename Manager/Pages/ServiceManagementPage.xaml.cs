using Firebase.Database;
using Manager.Classes.Utils;
using Manager.Windows;
using Newtonsoft.Json.Linq;
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

namespace Manager.Pages
{
    /// <summary>
    /// ServiceManagementPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ServiceManagementPage : Page
    {
        // MainWindow
        private MainWindow mainWindow;


        public ServiceManagementPage(MainWindow mainWindow)
        {
            InitializeComponent();

            // 인자 설정
            this.mainWindow = mainWindow;
        }



        /// <summary>
        /// Grid load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            // Progress dialog 설정
            mainWindow.loadingProgressbarGridPanel.Visibility = Visibility.Visible;
            mainWindow.loadingProgressbarPanel.getProgressbar().IsIndeterminate = true;
            mainWindow.loadingProgressbarPanel.setTitle("초기화 중...");

            await Task.Run(() => reloadTask());

            // Progress dialog 설정
            mainWindow.loadingProgressbarGridPanel.Visibility = Visibility.Hidden;
            mainWindow.loadingProgressbarPanel.getProgressbar().IsIndeterminate = false;
        }



        /// <summary>
        /// 데이터 로딩
        /// </summary>
        private async void reloadTask()
        {
            try
            {
                var getFB1 = await AdminLoginManager.getInstance().getRealtimeDatabaseClient().Child("app-info").OnceAsync<object>();
                var getFB2 = await AdminLoginManager.getInstance().getRealtimeDatabaseClient().Child("private-key").OnceAsync<object>();

                string privateKey = null;
                string isAccess = null;
                string version = null;

                foreach (var obj in getFB1)
                {
                    if (obj.Key.Equals("access"))
                        isAccess = (string)obj.Object;
                    if (obj.Key.Equals("version"))
                        version = (string)obj.Object;
                }

                foreach (var obj in getFB2)
                {
                    if (obj.Key.Equals("key"))
                        privateKey = (string)obj.Object;
                }

                Application.Current.Dispatcher.Invoke(() => accessKey.Text = privateKey);
                Application.Current.Dispatcher.Invoke(() => clientVersion.Text = version);
                if (isAccess.Equals("true"))
                {
                    Application.Current.Dispatcher.Invoke(() => serviceAccessComboBox.SelectedIndex = 0);
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(() => serviceAccessComboBox.SelectedIndex = 1);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.getInstance().showMessageBox(ex);
            }
        }


        
        /// <summary>
        /// 사용자 정보 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Progress dialog 설정
                mainWindow.loadingProgressbarGridPanel.Visibility = Visibility.Visible;
                mainWindow.loadingProgressbarPanel.getProgressbar().IsIndeterminate = true;
                mainWindow.loadingProgressbarPanel.setTitle("변경 사항 저장 중...");

                string inputVersion = clientVersion.Text;
                string inputKey = accessKey.Text;
                string inputAccess = "true";
                if (serviceAccessComboBox.SelectedIndex == 1)
                    inputAccess = "false";

                await Task.Run(async() => 
                { 
                    try
                    {
                        JObject object1 = new JObject();
                        JObject object2 = new JObject();

                        object1.Add("access", inputAccess);
                        object1.Add("version", inputVersion);
                        object1.Add("project-name", "kro.kr.rhya-network.bbedu-alert");
                        object2.Add("key", inputKey);

                        FirebaseClient firebaseClient = AdminLoginManager.getInstance().getRealtimeDatabaseClient();
                        await firebaseClient.Child("/app-info").PutAsync(object1.ToString());
                        await firebaseClient.Child("/private-key").PutAsync(object2.ToString());
                    }
                    catch (Exception ex)
                    {
                        ExceptionManager.getInstance().showMessageBox(ex);
                    }
                });

                // Progress dialog 설정
                mainWindow.loadingProgressbarGridPanel.Visibility = Visibility.Hidden;
                mainWindow.loadingProgressbarPanel.getProgressbar().IsIndeterminate = false;
            }
            catch (Exception ex)
            {
                ExceptionManager.getInstance().showMessageBox(ex);
            }
        }
    }
}
