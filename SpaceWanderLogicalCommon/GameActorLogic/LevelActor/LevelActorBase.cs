using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crazy.Common;
using GameServer.Configure;

namespace GameActorLogic
{
    public class LevelActorBase:
        ILevelActorBaseContainer,
        ILevelActorComponentBaseContainer
    {
        public static int PlayerCamp = 1;
        public static int EnemyCamp = 2;
        public static int WallCamp = 3;
        protected EventComponentBase _eventComponent;
        protected EnvirinfoComponentBase _envirinfoComponent;
        protected CommandComponentBase _commandComponent;
        protected HandlerComponentBase _handlerComponent;
        protected CreateComponentBase _createComponent;
        protected TaskEventComponentBase _taskEventComponent;
        protected ConfigComponentBase _configComponent;
        protected ContactListenerComponentBase contactListenerComponent;



        protected long levelid;
        protected ulong battleid;
        
        protected bool isStart = false;

        /// <summary>
        /// 在Update中每次加一
        /// LevelActor中Update的频率目前受外界调用控制
        /// 从1开始计算
        /// </summary>
        protected long Currentframe;

        protected Dictionary<string, ulong> players;

        public LevelActorBase()
        {
            Log.Trace("LevelActorBase 开始初始化");
            players = new Dictionary<string, ulong>();
            Currentframe = 0;
            //初始化组件
            CreateComponent();
            Log.Trace("LevelActorBase 初始化完成");
        }

        

        #region 创建组件

        protected void CreateComponent()
        {
            _configComponent = new ConfigComponentBase(this);
            _eventComponent = new EventComponentBase();
            _commandComponent = new CommandComponentBase();
            _envirinfoComponent = new EnvirinfoComponentBase();
            _handlerComponent = new HandlerComponentBase(this);
            _createComponent = new CreateComponentBase(this);
            _taskEventComponent = new TaskEventComponentBase(this);
            contactListenerComponent = new ContactListenerComponentBase(_envirinfoComponent);

        }

        #endregion

        #region 测试添加任务方法

        protected void AddPrepareTask()
        {
            var container = this as ILevelActorComponentBaseContainer;
            //准备任务对象
            var task = container.GetCreateInternalComponentBase().CreateTaskEvent(
                TaskConditionTypeConstDefine.KillTaskEvent,TaskResultTypeConstDefine.Victory, 1, new Dictionary<int, int>
                {
                    {0, 10}
                });
            var task1 = container.GetCreateInternalComponentBase().CreateTaskEvent(
                TaskConditionTypeConstDefine.TimeTaskEvent, TaskResultTypeConstDefine.Fail, 2, new Dictionary<int, int>
                {
                    {0,600000 }
                });
            //添加任务
            container.GeTaskEventComponentInternalBase().AddTaskEvent(task);
            container.GeTaskEventComponentInternalBase().AddTaskEvent(task1);

        }

        /// <summary>
        /// 准备敌人
        /// </summary>
        protected void PrepareEnemy()
        {
            var container = this as ILevelActorComponentBaseContainer;
            container.GetEventComponentBase().AddEventMessagesToHandlerForward(
                new InitEventMessage(actorid:_createComponent.GetCreateID(), actortype:ActorTypeBaseDefine.FighterShipActorA,camp: LevelActorBase.EnemyCamp, point_x:10, point_y:10, angle:0));
            container.GetEventComponentBase().AddEventMessagesToHandlerForward(
                new InitEventMessage(actorid: _createComponent.GetCreateID(), actortype: ActorTypeBaseDefine.EliteShipActorB, camp: LevelActorBase.PlayerCamp, point_x: -10, point_y: -10, angle: 0));
            
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

        public long GetCurrentFrame()
        {
            return Currentframe;
        }

        public void SetCurrentFrame(long frame)
        {
            Currentframe = frame;
        }

        public Dictionary<string, ulong> GetPlayerDict()
        {
            return players;
        }

        public void SetPlayerDict(Dictionary<string,ulong> dict)
        {
            players = dict;
        }
        public bool GetLevelState()
        {
            return isStart;
        }
        /// <summary>
        /// 初始化配置文件
        /// </summary>
        /// <param name="gameBarrierConfigs">关卡配置</param>
        /// <param name="gameShipConfigs">飞船配置</param>
        /// <param name="gameSkillConfig">技能配置</param>
        public void InitConfig(GameBarrierConfig gameBarrierConfigs, List<GameShipConfig> gameShipConfigs,
            GameSkillConfig gameSkillConfig)
        {
            _configComponent.InitializeConfig(gameShipConfigs.ToArray(), gameSkillConfig, gameBarrierConfigs);
            

            OnLoadingDone?.Invoke();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="players">玩家集合</param>
        /// <param name="barrierId">关卡类型id</param>
        public void Start(List<Tuple<string, int, int, int, int>> players)
        {
            Log.Trace("Start 开始生成");
            Random r = new Random();
            //TODO 可能进行动态初始化
            foreach (var player in players)
            {
                var id = _createComponent.GetCreateID();
                this.players.Add(player.Item1, id);
                _eventComponent.AddEventMessagesToHandlerForward(new InitEventMessage(id, LevelActorBase.PlayerCamp,
                    player.Item3, r.Next(0,400), r.Next(0, 400), 0, true, player.Item4, player.Item5,player.Item1));
            }

            //开启所有任务
            _taskEventComponent.StartTaskEvents();



            //动态初始化任务配置
            //AddPrepareTask();

            //设置空气墙
            _envirinfoComponent.AddAirWall();


            //动态初始化敌人
            //PrepareEnemy();
            isStart = true;
            Log.Trace("Start 生成完成");

            OnStartDone?.Invoke();
        }

        /// <summary>
        /// 客户端Start函数
        /// </summary>
        public void Start()
        {
            //AddPrepareTask();
            isStart = true;
            _envirinfoComponent.AddAirWall();

            OnStartDone?.Invoke();
        }

        protected Stopwatch stopwatch = new Stopwatch();

        public virtual void Update()
        {
            if (isStart == false) return;
            Currentframe++;

            stopwatch?.Restart();
            //Log.Trace("Update GOGOGOGOGO");


            _handlerComponent.Update();
            stopwatch?.Stop();
            if (stopwatch.ElapsedMilliseconds > 0)
                Log.Trace(Currentframe + "Update _handlerComponent 时间：" + stopwatch.ElapsedMilliseconds);


            stopwatch?.Restart();
            _envirinfoComponent.Tick();
            stopwatch?.Stop();
            if (stopwatch.ElapsedMilliseconds > 0)
                Log.Trace(Currentframe + "Update _envirinfoComponent 时间：" + stopwatch.ElapsedMilliseconds);

            stopwatch?.Restart();
            _taskEventComponent.Update();
            stopwatch?.Stop();

            if (stopwatch.ElapsedMilliseconds > 0)
                Log.Trace(Currentframe + "Update _taskEventComponent 时间：" + stopwatch.ElapsedMilliseconds);
        }

        public void Dispose()
        {
            isStart = false;
            //TODO 对子对象进行动态Dispose
            
           

            _taskEventComponent.Dispose();
            _taskEventComponent = null;

            _createComponent.Dispose();
            _createComponent = null;

            _handlerComponent.Dispose();
            _handlerComponent = null;

            _envirinfoComponent.Dispose();
            _envirinfoComponent = null;

            _commandComponent.Dispose();
            _commandComponent = null;


            _eventComponent.Dispose();
            _eventComponent = null;

            _configComponent.Dispose();
            _configComponent = null;
        }

        public ActorBase GetPlayerActorByString(string id)
        {
            if (players.TryGetValue(id, out battleid))
                return _envirinfoComponent.GetActor(battleid);
            else
            {
                return _envirinfoComponent.GetAllActors().Find(o => o.GetActorName() == id);
            }
        }

        ~LevelActorBase()
        {
            Log.Trace("从内存中释放Level Id = "+ battleid);
        }
        /// <summary>
        /// 在构造器中被调用
        /// </summary>
        public event Action OnLoadingDone;
        /// <summary>
        /// 在Start方法中被调用
        /// </summary>
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

        public ActorBase GetActorByBodyUserData(UserData bodyid)
        {
            return _envirinfoComponent.GetActorByBodyUserData(bodyid);
        }

       

        public void SetDelta(float delta)
        {
            _envirinfoComponent.SetDelta(delta);
        }

        public float GetDelta()
        {
            return _envirinfoComponent.GetDelta();
        }


        public void SetMapSize(float height ,float width)
        {
            _envirinfoComponent.SetMapSize(height, width);
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

        public void StartTaskEvents()
        {
            _taskEventComponent.StartTaskEvents();
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

        public void InitializeConfig(GameShipConfig[] ships, GameSkillConfig skill, GameBarrierConfig barrier)
        {
            _configComponent.InitializeConfig(ships, skill, barrier);
        }

        #endregion

        #endregion

        #region 对外接口

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

       
        #endregion

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

        public event Action OnGameVictory
        {
            add => ((IHandlerComponentBase) _handlerComponent).OnGameVictory += value;
            remove => ((IHandlerComponentBase) _handlerComponent).OnGameVictory -= value;
        }

        public event Action OnGameFail
        {
            add => ((IHandlerComponentBase) _handlerComponent).OnGameFail += value;
            remove => ((IHandlerComponentBase) _handlerComponent).OnGameFail -= value;
        }

        #endregion




    }
}
