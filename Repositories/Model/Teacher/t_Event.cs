namespace Edutrack.Models;

public class t_Schedular
{
    public int? C_TimeTable_Id { get; set; }

    public int C_Class_Id { get; set; }

    public int C_Section_Id { get; set; }

    public int C_Subject_Id { get; set; }

    public int C_Teacher_Id { get; set; }

    public string C_Task_Title { get; set; }

    public DateTime C_Task_Start_Time { get; set; }

    public DateTime C_Task_End_Time { get; set; }

    public string C_Task_Description { get; set; }

    public bool C_Task_Is_All_Day { get; set; }

    public DateTime? C_CreatedAt { get; set; }

}

public class Vm_Get_Schedular
{

    public string C_Task_Title { get; set; }

    public DateTime C_Task_Start_Time { get; set; }

    public DateTime C_Task_End_Time { get; set; }

    public string C_Task_Description { get; set; }

    public bool C_Task_Is_All_Day { get; set; }
}

public class t_Class
{
    public int C_Class_Id { get; set; }

    public string C_Class_Name { get; set; }

}

public class t_Section
{
    public int C_Section_Id { get; set; }

    public string C_Section_Name { get; set; }

    public int? C_Class_Id { get; set; }

}