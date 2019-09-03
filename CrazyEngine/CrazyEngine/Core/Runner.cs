using System;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;
using Crazy.Common;

namespace CrazyEngine.Core
{
    public class Runner
    {
        public double Fps { get; set; } = 60;

        public double Delta { get; set; } = 1000.0 / 60;

        public double DeltaMin { get; set; } = 1000.0 / 60;

        public double DeltaMax { get; set; } = 4000.0 / 60;

        public double TimeScalePrev { get; set; } = 1;

        public double Correction { get; set; } = 1;

        public bool Enable { get; set; } = true;

        public long Timestamp { get; private set; }

        public long FrameCount { get; set; }

        private double _timePrev;
        private long _frame;

        private Engine _engine;
        private FixedUpdate m_fixedUpdate;

        public Runner(Engine engine)
        {
            m_fixedUpdate= new FixedUpdate(TimeSpan.FromSeconds(1 / 60d), Tick);
            m_fixedUpdate.Start();
            _engine = engine;
        }
        public void Update2()
        {
            m_fixedUpdate.Update();
        }
        private void Tick()
        {
            _engine.Update(FixedStep);
        }

        public void Update(long time)
        {
            var delta = time - _timePrev;
            _timePrev = time;
            delta = delta < DeltaMin ? DeltaMin : delta;
            delta = delta > DeltaMax ? DeltaMax : delta;
            var correction = delta / Delta;
            Delta = delta;
            if (TimeScalePrev > 0.0001)
                correction *= _engine.Timescale / TimeScalePrev;
            if (_engine.Timescale < 0.0001)
                correction = 0;
            TimeScalePrev = _engine.Timescale;
            Correction = correction;
            _frame++;
            FrameCount++;
            if (time - Timestamp > 1e6)
            {
                Fps = 1e7d * _frame / (time - Timestamp);
                Timestamp = time;
                _frame = 0;
            }
            _engine.Update(delta);
        }

        #region 段瑞改，根据服务器逻辑层调用频率，设置积分次数，并记录回落数值
        public void Update()
        {
            var tempTime = DateTime.Now.Ticks;
            var interval = m_preExtraTime + tempTime - m_currentTime;

            int n = (int)((double)interval / ConversionDenominatorSecond / FixedStep);
            Log.Info("积分次数 = " + n);
            for (int i = 0; i < n; i++)
            {

                _engine.Update(FixedStep);
            }

            m_currentTime = DateTime.Now.Ticks;
            m_preExtraTime = m_currentTime - tempTime + (long)(interval / ConversionDenominatorSecond % FixedStep);
            Log.Info("剩余未处理时间为：" + m_preExtraTime / ConversionDenominatorMillisecond + "ms");

        }

        private static double FixedStep = 1D / 60D;
        private static double ConversionDenominatorSecond = 10000000D;
        private static double ConversionDenominatorMillisecond = 10000D;
        private long m_currentTime = DateTime.Now.Ticks;
        private long m_preExtraTime = 0L;


        #endregion


    }

    public class FixedUpdate
    {
        private readonly Stopwatch _gameTimer = new Stopwatch();

        public long Dt;

        public long LastTime;

        public long Accumulator;

        public readonly long FixedTime = TimeSpan.FromSeconds(0.25d).Ticks;

        public readonly Action Tick;

        public long TickCount;

        public FixedUpdate(TimeSpan dt, Action action)
        {
            Dt = dt.Ticks;
            Tick = action;
        }

        public void SetDt(TimeSpan dt)
        {
            Dt = dt.Ticks;
        }

        public void Start()
        {
            _gameTimer.Start();
            LastTime = _gameTimer.ElapsedTicks;
        }

        public void Reset()
        {
            LastTime = 0;
            Accumulator = 0;
            TickCount = 0;
            _gameTimer.Reset();
        }

        public void Update()
        {
            var now = _gameTimer.ElapsedTicks;
            var frameTime = now - LastTime;
            if (frameTime > FixedTime)
            {
                frameTime = FixedTime;
            }

            LastTime = now;
            Accumulator += frameTime;

            while (Accumulator >= Dt)
            {
                Tick.Invoke();
                Accumulator -= Dt;
                ++TickCount;
            }
        }
    }
}
