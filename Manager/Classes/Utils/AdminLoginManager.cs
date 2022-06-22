using Firebase.Database;
using Firebase.Storage;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Classes.Utils
{
    class AdminLoginManager
    {
        // Instance
        private static AdminLoginManager adminLoginManager = null;

        // Firebase realtime database file name
        private readonly string FILE_NAME = "rhya-network-bbedu-alert-admin-info.json";

        // Firebase realtime database url
        private string FIREBASE_REALTIME_DATABASE_URL = null;
        // Firebase admin password
        private string FIREBASE_PASSWORD = null;

        // Firebase Key Name
        private readonly string FIREBASE_REALTIME_DATABASE_KEY_APP_INFO = "app-info";
        private readonly string FIREBASE_REALTIME_DATABASE_KEY_PROJECT_NAME = "project-name";

        // JSON File key name
        private readonly string JSON_REALTIME_DATABASE_KEY = "realtime-database";
        private readonly string JSON_PASSWORD_KEY = "password";

        // VO List
        public List<UserVO> userVOList = new List<UserVO>();
        public Dictionary<string, DefaultDataVO> defaultDataVODict = new Dictionary<string, DefaultDataVO>();
        public Dictionary<string, List<RequestMessageVO>> requestMessageVODict = new Dictionary<string, List<RequestMessageVO>>();
        public Dictionary<string, UserUploadFileVO> userUploadFileVODict = new Dictionary<string, UserUploadFileVO>();

        // Reload date
        public string reloadedDate = null;

        // Version
        public string version = null;

        // Private key
        public string privateKey = null;

        // Auth info
        public string apiKey = "";
        public string managerPW = "";
        public string managerID = "";
        public string fcmAPIKey = "";




        /// <summary>
        /// Get instance 
        /// </summary>
        /// <returns></returns>
        public static AdminLoginManager getInstance()
        {
            if (adminLoginManager == null)
                adminLoginManager = new AdminLoginManager();

            return adminLoginManager;
        }



        /// <summary>
        /// 관리자 정보 설정 - 사용자 입력
        /// </summary>
        /// <param name="realtime_database">Realtime Databse URL</param>
        /// <param name="password">Databse admin password</param>
        public void setAdminInfoForInput(string realtime_database, string password)
        {
            FIREBASE_REALTIME_DATABASE_URL = realtime_database;
            FIREBASE_PASSWORD = password;
        }



        /// <summary>
        /// 관리자 정보 설정 - JSON 데이터 파일
        /// </summary>
        /// <return>
        /// 로딩 성공 여부 반환
        /// </return>
        public bool setAdminInfoForJson()
        {
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(FILE_NAME);
            if (fileInfo.Exists)
            {
                try
                {
                    string jsonFileValue = System.IO.File.ReadAllText(FILE_NAME);
                    JObject jsonObject = JObject.Parse(jsonFileValue);

                    FIREBASE_REALTIME_DATABASE_URL = (string)jsonObject[JSON_REALTIME_DATABASE_KEY];
                    FIREBASE_PASSWORD = (string)jsonObject[JSON_PASSWORD_KEY];

                    return true;
                }
                catch (Newtonsoft.Json.JsonReaderException ex)
                { 
                    ExceptionManager.getInstance().showMessageBox(ex);
                }
                catch (Exception ex)
                {
                    ExceptionManager.getInstance().showMessageBox(ex);
                }
            }

            return false;
        }



        /// <summary>
        /// 데이터 Null 확인
        /// </summary>
        /// <returns></returns>
        public bool isAdminInfoNotNull()
        {
            return FIREBASE_REALTIME_DATABASE_URL != null && FIREBASE_PASSWORD != null;
        }



        /// <summary>
        /// Realtime database client 
        /// </summary>
        /// <returns>
        /// FirebaseClient
        /// Realtime database 관리자 Client
        /// </returns>
        public FirebaseClient getRealtimeDatabaseClient()
        {
            if (FIREBASE_REALTIME_DATABASE_URL == null || FIREBASE_PASSWORD == null)
                return null;

            return new FirebaseClient(FIREBASE_REALTIME_DATABASE_URL, 
                new FirebaseOptions
                {
                  AuthTokenAsyncFactory = () => Task.FromResult(FIREBASE_PASSWORD)
                }); 
        }



        /// <summary>
        /// 키 유효성 검사
        /// </summary>
        /// <param name="firebaseClient">Firebase client</param>
        /// <param name="verifyFail">실패 Event</param>
        /// <param name="verifySuccess">성공 Event</param>
        public async void verifyRealtimeDatabaseKey(FirebaseClient firebaseClient, Action verifyFail, Action verifySuccess)
        {
            try
            {
                // Firebase realtime database 접근
                var getAppInfo = await firebaseClient.Child(FIREBASE_REALTIME_DATABASE_KEY_APP_INFO).OnceAsync<object>();
                foreach (var obj in getAppInfo)
                {
                    if (obj.Key.Equals(FIREBASE_REALTIME_DATABASE_KEY_PROJECT_NAME))
                    {
                        verifySuccess();

                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.getInstance().showMessageBox(ex);
            }

            verifyFail();
        }



        /// <summary>
        /// Admin 데이터 JSON 파일 저장
        /// </summary>
        public void saveAdminInfoToFile()
        {
            try
            {
                JObject jsonObject = new JObject();
                jsonObject.Add(JSON_REALTIME_DATABASE_KEY, FIREBASE_REALTIME_DATABASE_URL);
                jsonObject.Add(JSON_PASSWORD_KEY, FIREBASE_PASSWORD);

                System.IO.File.WriteAllText(FILE_NAME, jsonObject.ToString());
            }
            catch (Exception ex)
            {
                ExceptionManager.getInstance().showMessageBox(ex);
            }
        }



        /// <summary>
        /// 사용자 데이터 불러오기
        /// </summary>
        /// <param name="firebaseClient">Firebase client</param>
        /// <param name="noTryCatch">Try Catch 처리 여부</param>
        public async Task reloadUserData(FirebaseClient firebaseClient, bool noTryCatch)
        {
            this.userVOList.Clear();

            var getStroageUserInfo = await firebaseClient.Child("users").OnceAsync<object>();

            string privateKey = null;
            var getPrivateKey = await firebaseClient.Child("private-key").OnceAsync<object>();
            foreach (var obj in getPrivateKey)
            {
                privateKey = (string)obj.Object;
            }

            this.privateKey = privateKey;

            foreach (var obj in getStroageUserInfo)
            {
                try
                {
                    UserVO userVO = new UserVO();
                    userVO.uid = obj.Key;
                    JObject jsonObject = (JObject) obj.Object;
                    userVO.email = (string)jsonObject["email"];
                    userVO.date = (string)jsonObject["date"];
                    userVO.key = ((string)jsonObject["key"]).Replace("-" + privateKey, "");
                    userVO.token = (string)jsonObject["token"];

                    userVOList.Add(userVO);
                }
                catch (Exception ex)
                {
                    if (noTryCatch)
                        throw ex;
                    

                    ExceptionManager.getInstance().showMessageBox(ex);
                }
            }
        }



        /// <summary>
        /// 기본 데이터 불러오기
        /// </summary>
        /// <param name="firebaseClient">Firebase client</param>
        /// <param name="noTryCatch">Try Catch 처리 여부</param>
        public async Task reloadDefaultData(FirebaseClient firebaseClient, bool noTryCatch)
        {
            this.defaultDataVODict.Clear();

            var getInfo = await firebaseClient.Child("reg-key").OnceAsync<object>();

            foreach (var obj in getInfo)
            {
                try
                {
                    DefaultDataVO defaultDataVO = new DefaultDataVO();
                    defaultDataVO.num = obj.Key;
                    JObject jsonObject = (JObject)obj.Object;
                    defaultDataVO.founder = (string)jsonObject["founder"];
                    defaultDataVO.name = (string)jsonObject["name"];
                    defaultDataVO.type = (string)jsonObject["type"];

                    defaultDataVODict.Add(defaultDataVO.num, defaultDataVO);
                }
                catch (Exception ex)
                {
                    if (noTryCatch)
                        throw ex;


                    ExceptionManager.getInstance().showMessageBox(ex);
                }
            }
        }



        /// <summary>
        /// 메시지 데이터 불러오기
        /// </summary>
        /// <param name="firebaseClient">Firebase client</param>
        /// <param name="noTryCatch">Try Catch 처리 여부</param>
        public async Task reloadMessageData(FirebaseClient firebaseClient, bool noTryCatch)
        {
            this.requestMessageVODict.Clear();

            var getInfo = await firebaseClient.Child("messages").OnceAsync<object>();

            foreach (var obj in getInfo)
            {
                try
                {
                    JObject jsonObject = (JObject)obj.Object;
                    List<RequestMessageVO> requestMessageVOs = new List<RequestMessageVO>();
                    IList<string> keys = jsonObject.Properties().Select(p => p.Name).ToList();
                    foreach (string key in keys) 
                    {
                        RequestMessageVO requestMessageVO = new RequestMessageVO();
                        requestMessageVO.date = key;
                        requestMessageVO.file = (string)jsonObject[key]["file"];
                        requestMessageVO.account = (string)jsonObject[key]["account"];
                        requestMessageVO.message = (string)jsonObject[key]["message"];
                        requestMessageVO.title = (string)jsonObject[key]["title"];

                        requestMessageVOs.Add(requestMessageVO);
                    }

                    requestMessageVODict.Add(obj.Key, requestMessageVOs);
                }
                catch (Exception ex)
                {
                    if (noTryCatch)
                        throw ex;


                    ExceptionManager.getInstance().showMessageBox(ex);
                }
            }
        }



        /// <summary>
        /// 파일 데이터 불러오기
        /// </summary>
        /// <param name="firebaseClient">Firebase client</param>
        /// <param name="noTryCatch">Try Catch 처리 여부</param>
        public async Task reloadFileData(FirebaseClient firebaseClient, bool noTryCatch)
        {
            this.userUploadFileVODict.Clear();

            var getInfo = await firebaseClient.Child("files").OnceAsync<object>();

            foreach (var obj in getInfo)
            {
                try
                {
                    UserUploadFileVO userUploadFileVO = new UserUploadFileVO();
                    userUploadFileVO.fileUID = obj.Key;
                    JObject jsonObject = (JObject)obj.Object;
                    userUploadFileVO.fileDate = (string)jsonObject["date"];
                    userUploadFileVO.fileName = (string)jsonObject["name"];
                    userUploadFileVO.fileOwner = (string)jsonObject["owner"];
                    userUploadFileVODict.Add(obj.Key, userUploadFileVO);
                }
                catch (Exception ex)
                {
                    if (noTryCatch)
                        throw ex;


                    ExceptionManager.getInstance().showMessageBox(ex);
                }
            }
        }



        /// <summary>
        /// 안 읽은 메시지 개수 구하는 함수
        /// </summary>
        /// <param name="regNum">등록번호</param>
        /// <param name="requestMessageVOs">메시지 리스트</param>
        /// <returns></returns>
        public int dontReadMessageCount(string regNum, List<RequestMessageVO> requestMessageVOs)
        {
            // JSON 파일 읽기
            System.IO.FileInfo fileInfo = new System.IO.FileInfo("rhya-network-bbedu-alert-message.json");
            if (fileInfo.Exists)
            {
                string jsonFileValue = System.IO.File.ReadAllText("rhya-network-bbedu-alert-message.json");
                JObject jsonObject = JObject.Parse(jsonFileValue);

                if (jsonObject.ContainsKey(regNum))
                {
                    JArray array = (JArray)jsonObject[regNum];

                    int index = requestMessageVOs.Count;
                    
                    foreach (RequestMessageVO requestMessageVO in requestMessageVOs)
                    {
                        foreach(string date in array)
                        {
                            if (date.Equals(requestMessageVO.date))
                            {
                                index--;

                                break;
                            }
                        }
                    }

                    return index;
                }
            }

            return requestMessageVOs.Count;
        }
    }
}
