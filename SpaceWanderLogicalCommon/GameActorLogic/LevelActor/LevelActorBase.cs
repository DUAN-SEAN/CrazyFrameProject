using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyEngine.Base;
using GameServer.Configure;

namespace GameActorLogic
{
    public class LevelActorBase:
        ILevelActorBaseContainer,
        ILevelActorComponentBaseContainer
    {
        public static int PlayerCamp = 1;
        public static int EnemyCamp = 2;
        protected EventComponentBase _eventComponent;
        protected EnvirinfoComponentBase _envirinfoComponent;
        protected CommandComponentBase _commandComponent;
        protected HandlerComponentBase _handlerComponent;
        protected CreateComponentBase _createComponent;
        protected TaskEventComponentBase _taskEventComponent;
        protected ConfigComponentBase _configComponent;

        protected long levelid;
        protected ulong battleid;
        
        protected bool isStart = false;

        protected Dictionary<string, ActorBase> players;

        public LevelActorBase()
        {
            CreateComponent();
            OnLoadingDone?.Invoke();
            players = new Dictionary<string, ActorBase>();
        }


        #region 创建组件

        protected void CreateComponent()
        {
            _eventComponent = new EventComponentBase();
            _commandComponent = new CommandComponentBase();
            _envirinfoComponent = new EnvirinfoComponentBase();
            _handlerComponent = new HandlerComponentBase(this);
            _createComponent = new CreateComponentBase(this);
            _taskEventComponent = new TaskEventComponentBase(this);

        }

        #endregion

        #region 测试添加任务方法

        protected void AddPrepareTask()
        {
            var container = this as ILevelActorComponentBaseContainer;
            var task = container.GetCreateInternalComponentBase().CreateTaskEvent(
                TaskConditionTypeConstDefine.KillTaskEvent,TaskResultTypeConstDefine.Victory, 1, new Dictionary<int, int>
                {
                    {0, 10}
                });
            container.GeTaskEventComponentInternalBase().AddTaskEvent(task);
        }
        

        #endregion

        #region ILevelActorBaseContainer

        public long GetLevelID()
        {
            return levelid;
        }

        public ulong GetBattleID()
        {
            return battleid;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="players">玩家集合</param>
        /// <param name="barrierId">关卡类型id</param>
        public void Start(List<Tuple<string, int, int, int, int>> players,int barrierId)
        {
            isStart = true;
            //TODO 可能进行动态初始化
            foreach (var player in players)
            {
                this.players.Add(player.Item1, null);
            }
            //先放这里
            OnStartDone?.Invoke();
        }

        public virtual void Update()
        {
            if(isStart == false) return;

           _handlerComponent.Update();

           _envirinfoComponent.Tick();

        }

        public void Dispose()
        {
            isStart = false;
            //TODO 对子对象进行动态Dispose


        }

        public ActorBase GetPlayerActorByString(string id)
        {
            return players[id];
        }

        public event Action OnLoadingDone;
        public event Action OnStartDone;

        #region 关卡环境

        public List<ActorBase> GetAllActors()
        {
            return _envirinfoComponent.GetAllActors();
        }

        public List<ActorBase> GetPlayerActors()
        {
            return _envirinfoComponent.GetPlayerActors();
        }

        public List<ActorBase> GetShipActors()
        {
            return _envirinfoComponent.GetShipActors();
        }

        public List<ActorBase> GetWeaponActors()
        {
            return _envirinfoComponent.GetWeaponActors();
        }

        public ActorBase GetActor(ulong id)
        {
            return ((IEnvirinfoBase) _envirinfoComponent).GetActor(id);
        }

        #endregion


        #region 关卡事件组件
        public void AddHandleEventMessage(IEventMessage msg)
        {
            _eventComponent.AddHandleEventMessage(msg);
        }

        public void AddEventMessagesToHandlerForward(IEventMessage msg)
        {
            _eventComponent.AddEventMessagesToHandlerForward(msg);
        }

        public List<IEventMessage> GetForWardEventMessages()
        {
            return _eventComponent.GetForWardEventMessages();
        }
        #endregion


        #region 任务事件组件

        public void AddTaskEvent(ITaskEvent task)
        {
            _taskEventComponent.AddTaskEvent(task);
        }

        public List<ITaskEvent> GetAllTaskEvents()
        {
            return _taskEventComponent.GetAllTaskEvents();
        }

        public List<ITaskEvent> GetUnFinishTaskEvents()
        {
            return _taskEventComponent.GetUnFinishTaskEvents();
        }

      
        public void SetTaskConditionAndState(int id, int state, Dictionary<int, int> values)
        {
            _taskEventComponent.SetTaskConditionAndState(id, state, values);
        }

        public ITaskEvent GetTaskEvent(int id)
        {
            return _taskEventComponent.GetTaskEvent(id);
        }

        #endregion

        #region 指令集合组件

        public List<ICommand> GetCommands()
        {
            return _commandComponent.GetCommands();
        }
        public void PostCommand(ICommand command)
        {
            _commandComponent.PostCommand(command);
        }


        #endregion


        #region 配置文件组件

        public void InitializeConfig(GameShipConfig[] ships, GameSkillConfig skill, GameBarrierConfig[] barrier)
        {
            _configComponent.InitializeConfig(ships, skill, barrier);
        }

        #endregion

        #endregion

        ICommandInternalComponentBase ILevelActorComponentBaseContainer.GetCommandComponentBase()
        {
            return _commandComponent;
        }
        IEventInternalComponentBase ILevelActorComponentBaseContainer.GetEventComponentBase()
        {
            return _eventComponent;
        }

        IEnvirinfoInternalBase ILevelActorComponentBaseContainer.GetEnvirinfointernalBase()
        {
            return _envirinfoComponent;
        }

        ICreateInternalComponentBase ILevelActorComponentBaseContainer.GetCreateInternalComponentBase()
        {
            return _createComponent;
        }

        IHandlerComponentInternalBase ILevelActorComponentBaseContainer.GetHandlerComponentInternalBase()
        {
            return _handlerComponent;
        }

        ITaskEventComponentInternalBase ILevelActorComponentBaseContainer.GeTaskEventComponentInternalBase()
        {
            return _taskEventComponent;
        }

        public IConfigComponentInternalBase GetConfigComponentInternalBase()
        {
            return _configComponent;
        }

        #region Handler组件

        public event Action<ulong> OnInitMessageHandler
        {
            add => _handlerComponent.OnInitMessageHandler += value;
            remove => _handlerComponent.OnInitMessageHandler -= value;
        }

        public event Action<ulong> OnDestroyMessageHandler
        {
            add => _handlerComponent.OnDestroyMessageHandler += value;
            remove => _handlerComponent.OnDestroyMessageHandler -= value;
        }

        public event Action<int> OnTaskUpdateMessageHandler
        {
            add => _handlerComponent.OnTaskUpdateMessageHandler += value;
            remove => _handlerComponent.OnTaskUpdateMessageHandler -= value;
        }


        #endregion




    }
}
