using Firebase.Database;
using Manager.Classes.Utils;
using SharpVectors.Converters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Manager.Windows
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        // Text color
        // ================================================= //
        private readonly string COLOR_BLACK = "#000000";
        private readonly string COLOR_BLUE = "#FF006BFF";
        // ================================================= //

        // ListView item binding
        private ObservableCollection<MenuListViewItemVO> sideMenuItem = new ObservableCollection<MenuListViewItemVO>();

        // Page var
        private Pages.UserManagementPage userManagementPage = null;
        private Pages.OfficeManagementPage officeManagementPage = null;
        private Pages.MessageManagementPage messageManagementPage = null;
        private Pages.MessageSendPage messageSendPage = null;
        private Pages.ServiceManagementPage serviceManagementPage = null;



        public MainWindow()
        {
            InitializeComponent();
        }



        /// <summary>
        /// 프로그램 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            ExceptionManager.getInstance().exitProgram(0);
        }



        /// <summary>
        /// 최대화, 최소화 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonMaxAndMinimized_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized) 
                WindowState = WindowState.Normal;
            else 
                WindowState = WindowState.Maximized;
        }



        /// <summary>
        /// 최소화 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }



        /// <summary>
        /// Main 작업 진행
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            // Task Dialog 보여주기
            loadingProgressbarPanel.getProgressbar().IsIndeterminate = true;
            loadingProgressbarPanel.setTitle("UI Initializing .....");
            loadingProgressbarGridPanel.Visibility = Visibility.Visible;

            // 버전 설정
            pVersion.Text = "v" + AdminLoginManager.getInstance().version;

            // Side menu listView binding
            sideMenuListView.ItemsSource = sideMenuItem;
            // Side menu item init
            initSideMenuItem();

            // 마지막 동기화 시간 설정
            setSyncTime();

            // 기본 페이지 이동
            MyFrame.Navigate(userManagementPage);

            // Dialog 숨기기
            loadingProgressbarGridPanel.Visibility = Visibility.Hidden;
            dialogYesOrNoGridPanel.Visibility = Visibility.Hidden;

            // 메뉴 색상 변경
            menuTextColorChange("사용자 관리");
        }



        /// <summary>
        /// Window load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // 시작 위치 설정
            this.Left = (SystemParameters.WorkArea.Width) / 2 + SystemParameters.WorkArea.Left - Width / 2;
            this.Top = (SystemParameters.WorkArea.Height) / 2 + SystemParameters.WorkArea.Left - Height / 2;

            // Page init
            userManagementPage = new Pages.UserManagementPage(this);
            officeManagementPage = new Pages.OfficeManagementPage(this);
            messageManagementPage = new Pages.MessageManagementPage(this);
            messageSendPage = new Pages.MessageSendPage(this);
            serviceManagementPage = new Pages.ServiceManagementPage(this);

            // 전역 함수 설정
            Mediator.Register("_MAIN_WINDOW_SET_ITEM_COUNT_", setItemCount);
            Mediator.Register("_MAIN_WINDOW_SET_ITEM_COUNT_", setItemCount);

            // Topmost 설정
            this.Topmost = true;
            await Task.Run(() => Thread.Sleep(1000));
            this.Topmost = false;
        }



        /// <summary>
        /// Side menu click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuListView_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int index = GetCurrentIndex_ListView(e.GetPosition, sideMenuListView);
            if (!(index == -1))
            {
                string menu = sideMenuItem[index].title;

                // 메뉴 Page 변경
                switch (menu)
                {
                    case "사용자 관리":
                        {
                            menuTextColorChange("사용자 관리");

                            MyFrame.Navigate(userManagementPage);
                            break;
                        }

                    case "학원/교습소 관리":
                        {
                            menuTextColorChange("학원/교습소 관리");

                            MyFrame.Navigate(officeManagementPage);
                            break;
                        }

                    case "메시지 관리":
                        {
                            menuTextColorChange("메시지 관리");

                            MyFrame.Navigate(messageManagementPage);
                            break;
                        }

                    case "공지사항 전송":
                        {
                            menuTextColorChange("공지사항 전송");

                            MyFrame.Navigate(messageSendPage);
                            break;
                        }

                    case "서비스 관리":
                        {
                            menuTextColorChange("서비스 관리");

                            MyFrame.Navigate(serviceManagementPage);
                            break;
                        }
                }
            }
        }



        /// <summary>
        /// Menu color change
        /// </summary>
        /// <param name="menu">Menu title</param>
        private void menuTextColorChange(string menu)
        {
            // 색상 변경
            foreach (MenuListViewItemVO menuListViewItemVO in sideMenuItem)
            {
                if (menuListViewItemVO.title.Equals(menu))
                    menuListViewItemVO.color = COLOR_BLUE;
                else
                    menuListViewItemVO.color = COLOR_BLACK;
            }

            // 변경 적용
            sideMenuListView.Items.Refresh();
        }



        // ListView_1 Click CurrentIndex
        // ============================================================================================== //
        private int GetCurrentIndex_ListView(GetPositionDelegate_ListView getPosition, ListView listView)
        {
            int index = -1;
            for (int i = 0; i < listView.Items.Count; ++i)
            {
                ListViewItem item = GetListViewItem(i, listView);
                if (this.IsMouseOverTarget_ListView(item, getPosition))
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
        private bool IsMouseOverTarget_ListView(Visual target, GetPositionDelegate_ListView getPosition )
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(target);
            Point mousePos = getPosition((IInputElement)target);
            return bounds.Contains(mousePos);
        }
        delegate Point GetPositionDelegate_ListView(IInputElement element);
        ListViewItem GetListViewItem(int index, ListView listView)
        {
            if (listView.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated) { return null; }
            return listView.ItemContainerGenerator.ContainerFromIndex(index) as ListViewItem;
        }
        // ============================================================================================== //



        /// <summary>
        /// Init side item listView menu
        /// </summary>
        private void initSideMenuItem()
        {
            MenuListViewItemVO userManagement = new MenuListViewItemVO();
            userManagement.title = "사용자 관리";
            userManagement.color = COLOR_BLACK;
            userManagement.svgImage = "/Resources/icon/ic_user_management.svg";

            sideMenuItem.Add(userManagement);

            MenuListViewItemVO officeManagemnt = new MenuListViewItemVO();
            officeManagemnt.title = "학원/교습소 관리";
            officeManagemnt.color = COLOR_BLACK;
            officeManagemnt.svgImage = "/Resources/icon/ic_office_for_blue.svg";

            sideMenuItem.Add(officeManagemnt);

            MenuListViewItemVO messageManagement = new MenuListViewItemVO();
            messageManagement.title = "메시지 관리";
            messageManagement.color = COLOR_BLACK;
            messageManagement.svgImage = "/Resources/icon/ic_message_management.svg";

            sideMenuItem.Add(messageManagement);

            /*
            MenuListViewItemVO fileManagement = new MenuListViewItemVO();
            fileManagement.title = "파일 관리";
            fileManagement.color = COLOR_BLACK;
            fileManagement.svgImage = "/Resources/icon/ic_file_management.svg";

            sideMenuItem.Add(fileManagement);
            */

            MenuListViewItemVO sendMessage = new MenuListViewItemVO();
            sendMessage.title = "공지사항 전송";
            sendMessage.color = COLOR_BLACK;
            sendMessage.svgImage = "/Resources/icon/ic_send_message.svg";

            sideMenuItem.Add(sendMessage);

            /*
            MenuListViewItemVO webBroser = new MenuListViewItemVO();
            webBroser.title = "서울북부지원교육청";
            webBroser.color = COLOR_BLACK;
            webBroser.svgImage = "/Resources/icon/ic_web_browser.svg";

            sideMenuItem.Add(webBroser);
            */

            MenuListViewItemVO serviceManagement = new MenuListViewItemVO();
            serviceManagement.title = "서비스 관리";
            serviceManagement.color = COLOR_BLACK;
            serviceManagement.svgImage = "/Resources/icon/ic_service_management.svg";

            sideMenuItem.Add(serviceManagement);
        }



        /// <summary>
        /// 마지막 동기화 시간 설정
        /// </summary>
        private void setSyncTime()
        {
            syncTime.Text = AdminLoginManager.getInstance().reloadedDate;
        }



        /// <summary>
        /// 동기화 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void syncStartButton_Click(object sender, RoutedEventArgs e)
        {
            // SVG 파일 경로
            Uri SVG_IMAGE_PATH_SUCCESS = new Uri("/Resources/icon/ic_check.svg", UriKind.RelativeOrAbsolute);
            Uri SVG_IMAGE_PATH_TASK = new Uri("/Resources/icon/ic_sync_for_blue.svg", UriKind.RelativeOrAbsolute);
            Uri SVG_IMAGE_PATH_FAIL = new Uri("/Resources/icon/ic_fail.svg", UriKind.RelativeOrAbsolute);


            syncStartButton.IsEnabled = false;

            syncTaskProgressbar.IsIndeterminate = true;

            syncResultImage1.Visibility = Visibility.Hidden;
            syncResultImage2.Visibility = Visibility.Hidden;
            syncResultImage3.Visibility = Visibility.Hidden;
            syncResultImage4.Visibility = Visibility.Hidden;

            // Firebase client
            FirebaseClient firebaseClient = AdminLoginManager.getInstance().getRealtimeDatabaseClient();


            // 파일 데이터 동기화
            try
            {
                await AdminLoginManager.getInstance().reloadFileData(firebaseClient, true);
                svgImageSettingForSync(SVG_IMAGE_PATH_SUCCESS, syncResultImage1);
            }
            catch (Exception ex)
            {
                svgImageSettingForSync(SVG_IMAGE_PATH_FAIL, syncResultImage1);

                ExceptionManager.getInstance().showMessageBox(ex);
            }
            syncResultImage1.Visibility = Visibility.Visible;


            // 기본 데이터 동기화
            try
            {
                await AdminLoginManager.getInstance().reloadDefaultData(firebaseClient, true);
                svgImageSettingForSync(SVG_IMAGE_PATH_SUCCESS, syncResultImage2);
            }
            catch (Exception ex)
            {
                svgImageSettingForSync(SVG_IMAGE_PATH_FAIL, syncResultImage2);

                ExceptionManager.getInstance().showMessageBox(ex);
            }
            syncResultImage2.Visibility = Visibility.Visible;


            // 사용자 데이터 동기화
            try
            {
                await AdminLoginManager.getInstance().reloadUserData(firebaseClient, true);
                svgImageSettingForSync(SVG_IMAGE_PATH_SUCCESS, syncResultImage3);
            }
            catch (Exception ex)
            {
                svgImageSettingForSync(SVG_IMAGE_PATH_FAIL, syncResultImage3);

                ExceptionManager.getInstance().showMessageBox(ex);
            }
            syncResultImage3.Visibility = Visibility.Visible;


            // 메시지 데이터 동기화
            try
            {
                await AdminLoginManager.getInstance().reloadMessageData(firebaseClient, true);
                svgImageSettingForSync(SVG_IMAGE_PATH_SUCCESS, syncResultImage4);
            }
            catch (Exception ex)
            {
                svgImageSettingForSync(SVG_IMAGE_PATH_FAIL, syncResultImage4);

                ExceptionManager.getInstance().showMessageBox(ex);
            }
            syncResultImage4.Visibility = Visibility.Visible;


            // 동기화 날자 변경
            AdminLoginManager.getInstance().reloadedDate = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            setSyncTime();


            syncTaskProgressbar.IsIndeterminate = false;
            syncStartButton.IsEnabled = true;
        }



        /// <summary>
        /// SVG 이미지 설절
        /// </summary>
        /// <param name="uri">SVG 이미지 URI</param>
        /// <param name="viewbox">SVG View</param>
        private void svgImageSettingForSync(Uri uri, SvgViewbox viewbox)
        {
            var resourceInfo = Application.GetResourceStream(uri);

            if (resourceInfo != null)
            {
                using (var resourceStream = resourceInfo.Stream)
                {
                    viewbox.StreamSource = resourceStream;
                }
            }
        }



        /// <summary>
        /// Window 상태 변경 감지
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
                svgImageSettingForSync(new Uri("/Resources/icon/ic_restore.svg", UriKind.RelativeOrAbsolute), buttonMaxAndMinimizedImage);
            else
                svgImageSettingForSync(new Uri("/Resources/icon/ic_maximize.svg", UriKind.RelativeOrAbsolute), buttonMaxAndMinimizedImage);
        }



        /// <summary>
        /// 아이템 개수 설정
        /// </summary>
        /// <param name="input">개수</param>
        public void setItemCount(object input)
        {
            ItemCount.Content = (string)input;
        }
    }
}
