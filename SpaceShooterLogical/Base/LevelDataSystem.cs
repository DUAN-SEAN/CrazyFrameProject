using System;
using System.Collections.Generic;
using XMLDataLib;
using System.IO;
using SpaceShip.Base;
using CrazyEngine;
using SpaceShip.Factory;
using SpaceShip.AI;
namespace SpaceShip.System
{


    public class LevelDataSystem : ITickable
    {
        #region SystemMethod
        public static LevelDataSystem Instance
        {
            get
            {
                if (m_leveldataSystem == null) m_leveldataSystem = new LevelDataSystem();
                return m_leveldataSystem;
            }

        }

        private LevelDataSystem()
        {
            levelDatasDict = new Dictionary<int, LevelData>();
            levelnum = 0;
            LoadingLevelData();
        }

        #endregion

        /// <summary>
        /// 从配置文件中读取地图所有信息
        /// 地图物体信息 地图说明
        /// </summary>
        private void LoadingLevelData()
        {
            ///GameConfig.conf
            string path = "GameConfig.config";
#if UNITY
             path = Application.dataPath + "/Resources/Data/StreamingAssets/LevelConfigure/GameConfig.config";
                
#endif
#if UNITY_EDITOR
        path = Application.dataPath + "/StreamingAssets/LevelConfigure/GameConfig.config";
#endif
            try
            {
                GameDatasInfo datasInfo = Xml2Class.Xml2ClassByLocalPath<GameDatasInfo>(path);

                //LogUI.Log(datasInfo.dataInfos);
                foreach (DataInfo data in datasInfo.dataInfos)
                {
                }

                foreach (LevelDatasInfo leveldatas in datasInfo.LevelDatasInfo)
                {
                    LevelData levelData = new LevelData
                    {
                        id = leveldatas.Id,
                        level = leveldatas.level,
                        MemberCount = leveldatas.MemberCount,
                        Name = leveldatas.Name
                    };
                    //LogUI.Log("data 初始化");
                    XmlPoint[] points = leveldatas.SceneDatasInfo.xmlPoints;
                    //LogUI.Log("pointdata 赋值"+points);
                    XmlCircle[] circles = leveldatas.SceneDatasInfo.xmlCircles;
                    //LogUI.Log("circledata 赋值"+circles);
                    XmlRectangle[] rectangles = leveldatas.SceneDatasInfo.xmlRectangles;
                    //LogUI.Log("rectangledata 赋值"+rectangles);
                    if (points != null)
                    {
                        foreach (XmlPoint point in points)
                        {
                            //TODO 特化物体类型
                            switch (point.EntityType)
                            {
                                case 0:
                                    levelData.AddEntity(new PointEntity(
                                        new Vector2(point.position_x, point.position_y)));
                                    break;
                                default:
                                    break;
                            }

                        }
                        //LogUI.Log("Point Done" + points.Length);

                    }

                    if (circles != null)
                    {
                        foreach (XmlCircle circle in circles)
                        {
                            //TODO 特化物体类型
                            switch (circle.EntityType)
                            {
                                case 0:
                                    levelData.AddEntity(new CircleEntity(
                                        new Vector2(circle.position_x, circle.position_y),
                                        circle.Radius));
                                    break;
                                case 1:
                                    levelData.AddEntity(new MeteoriteInBody(
                                        new Vector2(circle.position_x, circle.position_y),
                                        circle.Radius)
                                    {
                                        Forward = new Vector2(circle.Forward_x, circle.Forward_y),
                                        Label = (Label)circle.Label
                                    });
                                    break;
                                default:
                                    break;
                            }

                        }

                        //LogUI.Log("Circle Done" + circles.Length);
                    }

                    if (rectangles != null)
                    {
                        foreach (XmlRectangle rectangle in rectangles)
                        {
                            switch (rectangle.EntityType)
                            {
                                //TODO 特化物体类型
                                case 0:
                                    levelData.AddEntity(new RectangleEntity(
                                        new Vector2(rectangle.Min_x, rectangle.Min_y),
                                        new Vector2(rectangle.Max_x, rectangle.Max_y)));
                                    break;
                                case 1:

                                    PlayerInBody body_ship = new PlayerInBody(
                                        new Vector2(rectangle.Min_x, rectangle.Min_y),
                                        new Vector2(rectangle.Max_x, rectangle.Max_y))
                                    {
                                        Forward = new Vector2(rectangle.Forward_x, rectangle.Forward_y),
                                        Label = (Label)rectangle.Label
                                    };

                                    levelData.AddEntity(body_ship);
                                    break;
                                case 2:
                                    //LogUI.Log("aiship");
                                    AISmallShipInBody body_ship2 = new AISmallShipInBody(
                                        new Vector2(rectangle.Min_x, rectangle.Min_y),
                                        new Vector2(rectangle.Max_x, rectangle.Max_y))
                                    {
                                        Forward = new Vector2(rectangle.Forward_x, rectangle.Forward_y),
                                        Label = (Label)rectangle.Label
                                    };

                                    levelData.AddEntity(body_ship2);
                                    //LogUI.Log("aiship done");
                                    break;
                                case 3:
                                    //LogUI.Log("Boss ship");
                                    AICarrierShipInBody carrierShipInBody = new AICarrierShipInBody(
                                        new Vector2(rectangle.Min_x, rectangle.Min_y),
                                        new Vector2(rectangle.Max_x, rectangle.Max_y))
                                    {
                                        Forward = new Vector2(rectangle.Forward_x, rectangle.Forward_y),
                                        Label = (Label)rectangle.Label
                                    };
                                    levelData.AddEntity(carrierShipInBody);
                                    //LogUI.Log("Boss done");
                                    break;

                                default:
                                    break;
                            }
                        }
                        //LogUI.Log("Rectangle Done" + rectangles.Length);
                    }

                    levelDatasDict.Add(levelData.id, levelData);
                    levelnum += 1;
                }
            }
            catch (Exception e)
            {
                //LogUI.LogError(e);
                //LogUI.Log(path);
                //LogUI.Log(File.OpenText(path).ReadToEnd());

            }
        }

        //TODO 将地图数据加载到物理世界 可弃用
        public void LoadingToWorld()
        {

        }

        public LevelData GetLevelDataByID(int id)
        {
            levelDatasDict.TryGetValue(id, out LevelData levelData);
            return levelData;
        }

        public List<LevelData> GetLevelDatasByLevel(int level)
        {
            List<LevelData> levelDatas = new List<LevelData>();
            foreach (LevelData data in levelDatasDict.Values)
            {
                if (data.level == level) levelDatas.Add(data);
            }
            return levelDatas;
        }

        public void Tick()
        {





        }

        private int levelnum;
        private readonly Dictionary<int, LevelData> levelDatasDict;
        private static LevelDataSystem m_leveldataSystem;
    }
}