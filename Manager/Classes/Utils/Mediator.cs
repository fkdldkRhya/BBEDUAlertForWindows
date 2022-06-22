using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Classes.Utils
{
    /// <summary>
    /// Copyright 골드러쉬 (goldrushing). All rights reserved.
    /// https://blog.naver.com/goldrushing/221663952698
    /// 
    /// Rhya Music Bot - utils/Mediator.cs
    /// </summary>
    public static class Mediator
    {
        //키와 함수명을 갖는 전역 Dictionary(일종의 리스트)
        static IDictionary<string, List<Action<object>>> pl_dict = new Dictionary<string, List<Action<object>>>();

        //등록 또는 덮어쓰기
        static public void Register(string token, Action<object> callback)
        {
            if (!pl_dict.ContainsKey(token))
            {
                var list = new List<Action<object>>();
                list.Add(callback);
                pl_dict.Add(token, list);
            }
            else
            {
                bool found = false;
                foreach (var item in pl_dict[token])
                    if (item.Method.ToString() == callback.Method.ToString())
                        found = true;
                if (!found)
                    pl_dict[token].Add(callback);
            }
        }

        //해제
        static public void Unregister(string token, Action<object> callback)
        {
            if (pl_dict.ContainsKey(token))
                pl_dict[token].Remove(callback);
        }

        //호출하기
        static public void NotifyColleagues(string token, object args)
        {
            if (pl_dict.ContainsKey(token))
                foreach (var callback in pl_dict[token])
                    callback(args);
        }
    }
}
