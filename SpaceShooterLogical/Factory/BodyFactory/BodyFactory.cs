using System;


namespace SpaceShip.Factory
{



    public class BodyFactory
    {
        private BodyFactory()
        {


        }


        #region LoadingShipBody 加载船body

        /// <summary>
        /// 船体生成 供船工厂使用
        /// </summary>
        public T LoadShipBodyByType<T>(Label label, Vector2 min, Vector2 max, Vector2 forward) where T : ShipBase, new()
        {


            LogUI.Log("ship load");
            //init in World
            T body_ship = World.Instanse.InitInWorld<T>(min, max);
            //set 参数
            body_ship.Forward = forward;
            body_ship.Label = label;
            body_ship.MaxVelocity = 100;


            return body_ship;
        }


        #endregion

        #region LoadingWeanponBody 加载武器body
        /// <summary>
        /// 小激光武器生成 供武器工厂使用
        /// </summary>
        public T LoadBoltWeaponByType<T>(Body body, Vector2 position = default, Vector2 forward = default) where T : BoltInBody, new()
        {
            T body_weanpon;
            body_weanpon = World.Instanse.InitInWorld<T>(position);
            body_weanpon.Forward = forward;
            body_weanpon.Owner = body;
            return body_weanpon;
        }

        /// <summary>
        /// 地雷武器生成 供武器工厂使用
        /// </summary>
        public T LoadMineWeaponByType<T>(Body body, Vector2 position = default, Vector2 forward = default) where T : MineInBody, new()
        {
            T body_weanpon;
            body_weanpon = World.Instanse.InitInWorld<T>(position, 0.5f);
            body_weanpon.Forward = forward;
            body_weanpon.Owner = body;
            return body_weanpon;
        }

        /// <summary>
        /// 导弹武器生成 供武器工厂使用
        /// </summary>
        public T LoadMissileWeaponByType<T>(Body body, Vector2 position = default, Vector2 forward = default) where T : MissileInBody, new()
        {
            T body_weanpon;
            body_weanpon = World.Instanse.InitInWorld<T>(position);
            body_weanpon.Forward = forward;
            body_weanpon.Owner = body;

            return body_weanpon;
        }
        /// <summary>
        /// 激光武器生成 供武器工厂使用
        /// </summary>
        public T LoadLightWeaponByType<T>(Body body, Vector2 position = default, Vector2 forward = default) where T : LightInBody, new()
        {
            T body_weanpon;
            body_weanpon = World.Instanse.InitInWorld<T>(new Line(position, new Vector2(position + forward.normalized * 20)));
            //LogUI.Log("飞船"+ new Vector2(body.Position + body.Forward.normalized * 10));
            body_weanpon.Forward = forward;
            body_weanpon.Owner = body;
            return body_weanpon;
        }

        #endregion

        #region LoadingEnviromentBody 加载环境资源

        public T LoadEnvironmentBodyByType<T>(Vector2 position, float radius, Vector2 forward) where T : EnviromentInBody, new()
        {

            T body_environ = World.Instanse.InitInWorld<T>(position, radius);
            body_environ.Label = Label.Environment;
            body_environ.Forward = forward;

            return body_environ;
        }

        #endregion

        public static BodyFactory Instance
        {
            get
            {
                if (bodyFactory == null) bodyFactory = new BodyFactory();
                return bodyFactory;
            }
        }

        private static BodyFactory bodyFactory;
    }
}