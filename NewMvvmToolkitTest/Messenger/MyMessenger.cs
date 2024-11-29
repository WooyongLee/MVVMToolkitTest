using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace NewMvvmToolkitTest.Messenger
{
    public class MyMessenger
    {
        private readonly Dictionary<Type, Action<object>> _actions;
        private readonly List<Tuple<Delegate, Action<object>>> _actionTuples;
        private static MyMessenger _instance;
        private static readonly object _lock = new object();

        //Singletone Instance
        public static MyMessenger Instance
        { 
            get 
            { 
                lock (_lock) 
                {
                    if (_instance == null)
                    {
                        return _instance = new MyMessenger(); 
                    }

                    return _instance;
                }
            } 
        }

        //Singletone 외부에서 생성이 안되도록
        private MyMessenger()
        {
            //실제로 Subscribe된 Delegate를 실행하는 곳.
            _actions = new Dictionary<Type, Action<object>>();
            //Subscribe한 Delegate를 Action<object> 형식으로 만들어서 가지고 있음.
            _actionTuples = new List<Tuple<Delegate, Action<object>>>();
        }

        public void Subscribe<T>(Action<T> action)
        {
            Action<object> input = null;
            // input = _actionTuples.Find(x => x.Item1.Equals(action))?.Item2;
            input = _actionTuples.FirstOrDefault(x => x.Item1 == (Delegate)action)?.Item2;
            if (input == null)
            {
                //Action<T>를 Action<object> 형태로 변형해서 List에 추가.
                input = new Action<object>(o => action((T)o));
                _actionTuples.Add(new Tuple<Delegate, Action<object>>(action, input));
            }
            else
            {
                return;
            }
            //Action<T>가 아닌 Action<object>를 Dctionary에 넣어준다.
            if (!_actions.ContainsKey(typeof(T)))
            {
                _actions.Add(typeof(T), input);
            }
            else
            {
                _actions[typeof(T)] += input;
            }
        }

        public void Unsubscribe<T>(Action<T> action)
        {
            //Action<T>로 Action<object>를 찾고 Dictionary에서 제거
            Action<object> input = null;
            input = _actionTuples.Find(x => x.Item1.Equals(action))?.Item2;
            if (input == null)
            {
                return;
            }
            else
            {
                _actions[typeof(T)] -= input;
            }
        }

        public void Publish<T>(T obj)
        {
            if (!_actions.ContainsKey(typeof(T)))
            {
                return;
            }
            _actions[typeof(T)]?.Invoke(obj);
        }
    }

    // Use Example
    // 1) Subscribe & handler ViewModelA
    class ViewModelA
    {
        public ViewModelA()
        {
            // subscribe
            MyMessenger.Instance.Subscribe<MessageEvent>(OnMessageReceived);
        }

        // message handler
        private void OnMessageReceived(MessageEvent @event)
        {
            throw new NotImplementedException();
        }
    }

    // 2) Publish ViewModelB
    class ViewModeB
    {
        public void SendMessage()
        {
            // publish
            MyMessenger.Instance.Publish(new MessageEvent("Str Send Message"));
        }
    }

    // pulishing message to subscribed message handler
}
