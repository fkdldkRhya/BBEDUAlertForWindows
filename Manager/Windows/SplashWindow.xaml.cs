using Firebase.Auth;
using Firebase.Database;
using Firebase.Storage;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json.Linq;
using Manager.Classes.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Manager.Windows
{
    /// <summary>
    /// SplashWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SplashWindow : Window
    {
        public SplashWindow()
        {
            InitializeComponent();
        }



        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // 시작 위치 설정
            this.Left = (SystemParameters.WorkArea.Width) / 2 + SystemParameters.WorkArea.Left - Width / 2;
            this.Top = (SystemParameters.WorkArea.Height) / 2 + SystemParameters.WorkArea.Left - Height / 2;

            // Topmost 설정
            this.Topmost = true;
            await Task.Run(() => Thread.Sleep(1000));
            this.Topmost = false;

            // Firebase 접근 확인
            firebaseAccessChecker();
        }



        private void firebaseAccessChecker()
        {
            SplashWindow_TaskLog.Content = "Verifying Firebase key...";

            // Firebase 접근 데이터 확인 [ Realtime Database ]
            if (AdminLoginManager.getInstance().setAdminInfoForJson()) // 키 로딩 성공
            {
                try
                {
                    // Realtime database Client
                    FirebaseClient firebaseClient = AdminLoginManager.getInstance().getRealtimeDatabaseClient();
                    AdminLoginManager.getInstance().verifyRealtimeDatabaseKey(
                        firebaseClient,
                        () => { // 실패 Listener
                            showForFirebaseAuthInputDialog();
                            firebaseAccessChecker();
                        },
                        async () => { // 성공 Listener
                            // Firebase 접근 데이터 확인 [ Storage ]
                            try
                            {
                                var getStroageInfo = await firebaseClient.Child("admin-info").OnceAsync<object>();
                                string jsonValue = null;
                                string managerIdValue = null;
                                string managerPwValue = null;
                                string apiKey = null;
                                string version = null;

                                foreach (var obj in getStroageInfo)
                                {
                                    if (obj.Key.Equals("admin-json"))
                                        jsonValue = (string)obj.Object;
                                    else if (obj.Key.Equals("manager-id"))
                                        managerIdValue = (string)obj.Object;
                                    else if (obj.Key.Equals("manager-pw"))
                                        managerPwValue = (string)obj.Object;
                                    else if (obj.Key.Equals("api-key"))
                                        apiKey = (string)obj.Object;
                                    else if (obj.Key.Equals("amin-version"))
                                        version = (string)obj.Object;
                                    else if (obj.Key.Equals("fcm-api-key"))
                                        AdminLoginManager.getInstance().fcmAPIKey = (string)obj.Object;
                                }

                                AdminLoginManager.getInstance().version = version;

                                string jsonFileName = "rhya-network-bbedu-alert-e768b-firebase-adminsdk-ijcxi-d732ea17b4.json";

                                byte[] byte64 = Convert.FromBase64String(jsonValue);
                                string jsonFileData = Encoding.UTF8.GetString(byte64);

                                System.IO.File.WriteAllText(jsonFileName, jsonFileData);

                                var defaultApp = FirebaseApp.Create(new AppOptions()
                                {
                                    Credential = GoogleCredential.FromFile(jsonFileName),
                                });

                                AdminLoginManager.getInstance().apiKey = apiKey;
                                AdminLoginManager.getInstance().managerID = managerIdValue;
                                AdminLoginManager.getInstance().managerPW = managerPwValue;

                                var auth = new Firebase.Auth.FirebaseAuthProvider(new FirebaseConfig(apiKey));
                                var a = await auth.SignInWithEmailAndPasswordAsync(managerIdValue, managerPwValue);

                                var task = await new FirebaseStorage(
                                    "rhya-network-bbedu-alert-e768b.appspot.com",
                                     new FirebaseStorageOptions
                                     {
                                         AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                                         ThrowOnCancel = true,
                                     })
                                    .Child("client_download_check.txt")
                                    .GetDownloadUrlAsync();

                                WebClient webClient = new WebClient();
                                string html = await webClient.DownloadStringTaskAsync(task);

                                if (html.Equals("Hello Admin Client!"))
                                {
                                    SplashWindow_TaskLog.Content = "Loading user data...";
                                    await AdminLoginManager.getInstance().reloadUserData(firebaseClient, false);
                                    SplashWindow_TaskLog.Content = "Loading default data...";
                                    await AdminLoginManager.getInstance().reloadDefaultData(firebaseClient, false);
                                    SplashWindow_TaskLog.Content = "Loading message data...";
                                    await AdminLoginManager.getInstance().reloadMessageData(firebaseClient, false);
                                    SplashWindow_TaskLog.Content = "Loading file data...";
                                    await AdminLoginManager.getInstance().reloadFileData(firebaseClient, false);

                                    AdminLoginManager.getInstance().reloadedDate = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

                                    SplashWindow_TaskLog.Content = "Loading main window...";
                                    MainWindow mainWindow = new MainWindow();
                                    mainWindow.Show();
                                    this.Close();
                                }
                                else
                                {
                                    StringBuilder stringBuilder = new StringBuilder();
                                    stringBuilder.AppendLine("Firebase stroage 관리자 인증 실패!");
                                    stringBuilder.AppendLine("\"client_download_check.txt\" 파일을 확인하는 도중 오류가 발생하였습니다.해당 오류가 지속적으로 발생하면 관리자에게 문의 해 주십시오.");
                                    stringBuilder.AppendLine(Environment.NewLine);
                                    stringBuilder.Append("Firebase storage Check URL: ");
                                    stringBuilder.AppendLine(task);
                                    stringBuilder.AppendLine(Environment.NewLine);
                                    stringBuilder.Append("Firebase storage Check Value: ");
                                    stringBuilder.AppendLine(html);
                                    MessageBox.Show(stringBuilder.ToString(), "Authentication error", MessageBoxButton.OK, MessageBoxImage.Error);
                                    ExceptionManager.getInstance().exitProgram(0);
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionManager.getInstance().showMessageBox(ex);
                                ExceptionManager.getInstance().exitProgram(0);
                            }
                        });
                }
                catch (Exception ex)
                {
                    ExceptionManager.getInstance().showMessageBox(ex);
                    ExceptionManager.getInstance().exitProgram(0);
                }
            }
            else // 키 로딩 실패
            {
                showForFirebaseAuthInputDialog();
                firebaseAccessChecker();
            }
        }





        /// <summary>
        /// 관리자 키 입력 Dialog
        /// </summary>
        private void showForFirebaseAuthInputDialog()
        {
            SplashWindow_TaskLog.Content = "Waiting for Firebase key input...";

            Dialog.FirebaseAuthInputDialog firebaseAuthInputDialog = new Dialog.FirebaseAuthInputDialog();
            firebaseAuthInputDialog.ShowDialog();
        }
    }
}
