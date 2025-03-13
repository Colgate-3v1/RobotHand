using fairino;

public class Program
{
    private static Robot robot;
    private static string _pathLog = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Amplituda", "Robot");

    private static void Main(string[] args)
    {
        Init();
    }

    public static void Init()
    {
        robot = new Robot();
        //robot.RPC("192.168.58.2");
        //robot.LoggerInit(FrLogType.BUFFER, FrLogLevel.INFO, _pathLog, 5, 5);
        //robot.SetLoggerLevel(FrLogLevel.INFO);
        //robot.RobotEnable(1);
        //JointPos jointPos = new JointPos();
        //robot.GetActualJointPosDegree(0, ref jointPos);
    }

    public void GetPosition()
    {
        DescPose pos = new DescPose();
        robot.GetActualTCPPose(1, ref pos);
        Console.WriteLine(pos.ToString());
    }

    public void GetJoint()
    {
        JointPos jointPos = new JointPos();
        robot.GetActualJointPosDegree(0, ref jointPos);
    }

    public static async void StartStepsJoint()
    {
        robot.SetSpeed(35);
        robot.StartJOG(0, 1, 0, 15, 20.0f, 30.0f);
        await Task.Delay(1000);
        robot.ImmStopJOG();
    }

    // движение в суставном пространстве
    public static void JointSpaceMotion()
    {
        JointPos j1, j2, j3, j4;
        DescPose desc_pos1, desc_pos2, desc_pos3, desc_pos4, offset_pos;
        ExaxisPos epos;
        j1 = new JointPos(-58.982, -90.717, 127.647, -129.041, -87.989, -0.062);
        desc_pos1 = new DescPose(-437.039, 411.064, 426.189, -177.886, 2.007, 31.155);

        j2 = new JointPos(-58.978, -76.817, 112.494, -127.348, -89.145, -0.063);
        desc_pos2 = new DescPose(-525.55, 562.3, 417.199, -178.325, 0.847, 31.109);

        j3 = new JointPos(-71.746, -87.177, 123.953, -126.25, -89.429, -0.089);
        desc_pos3 = new DescPose(-345.155, 535.733, 421.269, 179.475, 0.571, 18.332);

        ExaxisPos ePos = new ExaxisPos(0, 0, 0, 0);
        DescPose offset = new DescPose();

        int tool = 0;
        int user = 0;
        float vel = 100.0f;
        float acc = 100.0f;
        float ovl = 100.0f;
        float blendT = 0.0f;
        float blendR = 0.0f;
        byte flag = 0;
        byte search = 0;

        robot.SetSpeed(20);
        int err = -1;
        err = robot.MoveJ(j1, desc_pos1, tool, user, vel, acc, ovl, ePos, blendT, flag, offset);

        Thread.Sleep(1000);
        err = robot.MoveL(j2, desc_pos2, tool, user, vel, acc, ovl, blendR, ePos, search, flag, offset);
        Console.WriteLine($"moveL errcode: {err}");

        Thread.Sleep(1000);
        err = robot.MoveL(j1, desc_pos1, tool, user, vel, acc, ovl, blendR, ePos, search, flag, offset);
        Console.WriteLine($"moveL errcode: {err}");

        Thread.Sleep(1000);
        err = robot.MoveC(j2, desc_pos2, tool, user, vel, acc, ePos, flag, offset, j3, desc_pos3, tool, user, vel, acc, ePos, flag, offset, ovl, blendR);
        Console.WriteLine($"circle errcode: {err}");

        Thread.Sleep(1000);
        err = robot.MoveJ(j1, desc_pos1, tool, user, vel, acc, ovl, ePos, blendT, flag, offset);
        Console.WriteLine($"movej errcode: {err}");

        Thread.Sleep(1000);
        err = robot.Circle(j2, desc_pos2, tool, user, vel, acc, ePos, j3, desc_pos3, tool, user, vel, acc, ePos, ovl, flag, offset);
        Console.WriteLine($"circle errcode: {err}");
    }

    // движение через сплайны
    public static async void WorkWithSpline()
    {
        JointPos j1, j2, j3, j4;
        DescPose desc_pos1, desc_pos2, desc_pos3, desc_pos4, offset_pos;
        ExaxisPos epos = new ExaxisPos(0, 0, 0, 0);

        j1 = new JointPos(-58.982, -90.717, 127.647, -129.041, -87.989, -0.062);
        desc_pos1 = new DescPose(-437.039, 411.064, 426.189, -177.886, 2.007, 31.155);

        j2 = new JointPos(-58.978, -76.817, 112.494, -127.348, -89.145, -0.063);
        desc_pos2 = new DescPose(-525.55, 562.3, 417.199, -178.325, 0.847, 31.109);

        offset_pos = new DescPose(0, 0, 0, 0, 0, 0);

        int tool = 0;
        int user = 0;
        float vel = 100.0f;
        float acc = 100.0f;
        float ovl = 100.0f;
        float blendT = -1.0f;
        byte flag = 0;
        robot.SetSpeed(20);
        int err = -1;
        err = robot.MoveJ(j1, desc_pos1, tool, user, vel, acc, ovl, epos, blendT, flag, offset_pos);
        Console.WriteLine($"movej errcode: {err}");

        robot.SplineStart();
        robot.SplinePTP(j1, desc_pos1, tool, user, vel, acc, ovl);
        robot.SplinePTP(j2, desc_pos2, tool, user, vel, acc, ovl);
        robot.SplineEnd();
    }


}