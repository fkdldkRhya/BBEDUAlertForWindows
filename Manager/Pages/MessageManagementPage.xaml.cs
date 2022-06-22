using Firebase.Auth;
using Firebase.Database;
using Firebase.Storage;
using Newtonsoft.Json.Linq;
using Manager.Classes.Utils;
using Manager.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// MessageManagementPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MessageManagementPage : Page
    {
        // MainWindow
        private MainWindow mainWindow;

        // TreeView item
        private ObservableCollection<MessageOfficeSelectTreeViewVO> officeMessageSelectItem = new ObservableCollection<MessageOfficeSelectTreeViewVO>();
        // ListView item
        private ObservableCollection<MessageDateListViewVO> messageDateSelectListItem = new ObservableCollection<MessageDateListViewVO>();

        // 마우스 더블클릭 변수
        private int doubleClickIndex = 0;
        private int clickNum = 0; // 클릭 횟수
        private long Firsttime = 0; // 첫번째 클릭시간

        // 선택한 학원 정보
        private MessageOfficeSelectTreeViewVO messageOfficeSelectTreeViewVO = null;
        // 선택한 메시지 정보
        private int messageDateSelectedindex = -1;




        public MessageManagementPage(MainWindow mainWindow)
        {
            InitializeComponent();

            // 인자 설정
            this.mainWindow = mainWindow;
        }



        /// <summary>
        /// Grid loaded event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            // Item 개수 설정
            Mediator.NotifyColleagues("_MAIN_WINDOW_SET_ITEM_COUNT_", "");

            // Progress dialog 설정
            mainWindow.loadingProgressbarGridPanel.Visibility = Visibility.Visible;
            mainWindow.loadingProgressbarPanel.getProgressbar().IsIndeterminate = false;
            mainWindow.loadingProgressbarPanel.getProgressbar().Maximum = AdminLoginManager.getInstance().defaultDataVODict.Count;
            mainWindow.loadingProgressbarPanel.getProgressbar().Value = 0;
            mainWindow.loadingProgressbarPanel.setTitle("메시지 정보 초기화 중...");

            // Tree view setting
            await Task.Run(() => treeViewItemReload());

            // Progressbar dialog 숨기기
            mainWindow.loadingProgressbarGridPanel.Visibility = Visibility.Hidden;
        }



        /// <summary>
        /// TreeView 리스트 초기화
        /// </summary>
        private void treeViewItemReload()
        {
            Application.Current.Dispatcher.Invoke(() => officeMessageSelectItem.Clear());

            // 데이터 추가
            foreach (string keys in AdminLoginManager.getInstance().defaultDataVODict.Keys)
            {
                try
                {
                    // 학원.교습소 데이터
                    DefaultDataVO defaultDataVO = AdminLoginManager.getInstance().defaultDataVODict[keys];

                    MessageOfficeSelectTreeViewVO messageOfficeSelectTreeViewVO = new MessageOfficeSelectTreeViewVO()
                    {
                        officeName = defaultDataVO.name,
                        officeRegNum = defaultDataVO.num
                    };

                    // 메시지 확인
                    if (AdminLoginManager.getInstance().requestMessageVODict.ContainsKey(defaultDataVO.num))
                    {
                        List<RequestMessageVO> requestMessageVOs = AdminLoginManager.getInstance().requestMessageVODict[defaultDataVO.num];

                        messageOfficeSelectTreeViewVO.isShow = Visibility.Visible;

                        int messageCount = AdminLoginManager.getInstance().dontReadMessageCount(defaultDataVO.num, requestMessageVOs);
                        if (messageCount <= 0)
                        {
                            messageOfficeSelectTreeViewVO.messageCount = "0";
                            messageOfficeSelectTreeViewVO.isShow = Visibility.Collapsed;
                        }
                        else
                        {
                            messageOfficeSelectTreeViewVO.messageCount = messageCount.ToString();
                        }
                    }
                    else
                    {
                        messageOfficeSelectTreeViewVO.messageCount = "0";
                        messageOfficeSelectTreeViewVO.isShow = Visibility.Collapsed;
                    }

                    // TreeView 추가
                    Application.Current.Dispatcher.Invoke(() => officeMessageSelectItem.Add(messageOfficeSelectTreeViewVO));
                }
                catch (Exception ex)
                {
                    ExceptionManager.getInstance().showMessageBox(ex);
                }

                // Progressbar 증가
                Application.Current.Dispatcher.Invoke(() => mainWindow.loadingProgressbarPanel.getProgressbar().Value = mainWindow.loadingProgressbarPanel.getProgressbar().Value + 1);
            }

            Application.Current.Dispatcher.Invoke(() => officeMessageSelectItem = new ObservableCollection<MessageOfficeSelectTreeViewVO>(officeMessageSelectItem.OrderByDescending(x => x.messageCount)));
            Application.Current.Dispatcher.Invoke(() => messageForOfficeSelectTreeView.ItemsSource = officeMessageSelectItem);
        }



        // ListView_1 Click CurrentIndex
        // ============================================================================================== //
        private int GetCurrentIndex_ListView(GetPositionDelegate_ListView getPosition, ListView listView)
        {
            if (getPosition == null) return -1;

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
        private bool IsMouseOverTarget_ListView(Visual target, GetPositionDelegate_ListView getPosition)
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



        // 마우스 더블 클릭 감지
        // ------------------------------------------------------------------------------------------------------ //
        private int clickNth()
        {
            clickNum++; // 클릭 횟수 증가

            // 현재시각 CurrentTime에 저장 
            long CurrentTime = DateTime.Now.Ticks;

            // 원클릭 시 실행
            if (CurrentTime - Firsttime > 4000000) // 0.4초
            {
                clickNum = 1;
                Firsttime = CurrentTime;
            }

            return clickNum;
        }
        // ------------------------------------------------------------------------------------------------------ //



        /// <summary>
        /// 메시지 상세 내용 출력
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void messageDateListView_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e != null)
                {
                    int index = GetCurrentIndex_ListView(e.GetPosition, messageDateListView);
                    messageDateSelectedindex = index;
                    if (!(index == -1) && clickNth() == 2 && index == doubleClickIndex && doubleClickIndex != -1 && messageOfficeSelectTreeViewVO != null)
                    {
                        doubleClickIndex = -1;

                        // Progress dialog 설정
                        mainWindow.loadingProgressbarGridPanel.Visibility = Visibility.Visible;
                        mainWindow.loadingProgressbarPanel.getProgressbar().IsIndeterminate = true;
                        mainWindow.loadingProgressbarPanel.getProgressbar().Value = 0;
                        mainWindow.loadingProgressbarPanel.setTitle("메시지 불러오는 중...");

                        MessageDateListViewVO messageDateListViewVO = messageDateSelectListItem[index];
                        string fileList = "NoFile";
                        bool isNext = false;
                        await Task.Run(() =>
                        {
                            try
                            {
                                System.IO.FileInfo fileInfo = new System.IO.FileInfo("rhya-network-bbedu-alert-message.json");
                                JObject jsonObject;
                                if (fileInfo.Exists)
                                {
                                    string jsonFileValue = System.IO.File.ReadAllText("rhya-network-bbedu-alert-message.json");
                                    jsonObject = JObject.Parse(jsonFileValue);

                                    if (jsonObject.ContainsKey(messageOfficeSelectTreeViewVO.officeRegNum))
                                    {
                                        JArray array = (JArray)jsonObject[messageOfficeSelectTreeViewVO.officeRegNum];
                                        bool isExt = false;
                                        foreach (string date in array)
                                        {
                                            if (date.Equals(messageDateListViewVO.date))
                                            {
                                                isExt = true;
                                                break;
                                            }
                                        }

                                        if (!isExt)
                                            array.Add(messageDateListViewVO.date);
                                    }
                                    else
                                    {
                                        JArray array = new JArray();
                                        array.Add(messageDateListViewVO.date);
                                        jsonObject.Add(messageOfficeSelectTreeViewVO.officeRegNum, array);
                                    }
                                }
                                else
                                {
                                    jsonObject = new JObject();
                                    JArray array = new JArray();
                                    array.Add(messageDateListViewVO.date);
                                    jsonObject.Add(messageOfficeSelectTreeViewVO.officeRegNum, array);
                                }

                                // 메시지 정보 설정
                                Application.Current.Dispatcher.Invoke(() => mainWindow.dialogMessageInfoPanel.setMessage("", "", "", "", ""));

                                if (AdminLoginManager.getInstance().defaultDataVODict.ContainsKey(messageOfficeSelectTreeViewVO.officeRegNum))
                                {
                                    // 학원 정보
                                    DefaultDataVO defaultDataVO = AdminLoginManager.getInstance().defaultDataVODict[messageOfficeSelectTreeViewVO.officeRegNum];

                                    // 메시지 정보
                                    if (AdminLoginManager.getInstance().requestMessageVODict.ContainsKey(messageOfficeSelectTreeViewVO.officeRegNum))
                                    {
                                        foreach (RequestMessageVO requestMessageVO in AdminLoginManager.getInstance().requestMessageVODict[messageOfficeSelectTreeViewVO.officeRegNum])
                                        {
                                            if (requestMessageVO.date.Equals(messageDateListViewVO.date))
                                            {
                                                fileList = requestMessageVO.file;
                                                Application.Current.Dispatcher.Invoke(() => mainWindow.dialogMessageInfoPanel.setMessage(messageOfficeSelectTreeViewVO.officeName, defaultDataVO.founder, messageOfficeSelectTreeViewVO.officeRegNum, requestMessageVO.title, requestMessageVO.message.Replace("#</br>#", Environment.NewLine)));
                                                isNext = true;
                                                break;
                                            }
                                        }
                                    }
                                }

                                System.IO.File.WriteAllText("rhya-network-bbedu-alert-message.json", jsonObject.ToString());
                            }
                            catch (Exception ex)
                            {
                                ExceptionManager.getInstance().showMessageBox(ex);

                                isNext = false;
                            }

                        });


                        if (isNext)
                        {
                            bool isDontRead = messageDateListViewVO.isShow == Visibility.Visible;
                            messageDateListViewVO.isShow = Visibility.Hidden;
                            messageDateListView.Items.Refresh();

                            try
                            {
                                if (isDontRead)
                                {
                                    if (messageOfficeSelectTreeViewVO.messageCount == null)
                                    {
                                        messageOfficeSelectTreeViewVO.messageCount = "0";
                                    }

                                    if (Int32.Parse(messageOfficeSelectTreeViewVO.messageCount) - 1 <= 0)
                                    {
                                        messageOfficeSelectTreeViewVO.isShow = Visibility.Hidden;
                                    }

                                    messageOfficeSelectTreeViewVO.messageCount = (Int32.Parse(messageOfficeSelectTreeViewVO.messageCount) - 1).ToString();

                                    messageForOfficeSelectTreeView.Items.Refresh();

                                    officeMessageSelectItem = new ObservableCollection<MessageOfficeSelectTreeViewVO>(officeMessageSelectItem.OrderByDescending(x => x.messageCount));
                                    messageForOfficeSelectTreeView.ItemsSource = officeMessageSelectItem;
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionManager.getInstance().showMessageBox(ex);
                            }
                        }

                        // Progress dialog 설정
                        mainWindow.loadingProgressbarGridPanel.Visibility = Visibility.Hidden;
                        mainWindow.loadingProgressbarPanel.getProgressbar().IsIndeterminate = false;


                        if (isNext)
                        {
                            // Message info setting
                            mainWindow.dialogMessageInfoPanel.setAction1(async () =>
                            {
                                // Progress dialog 설정
                                mainWindow.loadingProgressbarGridPanel.Visibility = Visibility.Visible;
                                mainWindow.loadingProgressbarPanel.getProgressbar().IsIndeterminate = true;
                                mainWindow.loadingProgressbarPanel.getProgressbar().Value = 0;
                                mainWindow.loadingProgressbarPanel.setTitle("첨부파일 리스트 불러오는 중...");

                                try
                                {

                                    mainWindow.dialogMessageFileInfoPanel.item.Clear();

                                    await Task.Run(() =>
                                    {
                                        if (!fileList.Equals("NoFile"))
                                        {
                                            if (fileList.Contains(","))
                                            {
                                                string[] split = fileList.Split(',');
                                                foreach (string uid in split)
                                                {
                                                    if (AdminLoginManager.getInstance().userUploadFileVODict.ContainsKey(uid))
                                                    {
                                                        UserUploadFileVO userUploadFileVO = AdminLoginManager.getInstance().userUploadFileVODict[uid];
                                                        Application.Current.Dispatcher.Invoke(() => mainWindow.dialogMessageFileInfoPanel.item.Add(new RHYA.Network.Layout.DialogForFileList.ItemVO() { name = userUploadFileVO.fileName, uid = userUploadFileVO.fileUID }));
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (AdminLoginManager.getInstance().userUploadFileVODict.ContainsKey(fileList))
                                                {
                                                    UserUploadFileVO userUploadFileVO = AdminLoginManager.getInstance().userUploadFileVODict[fileList];
                                                    Application.Current.Dispatcher.Invoke(() => mainWindow.dialogMessageFileInfoPanel.item.Add(new RHYA.Network.Layout.DialogForFileList.ItemVO() { name = userUploadFileVO.fileName, uid = userUploadFileVO.fileUID }));
                                                }
                                            }
                                        }
                                    });
                                    mainWindow.dialogMessageFileInfoPanel.action2 = new Action<string>(async uid =>
                                    {
                                        // 파일 다운로드
                                        if (AdminLoginManager.getInstance().userUploadFileVODict.ContainsKey(uid))
                                        {
                                            // Progress dialog 설정
                                            mainWindow.loadingProgressbarGridPanel.Visibility = Visibility.Visible;
                                            mainWindow.loadingProgressbarPanel.getProgressbar().IsIndeterminate = true;
                                            mainWindow.loadingProgressbarPanel.getProgressbar().Value = 0;
                                            mainWindow.loadingProgressbarPanel.setTitle("첨부파일 다운로드 중...");

                                            UserUploadFileVO userUploadFileVO = AdminLoginManager.getInstance().userUploadFileVODict[uid];

                                            var auth = new Firebase.Auth.FirebaseAuthProvider(new FirebaseConfig(AdminLoginManager.getInstance().apiKey));
                                            var a = await auth.SignInWithEmailAndPasswordAsync(AdminLoginManager.getInstance().managerID, AdminLoginManager.getInstance().managerPW);
                                            var task = await new FirebaseStorage(
                                                "rhya-network-bbedu-alert-e768b.appspot.com",
                                                 new FirebaseStorageOptions
                                                 {
                                                     AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                                                     ThrowOnCancel = true,
                                                 })
                                                .Child("users/%uid%".Replace("%uid%", uid))
                                                .GetDownloadUrlAsync();

                                            WebClient mywebClient = new WebClient();
                                            KnownFolders knownFolders = new KnownFolders();
                                            StringBuilder sb = new StringBuilder();
                                            sb.Append(knownFolders.GetPath((KnownFolder.Downloads)));
                                            sb.Append(System.IO.Path.DirectorySeparatorChar);
                                            sb.Append(userUploadFileVO.fileName);

                                            await mywebClient.DownloadFileTaskAsync(task, sb.ToString());

                                            // Progress dialog 설정
                                            mainWindow.loadingProgressbarGridPanel.Visibility = Visibility.Hidden;
                                            mainWindow.loadingProgressbarPanel.getProgressbar().IsIndeterminate = false;
                                        }
                                    });

                                    mainWindow.dialogMessageFileInfoPanel.action = new Action(() => { mainWindow.dialogMessageFileInfoPanel.item.Clear(); mainWindow.dialogMessageFileInfoGridPanel.Visibility = Visibility.Hidden; });
                                    mainWindow.dialogMessageFileInfoGridPanel.Visibility = Visibility.Visible;
                                }
                                catch (Exception ex)
                                {
                                    ExceptionManager.getInstance().showMessageBox(ex);
                                }

                                // Progress dialog 설정
                                mainWindow.loadingProgressbarGridPanel.Visibility = Visibility.Hidden;
                                mainWindow.loadingProgressbarPanel.getProgressbar().IsIndeterminate = false;
                            });

                            mainWindow.dialogMessageInfoPanel.setAction2(() => { mainWindow.dialogMessageInfoGridPanel.Visibility = Visibility.Hidden; });
                            mainWindow.dialogMessageInfoGridPanel.Visibility = Visibility.Visible;

                        }
                    }

                    doubleClickIndex = index;
                }
            }
            catch (Exception) { }
        }



        /// <summary>
        /// Message date select listener
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param
        private async void messageForOfficeSelectTreeView_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e != null)
            {
                int index = GetCurrentIndex_ListView(e.GetPosition, messageForOfficeSelectTreeView);

                if (!(index == -1))
                {
                    // Item
                    ObservableCollection<MessageDateListViewVO> officeMessageSelectItem = new ObservableCollection<MessageDateListViewVO>();

                    messageDateSelectListItem.Clear();

                    MessageOfficeSelectTreeViewVO messageOfficeSelectTreeViewVO = this.officeMessageSelectItem[index];
                    this.messageOfficeSelectTreeViewVO = messageOfficeSelectTreeViewVO;

                    if (messageOfficeSelectTreeViewVO != null && AdminLoginManager.getInstance().requestMessageVODict.ContainsKey(messageOfficeSelectTreeViewVO.officeRegNum))
                    {
                        List<RequestMessageVO> messageList = AdminLoginManager.getInstance().requestMessageVODict[messageOfficeSelectTreeViewVO.officeRegNum];
                        if (messageList.Count > 0)
                        {

                            // Progress dialog 설정
                            mainWindow.loadingProgressbarGridPanel.Visibility = Visibility.Visible;
                            mainWindow.loadingProgressbarPanel.getProgressbar().IsIndeterminate = false;
                            mainWindow.loadingProgressbarPanel.getProgressbar().Maximum = messageList.Count;
                            mainWindow.loadingProgressbarPanel.getProgressbar().Value = 0;
                            mainWindow.loadingProgressbarPanel.setTitle("메시지 불러오는 중...");

                            // No Value
                            noItemValueLabel.Visibility = Visibility.Hidden;

                            // 비동기 작업 진행
                            await Task.Run(() =>
                            {
                                try
                                {
                                // JSON 파일 읽기
                                System.IO.FileInfo fileInfo = new System.IO.FileInfo("rhya-network-bbedu-alert-message.json");
                                JArray array = null;
                                if (fileInfo.Exists)
                                {
                                    string jsonFileValue = System.IO.File.ReadAllText("rhya-network-bbedu-alert-message.json");
                                    JObject jsonObject = JObject.Parse(jsonFileValue);

                                    if (jsonObject.ContainsKey(messageOfficeSelectTreeViewVO.officeRegNum))
                                    {
                                        array = (JArray)jsonObject[messageOfficeSelectTreeViewVO.officeRegNum];
                                    }
                                }

                                // 메시지 추가
                                foreach (RequestMessageVO requestMessageVO in messageList)
                                    {
                                        try
                                        {
                                            MessageDateListViewVO messageDateListViewVO = new MessageDateListViewVO();

                                            messageDateListViewVO.account = requestMessageVO.account;
                                            foreach (UserVO userVO in AdminLoginManager.getInstance().userVOList)
                                            {
                                                if (userVO.uid.Equals(requestMessageVO.account))
                                                {
                                                    messageDateListViewVO.account = userVO.email;
                                                    break;
                                                }
                                            }

                                            messageDateListViewVO.date = requestMessageVO.date;

                                            if (array != null)
                                            {
                                                foreach (string date in array)
                                                {
                                                    if (date.Equals(requestMessageVO.date))
                                                    {
                                                        messageDateListViewVO.isShow = Visibility.Hidden;

                                                        break;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                messageDateListViewVO.isShow = Visibility.Visible;
                                            }

                                            Application.Current.Dispatcher.Invoke(() => officeMessageSelectItem.Add(messageDateListViewVO));

                                        // Progressbar 증가
                                        Application.Current.Dispatcher.Invoke(() => mainWindow.loadingProgressbarPanel.getProgressbar().Value = mainWindow.loadingProgressbarPanel.getProgressbar().Value + 1);
                                        }
                                        catch (Exception ex)
                                        {
                                            ExceptionManager.getInstance().showMessageBox(ex);
                                        }
                                    }

                                    Application.Current.Dispatcher.Invoke(() => messageDateSelectListItem = new ObservableCollection<MessageDateListViewVO>(officeMessageSelectItem.OrderByDescending(x => x.date)));
                                }
                                catch (Exception ex)
                                {
                                    ExceptionManager.getInstance().showMessageBox(ex);
                                }
                            });

                            // Progressbar dialog 숨기기
                            mainWindow.loadingProgressbarGridPanel.Visibility = Visibility.Hidden;
                        }
                        else
                        {
                            noItemValueLabel.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    {
                        noItemValueLabel.Visibility = Visibility.Visible;
                    }

                    messageDateListView.ItemsSource = messageDateSelectListItem;
                }
            }
        }



        /// <summary>
        /// 선택한 메시지 제거
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeSelectedMessages_Click(object sender, RoutedEventArgs e)
        {
            if (messageDateSelectedindex != -1 && messageOfficeSelectTreeViewVO != null)
            {
                try
                {
                    // 메시지 날짜 구하기
                    MessageDateListViewVO messageDateListViewVO = this.messageDateSelectListItem[messageDateSelectedindex];

                    // 데이터 확인
                    if (AdminLoginManager.getInstance().requestMessageVODict.ContainsKey(messageOfficeSelectTreeViewVO.officeRegNum))
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.Append("정말로 '");
                        stringBuilder.Append(messageDateListViewVO.date);
                        stringBuilder.Append(" [ ");
                        stringBuilder.Append(messageDateListViewVO.account);
                        stringBuilder.Append(" / ");
                        stringBuilder.Append(messageOfficeSelectTreeViewVO.officeName);
                        stringBuilder.Append(" ] ");
                        stringBuilder.Append("' 메시지를 삭제하시겠습니까? 삭제하시려면 `예` 취소하시려면 `아니요` 버튼을 눌러 주세요. (이 사항은 되돌릴 수 없습니다)");

                        mainWindow.dialogYesOrNoPanel.setMessage(stringBuilder.ToString());
                        mainWindow.dialogYesOrNoPanel.getButton1().Content = "예";
                        mainWindow.dialogYesOrNoPanel.getButton2().Content = "아니요";
                        mainWindow.dialogYesOrNoPanel.setAction1(async () =>
                        {
                            try
                            {
                                // Progress dialog 설정
                                mainWindow.loadingProgressbarGridPanel.Visibility = Visibility.Visible;
                                mainWindow.loadingProgressbarPanel.getProgressbar().IsIndeterminate = true;
                                mainWindow.loadingProgressbarPanel.setTitle("메시지 삭제 중...");

                                List<RequestMessageVO> requestMessageVOs = AdminLoginManager.getInstance().requestMessageVODict[messageOfficeSelectTreeViewVO.officeRegNum];
                                RequestMessageVO deleteTarget = null;
                                await Task.Run(() => 
                                {
                                    foreach (RequestMessageVO requestMessageVO in requestMessageVOs)
                                    {
                                        if (requestMessageVO.date.Equals(messageDateListViewVO.date))
                                        {
                                            deleteTarget = requestMessageVO;
                                            break;
                                        }
                                    }
                                });

                                FirebaseSaveDataUserVO firebaseSaveDataUserVO = new FirebaseSaveDataUserVO();
                                FirebaseClient firebaseClient = AdminLoginManager.getInstance().getRealtimeDatabaseClient();

                                if (deleteTarget == null)
                                {
                                    // Dialog 설정
                                    mainWindow.dialogOKPanel.setMessage("알 수 없는 오류가 발생하였습니다. 프로그램을 재시작해주십시오.");
                                    mainWindow.dialogOKPanel.setAction(() => { mainWindow.dialogOKGridPanel.Visibility = Visibility.Hidden; });
                                    mainWindow.dialogOKGridPanel.Visibility = Visibility.Visible;
                                }
                                else
                                {
                                    // 파일 확인
                                    if (!deleteTarget.file.Equals("NoFile"))
                                    {
                                        if (deleteTarget.file.Contains(","))
                                        {
                                            string[] split = deleteTarget.file.Split(',');
                                            foreach(string uid in split)
                                            {
                                                if (AdminLoginManager.getInstance().userUploadFileVODict.ContainsKey(uid))
                                                {
                                                    var auth = new Firebase.Auth.FirebaseAuthProvider(new FirebaseConfig(AdminLoginManager.getInstance().apiKey));
                                                    var a = await auth.SignInWithEmailAndPasswordAsync(AdminLoginManager.getInstance().managerID, AdminLoginManager.getInstance().managerPW);
                                                    await new FirebaseStorage(
                                                        "rhya-network-bbedu-alert-e768b.appspot.com",
                                                         new FirebaseStorageOptions
                                                         {
                                                             AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                                                             ThrowOnCancel = true,
                                                         })
                                                        .Child("users/%uid%".Replace("%uid%", uid))
                                                        .DeleteAsync();
                                                   
                                                    await firebaseClient.Child("/files/%UID%".Replace("%UID%", uid)).DeleteAsync();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (AdminLoginManager.getInstance().userUploadFileVODict.ContainsKey(deleteTarget.file))
                                            {
                                                UserUploadFileVO deleteTargetFile = AdminLoginManager.getInstance().userUploadFileVODict[deleteTarget.file];

                                                var auth = new Firebase.Auth.FirebaseAuthProvider(new FirebaseConfig(AdminLoginManager.getInstance().apiKey));
                                                var a = await auth.SignInWithEmailAndPasswordAsync(AdminLoginManager.getInstance().managerID, AdminLoginManager.getInstance().managerPW);
                                                await new FirebaseStorage(
                                                    "rhya-network-bbedu-alert-e768b.appspot.com",
                                                     new FirebaseStorageOptions
                                                     {
                                                         AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                                                         ThrowOnCancel = true,
                                                     })
                                                    .Child("users/%uid%".Replace("%uid%", deleteTargetFile.fileUID))
                                                    .DeleteAsync();

                                                await firebaseClient.Child("/files/%UID%".Replace("%UID%", deleteTargetFile.fileUID)).DeleteAsync();
                                            }
                                        }
                                    }
                                }



                                // Realtime database 데이터 수정
                                // -------------------------------------------------------------------------------------------------------------------
                                await firebaseClient.Child("/messages/%REG-NUM%/%DATE%".Replace("%DATE%", messageDateListViewVO.date).Replace("%REG-NUM%", messageOfficeSelectTreeViewVO.officeRegNum)).DeleteAsync();
                                // -------------------------------------------------------------------------------------------------------------------


                                await Task.Run(() =>
                                {
                                    // ListView 데이터 수정
                                    for (int index = 0; index < messageDateSelectListItem.Count; index++)
                                    {
                                        if (messageDateSelectListItem[index].date.Equals(messageDateListViewVO.date))
                                        {
                                            Application.Current.Dispatcher.Invoke(() => messageDateSelectListItem.RemoveAt(index));
                                            Application.Current.Dispatcher.Invoke(() => messageDateListView.Items.Refresh());

                                            break;
                                        }
                                    }
                                });


                                // 원본 데이터 수정
                                await Task.Run(() =>
                                {
                                    if (AdminLoginManager.getInstance().requestMessageVODict.ContainsKey(messageOfficeSelectTreeViewVO.officeRegNum))
                                    {
                                        List<RequestMessageVO> messageVOs = AdminLoginManager.getInstance().requestMessageVODict[messageOfficeSelectTreeViewVO.officeRegNum];
                                        for (int index = 0; index < messageVOs.Count; index ++)
                                        {
                                            if (messageVOs[index].date.Equals(messageDateListViewVO.date))
                                            {
                                                messageVOs.RemoveAt(index);

                                                break;
                                            }
                                        }
                                    }
                                });
                            }
                            catch (Exception ex)
                            {
                                ExceptionManager.getInstance().showMessageBox(ex);
                            }

                            messageDateSelectedindex = -1;

                            if (messageDateListViewVO.isShow == Visibility.Visible)
                            {
                                if (messageOfficeSelectTreeViewVO.messageCount == null)
                                {
                                    messageOfficeSelectTreeViewVO.messageCount = "0";
                                }

                                if (Int32.Parse(messageOfficeSelectTreeViewVO.messageCount) - 1 <= 0)
                                {
                                    messageOfficeSelectTreeViewVO.isShow = Visibility.Hidden;
                                }

                                messageOfficeSelectTreeViewVO.messageCount = (Int32.Parse(messageOfficeSelectTreeViewVO.messageCount) - 1).ToString();

                                messageForOfficeSelectTreeView.Items.Refresh();

                                officeMessageSelectItem = new ObservableCollection<MessageOfficeSelectTreeViewVO>(officeMessageSelectItem.OrderByDescending(x => x.messageCount));
                                messageForOfficeSelectTreeView.ItemsSource = officeMessageSelectItem;
                            }

                            // Progress dialog 숨기기
                            mainWindow.loadingProgressbarGridPanel.Visibility = Visibility.Hidden;
                            mainWindow.loadingProgressbarPanel.getProgressbar().IsIndeterminate = false;

                            // YesOrNo dialog 숨기기
                            mainWindow.dialogYesOrNoGridPanel.Visibility = Visibility.Hidden;
                        });
                        mainWindow.dialogYesOrNoPanel.setAction2(() =>
                        {
                            // YesOrNo dialog 숨기기
                            mainWindow.dialogYesOrNoGridPanel.Visibility = Visibility.Hidden;
                        });

                        mainWindow.dialogYesOrNoGridPanel.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        // Dialog 설정
                        mainWindow.dialogOKPanel.setMessage("알 수 없는 오류가 발생하였습니다. 프로그램을 재시작해주십시오.");
                        mainWindow.dialogOKPanel.setAction(() => { mainWindow.dialogOKGridPanel.Visibility = Visibility.Hidden; });
                        mainWindow.dialogOKGridPanel.Visibility = Visibility.Visible;
                    }
                }
                catch (Exception ex)
                {
                    ExceptionManager.getInstance().showMessageBox(ex);
                }
            }
            else
            {
                // Dialog 설정
                mainWindow.dialogOKPanel.setMessage("삭제할 메시지가 선택되지 않았습니다. 메시지를 선택 후 다시 시도해 주십시오.");
                mainWindow.dialogOKPanel.setAction(() => { mainWindow.dialogOKGridPanel.Visibility = Visibility.Hidden; });
                mainWindow.dialogOKGridPanel.Visibility = Visibility.Visible;
            }
        }



        /// <summary>
        /// 메시지 전체 제거
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeAllMessages_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("정말로 수신된 모든 메시지를 삭제하시겠습니까? 삭제하시려면 `예` 취소하시려면 `아니요` 버튼을 눌러 주세요.");
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append("주의! 해당 작업은 삭제되는 메시지 양에 따라 시간이 오래 걸릴 수도 있습니다. 또한 이 작업은 되돌릴 수 없습니다. 위 내용을 충분히 숙지 후 삭제하시길 바랍니다.");


            mainWindow.dialogYesOrNoPanel.setMessage(stringBuilder.ToString());
            mainWindow.dialogYesOrNoPanel.getButton1().Content = "예";
            mainWindow.dialogYesOrNoPanel.getButton2().Content = "아니요";
            mainWindow.dialogYesOrNoPanel.setAction1(async () =>
            {
                // Progress dialog 설정
                mainWindow.loadingProgressbarGridPanel.Visibility = Visibility.Visible;
                mainWindow.loadingProgressbarPanel.getProgressbar().IsIndeterminate = true;
                mainWindow.loadingProgressbarPanel.setTitle("메시지 삭제 준비 중...");

                FirebaseSaveDataUserVO firebaseSaveDataUserVO = new FirebaseSaveDataUserVO();
                FirebaseClient firebaseClient = AdminLoginManager.getInstance().getRealtimeDatabaseClient();

                List<string> deleteFileUid = new List<string>();

                await Task.Run(() => 
                {
                    try
                    {
                        Application.Current.Dispatcher.Invoke(() => mainWindow.loadingProgressbarPanel.getProgressbar().IsIndeterminate = false);
                        foreach (string regNum in AdminLoginManager.getInstance().requestMessageVODict.Keys)
                        {
                            try
                            {
                                stringBuilder.Clear();
                                stringBuilder.Append("메시지 삭제 중...  ");
                                stringBuilder.Append(regNum);
                                Application.Current.Dispatcher.Invoke(() => mainWindow.loadingProgressbarPanel.setTitle(stringBuilder.ToString()));
                                Application.Current.Dispatcher.Invoke(() => mainWindow.loadingProgressbarPanel.getProgressbar().Value = 0);

                                if (AdminLoginManager.getInstance().requestMessageVODict.ContainsKey(regNum))
                                {
                                    // 메시지 리스트
                                    List<RequestMessageVO> requestMessageVOs = AdminLoginManager.getInstance().requestMessageVODict[regNum];
                                    Application.Current.Dispatcher.Invoke(() => mainWindow.loadingProgressbarPanel.getProgressbar().Maximum = requestMessageVOs.Count);

                                    foreach (RequestMessageVO requestMessageVO in requestMessageVOs)
                                    {
                                        // 파일 확인
                                        if (!requestMessageVO.file.Equals("NoFile"))
                                        {
                                            if (requestMessageVO.file.Contains(","))
                                            {
                                                string[] split = requestMessageVO.file.Split(',');
                                                foreach (string uid in split)
                                                {
                                                    deleteFileUid.Add(uid);

                                                    Application.Current.Dispatcher.Invoke(async () => await firebaseClient.Child("/files/%UID%".Replace("%UID%", uid)).DeleteAsync());
                                                }
                                            }
                                            else
                                            {
                                                deleteFileUid.Add(requestMessageVO.file);

                                                Application.Current.Dispatcher.Invoke(async () => await firebaseClient.Child("/files/%UID%".Replace("%UID%", requestMessageVO.file)).DeleteAsync());
                                            }
                                        }

                                        // Realtime database 데이터 수정
                                        // -------------------------------------------------------------------------------------------------------------------
                                        Application.Current.Dispatcher.Invoke(async () => await firebaseClient.Child("/messages/%REG-NUM%/%DATE%".Replace("%DATE%", requestMessageVO.date).Replace("%REG-NUM%", regNum)).DeleteAsync());
                                        // -------------------------------------------------------------------------------------------------------------------

                                        Application.Current.Dispatcher.Invoke(() => mainWindow.loadingProgressbarPanel.getProgressbar().Value = mainWindow.loadingProgressbarPanel.getProgressbar().Value + 1);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionManager.getInstance().showMessageBox(ex);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionManager.getInstance().showMessageBox(ex);
                    }
                });

                // 파일 제거
                mainWindow.loadingProgressbarPanel.getProgressbar().Value = 0;
                mainWindow.loadingProgressbarPanel.getProgressbar().Maximum = deleteFileUid.Count;
                mainWindow.loadingProgressbarPanel.setTitle("메시지 삭제 중...");
                foreach (string uid in deleteFileUid)
                {
                    try
                    {
                        var auth = new Firebase.Auth.FirebaseAuthProvider(new FirebaseConfig(AdminLoginManager.getInstance().apiKey));
                        var a = await auth.SignInWithEmailAndPasswordAsync(AdminLoginManager.getInstance().managerID, AdminLoginManager.getInstance().managerPW);
                        await new FirebaseStorage(
                            "rhya-network-bbedu-alert-e768b.appspot.com",
                             new FirebaseStorageOptions
                             {
                                 AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                                 ThrowOnCancel = true,
                             })
                            .Child("users/%uid%".Replace("%uid%", uid))
                            .DeleteAsync();

                        mainWindow.loadingProgressbarPanel.getProgressbar().Value = mainWindow.loadingProgressbarPanel.getProgressbar().Value + 1;
                    }
                    catch (Exception ex)
                    {
                        ExceptionManager.getInstance().showMessageBox(ex);
                    }
                }

                messageDateSelectListItem.Clear();
                AdminLoginManager.getInstance().requestMessageVODict.Clear();
                AdminLoginManager.getInstance().userUploadFileVODict.Clear();
                await Task.Run(() => treeViewItemReload());
                messageForOfficeSelectTreeView.Items.Refresh();

                // Progress dialog 숨기기
                mainWindow.loadingProgressbarGridPanel.Visibility = Visibility.Hidden;
                mainWindow.loadingProgressbarPanel.getProgressbar().Value = 0;

                // YesOrNo dialog 숨기기
                mainWindow.dialogYesOrNoGridPanel.Visibility = Visibility.Hidden;
            });
            mainWindow.dialogYesOrNoPanel.setAction2(() =>
            {
                // YesOrNo dialog 숨기기
                mainWindow.dialogYesOrNoGridPanel.Visibility = Visibility.Hidden;
            });

            mainWindow.dialogYesOrNoGridPanel.Visibility = Visibility.Visible;
        }
    }
}
