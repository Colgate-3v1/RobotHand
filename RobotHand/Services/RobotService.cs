using fairino;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RobotHand.Services
{
    //Структуры:

    // JointPos - структура позиции соединения (размерность 6, измерение в градусах)
    // DescTran - структура с координатами в декартовом пространстве
    // Rpy - структура с углами Эйлера
    // DescPose - поза в декартовом пространстве(хранит позицию DescTran и ориентацию Rpy)
    // ExaxisPos - структура, представляющая позиции расширенных осей
    // ForceTorque - структура, представляющая силы и моменты
    // SpiralParam - структура, представляющая параметры спирального движения.
    // ROBOT_STATE_PKG - структура о состоянии робота
    // ROBOT_AUX_STATE - структура, представляющая состояние расширенной оси

    //
    /*Базовая система координат
     * В базовой системе координат WebAPP системный робот отображается 
     * в трехмерной виртуальной области по умолчанию, а фиксированная метка
     * находится в нижнем центре базы робота.
     * Виртуальная трехмерная базовая система координат может быть отображена вручную.
    */
    public class RobotService
    {
        private Robot robot;
        private ROBOT_STATE_PKG robotState;
        private string _pathLog = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Amplituda", "Robot");

        private Timer _timerDI;
        private Timer _timerX;
        private Timer _timerY;
        private Timer _timerZ;

        public static RobotService Instance { get; private set; }

        public RobotService() {
            Instance = this;
            Init();

            _timerDI = new Timer(CheckDI, null, 0, 1000);
            //_timerX = new Timer(CheckPositiveXButtonPhysical, null, 0, 1000);
            //_timerY = new Timer(CheckPositiveYButtonPhysical, null, 0, 1000);
            //_timerZ = new Timer(CheckPositiveZButtonPhysical, null, 0, 1000);
        }

        #region Свойства
        public JointPos CurrentJointPos { get; set; } = new JointPos();
        public DescPose CurrentDescPose { get; set; } = new DescPose();
        #endregion


        // инициализация
        public void Init()
        {
            robot = new Robot();
            robotState = new ROBOT_STATE_PKG();
            robot.RPC("192.168.58.2");
            robot.LoggerInit(FrLogType.BUFFER, FrLogLevel.INFO, _pathLog, 5, 5);
            robot.SetLoggerLevel(FrLogLevel.INFO);
            robot.RobotEnable(1);
            robot.SetSpeed(80);
            //JointPos jointPos = new JointPos();
            //robot.GetActualJointPosDegree(0, ref jointPos);
            //CurrentJointPos = jointPos;
        }

        public void Disconnect()
        {
            robot.CloseRPC();
            robot = null;
        }

        // StartSteps - используется вдоль одной оси или одного сустава по шагам на 1 секунду
        public void StartSteps(byte refType, byte nb, byte dir, float vel, float acc, float max_dis)
        {
            robot.SetSpeed(20);
            robot.StartJOG(refType, nb, dir, vel, acc, max_dis);
            //await Task.Delay(1000);
            //robot.ImmStopJOG();
        }
        
        public void StopSteps()
        {
            robot.ImmStopJOG();
        }

        // для перемещения робота в пространстве суставов
        public void MoveJoint(JointPos joint_pos, DescPose desc_pos, int tool, int user, 
                              float vel, float acc, float ovl, ExaxisPos epos, float blendT,
                              byte offset_flag, DescPose offset_pos)
        {
            // @ joint_pos - целевое положение сустава
            // @ desc_pos - целевое положение работа в декартовом пространстве
            // @ tool - номер координаты инструмента ???
            // @ user - позиция расширенных осей ???
            // @ vel - скорость движения в процентах от максимальной скорости
            // @ acc - определяет ускорение движения в процентах от максимального ускорения
            // @ ovl - коэффициент масштабирования скорости
            // @ epos - позиция расширенных осей
            // @ blendT - время сглаживания
            // @ offset_flag - флаг смещения: Определяет, будет ли применяться смещение и в какой системе координат
            // @ offset_pos - cмещение позиции
            robot.MoveJ(joint_pos, desc_pos, tool, user, vel, acc, ovl, epos, blendT, offset_flag, offset_pos);
        }

        public void MoveLine(JointPos joint_pos, DescPose desc_pos, int tool,
                             int user, float vel, float acc, float ovl, float blendR,
                             ExaxisPos epos, byte search, byte offset_flag,
                             DescPose offset_pos, int overSpeedStrategy = 0, int speedPercent = 10)
        {

        }


        public DescPose GetPosition()
        {
            DescPose pos = new DescPose();
            robot.GetActualTCPPose(1, ref pos);
            return pos;
        }
        
        public JointPos GetJoint()
        {
            JointPos pos = new JointPos(0,0,0,0,0,0);
            robot.GetActualJointPosDegree(1, ref pos);
            //Debug.WriteLine("Curr   " + robot.GetActualJointPosDegree(1, ref pos));
            return pos;
        }

        public bool buttonWorking = false;
        public byte LevelXPos = 0;
        public byte LevelXNeg = 0;

        public void CheckDI(object o)
        {
            robot.GetDI(0, 0, ref LevelXPos);
            robot.GetDI(1, 0, ref LevelXNeg);

            if (buttonWorking)
            {
                if (LevelXPos == 0 & LevelXNeg == 0)
                {
                    Debug.WriteLine("Stop in DI");
                    StopSteps();
                    buttonWorking = false;
                }
            }
            else
            {
                if (LevelXPos == 1 & LevelXNeg == 1)
                {
                    StartSteps(2, 1, 1, 100, 100, 1500);
                    buttonWorking = true;
                } else
                if (LevelXPos == 1)
                {
                    StartSteps(2, 1, 1, 100, 100, 1500);
                    buttonWorking = true;
                }
                else if (LevelXNeg == 1)
                {
                    StartSteps(2, 1, 0, 100, 100, 1500);
                    buttonWorking = true;
                }
            }
        }





        private bool enableCommand = true;
        public void CheckPositiveXButtonPhysical(object o)
        {
            byte level = 0;
            robot.GetDI(0, 0, ref level);
            if (level == 1)
            {
                Debug.WriteLine("Pos x button");
                if (enableCommand)
                {
                    StartSteps(2, 1, 1, 20, 30, 2000);
                    enableCommand = false;
                }
            }
            else
            {
                StopSteps();
                enableCommand = true;
            }
        }

        public void CheckNegativeXButtonPhysical(object o)
        {
            byte level = 0;
            robot.GetDI(0, 0, ref level);
            if (level == 1)
            {
                Debug.WriteLine("Pos x button");
                if (enableCommand)
                {
                    StartSteps(2, 1, 0, 20, 30, 100);
                    enableCommand = false;
                }
            }
            else
            {
                StopSteps();
                enableCommand = true;
            }
        }

        public void CheckPositiveYButtonPhysical(object o)
        {
            byte level = 0;
            robot.GetDI(0, 0, ref level);
            if (level == 1)
            {
                Debug.WriteLine("Pos y button");
                StartSteps(2, 2, 1, 20, 30, 100);
            }
        }

        public void CheckNegativeYButtonPhysical(object o)
        {
            byte level = 0;
            robot.GetDI(0, 0, ref level);
            if (level == 1)
            {
                Debug.WriteLine("Neg y button");
                StartSteps(2, 2, 0, 20, 30, 100);
            }
        }

        public void CheckPositiveZButtonPhysical(object o)
        {
            byte level = 0;
            robot.GetDI(0, 0, ref level);
            if (level == 1)
            {
                Debug.WriteLine("Pos z button");
                StartSteps(2, 3, 1, 20, 30, 100);
            }
        }

        public void CheckNegativeZButtonPhysical(object o)
        {
            byte level = 0;
            robot.GetDI(1, 0, ref level);
            if (level == 1)
            {
                Debug.WriteLine("Neg z button");
                StartSteps(2, 3, 0, 20, 30, 100);
            }
        }
    }
}
