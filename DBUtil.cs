using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace TimeReg
{
    class DBUtil
    {
        #region ClassEmployee
        /// <summary>
        /// Class needed for making a List of Employees. 
        /// Identity is based on the netname as Employee Number came in later (and is not known by the PC)
        /// Sortable by first- then last-name
        /// </summary>
        public class Employee: IComparable<Employee>
        {
            public string   NetName { get; set; }
            public string   FullName { get; set; }
            public int      Number { get; set; }
            public int      Department {get; set;}
            public bool     SuperUser { get; set; }

            public int      EmployeeID { get { return m_employeeID; } }


            public Employee(string netname, string fullname, int number, int department, bool super, int employeeID)
            {
                NetName         = netname;
                FullName        = fullname;
                Number          = number;
                Department      = department;
                SuperUser       = super;
                m_employeeID    =  employeeID;
            }

            // Inhbits the default constructor outside this class
            private Employee()
            {
            }

            private int m_employeeID;

            public int CompareTo(Employee other)
            {
                if (other == null) return 1;

                return this.FullName.CompareTo(other.FullName);
            }

            /// <summary>
            /// Two weeks are the "same" if the project is the same (and first day is the same monday - should always be here)
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(object obj)
            {
                if (obj == null || obj.GetType() != GetType())
                    return false;

                Employee otherEmployee = obj as Employee;

                return (this.NetName.Equals(otherEmployee.NetName));
            }

            /// <summary>
            /// Hashcode is sometimes also used for equality tests - for now skip test of date
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return this.NetName.GetHashCode();
            }
        }
        #endregion "ClassEmployee"

        #region ClassCellData
        /// <summary>
        /// A central class - used in the Week-class
        /// </summary>
        public class CellData
        {
            protected int m_mainID;
            readonly int m_employeeID;
            readonly int m_parentID;
            readonly DateTime m_date;
            readonly int m_projectID;
            protected float m_hours;
            protected string m_comment;

            public CellData(DateTime date, int employeeID, int projectID, int parentID)
            {
                m_date = date;
                m_employeeID = employeeID;
                m_projectID = projectID;
                m_parentID = parentID;
                m_mainID = -1;
                m_hours = 0;
                m_comment = "";

                Changed = false;
                InOracle = false;
            }


            public int MainID { get { return m_mainID; } set { m_mainID = value; } }
            public int EmployeeID { get { return m_employeeID; } }
            public DateTime Date { get { return m_date; } }
            public int ProjectID { get { return m_projectID; } }
            public int ParentID { get { return m_parentID; } }
            public float Hours { get { return m_hours; } set { m_hours = value; } }
            public string Comment { get { return m_comment; } set { m_comment = value; } }
            public bool Changed { get; set; }
            public bool InOracle { get; set; }
        }


        /// <summary>
        /// This is the class used as a List in the main-view
        /// Sortable after Parent- then Child Project ID
        /// </summary>
        public class Week : IComparable<Week>
        {
            private CellData[] Days = new CellData[7];
            private string projectName;

            public string ProjectName { get { return projectName; } set { projectName = value; } }

            public Week(string projectName)
            {
                this.projectName = projectName;
            }

            private Week()
            {
            }

            /// <summary>
            /// We want to facilitate a hierarchical listing
            /// parent 1
            ///   child of parent 1
            ///   child of parent 2
            /// parent 2
            ///     chil of parent 2
            /// </summary>
            /// <param name="other"></param>
            /// <returns>1 when we are greates, -1 when other is greatest, 0 if equal</returns>
            public int CompareTo(Week other)
            {
                if (other == null) return 1;

                return DBUtil.CompareTwo(this[0].ProjectID, this[0].ParentID, other[0].ProjectID, other[0].ParentID);
            }

            /// <summary>
            /// Two weeks are the "same" if the project is the same (and first day is the same monday - should always be here)
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(object obj)
            {
                if (obj == null || obj.GetType() != GetType())
                    return false;

                Week otherweek = obj as Week;

                return (otherweek[0].ProjectID == this[0].ProjectID && otherweek[0].Date == this[0].Date);
            }

            /// <summary>
            /// Hashcode is sometimes also used for equality tests - for now skip test of date
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return this[0].ProjectID;
            }
            public CellData this[int index]
            {
                get
                {
                    return Days[index];
                }

                set
                {
                    Days[index] = value;
                }
            }
        }

        #endregion "ClassCellData"

        #region ForecastData

        /// <summary>
        /// An instance of this class represents a specific cell in a forecast Grid
        /// It is part of a line that is defined by Project-Employee-TaskDescr
        /// Actual name etc. of project and employee is looked up in dictionaries
        /// Objects are created with "Changed" = False.
        /// If TaskID >= 0 its originates from the database. If not its a "virgin" entry
        /// </summary>
        public class ForecastCellData
        {
            public int TaskID { get { return m_taskID; } }          /// The database assigned unique ID
            public int EmployeeID { get { return m_employeeID; } }      /// There can be only one instance for given ProjectID-EmployeeID-TaskDesc-CalMonth
            public int ProjectID { get { return m_projectID; } }
            public int Calmonth { get { return m_calmonth; } }
            public float Effort { get { return m_effort; } set { m_changed = true; m_effort = value; } }
            public string TaskDescr { get { return m_taskDescr; } }
            public bool Changed { get { return m_changed; } }           /// Registers whether user has updated
                                                        /// 
            private int m_taskID;
            private int m_employeeID;
            private int m_projectID;
            private int m_calmonth;
            private float m_effort;
            private string m_taskDescr;
            private bool m_changed;

            // This constructor is used when we are retrieving data from the database.
            // The taskID is the Primary Key and will be used for an UPDATE
            public ForecastCellData(int taskID, int projectID, int employeeID, string taskDescr, int calMonth, float effort)
            {
                m_taskID = taskID;
                m_employeeID = employeeID;
                m_projectID = projectID;
                m_calmonth = calMonth;
                m_effort = effort;
                m_taskDescr = taskDescr;
                m_changed = false;
            }

            // This constructor is used when we are filling in new data from the UI
            // As the taskID is -1 we know that we need to INSERT in the DB later
            public ForecastCellData()
            {
                m_taskID = -1;
                m_changed = false;
            }

            // Called when at least one of the common line-fields are updated
            public void updateLineInfo(int projectID, int employeeID, string taskDescr)
            {
                m_projectID = projectID;
                m_employeeID = employeeID;
                m_taskDescr = taskDescr;
                m_changed = true;
            }

            /// <summary>
            /// if this is an existing entry with "Changed" set - then update
            /// if it is a new entry then insert
            /// </summary>
            public void writeToDB()
            {
                string sql;

                // It is important to UPDATE/DELETE existing entries BEFORE inserting new ones as we might otherwise run into a conflict between new data and old-to-be-deleted
                // If TaskID is not -1 it is existing, and if it is changed...
                if (m_taskID != -1 && Changed)
                {
                    if (m_effort == 0.0f)
                        sql = "DELETE from Forecasts WHERE TaskID = @TaskID";
                    else
                        sql = "update Forecasts set ProjectID = @ProjectID, EmployeeID = @EmployeeID , TaskDescr = @TaskDescr , Effort = @Effort " +
                                    "where TaskID = @TaskID";

                    using (conn = new SqlConnection(LocalConnectionStr))
                    {
                        try
                        {
                            conn.Open();
                            if (conn.State == ConnectionState.Closed)
                                return;
                        }
                        catch (SqlException)
                        {
                            System.Windows.Forms.MessageBox.Show("Exception opening SQL-server using:\n\r" + LocalPublicConnectionStr);
                            return;
                        }

                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@TaskID", m_taskID);
                            cmd.Parameters.AddWithValue("@ProjectID", m_projectID);
                            cmd.Parameters.AddWithValue("@EmployeeID", m_employeeID);
                            cmd.Parameters.AddWithValue("@TaskDescr", m_taskDescr);
                            cmd.Parameters.AddWithValue("@Effort", m_effort);

                            try
                            {
                                cmd.ExecuteNonQuery();
                            }
                            catch (SqlException e)
                            {
                                System.Windows.Forms.MessageBox.Show(e.ToString() + "\n\rStacktrace:\n\r" + e.StackTrace, "Could not UPDATE/DELETE data in Forecasts");
                            }
                        }
                    }
                }
                // Is it new - and is the effort worth storing?
                else if (m_taskID == -1 && m_effort > 0.0f)
                {
                    sql = "insert into Forecasts (ProjectID, EmployeeID, TaskDescr, CalMonth, Effort) values (@ProjectID, @EmployeeID, @TaskDescr, @CalMonth, @Effort)";

                    using (conn = new SqlConnection(LocalConnectionStr))
                    {
                        try
                        {
                            conn.Open();
                            if (conn.State == ConnectionState.Closed)
                                return;
                        }
                        catch (SqlException)
                        {
                            System.Windows.Forms.MessageBox.Show("Exception opening SQL-server using:\n\r" + LocalPublicConnectionStr);
                            return;
                        }

                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@ProjectID", m_projectID);
                            cmd.Parameters.AddWithValue("@EmployeeID", m_employeeID);
                            cmd.Parameters.AddWithValue("@TaskDescr", m_taskDescr);
                            cmd.Parameters.AddWithValue("@CalMonth", m_calmonth);
                            cmd.Parameters.AddWithValue("@Effort", m_effort);

                            try
                            {
                                cmd.ExecuteNonQuery();
                            }
                            catch (SqlException e)
                            {
                                System.Windows.Forms.MessageBox.Show(e.ToString() + "\n\rStacktrace:\n\r" + e.StackTrace, "Could not INSERT data into Forecasts");
                            }
                        }
                    }
                }
                // All existing,unchanged tasks as well as new tasks with effort = 0 are skipped
            }
        }

        public class ForecastLine
        {
            public int      EmployeeID {get; set;}      /// A Line is unique for ProjectID-EmployeeID-TaskDescr
            public int      ProjectID  {get; set;}
            public string   TaskDescr  {get; set;}
            public int      FirstCalMonth 
            {  /// Format <Year><Month> - 2017 february is 201702
                get {return m_baseYear*100+m_baseMonth;}
            }    
          
            public int      FirstYear  {get {return m_baseYear;}}
            public int      FirstMonth {get {return m_baseMonth;}}
            public int      Columns    {get {return m_columns;}}

            public          ForecastCellData [] Months = null;

            private int     m_baseYear;
            private int     m_baseMonth;
            private int     m_columns;

            /// <summary>
            /// STATIC method servicing the GUI by calculating no of columns based on CalcMonths
            /// </summary>
            /// <param name="firstCalMonth"></param>
            /// <param name="lastCalMonth"></param>
            /// <returns></returns>
            public static int noOfColumns(int firstCalMonth, int lastCalMonth)
            {
                int fMonth = firstCalMonth % 100;
                int fYear  = firstCalMonth / 100;
                int lMonth = lastCalMonth % 100;
                int lYear  = lastCalMonth / 100;

                return (lYear - fYear) * 12 + lMonth - fMonth+1;
            }

            /// <summary>
            /// STATIC method servicing the GUI by doting out month-names
            /// </summary>
            /// <param name="firstCalMonth"></param>
            /// <param name="index"></param>
            /// <returns></returns>
            public static string indexToText(int firstCalMonth, int index)
            {
                int year = index / 12 + firstCalMonth/100;
                int month = index % 12 + firstCalMonth%100;
                if (month > 12)
                {
                    year++;
                    month -= 12;
                }
                return year.ToString() + "-" + month.ToString();
            }


            /// <summary>
            /// Construct the object with the right number of columns
            /// Each cell constructed with minimal info - mainly the "Changed" property is False
            /// </summary>
            /// <param name="firstCalMonth"></param>
            /// <param name="lastCalMonth"></param>
            public ForecastLine(int firstCalMonth, int lastCalMonth, int projectID, int employeeID, string taskDescr)
            {
                m_baseYear    = firstCalMonth / 100;
                m_baseMonth   = firstCalMonth % 100;

                m_columns     = calMonthToIndex(lastCalMonth)+1;
                Months      = new ForecastCellData[m_columns];

                ProjectID   = projectID;
                EmployeeID  = employeeID;
                TaskDescr   = taskDescr;
            }

            /// <summary>
            /// Return the latest date present in a forecast.
            /// </summary>
            /// <returns></returns>
            public DateTime getHighestDate()
            {
                int year = m_columns / 12 + m_baseYear;
                int month = m_columns % 12 + m_baseMonth;
                while (month > 12)
                {
                    year++;
                    month -= 12;
                }

                // The above brings us one month to far. Select day one and back a day
                DateTime latest = new DateTime(year, month, 1);
                latest = latest.AddDays(-1);

                return latest;
            }

            /// <summary>
            /// Update the database with the changed contents of this line
            /// If the ProjectID is -1 dont do anything as this is a read-only SUM
            /// </summary>
            public void writeToDB()
            {
                if (ProjectID >= 0)
                {
                    foreach (ForecastCellData cell in Months)
                    {
                        // Skip all the empty cells
                        if (cell != null)
                            cell.writeToDB();
                    }
                }
            }

            /// <summary>
            /// Take e.g. 201806 and convert to a month-index taking FirstCalMonth into account
            /// </summary>
            /// <param name="?"></param>
            /// <returns></returns>
            public int calMonthToIndex(int calMonth)
            {
                int inMonth = calMonth % 100;
                int inYear = calMonth / 100;

                return (inYear-m_baseYear)*12+inMonth-m_baseMonth;
            }

            /// <summary>
            /// Typically used in header in UI
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public string indexToText(int index)
            {
                int year = index / 12 + m_baseYear;
                int month = index % 12 + m_baseMonth;
                while (month > 12)
                {
                    year ++;
                    month -= 12;
                }
                return year.ToString()+"-"+month.ToString();
            }

            /// <summary>
            /// The GUI knows the index and needs to calculate calMonth
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public int indexToCalmonth(int index)
            {
                int year = index / 12 + m_baseYear;
                int month = index % 12 + m_baseMonth;
                if (month > 12)
                {
                    year++;
                    month -= 12;
                }
                return year * 100 + month;
            }

            /// <summary>
            /// When the User creates new data in the GUI, we need the same in our model
            /// </summary>
            /// <param name="index"></param>
            /// <param name="effort"></param>
            public void createNewCell(int index, float effort)
            {
                this.Months[index] = new DBUtil.ForecastCellData(-1, ProjectID, EmployeeID, TaskDescr, indexToCalmonth(index), effort);
            }

            /// <summary>
            /// Copy the fixed line info to all real cells in the line.
            /// Called after changes to the "fixed" info
            /// </summary>
            public void updateCellsWithLineInfo()
            {
                foreach (ForecastCellData cell in this.Months)
                {
                    if (cell != null)
                    {
                        cell.updateLineInfo(ProjectID, EmployeeID, TaskDescr);
                    }
                }
            }

            /// <summary>
            /// Clear the line
            /// </summary>
            public void delete()
            {
                foreach (ForecastCellData cell in this.Months)
                {
                    if (cell != null)
                    {
                        cell.Effort = 0.0f;
                        cell.writeToDB();
                    }
                }
            }
        }

      

        /// <summary>
        /// Retrieves a grid of forecasts from a given month to a given month
        /// The query will be based on a list of projects - if null then all
        /// Also it is based on a list of employees - if null then all
        /// Returns a list of ForecastLines sorted by Project then Employee or vice-versa
        /// We could easily JOIN with Projects to get Oracle number, name etc but instead of carrying
        /// this with all objects - much easier to use the existing dictionary.
        /// </summary>
        /// <param name="firstCalMonth"></param>
        /// <param name="lastCalMonth"></param>
        /// <param name="projectIDs"></param>
        /// <param name="employeeIDs"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public static List<ForecastLine> createForecast(int firstCalMonth, int lastCalMonth, string projectIDs, bool sortByProject)
        {
            string sql = "SELECT F.TaskID, F.ProjectID, F.EmployeeID, F.TaskDescr, F.CalMonth, F.Effort, P.Closed, E.FullName " +
                         "FROM Forecasts as F " +
                         "LEFT JOIN Projects as P " +
                         "ON F.ProjectID = P.ProjectID " +
                         "LEFT JOIN Employees as E " +
                         "ON F.EmployeeID = E.EmployeeID " +
                         "WHERE P.Closed = 'False' " +
                         "AND CalMonth >= @Firstmonth AND CalMonth <= @Lastmonth ";
                if (!projectIDs.Equals("-1"))
                {
                    sql += "AND F.ProjectID IN (" + projectIDs + ") ";
                }

                sql += (sortByProject)  ?   
                    "ORDER BY P.Name, E.FullName, F.TaskDescr, F.CalMonth" :
                    "ORDER BY E.FullName, P.Name, F.TaskDescr, F.CalMonth";

            List<ForecastLine> forecasts = new List<ForecastLine>();

            using (conn = new SqlConnection(LocalConnectionStr))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Closed)
                        return forecasts;
                }
                catch (SqlException)
                {
                    System.Windows.Forms.MessageBox.Show("Exception opening SQL-server using:\n\r" + LocalPublicConnectionStr);
                    return forecasts;
                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Firstmonth", firstCalMonth);
                    cmd.Parameters.AddWithValue("@Lastmonth", lastCalMonth);

                    ForecastLine line = null;

                    int oldProjectID = -1;
                    int oldEmployeeID = -1;
                    string oldTaskDescr = "";

                    try
                    {
                        SqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            int projectID    = rdr.GetInt32(rdr.GetOrdinal("ProjectID"));
                            int employeeID   = rdr.GetInt32(rdr.GetOrdinal("EmployeeID"));
                            string taskDescr = rdr.GetString(rdr.GetOrdinal("TaskDescr"));


                            if (oldEmployeeID != employeeID || oldProjectID != projectID || !oldTaskDescr.Equals(taskDescr))
                            {
                                // This is a new line - create it 
                                line = new ForecastLine(firstCalMonth, lastCalMonth, projectID, employeeID, taskDescr);
                                forecasts.Add(line);

                                oldEmployeeID = employeeID;
                                oldProjectID = projectID;
                                oldTaskDescr = taskDescr;
                            }

                            int index  = line.calMonthToIndex(rdr.GetInt32(rdr.GetOrdinal("CalMonth")));

                            ForecastCellData cell = new ForecastCellData(
                                rdr.GetInt32(rdr.GetOrdinal("TaskID")),
                                rdr.GetInt32(rdr.GetOrdinal("ProjectID")),
                                rdr.GetInt32(rdr.GetOrdinal("EmployeeID")),
                                rdr.GetString(rdr.GetOrdinal("TaskDescr")),
                                rdr.GetInt32(rdr.GetOrdinal("CalMonth")),
                                rdr.GetFloat(rdr.GetOrdinal("Effort")));

                            line.Months[index] = cell;
                        }
                        rdr.Close();
                    }
                    catch (Exception e)
                    {
                        System.Windows.Forms.MessageBox.Show(e.ToString() + "\n\rStacktrace:\n\r" + e.StackTrace, "Could not read from Forecasts");
                    }

                    return forecasts;
                }
            }
        }

        public static ForecastLine createTotalsForEmployee(int firstCalMonth, int lastCalMonth, int employeeID)
        {
            string sql = "SELECT F.CalMonth, SUM(F.Effort) AS TotalDays " +
                         "FROM Forecasts as F " +
                         "LEFT JOIN Projects as P " +
                         "ON F.ProjectID = P.ProjectID " +
                         "WHERE P.Closed = 'False' " +
                         "AND F.CalMonth >= @Firstmonth AND F.CalMonth <= @Lastmonth " +
                         "AND F.EmployeeID = @EmployeeID " +
                         "GROUP BY F.CalMonth "+
                         "ORDER BY F.CalMonth";

            ForecastLine forecast = new ForecastLine(firstCalMonth, lastCalMonth, -1, employeeID, "Total");

            using (conn = new SqlConnection(LocalConnectionStr))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Closed)
                        return forecast;
                }
                catch (SqlException)
                {
                    System.Windows.Forms.MessageBox.Show("Exception opening SQL-server using:\n\r" + LocalPublicConnectionStr);
                    return forecast;
                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Firstmonth", firstCalMonth);
                    cmd.Parameters.AddWithValue("@Lastmonth", lastCalMonth);
                    cmd.Parameters.AddWithValue("@EmployeeID", employeeID);

                    try
                    {
                        SqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            int index = forecast.calMonthToIndex(rdr.GetInt32(rdr.GetOrdinal("CalMonth")));

                            ForecastCellData cell = new ForecastCellData(
                                -1,                                             // "TaskID"
                                -1,                                             // "ProjectID
                                employeeID,
                                "Total",                                        // "TaskDescr"
                                rdr.GetInt32(rdr.GetOrdinal("CalMonth")),
                                (float) rdr.GetDouble(rdr.GetOrdinal("TotalDays")));

                            forecast.Months[index] = cell;
                        }
                        rdr.Close();
                    }
                    catch (Exception e)
                    {
                        System.Windows.Forms.MessageBox.Show(e.ToString() + "\n\rStacktrace:\n\r" + e.StackTrace, "Could not read from Forecast Sum");
                    }

                    return forecast;
                }
            }
        }

        /// <summary>
        /// This function receives a date as parameter.
        /// It is responsible for adding all dates up to this into the Dates table
        /// </summary>
        /// <param name="date"></param>
        public static void updateDates(DateTime date)
        {
            string sql = "insert into Dates (aDate) values (@Date)";

            string sqlMax = "SELECT MAX(aDate) FROM Dates";

            using (conn = new SqlConnection(LocalConnectionStr))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Closed)
                        return;
                }
                catch (SqlException)
                {
                    System.Windows.Forms.MessageBox.Show("Exception opening SQL-server using:\n\r" + LocalPublicConnectionStr);
                    return;
                }

                // Set a high default - if not changed then no action
                DateTime curMaxDate = new DateTime(3000, 1, 1);

                // Find currently latest date
                using (SqlCommand cmd = new SqlCommand(sqlMax, conn))
                {
                    try
                    {
                        //Run the command by using SqlDataReader.
                        SqlDataReader rdr = cmd.ExecuteReader();

                        if (rdr.Read())
                        {
                            curMaxDate = rdr.GetDateTime(0);
                        }

                        rdr.Close();
                    }
                    catch (Exception e)
                    {
                        System.Windows.Forms.MessageBox.Show(e.ToString() + "\n\rStacktrace:\n\r" + e.StackTrace, "Could not find Max Date");
                    }
                }


                while (curMaxDate < date)
                {
                    curMaxDate = curMaxDate.AddDays(1);

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Date", curMaxDate);

                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (SqlException e)
                        {
                            System.Windows.Forms.MessageBox.Show(e.ToString() + "\n\rStacktrace:\n\r" + e.StackTrace, "Could not INSERT date into Dates");
                        }
                    }
                }
            }
        }



        #endregion "ForecastData"

        #region ClassProjTupple
        /// <summary>
        /// Another class used in Lists to carry around information. Also sortable after Parent & Child projects
        /// </summary>
        public class ProjTupple: IComparable<ProjTupple>
        {
            private int m_projID;
            private int m_parentID;
            private string m_projName;
            private string m_projNo;
            private bool m_closed;
            private string m_parentName;
            private int m_psoNumber;
            private string m_category;
            private string m_manager;
            private string m_grouptag;

            public int ProjectID { get { return m_projID; } }
            public int ParentID { get { return m_parentID; } }
            public string Name { get { return m_projName; } }
            public string Manager { get { return m_manager; } }
            public bool Closed { get { return m_closed; } }
            public string ParentName { get { return m_parentName; } }
            public string ProjectNo { get { return m_projNo; } set { m_projNo = value; } }
            public int PSONumber { get { return m_psoNumber; } }
            public string Category { get { return m_category; } set { m_category = value; } }
            public string Grouptag { get { return m_grouptag; } set { m_grouptag = value; } }




            // Do not let anyone instantiate a blank tupple.
            private ProjTupple()
            {
            }

            public ProjTupple(int projectID, string projectName, int parentID, string projectNo, bool closed, string parentName, int psoNumber, string category, string manager, string grouptag)
            {
                m_projID = projectID;
                m_projName = projectName;
                m_parentID = parentID;
                m_projNo = projectNo;
                m_closed = closed;
                m_parentName = parentName;
                m_psoNumber = psoNumber;
                m_category = category;
                m_manager = manager;
                m_grouptag = grouptag;
            }

            public int CompareTo(ProjTupple other)
            {
                if (other == null) return 1;

                return CompareTwo(this.ProjectID, this.ParentID, other.ProjectID, other.ParentID);
            }

            public override string ToString()
            {
                string val = m_projName;
                if (m_projNo.Length > 0) val += " | No: " + m_projNo;
                if (m_psoNumber != 0) val += " | PSO: " + m_psoNumber;
                if (m_parentName.Length > 0) val += " | Parent: " + m_parentName;
                
                return val;
            }

            /// <summary>
            /// To scan a collection for a given Project, we need an Equals implementation
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(object obj)
            {
                if (obj == null ||obj.GetType() != GetType())
                    return false;

                return (obj as ProjTupple).m_projID == this.m_projID;
            }

            /// <summary>
            /// Hashcode is sometimes also used for equality tests
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return m_projID;
            }
        }
        #endregion "ClassProjTupple"


        public class AggrReport
        {
            public int   Project {get; set;}
            public int   Year {get; set;}
            public int   Month {get; set;}
            public float Hours {get; set;}

            public AggrReport(int project, int year, int month, float hours)
            {
                Project = project;
                Year    = year;
                Month   = month;
                Hours   = hours;
            }
        }

        public class ExportItem
        {
            public DateTime Date { get; set; }              // Needed by Oracle
            public string   ProjectName { get; set; }       // Needed by GUI
            public string   ProjectNo {get;set;}            // Needed by Oracle
            public string   EmployeeName { get; set; }      // Needed by GUI
            public float    Hours { get; set; }             // Needed by Oracle
            public string   Comment {get; set;}             // Legal  in Oracle 
            public int      EmployeeNo { get; set; }        // Needed by Oracle
            public int      MainID { get; set;}             // reference for Oracle back tos ource

            public ExportItem(DateTime date, string projectName, string projectNo, string employeeName, float hours, string comment,  int employeeNo, int mainID)
            {
                Date = date;
                ProjectName = projectName;
                ProjectNo = projectNo;
                EmployeeName = employeeName;
                Hours = hours;
                Comment = comment;
                EmployeeNo = employeeNo;
                MainID = mainID;
            }
        }

        #region ClassProjectReport


        public class AggrProjectReport
        {
            public string   Employee { get; set; }
            public int      ProjectID { get; set; }
            public float    Hours { get; set; }
            public int      Department { get; set; }
            public int      Year {get; set;}
            public int      Month {get; set;}

            public AggrProjectReport(string employee, int department, int projectID, int year, int month, float hours)
            {
                Employee = employee;
                Department = department;
                ProjectID = projectID;
                Year = year;
                Month = month;
                Hours = hours;
            }
        }

        /// <summary>
        /// Read-Only information for Reports
        /// Now includes Project- and Parent-ID so that it is sortable by these
        /// </summary>
        public class ProjectReport: IComparable<ProjectReport>
        {
            public string Employee {get; set;}
            public string Parent { get; set; }
            public int    ParentID { get; set; }
            public string Project {get; set;}
            public int    ProjectID { get; set; }
            public string ProjectNo { get; set; }
            public int    PSONumber { get; set; }
            public string Category { get; set; }
            public string Grouptag { get; set; }
            public string Manager { get; set; }
            public int    Department { get; set; }
            public float  Hours { get; set; }

            public ProjectReport(string employee, string project, string projectno, string parent, float hours, int projectID, int parentID, int dept, int psoNumber, string category, string manager, string grouptag)
            {
                Employee = employee;
                Project = project;
                ProjectNo = projectno;
                Parent = parent;
                Hours = hours;
                ProjectID = projectID;
                ParentID = parentID;
                Department = dept;
                PSONumber = psoNumber;
                Category = category;
                Manager = manager;
                Grouptag = grouptag;
            }
            public int CompareTo(ProjectReport other)
            {
                if (other == null) return 1;

                return CompareTwo(this.ProjectID, this.ParentID, other.ProjectID, other.ParentID);
            }


            /// <summary>
            /// To scan a collection for a given Project, we need an Equals implementation
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(object obj)
            {
                if (obj == null || obj.GetType() != GetType())
                    return false;

                return (obj as ProjectReport).ProjectID == this.ProjectID;
            }

            /// <summary>
            /// Hashcode is sometimes also used for equality tests
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return ProjectID;
            }
        }
        #endregion "ClassProjectReport"

        #region ClassPersonalReport
        /// <summary>
        /// The class used when reporting all entries for a given Employee
        /// </summary>
        public class PersonalReport
        {
            public DateTime Date { get; set; }
            public string Project { get; set; }
            public string ProjectNo { get; set; }
            public float Hours { get; set; }
            public string Comment {get; set;}

            public PersonalReport(DateTime date, string project, string projectno, float hours, string comment)
            {
                Date = date;
                Project = project;
                ProjectNo = projectno;
                Hours = hours;
                Comment = comment;
            }
        }
        #endregion "ClassPersonalReport"

        #region PersonWorkdays

        public class PersonWorkdays
        {
            public string Employee { get; set; }
            public int Days {get;set; }
            public int MonthNo { get; set; }
            public int YearNo { get; set; }

            public PersonWorkdays(string employee, int days, int yearno, int monthno)
            {
                Employee = employee;
                Days = days;
                YearNo = yearno;
                MonthNo = monthno;
            }
        }
        #endregion "PersonWorkdays

        #region Helpers
        /// <summary>
        /// All strings in the database - except "netname" and "comment" - are length 50. Practical!
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public static string truncStr(string inStr)
        {
            return (inStr.Length <= 50) ? inStr : inStr.Substring(0, 50);
        }


        public static string truncComment(string inStr)
        {
            return (inStr.Length <= 200) ? inStr : inStr.Substring(0, 200);
        }

        /// <summary>
        /// This is where the actual sorting of projects after parents takes place
        /// </summary>
        /// <param name="thisProjectID"></param>
        /// <param name="thisParentID"></param>
        /// <param name="otherProjectID"></param>
        /// <param name="otherParentID"></param>
        /// <returns></returns>
        private static int CompareTwo(int thisProjectID, int thisParentID, int otherProjectID, int otherParentID)
        {
            if (thisParentID == 0 && otherParentID == 0)    // We are both parents - sort by our own ProjectID's
                if (thisProjectID == otherProjectID)        // We are the same parent
                    return 0;
                else
                    return projectsDict[thisProjectID].Name.CompareTo(projectsDict[otherProjectID].Name);       //  WAS (thisProjectID > otherProjectID) ? 1 : -1;

            if (thisParentID == 0)                          // I am a parent - and the other must be a child. Sort by me and his father
                if (otherParentID == thisProjectID)         // I am his parent - so he is "bigger"
                    return -1;
                else
                    return projectsDict[thisProjectID].Name.CompareTo(projectsDict[otherParentID].Name);                      // WAS (thisProjectID > otherParentID) ? 1 : -1;

            if (otherParentID == 0)                         // Other is a parent - and I must be a child. Sort by him and my father
                if (thisParentID == otherProjectID)         // He is my father - I am "bigger"
                    return 1;
                else
                    return  projectsDict[thisParentID].Name.CompareTo(projectsDict[otherProjectID].Name);        // WAS (thisParentID > otherProjectID) ? 1 : -1;


            if (thisParentID == otherParentID)              // Obviously we are both kids - sort by our parents - then us
                if (thisProjectID == otherProjectID)
                    return 0;                               // We are the same kid
                else
                    return  projectsDict[thisProjectID].Name.CompareTo(projectsDict[otherProjectID].Name);// WAS (thisProjectID > otherProjectID) ? 1 : -1;


            return projectsDict[thisParentID].Name.CompareTo(projectsDict[otherParentID].Name);    // WAS (thisParentID > otherParentID) ? 1 : -1;
        }
        #endregion "Helpers"

        // The following connection strings allow you to use windows own security
        // This is in many ways good - but it does allow the user to access the database directly - circumventing this tool
        // private static string DKLocalConnectionStr = "Data Source=<Server Here>;Initial Catalog=TimeReg;Integrated Security=True";
        // private static string UKLocalConnectionStr = "Data Source=DKDB08_prod;Initial Catalog=TimeReg2;Integrated Security=True";

        // The following connection strings use SQL-Server logins. Thus access is through the tool. Consider encryption...
//      private static string DKLocalConnectionStr = "Data Source=localhost\\sqlexpress;Initial Catalog=TimeReg;Persist Security Info = False;User ID=TimeRegUser;Password=TimeIsOnMySide";


        private static string DKLocalConnectionStr = "Data Source=localhost\\sqlexpress;Initial Catalog=TimeReg2;Integrated Security=True";

        //      private static string DKLocalConnectionStr = "Data Source=<Server Here>;Initial Catalog=TimeReg;Persist Security Info = False;User ID=TimeRegUser;Password=<Password Here>";
        private static string DELocalConnectionStr = "Data Source=<Server Here>;Initial Catalog=TimeReg3;Persist Security Info = False;User ID=TimeRegUser;Password=<Password Here>";
        private static string GCCLocalConnectionStr = "Data Source=<Server Here>;Initial Catalog=TimeReg4;Persist Security Info = False;User ID=TimeRegUser;Password=<Password Here>";
        private static string UKLocalConnectionStr = "Data Source=<Server Here>;Initial Catalog=TimeReg2;Persist Security Info = False;User ID=TimeRegUser;Password=<Password Here>";
        private static string TestLocalConnectionStr = "Data Source=localhost\\sqlexpress;Initial Catalog=TimeReg2;Integrated Security=True";
        private static string LocalConnectionStr = null;
        private static string LocalPublicConnectionStr = null;
        private static SqlConnection conn = null;
        public  static Dictionary<int, DBUtil.ProjTupple> projectsDict = null;  // We often need the parent name or similar
        public static Dictionary<int, DBUtil.Employee> employeesDict = null;
        private static List<DBUtil.ProjTupple> allProjects = null;


        public static bool connect(string db)
        {
            switch (db)
            {
                case "Debug":
                    LocalConnectionStr = TestLocalConnectionStr; break;
                case "DK":
                    LocalConnectionStr = DKLocalConnectionStr; break;
                case"UK":
                    LocalConnectionStr = UKLocalConnectionStr; break;
                case "DE":
                    LocalConnectionStr = DELocalConnectionStr; break;
                case "GCC":
                    LocalConnectionStr = GCCLocalConnectionStr; break;

                default:
                    LocalConnectionStr = "Illegal"; return false;
            }

            int pwStart = LocalConnectionStr.IndexOf("Password");
            if (pwStart > 0)
            {
                LocalPublicConnectionStr = LocalConnectionStr.Substring(0, pwStart);
                LocalPublicConnectionStr += "Password=****";
            }
            else
                LocalPublicConnectionStr = LocalConnectionStr;

            fillAllProjects();

            return true;
        }

        #region Modelhelpers
        /// <summary>
        /// With a cached list of projects, we simply return a copy of this
        /// </summary>
        /// <returns></returns>
        public static List<ProjTupple> getAllProjects()
        {
            return new List<ProjTupple>(allProjects);
        }

        public static List<ProjTupple> getAllProjects(bool isClosed)
        {
            List<ProjTupple> projects = new List<ProjTupple>();

            foreach (ProjTupple project in allProjects)
                if (project.Closed == isClosed)
                    projects.Add(project);

            return projects;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectID"></param>
        public static void closeProject(ProjTupple project)
        {           
            // Close project and any children in the database in one operation
            setClose(project.ProjectID);

            // Remove all entries in MyProjects with children of the project
            foreach (ProjTupple aProject in allProjects)
                if (aProject.ParentID == project.ProjectID)
                    deleteFromMyProjects(aProject.ProjectID);

            // Remove entries to the original Parent in My Projects
            deleteFromMyProjects(project.ProjectID);
        }

        public static Dictionary<string, string> getAccessRights()
        {
            string sql = "select Netname, Roles from AccessRights";
            using (conn = new SqlConnection(LocalConnectionStr))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Closed)
                        return null;
                }
                catch (SqlException)
                {
                    System.Windows.Forms.MessageBox.Show("Exception opening SQL-server using:\n\r" + LocalPublicConnectionStr);
                    return null;
                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;

                    Dictionary<string, string> dict = new Dictionary<string, string>();

                    try
                    {
                        //Run the command by using SqlDataReader.
                        SqlDataReader rdr = cmd.ExecuteReader();

                        while (rdr.Read())
                            dict.Add(rdr.GetString(0), rdr.GetString(1));
                        rdr.Close();
                    }
                    catch (Exception e)
                    {
                        System.Windows.Forms.MessageBox.Show(e.ToString() + "\n\rStacktrace:\n\r" + e.StackTrace, "Could not get AccessRights");
                    }
                    return dict;
                }
            }

        }

        #endregion "Modelhelpers"

        #region SQLProjects
        /// <summary>
        /// Get the Project Name when the ID is known
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public static string projectName(int projectID)
        {
            string sql = "select Name from Projects where ProjectID = @ProjectID";
            using (conn = new SqlConnection(LocalConnectionStr))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Closed)
                        return "";
                }
                catch (SqlException)
                {
                    System.Windows.Forms.MessageBox.Show("Exception opening SQL-server using:\n\r" + LocalPublicConnectionStr);
                    return "";
                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@ProjectID";
                    param.Value = projectID;
                    cmd.Parameters.Add(param);

                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;

                    DataTable projtable = new DataTable();
                    try
                    {
                        adapter.Fill(projtable);
                    }
                    catch (Exception e)
                    {
                        System.Windows.Forms.MessageBox.Show(e.ToString() + "\n\rStacktrace:\n\r" + e.StackTrace, "Could not fill Table from Projects");
                        return "";
                    }

                    if (projtable.Rows.Count == 1)
                    {
                        return projtable.Rows[0]["Name"].ToString();
                    }
                    else if (projtable.Rows.Count > 1)
                    {
                        System.Windows.Forms.MessageBox.Show("Database Integrity Problem - more projects with same ProjectID");
                        return "";
                    }
                    else
                        return "";
                }
            }
        }

        /// <summary>
        /// This function resembles the other SELECT functions by fetching data with a query.
        /// But it also differs, by being the only function that creates cached info:
        /// 1) A sorted List of All projects 
        /// 2) A dictionary of the same
        /// </summary>
        public static void fillAllProjects()
        {
            string sqlProjs = "SELECT P1.ProjectID, P1.Name, P1.Parent, P1.ProjectNo, P1.Closed, P2.Name As ParentName, P1.PSONumber, P1.Category, E.FullName, P1.Grouptag  " +
                              "FROM Projects AS P1 " +
                              "LEFT JOIN Projects AS P2 " +
                              "ON P1.Parent = P2.ProjectID " +
                              "LEFT JOIN Employees as E " +
                              "ON P1.ManagerID = E.EmployeeID";

            List<ProjTupple> result = new List<ProjTupple>();

            using (conn = new SqlConnection(LocalConnectionStr))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Closed)
                        return;
                }
                catch (SqlException e)
                {
                    System.Windows.Forms.MessageBox.Show("Exception opening SQL-server using:\n\r"+e.Message + LocalPublicConnectionStr);
                    return;
                }

                using (SqlCommand cmdProjs = new SqlCommand(sqlProjs, conn))
                {
                    try
                    {
                        //Run the command by using SqlDataReader.
                        SqlDataReader rdr = cmdProjs.ExecuteReader();

                        while (rdr.Read())
                            result.Add(new ProjTupple(rdr.GetInt32(0),                              // ProjectID
                                                      rdr.GetString(1),                             // Name
                                                      rdr.GetInt32(2),                              // ParentID
                                                      (rdr.IsDBNull(3)) ? "" : rdr.GetString(3),    // ProjectNo (string)
                                                      rdr.GetBoolean(4),                            // Closed
                                                      (rdr.IsDBNull(5)) ? "" : rdr.GetString(5),    // ParentName
                                                      rdr.GetInt32(6),                              // PSONumber
                                                      rdr.GetString(7),                             // Category
                                                      (rdr.IsDBNull(8) ? "" : rdr.GetString(8)),    // Full Name of manager
                                                      rdr.GetString(9)));                           // Grouptag

                        rdr.Close();
                    }

                    catch (Exception e)
                    {
                        System.Windows.Forms.MessageBox.Show(e.ToString() + "\n\rStacktrace:\n\r" + e.StackTrace, "Could not read Projects");
                        return;
                    }

                    allProjects = result;

                    // Now create the dictionary of projects
                    // We could use an array, but as projects are deleted this will become messy
                    projectsDict = new Dictionary<int, ProjTupple>();

                    // Add "parents" to Top-level parents
                    projectsDict.Add(-1, new ProjTupple(-1, "", -1, "", false, "", 0,"OUT","", "OUT"));   // Sickness And Vacation have "-1" as parent
                    projectsDict.Add(0, new ProjTupple(0, "", 0, "", false, "", 0, "NON","", "NON"));      // All other top-level projects have "0" as parent

                    foreach (ProjTupple project in allProjects)
                        projectsDict.Add(project.ProjectID, project);

                    // CANNOT SORT UNTIL THE DICTIONARY IS IN PLACE! -  Now's the time:
                    allProjects.Sort();
                }
            }
        }

        /// <summary>
        /// This code returns ProjectTupples consisting of ProjectID from MyProjects - linked to the name of the project
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public static List<ProjTupple> myProjects(int employeeID)
        {
            // Note that the indices on the reader must match the order in the select part
            string sql = "SELECT MyP.ProjectID, P.Name, P.Parent, P.ProjectNo, P.Closed, P2.Name As ParentName, P.PSONumber, P.Category, E.FullName, P.Grouptag " +
                         "FROM MyProjects AS MyP, " +
                         "Projects AS P " +
                         "LEFT JOIN Projects AS P2 " +
                         "ON P.Parent = p2.ProjectID " +
                         "LEFT JOIN Employees as E " +
                         "ON P.ManagerID = E.EmployeeID " +
                         "WHERE MyP.EmployeeID = @EmployeeID AND " +
                         "P.ProjectID = MyP.ProjectID " +
                         "ORDER BY MyP.ProjectID";

            List<ProjTupple> result = new List<ProjTupple>();

            using (conn = new SqlConnection(LocalConnectionStr))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Closed)
                        return result;
                }
                catch (SqlException)
                {
                    System.Windows.Forms.MessageBox.Show("Exception opening SQL-server using:\n\r" + LocalPublicConnectionStr);
                    return result;
                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployeeID", employeeID);

                    try
                    {
                        //Run the command by using SqlDataReader.
                        SqlDataReader rdr = cmd.ExecuteReader();

                        while (rdr.Read())
                            result.Add(new ProjTupple(rdr.GetInt32(0),              // ProjectID
                                rdr.GetString(1),                                   // Project Name
                                rdr.GetInt32(2),                                    // Parent ID
                                (rdr.IsDBNull(3)) ? "" : rdr.GetString(3),          // ProjectNo (string)
                                rdr.GetBoolean(4),                                  // Closed ?
                                (rdr.IsDBNull(5)) ? "" : rdr.GetString(5),          // ProjectNo (string)
                                rdr.GetInt32(6),                                    // PSONumber
                                rdr.GetString(7),                                   // Category
                                (rdr.IsDBNull(8)) ? "" : rdr.GetString(8),          // ProjectNo (string)
                                rdr.GetString(9)));                                 // Grouptag

                        rdr.Close();
                    }
                    catch (Exception e)
                    {
                        System.Windows.Forms.MessageBox.Show(e.ToString() + "\n\rStacktrace:\n\r" + e.StackTrace, "Could not read MyProjects");
                    }

                    result.Sort();
                    return result;
                }
            }
        }

        /// <summary>
        /// This method adds a day to the date. If weekend it moves to monday
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private static DateTime incSkipWeekend(DateTime date)
        {
            date = date.AddDays(1);
            if (date.DayOfWeek == DayOfWeek.Saturday)
                return date.AddDays(2);

            if (date.DayOfWeek == DayOfWeek.Sunday)
                return date.AddDays(1);

            return date;
        }

       
        /// <summary>
        /// This class is used by the "Except" method on Date-Lists to compare dates by value instead of by reference
        /// </summary>
        class  DateCompare : IEqualityComparer<DateTime>
        {
            public bool Equals(DateTime a, DateTime b)
            {
                return (a.Date == b.Date);
            }
            public int GetHashCode(DateTime date)
            {
                return (date.Year * 1000 + date.DayOfYear);
            }
        }

        /// <summary>
        /// This function returns a list of dates that are normal workdays - but has no hours registered.
        /// First we get DISTINCT dates from the database - that is only one record from each day with content.
        /// Then we build a list of normal workdays (ignoring hollidays at first)
        /// We then extract - from workdays - days that we worked and then hollidays.
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public static List<DateTime> dates(int employeeID, DateTime firstDate, DateTime lastDate, List<DateTime> hollidays)
        {
            // Note that the indices on the reader must match the order in the select part
            string sql = "SELECT DISTINCT Date FROM Main "+
                         "WHERE EmployeeID = @EmployeeID "+
                         "AND Date >= @First "+
                         "AND Date <= @Last "+
                         "ORDER BY Date";

            List<DateTime> result = new List<DateTime>();
            List<DateTime> worked = new List<DateTime>();

            using (conn = new SqlConnection(LocalConnectionStr))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Closed)
                        return result;
                }
                catch (SqlException)
                {
                    System.Windows.Forms.MessageBox.Show("Exception opening SQL-server using:\n\r" + LocalPublicConnectionStr);
                    return result;
                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                    cmd.Parameters.AddWithValue("@First", firstDate.Date);
                    cmd.Parameters.AddWithValue("@Last", lastDate.Date);

                    try
                    {
                        //Run the command by using SqlDataReader.
                        SqlDataReader rdr = cmd.ExecuteReader();

                        DateTime lookFor = firstDate;

                        while (rdr.Read())
                            worked.Add(rdr.GetDateTime(0));

                        rdr.Close();
                    }

                    catch (Exception e)
                    {
                        System.Windows.Forms.MessageBox.Show(e.ToString() + "\n\rStacktrace:\n\r" + e.StackTrace, "Could not read Main");
                    }

                }
            }

            List<DateTime> workdays = new List<DateTime>();

            // Loop starts by adding a day - so start by subtracting one
            DateTime date = firstDate.AddDays(-1);

            while (true)
            {
                date = incSkipWeekend(date);
                if (date.Date > lastDate.Date)
                    break;
                workdays.Add(date);
            }

            // Now make a list with all workdays - except the ones worked on - and afterwards subtract all hollidays
            result = workdays.Except(worked, new DateCompare()).ToList();
            result = result.Except(hollidays, new DateCompare()).ToList();

            return result;           
        }



        /// <summary>
        /// User may add a single existing Project into his "MyProjects"
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="EmployeeID"></param>
        public static void insertIntoMyProjects(int ProjectID, int EmployeeID)
        {
            string sql = "insert into MyProjects (ProjectID, EmployeeID) values (@ProjectID, @EmployeeID)";

            using (conn = new SqlConnection(LocalConnectionStr))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Closed)
                        return;
                }
                catch (SqlException)
                {
                    System.Windows.Forms.MessageBox.Show("Exception opening SQL-server using:\n\r" + LocalPublicConnectionStr);
                    return;
                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProjectID", ProjectID);
                    cmd.Parameters.AddWithValue("@EmployeeID", EmployeeID);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        System.Windows.Forms.MessageBox.Show(e.ToString() + "\n\rStacktrace:\n\r" + e.StackTrace, "Could not INSERT data into MyProjects");
                    }
                }
            }
        }
                

        /// <summary>
        /// User may delete a single project from his MyProjects
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="EmployeeID"></param>
        public static void deleteFromMyProjects(int ProjectID, int EmployeeID)
        {
            string sql = "delete from MyProjects where ProjectID = @ProjectID and EmployeeID = @EmployeeID";

            using (conn = new SqlConnection(LocalConnectionStr))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Closed)
                        return;
                }
                catch (SqlException)
                {
                    System.Windows.Forms.MessageBox.Show("Exception opening SQL-server using:\n\r" + LocalPublicConnectionStr);
                    return;
                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProjectID", ProjectID);
                    cmd.Parameters.AddWithValue("@EmployeeID", EmployeeID);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        System.Windows.Forms.MessageBox.Show(e.ToString() + "\n\rStacktrace:\n\r" + e.StackTrace, "Could not DELETE data from MyProjects");
                    }
                }
            }
        }

        /// <summary>
        /// When closing projects we need them to disappear from ALL users MyProjects List
        /// </summary>
        /// <param name="ProjectID"></param>
        private static void deleteFromMyProjects(int ProjectID)
        {
            string sql = "delete from MyProjects where ProjectID = @ProjectID";

            using (conn = new SqlConnection(LocalConnectionStr))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Closed)
                        return;
                }
                catch (SqlException)
                {
                    System.Windows.Forms.MessageBox.Show("Exception opening SQL-server using:\n\r" + LocalPublicConnectionStr);
                    return;
                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProjectID", ProjectID);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        System.Windows.Forms.MessageBox.Show(e.ToString() + "\n\rStacktrace:\n\r" + e.StackTrace, "Could not DELETE multiple items from MyProjects");
                    }
                }
            }
        }

        /// <summary>
        /// Insert a new Project with a parentID (not using other columns)
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Parent"></param>
        public static void insertIntoAllProjects(string name, int parent, string projectNo, int psoNumber, string category, int manID)
        {
            string sql = "insert into Projects (Name, Parent, ProjectNo, PSONumber, Category, ManagerID) values (@Name, @Parent, @ProjectNo, @PSONumber, @Category, @ManagerID)";

            using (conn = new SqlConnection(LocalConnectionStr))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Closed)
                        return;
                }
                catch (SqlException)
                {
                    System.Windows.Forms.MessageBox.Show("Exception opening SQL-server using:\n\r" + LocalPublicConnectionStr);
                    return;
                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Parent", parent);
                    cmd.Parameters.AddWithValue("@ProjectNo", projectNo);
                    cmd.Parameters.AddWithValue("@PSONumber", psoNumber);
                    cmd.Parameters.AddWithValue("@Category", category);
                    cmd.Parameters.AddWithValue("@ManagerID", manID);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        System.Windows.Forms.MessageBox.Show(e.ToString() + "\n\rStacktrace:\n\r" + e.StackTrace, "Could not INSERT data into Projects");
                    }
                }
            }
        }

        /// <summary>
        /// Simple update of Project Name or other simple field for a given ProjectID
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="Name"></param>
        public static void renameProject(int projectID, string name, string projectno, int psoNumber, string category, string grouptag)
        {
            string sql = "update Projects set Name = @Name, ProjectNo = @ProjectNo , PSONumber = @PSONumber , Category = @Category , Grouptag = @grouptag " +
                            "where ProjectID = @ProjectID";

            using (conn = new SqlConnection(LocalConnectionStr))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Closed)
                        return;
                }
                catch (SqlException)
                {
                    System.Windows.Forms.MessageBox.Show("Exception opening SQL-server using:\n\r" + LocalPublicConnectionStr);
                    return;
                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProjectID", projectID);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@ProjectNo", projectno);
                    cmd.Parameters.AddWithValue("@PSONumber", psoNumber);
                    cmd.Parameters.AddWithValue("@Category", category);
                    cmd.Parameters.AddWithValue("@Grouptag", grouptag);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        System.Windows.Forms.MessageBox.Show(e.ToString() + "\n\rStacktrace:\n\r" + e.StackTrace, "Could not UPDATE data in Projects");
                    }
                }
            }
        }

        /// <summary>
        /// Set the Close flag on a parent and its children
        /// </summary>
        /// <param name="projectID"></param>
        private static void setClose(int projectID)
        {
            string sql = "UPDATE Projects SET Closed = 1 WHERE ProjectID = @ProjectID OR Parent = @ProjectID";

            using (conn = new SqlConnection(LocalConnectionStr))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Closed)
                        return;
                }
                catch (SqlException)
                {
                    System.Windows.Forms.MessageBox.Show("Exception opening SQL-server using:\n\r" + LocalPublicConnectionStr);
                    return;
                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProjectID", projectID);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        System.Windows.Forms.MessageBox.Show(e.ToString() + "\n\rStacktrace:\n\r" + e.StackTrace, "Could not CLOSE projectID and children" + projectID.ToString());
                    }
                }
            }
        }


        /// <summary>
        /// Set the Project Manager on a parent and its children
        /// </summary>
        /// <param name="projectID"></param>
        public static void setManager(int projectID, int managerID)
        {
/*
            string sql =    "UPDATE Projects " +
                            "SET ManagerID = EmployeeID " +
                            "FROM Projects " +
                            "INNER JOIN Employees " +
                            "ON FullName = @FullName " +
                            "WHERE ProjectID = @ProjectID";
*/

            string sql = "UPDATE Projects SET ManagerID = @ManagerID WHERE ProjectID = @ProjectID OR Parent = @ProjectID";
            using (conn = new SqlConnection(LocalConnectionStr))
            { 
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Closed)
                        return;
                }
                catch (SqlException)
                {
                    System.Windows.Forms.MessageBox.Show("Exception opening SQL-server using:\n\r" + LocalPublicConnectionStr);
                    return;
                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProjectID", projectID);
                    cmd.Parameters.AddWithValue("@ManagerID", managerID);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        System.Windows.Forms.MessageBox.Show(e.ToString() + "\n\rStacktrace:\n\r" + e.StackTrace, "Could not CLOSE projectID and children" + projectID.ToString());
                    }
                }
            }
        }
        
        #endregion "SQLProjects"

        #region SQLWeek
        /// <summary>
        /// Get project registrations for a given employee in a given week - with parent to the project as well
        /// This is returned as full weeks in a list. Empty days included.
        /// Each week represents ONE Project with sparse data in ONE week for ONE employee
        /// We do not need the Date of the Oracle Registration - just whether it exists
        /// OBS! It may be tempting to filter out e.g. entries with 0 hours
        ///     - but that could eventually lead to two entries on same project and day!
        /// </summary>
        /// <param name="firstday"></param>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public static List<Week> createListFromDB(DateTime firstday, int employeeID)
        {
            string sql = "SELECT M.MainID, M.Date, M.ProjectID, M.Hours, M.Comment, M.InOracle, P.Parent, P.Name " +
                         "FROM Main as M, Projects AS P " +
                         "WHERE M.EmployeeID = @EmployeeID  " +
                         "AND P.ProjectID = M.ProjectID " +
                         "AND Date >= @Firstday AND Date <= @Lastday " +
                         "ORDER BY M.ProjectID, M.Date";

            List<Week> mainList = new List<Week>();

            using (conn = new SqlConnection(LocalConnectionStr))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Closed)
                        return mainList;
                }
                catch (SqlException)
                {
                    System.Windows.Forms.MessageBox.Show("Exception opening SQL-server using:\n\r" + LocalPublicConnectionStr);
                    return mainList;
                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    SqlParameter param1 = new SqlParameter();
                    param1.ParameterName = "@EmployeeID";
                    param1.Value = employeeID;
                    cmd.Parameters.Add(param1);

                    SqlParameter param2 = new SqlParameter();
                    param2.ParameterName = "@Firstday";
                    param2.Value = firstday;
                    cmd.Parameters.Add(param2);

                    SqlParameter param3 = new SqlParameter();
                    param3.ParameterName = "@Lastday";
                    param3.Value = firstday.AddDays(6);
                    cmd.Parameters.Add(param3);

                    Week projectWeek = null;

                    int mainID = 0;
                    DateTime date;
                    int projectID = 0;
                    int oldProjectID = 0;
                    float hours = 0;
                    string comment = null;
                    int parentID = 0;
                    string name = null;
                    bool inOracle = false;

                    try
                    {
                        SqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            mainID = rdr.GetInt32(0);                               // MainID Autonum Key
                            date = rdr.GetDateTime(1);                              // Date Entry
                            projectID = rdr.GetInt32(2);                            // ProjectID - foreign key
                            hours = rdr.GetFloat(3);                                // Hours
                            comment = (rdr.IsDBNull(4)) ? "" : rdr.GetString(4);    // Comment
                            inOracle = (!rdr.IsDBNull(5));                          // Date of Oracle Export exists?
                            parentID = rdr.GetInt32(6);                             // Parent Project - foreign key
                            name = rdr.GetString(7);                                // Parent Project Name

                            if (oldProjectID != projectID)
                            {
                                // At this point we are for sure starting a new week (not necessarily with data from monday though
                                oldProjectID = projectID;
                                projectWeek = new Week(name); // 7 new days of references to CellData

                                // Create a Project Row for a full week with the given data
                                DateTime day = firstday;

                                // Create a weekfull of readonly and "empty" data for now
                                for (int weekday = 0; weekday < 7; weekday++)
                                {
                                    projectWeek[weekday] = new CellData(day.AddDays(weekday), employeeID, projectID, parentID);
                                }
                                // Add the entry
                                mainList.Add(projectWeek);
                            }

                            int dayInx = date.DayOfWeek - firstday.DayOfWeek;
                            if (dayInx == -1)   // Watch out for sundays!
                                dayInx = 6;
                            projectWeek[dayInx].MainID = mainID;
                            projectWeek[dayInx].Hours = hours;
                            projectWeek[dayInx].Comment = comment;
                            projectWeek[dayInx].InOracle = inOracle;
                        }
                        rdr.Close();
                    }
                    catch (Exception e)
                    {
                        System.Windows.Forms.MessageBox.Show(e.ToString() + "\n\rStacktrace:\n\r" + e.StackTrace, "Could not read from Main");
                    }

                    // Sort according to projects and their parents
                    mainList.Sort();
                    return mainList;
                }
            }
        }
        #endregion "SQLWeek"

        #region SQLEmployee
        /// <summary>
        /// Get a full list of all employees - netname and fullname
        /// </summary>
        /// <returns></returns>
        public static List<Employee> getEmployees(bool superOnly)
        {
            //Define a query string that has a parameter for orderID.
            string sqlAll = "select Netname, FullName, Number, Department, SuperUser, EmployeeID from Employees";
            string sqlMan = "select Netname, FullName, Number, Department, SuperUser, EmployeeID from Employees WHERE SuperUser = 1";

            string sql = (superOnly) ? sqlMan : sqlAll;

            List<Employee> result = new List<Employee>();

            using (conn = new SqlConnection(LocalConnectionStr))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Closed)
                        return result;
                }
                catch (SqlException)
                {
                    System.Windows.Forms.MessageBox.Show("Exception opening SQL-server using:\n\r" + LocalPublicConnectionStr);
                    return result;
                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    try
                    {
                        //Run the command by using SqlDataReader.
                        SqlDataReader rdr = cmd.ExecuteReader();

                        while (rdr.Read())
                            result.Add(new Employee(rdr.GetString(0), rdr.GetString(1), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetBoolean(4), rdr.GetInt32(5)));

                        rdr.Close();
                    }
                    catch (Exception e)
                    {
                        System.Windows.Forms.MessageBox.Show(e.ToString() + "\n\rStacktrace:\n\r" + e.StackTrace, "Could not get list of employees");
                    }
                    result.Sort();
                    return result;
                }
            }
        }

        /// <summary>
        /// Get the database ID of an Employee from the netname (without domain, in small caps - e.g. "kelk")
        /// </summary>
        /// <param name="netname"></param>
        /// <returns></returns>
        public static int getEmployeeID(string netname, out bool superUser)
        {
            string fullName="";
            return getEmployeeInfo(netname, out superUser, out fullName);
        }

            
        public static int getEmployeeInfo(string netname, out bool superUser, out string fullName)
        {
            int ID = 0;
            superUser = false;
            fullName = "";

            //Define a query string that has a parameter for orderID.
            string sql = "select EmployeeID, SuperUser, FullName from Employees where Netname = @Netname";

            using (conn = new SqlConnection(LocalConnectionStr))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Closed)
                        return 0;
                }
                catch (SqlException)
                {
                    System.Windows.Forms.MessageBox.Show("Exception opening SQL-server using:\n\r" + LocalPublicConnectionStr);
                    return 0;
                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    SqlParameter param = new SqlParameter();
                    cmd.Parameters.AddWithValue("@Netname", netname);

                    try
                    {
                        //Run the command by using SqlDataReader.
                        SqlDataReader rdr = cmd.ExecuteReader();

                        if (rdr.Read())
                        {
                            //employeeIDStr = rdr["EmployeeID"].ToString(); also works
                            ID = rdr.GetInt32(0); // Get 0'th column as int
                            superUser = rdr.GetBoolean(1);
                            fullName = rdr.GetString(2);
                        }

                        rdr.Close();
                    }
                    catch (Exception e)
                    {
                        System.Windows.Forms.MessageBox.Show(e.ToString() + "\n\rStacktrace:\n\r" + e.StackTrace, "Could not find EmployeeID");
                    }
                    return ID;
                }
            }
        }

        /// <summary>
        /// Insert an Employee
        /// GUI has checked that this does not exist already, but DB will not permit a duplicate anyway
        /// </summary>
        /// <param name="netname"></param>
        /// <param name="fullname"></param>
        /// <param name="super"></param>
        public static void insertNewEmployee(Employee employee)
        {
            string sql = "insert into Employees (Netname, FullName, SuperUser, Number, Department) values (@NetName, @FullName, @SuperUser, @Number, @Department)";

            using (conn = new SqlConnection(LocalConnectionStr))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Closed)
                        return;
                }
                catch (SqlException)
                {
                    System.Windows.Forms.MessageBox.Show("Exception opening SQL-server using:\n\r" + LocalPublicConnectionStr);
                    return;
                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@NetName", employee.NetName);
                    cmd.Parameters.AddWithValue("@FullName", employee.FullName);
                    cmd.Parameters.AddWithValue("@Number", employee.Number);
                    cmd.Parameters.AddWithValue("@Department", employee.Department);
                    cmd.Parameters.AddWithValue("@SuperUser", employee.SuperUser);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        System.Windows.Forms.MessageBox.Show(e.ToString() + "\n\rStacktrace:\n\r" + e.StackTrace, "Could not INSERT data into Employees");
                    }
                }
            }
        }

        /// <summary>
        /// Update existing Employee entry - based on Netname
        /// SuperUser not changed
        /// </summary>
        /// <param name="employee"></param>
        public static void updateEmployee(Employee employee)
        {
            string sql = "UPDATE Employees set FullName = @FullName, Number = @Number, Department = @Department " +
                "where NetName = @NetName";

            using (conn = new SqlConnection(LocalConnectionStr))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Closed)
                        return ;
                }
                catch (SqlException)
                {
                    System.Windows.Forms.MessageBox.Show("Exception opening SQL-server using:\n\r" + LocalPublicConnectionStr);
                    return ;
                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@NetName", employee.NetName);
                    cmd.Parameters.AddWithValue("@FullName", employee.FullName);
                    cmd.Parameters.AddWithValue("@Number", employee.Number);
                    cmd.Parameters.AddWithValue("@Department", employee.Department);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        System.Windows.Forms.MessageBox.Show(e.ToString() + "\n\rStacktrace:\n\r" + e.StackTrace, "Could not UPDATE data into Employees");
                    }
                }
            }
        }

        #endregion "SQLEmployee"

        #region SQLMain
        /// <summary>
        /// This inserts one new row into the main table - based on the parameters given
        /// </summary>
        /// <param name="employeeID"></param>
        /// <param name="date"></param>
        /// <param name="projectID"></param>
        /// <param name="hours"></param>
        /// <param name="comment"></param>
        public static int insertNewMainRow(int employeeID, DateTime date, int projectID, float hours, string comment)
        {
            string sql = "INSERT INTO Main (EmployeeID, Date, ProjectID, Hours, Comment)" +
                            "OUTPUT INSERTED.MainID "+  
                            "VALUES (@EmployeeID, @Date, @ProjectID, @Hours, @Comment)";

            if (comment == null)
                comment = "Created " + DateTime.Now.ToShortDateString();

            using (conn = new SqlConnection(LocalConnectionStr))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Closed)
                        return -1;
                }
                catch (SqlException)
                {
                    System.Windows.Forms.MessageBox.Show("Exception opening SQL-server using:\n\r" + LocalPublicConnectionStr);
                    return -1;
                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    int ID;

                    cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                    cmd.Parameters.AddWithValue("@Date", date);
                    cmd.Parameters.AddWithValue("@ProjectID", projectID);
                    cmd.Parameters.AddWithValue("@Hours", hours);
                    cmd.Parameters.AddWithValue("@Comment", comment);
                    try
                    {
                        ID = (int)cmd.ExecuteScalar();
                    }
                    catch (SqlException e)
                    {
                        System.Windows.Forms.MessageBox.Show(e.ToString() + "\n\rStacktrace:\n\r" + e.StackTrace, "Could not INSERT data into MainTable");
                        return -1;
                    }
                    return ID;
                }
            }
        }

        /// <summary>
        /// This updates an existing row in the mainTable
        /// We only need the paramers that may have been changed - plus the key (mainID)
        /// </summary>
        /// <param name="mainID"></param>
        /// <param name="hours"></param>
        /// <param name="comment"></param>
        public static void updateExistingMainRow(int mainID, float hours, string comment)
        {
            string sql = "update Main set Hours = @Hours, Comment = @Comment " +
                            "where MainID = @MainID";

            if (comment == null)
                comment = "Created " + DateTime.Now.ToShortDateString();

            using (conn = new SqlConnection(LocalConnectionStr))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Closed)
                        return ;
                }
                catch (SqlException)
                {
                    System.Windows.Forms.MessageBox.Show("Exception opening SQL-server using:\n\r" + LocalPublicConnectionStr);
                    return ;
                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Hours", hours);
                    cmd.Parameters.AddWithValue("@Comment", comment);
                    cmd.Parameters.AddWithValue("@MainID", mainID);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        System.Windows.Forms.MessageBox.Show(e.ToString() + "\n\rStacktrace:\n\r" + e.StackTrace, "Could not UPDATE data in MainTable");
                    }
                }
            }
        }


        public static int getMainRowID(int employeeID, DateTime date, int projectID)
        {
            string sql = "SELECT MainID " +
                         "FROM Main " +
                         "WHERE EmployeeID = @EmployeeID  " +
                         "AND ProjectID = @ProjectID " +
                         "AND Date = @Day";

            int mainID = -1;

            using (conn = new SqlConnection(LocalConnectionStr))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Closed)
                        return -1;
                }
                catch (SqlException)
                {
                    System.Windows.Forms.MessageBox.Show("Exception opening SQL-server using:\n\r" + LocalPublicConnectionStr);
                    return -1;
                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    SqlParameter param1 = new SqlParameter();
                    param1.ParameterName = "@EmployeeID";
                    param1.Value = employeeID;
                    cmd.Parameters.Add(param1);

                    SqlParameter param2 = new SqlParameter();
                    param2.ParameterName = "@Day";
                    param2.Value = date;
                    cmd.Parameters.Add(param2);

                    SqlParameter param3 = new SqlParameter();
                    param3.ParameterName = "@ProjectID";
                    param3.Value = projectID;
                    cmd.Parameters.Add(param3);


                    try
                    {
                        SqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            mainID = rdr.GetInt32(0);                               // MainID Autonum Key
                        }
                    }
                    catch (SqlException e)
                    {
                        System.Windows.Forms.MessageBox.Show("Exception un getMainRow using:\n\r" + e.Message);
                        return -1;
                    }
                }
            }
             
            return mainID;
        }

        /// <summary>
        /// Typically this is used when the user writes "0" hours, to avoid an empty project line..
        /// But it might be used for other purposes as well.
        /// </summary>
        /// <param name="mainID"></param>
        public static void deleteMainRow(int mainID)
        {
            string sql = "DELETE FROM Main "+
                         "WHERE MainID = @MainID";

            using (conn = new SqlConnection(LocalConnectionStr))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Closed)
                        return ;
                }
                catch (SqlException)
                {
                    System.Windows.Forms.MessageBox.Show("Exception opening SQL-server using:\n\r" + LocalPublicConnectionStr);
                    return ;
                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@MainID", mainID);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        System.Windows.Forms.MessageBox.Show(e.ToString() + "\n\rStacktrace:\n\r" + e.StackTrace, "Could not DELETE row in MainTable");
                    }
                }
            }
        }

        /// <summary>
        /// This method updates all the entries that were submitted to Oracle with todays date in "InOracle"
        /// Set them back with:
        /// UPDATE [TimeReg].[dbo].[Main] 
        /// SET InOracle = NULL
        /// WHERE InOracle > '2015-02-16' // Use todays date
        /// </summary>
        /// <param name="entries"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public static void markInOracle(string entries)
        {
            string sql = "UPDATE Main SET InOracle = @Date " +
                            "WHERE MainID IN ("+entries+")";

            using (conn = new SqlConnection(LocalConnectionStr))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Closed)
                        return;
                }
                catch (SqlException)
                {
                    System.Windows.Forms.MessageBox.Show("Exception opening SQL-server using:\n\r" + LocalPublicConnectionStr);
                    return;
                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        System.Windows.Forms.MessageBox.Show(e.ToString() + "\n\rStacktrace:\n\r" + e.StackTrace, "Could not UPDATE data in Projects");
                    }
                }

            }
        }

        #endregion "SQLMain"

        #region SQLReports
        /// <summary>
        /// Get a sum for all projects per employee - with name of project and name of parent
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public static List<ProjectReport> getProjectTotalsPerEmployee(DateTime fromDate, DateTime toDate, bool showSicknessVacation)
        {
            string sql = "SELECT  Employees.FullName, projects.Name AS ProjectName, projects.ProjectNo, projects2.Name AS ParentName, " +
                          "ROUND(SUM(main.Hours),0) As TotalHours, projects.ProjectID, projects.Parent, Employees.Department, Projects.PSONumber, projects.Category, projects.Grouptag " +
                         "FROM PROJECTS projects " +
                         "LEFT OUTER JOIN PROJECTS projects2 " +
                         "ON projects.PARENT = projects2.PROJECTID " +
                         "INNER JOIN Main main " +
                         "ON main.ProjectID = projects.ProjectID " +
                         "INNER JOIN Employees " +
                         "On main.EmployeeID = Employees.EmployeeID " +
                         "WHERE main.Date >= @FromDate AND main.Date <= @ToDate " +
                         "GROUP BY Employees.FullName, Employees.Department, projects2.Name, projects.Parent, projects.ProjectID, projects.Name, projects.ProjectNo, projects.PSONumber, projects.Category, projects.Grouptag " +
                         "ORDER BY Employees.FullName, projects2.Name, projects.Name";



            List<ProjectReport> result = new List<ProjectReport>();

            using (conn = new SqlConnection(LocalConnectionStr))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Closed)
                        return result;
                }
                catch (SqlException)
                {
                    System.Windows.Forms.MessageBox.Show("Exception opening SQL-server using:\n\r" + LocalPublicConnectionStr);
                    return result;
                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@FromDate", fromDate);
                    cmd.Parameters.AddWithValue("@ToDate", toDate);

                    try
                    {
                        SqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            int parentID = rdr.GetInt32(6);
                            if (parentID != -1 || showSicknessVacation)
                                result.Add(new ProjectReport(
                                    rdr.GetString(0),                         // Employee FullName
                                    rdr.GetString(1),                         // Project Name
                                    rdr.IsDBNull(2) ? "" : rdr.GetString(2),  // Project No
                                    rdr.IsDBNull(3) ? "" : rdr.GetString(3),  // Parent Name
                                    (float)rdr.GetSqlDouble(4),               // Sum
                                    rdr.GetInt32(5),                          // ProjectID
                                    parentID,                                 // ParentID
                                    rdr.GetInt32(7),                          // Department
                                    rdr.GetInt32(8),                          // PSO Number
                                    rdr.GetString(9),                         // Category
                                    projectsDict[rdr.GetInt32(5)].Manager,    // Manager (with some cheating)
                                    rdr.GetString(10)));                      // Grouptag
                        }
                        rdr.Close();
                    }
                    catch (SqlException e)
                    {
                        System.Windows.Forms.MessageBox.Show(e.ToString() + "\n\rStacktrace:\n\r" + e.StackTrace, "Could not SELECT Sum of Projects");
                    }

                    return result;
                }
            }
        }

        /// <summary>
        /// Project Totals over all Employees
        /// This method uses an "IN" statement with a string, which is a little unsafe. However it is not directly input by user, but generated from listbox.
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="departments">Comma separated string of department numbers. Null if all</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public static List<ProjectReport> getProjectTotals(DateTime fromDate, DateTime toDate, bool showSicknessVacation, string departments)
        {
            string sql;
            if (departments == null)
               sql        = "SELECT projects.Name AS ProjectName, projects.ProjectNo, projects2.Name AS ParentName, " +
                            "ROUND(SUM(main.Hours),0) As TotalHours, projects.ProjectID, projects.Parent, projects.PSONumber, projects.Category, projects.Grouptag " +
                            "FROM PROJECTS projects " +
                            "LEFT OUTER JOIN PROJECTS projects2 " +
                            "ON projects.PARENT = projects2.PROJECTID " +
                            "INNER JOIN Main main " +
                            "ON main.ProjectID = projects.ProjectID " +
                            "WHERE main.Date >= @FromDate AND main.Date <= @ToDate "+
                            "GROUP BY projects2.Name, projects.Parent, projects.ProjectID, projects.Name, projects.ProjectNo, projects.PSONumber, projects.Category, projects.Grouptag " +
                            "ORDER BY projects2.Name, projects.Name";

            else
                sql = "SELECT projects.Name AS ProjectName, projects.ProjectNo, projects2.Name AS ParentName, " +
                             "ROUND(SUM(main.Hours),0) As TotalHours, projects.ProjectID, projects.Parent, projects.PSONumber, projects.Category, projects.Grouptag  " +
                             "FROM PROJECTS projects " +
                             "LEFT OUTER JOIN PROJECTS projects2 " +
                             "ON projects.PARENT = projects2.PROJECTID " +
                             "INNER JOIN Main main " +
                             "ON main.ProjectID = projects.ProjectID " +
                             "INNER JOIN Employees " +
                             "ON main.EmployeeID = Employees.EmployeeID " +
                             "WHERE main.Date >= @FromDate AND main.Date <= @ToDate " +
                             "AND Employees.Department IN (" + departments + ") " +
                             "GROUP BY projects2.Name, projects.Parent, projects.ProjectID, projects.Name, projects.ProjectNo, projects.PSONumber, projects.Category, projects.Grouptag " +
                             "ORDER BY projects2.Name, projects.Name";

            List<ProjectReport> result = new List<ProjectReport>();

            using (conn = new SqlConnection(LocalConnectionStr))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Closed)
                        return result;
                }
                catch (SqlException)
                {
                    System.Windows.Forms.MessageBox.Show("Exception opening SQL-server using:\n\r" + LocalPublicConnectionStr);
                    return result;
                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    cmd.Parameters.AddWithValue("@FromDate", fromDate);
                    cmd.Parameters.AddWithValue("@ToDate", toDate);
                    try
                    {
                        SqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            int parentID = rdr.GetInt32(5);
                            if (parentID != -1 || showSicknessVacation)
                                result.Add(new ProjectReport("All",           // Employee (not relevant when aggregated)
                                    rdr.GetString(0),                         // Project Name
                                    rdr.IsDBNull(1) ? "" : rdr.GetString(1),  // Project No
                                    rdr.IsDBNull(2) ? "" : rdr.GetString(2),  // Parent Name
                                    (float)rdr.GetSqlDouble(3),               // SUM
                                    rdr.GetInt32(4),                          // Project ID
                                    parentID,                                 // Parent ID
                                    0,                                        // Department (not relevant when aggregated)
                                    rdr.GetInt32(6),                          // PSONumber
                                    rdr.GetString(7),                         // Category
                                    projectsDict[rdr.GetInt32(4)].Manager,    // Manager
                                    rdr.GetString(8)));                       // Grouptag
                        }
                        rdr.Close();
                    }
                    catch (SqlException e)
                    {
                        System.Windows.Forms.MessageBox.Show(e.ToString() + "\n\rStacktrace:\n\r" + e.StackTrace, "Could not SELECT Sum of Projects");
                    }
                    result.Sort();
                    return result;
                }
            }
        }

        /// <summary>
        /// This method directly aggregates on parents - and amortizes in months
        /// It uses a "SuperProject" which is the Project - or the Parent if this is > 0
        /// 2018-01-10: The above gives the right results and grouping. BUT: No sense in the order of data
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="showSicknessVacation"></param>
        /// <param name="departments"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public static List<AggrReport> getAmortizedParentTotals(DateTime fromDate, DateTime toDate, bool showSicknessVacation, string departments)
        {
            string sql;

            if (departments == null)
            {
                sql = 
                    "WITH CTE AS ( " +
                    "SELECT Year(main.Date) AS Year, Month(main.Date) AS Month, projects.ProjectID, projects.Parent, main.Hours, " +
                    "CASE WHEN projects.Parent > 0 THEN projects.Parent " +
                    "ELSE projects.ProjectID " +
                    "END SuperProj " +
                    "FROM Projects projects " +
                    "INNER JOIN Main main " +
                    "ON main.ProjectID = projects.ProjectID " +
                    "WHERE main.Date >= @FromDate AND main.Date <= @ToDate " +
                    ") " +
                    "SELECT SuperProj, Year, Month, ROUND(SUM(Hours),0) AS Total " +
                    "FROM CTE " +
                    "GROUP BY SuperProj, Year, Month " +
                    "ORDER BY SuperProj, Year, Month ";
            }
            else
            {
                sql =
                    "WITH CTE AS ( " +
                    "SELECT Year(main.Date) AS Year, Month(main.Date) AS Month, projects.ProjectID, projects.Parent, main.Hours, " +
                    "CASE WHEN projects.Parent > 0 THEN projects.Parent " +
                    "ELSE projects.ProjectID " +
                    "END SuperProj " +
                    "FROM Projects projects " +
                    "INNER JOIN Main main " +
                    "ON main.ProjectID = projects.ProjectID " +
                    "INNER JOIN Employees " +
                    "ON main.EmployeeID = Employees.EmployeeID " +
                    "WHERE main.Date >= @FromDate AND main.Date <= @ToDate " +
                    "AND Employees.Department IN (" + departments + ") " +
                    ") " +
                    "SELECT SuperProj, Year, Month, ROUND(SUM(Hours),0) AS Total " +
                    "FROM CTE " +
                    "GROUP BY SuperProj, Year, Month " +
                    "ORDER BY SuperProj, Year, Month ";
            } 
            
            List<AggrReport> result = new List<AggrReport>();

            using (conn = new SqlConnection(LocalConnectionStr))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Closed)
                        return result;
                }
                catch (SqlException)
                {
                    System.Windows.Forms.MessageBox.Show("Exception opening SQL-server using:\n\r" + LocalPublicConnectionStr);
                    return result;
                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    cmd.Parameters.AddWithValue("@FromDate", fromDate);
                    cmd.Parameters.AddWithValue("@ToDate", toDate);
                    try
                    {
                        SqlDataReader rdr = cmd.ExecuteReader();
                        int no = 0;
                        while (rdr.Read())
                        {
                            result.Add(new AggrReport(rdr.GetInt32(0),          // SuperProject
                                                      rdr.GetInt32(1),          // Year
                                                      rdr.GetInt32(2),          // Month
                                              (float)rdr.GetDouble(3)));        // Accumulated Hours
                            no++;
                        }
                        rdr.Close();
                    }
                    catch (SqlException e)
                    {
                        System.Windows.Forms.MessageBox.Show(e.ToString() + "\n\rStacktrace:\n\r" + e.StackTrace, "Could not SELECT Aggregated list");
                    }

                    return result;
                }
            }
        }

        /// <summary>
        /// As Above - but now using the Projects directly and only the SuperProject as an organizer
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="showSicknessVacation"></param>
        /// <param name="departments"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public static List<AggrReport> getAmortizedProjectTotals(DateTime fromDate, DateTime toDate, bool showSicknessVacation, string departments)
        {
            string sql;

            if (departments == null)
            {
                sql =
                    "WITH CTE AS ( " +
                    "SELECT Year(main.Date) AS Year, Month(main.Date) AS Month, projects.ProjectID, projects.Parent, main.Hours, " +
                    "CASE WHEN projects.Parent > 0 THEN projects.Parent " +
                    "ELSE projects.ProjectID " +
                    "END SuperProj " +
                    "FROM Projects projects " +
                    "INNER JOIN Main main " +
                    "ON main.ProjectID = projects.ProjectID " +
                    "WHERE main.Date >= @FromDate AND main.Date <= @ToDate " +
                    ") " +
                    "SELECT SuperProj, ProjectID, Year, Month, ROUND(SUM(Hours),0) AS Total " +
                    "FROM CTE " +
                    "GROUP BY SuperProj, ProjectID, Year, Month " +
                    "ORDER BY SuperProj, ProjectID, Year, Month ";
            }
            else
            {
                sql =
                    "WITH CTE AS ( " +
                    "SELECT Year(main.Date) AS Year, Month(main.Date) AS Month, projects.ProjectID, projects.Parent, main.Hours, " +
                    "CASE WHEN projects.Parent > 0 THEN projects.Parent " +
                    "ELSE projects.ProjectID " +
                    "END SuperProj " +
                    "FROM Projects projects " +
                    "INNER JOIN Main main " +
                    "ON main.ProjectID = projects.ProjectID " +
                    "INNER JOIN Employees " +
                    "ON main.EmployeeID = Employees.EmployeeID " +
                    "WHERE main.Date >= @FromDate AND main.Date <= @ToDate " +
                    "AND Employees.Department IN (" + departments + ") " +
                    ") " +
                    "SELECT SuperProj, ProjectID, Year, Month, ROUND(SUM(Hours),0) AS Total " +
                    "FROM CTE " +
                    "GROUP BY SuperProj, ProjectID, Year, Month " +
                    "ORDER BY SuperProj, ProjectID, Year, Month ";
            } 

            List<AggrReport> result = new List<AggrReport>();

            using (conn = new SqlConnection(LocalConnectionStr))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Closed)
                        return result;
                }
                catch (SqlException)
                {
                    System.Windows.Forms.MessageBox.Show("Exception opening SQL-server using:\n\r" + LocalPublicConnectionStr);
                    return result;
                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    cmd.Parameters.AddWithValue("@FromDate", fromDate);
                    cmd.Parameters.AddWithValue("@ToDate", toDate);
                    try
                    {
                        SqlDataReader rdr = cmd.ExecuteReader();
                        int no = 0;
                        while (rdr.Read())
                        {
                            result.Add(new AggrReport(rdr.GetInt32(1),          // ProjectID - skipping SuperProject...
                                                      rdr.GetInt32(2),          // Year
                                                      rdr.GetInt32(3),          // Month
                                              (float)rdr.GetDouble(4)));        // Accumulated Hours
                            no++;
                        }
                        rdr.Close();
                    }
                    catch (SqlException e)
                    {
                        System.Windows.Forms.MessageBox.Show(e.ToString() + "\n\rStacktrace:\n\r" + e.StackTrace, "Could not SELECT Aggregated Project list");
                    }

                    return result;
                }
            }
        }

        /// <summary>
        /// Get data per Emploryee, Per project - accumulated on a monthly basis
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="showSicknessVacation"></param>
        /// <returns></returns>
        public static List<AggrProjectReport> getAmortizedEmployees(DateTime fromDate, DateTime toDate)
        {

            string sql = "SELECT Employees.FullName, Employees.Department, projects.ProjectID, Year(main.Date) AS Year, Month(main.Date) AS Month, ROUND(SUM(main.Hours),0) as Total " +
                         "FROM PROJECTS projects " +
                         "INNER JOIN Main main " +
                         "ON main.ProjectID = projects.ProjectID " +
                         "INNER JOIN Employees " +
                         "ON main.EmployeeID = Employees.EmployeeID " +
                         "WHERE main.Date >= @FromDate AND main.Date <= @ToDate " +
                         "GROUP BY Employees.FullName, Employees.Department, projects.ProjectID, Year(main.Date), Month(main.Date)" +
                         "ORDER BY Employees.FullName, Employees.Department, projects.ProjectID, Year(main.Date), Month(main.Date)";
            
            List<AggrProjectReport> result = new List<AggrProjectReport>();

            using (conn = new SqlConnection(LocalConnectionStr))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Closed)
                        return result;
                }
                catch (SqlException)
                {
                    System.Windows.Forms.MessageBox.Show("Exception opening SQL-server using:\n\r" + LocalPublicConnectionStr);
                    return result;
                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    cmd.Parameters.AddWithValue("@FromDate", fromDate);
                    cmd.Parameters.AddWithValue("@ToDate", toDate);
                    try
                    {
                        SqlDataReader rdr = cmd.ExecuteReader();
                        int no = 0;
                        while (rdr.Read())
                        {
                            result.Add(new AggrProjectReport(rdr.GetString(0),          // Employee
                                                             rdr.GetInt32(1),           // Department
                                                             rdr.GetInt32(2),           // ProjectID
                                                             rdr.GetInt32(3),           // Year
                                                             rdr.GetInt32(4),           // Month
                                                            (float)rdr.GetDouble(5)));  // Accumulated Hours
                            no++;
                        }
                        rdr.Close();
                    }
                    catch (SqlException e)
                    {
                        System.Windows.Forms.MessageBox.Show(e.ToString() + "\n\rStacktrace:\n\r" + e.StackTrace, "Could not SELECT Aggregated Project list");
                    }

                    return result;
                }
            }
        }



        /// <summary>
        /// Query for getting all Tasks - with dates & Comments - for a given Employee
        /// </summary>
        /// <param name="employeeID"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public static List<PersonalReport> getPersonalReport(int employeeID, DateTime fromDate, DateTime toDate)
        {
            string sql = "SELECT main.Date, projects.Name AS ProjectName, projects.ProjectNo, main.Hours, main.Comment " +
                         "FROM main " +
                         "LEFT JOIN Projects projects "+
                         "ON projects.ProjectID = main.ProjectID "+
                         "WHERE main.EmployeeID = @EmployeeID AND " +
                         "main.Date >= @FromDate AND main.Date <= @ToDate " +
                         "ORDER BY main.Date, projects.Name";

            List<PersonalReport> result = new List<PersonalReport>();

            using (conn = new SqlConnection(LocalConnectionStr))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Closed)
                        return result;
                }
                catch (SqlException)
                {
                    System.Windows.Forms.MessageBox.Show("Exception opening SQL-server using:\n\r" + LocalPublicConnectionStr);
                    return result;
                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                    cmd.Parameters.AddWithValue("@FromDate", fromDate);
                    cmd.Parameters.AddWithValue("@ToDate", toDate);
                    try
                    {
                        SqlDataReader rdr = cmd.ExecuteReader();
                        int no = 0;
                        while (rdr.Read())
                        {
                            result.Add(new PersonalReport(rdr.GetDateTime(0),
                                                          rdr.GetString(1),
                                                          rdr.IsDBNull(2) ? "" : rdr.GetString(2),
                                                          rdr.GetFloat(3),
                                                          rdr.IsDBNull(4) ? "" : rdr.GetString(4)));
                            no++;
                        }
                        rdr.Close();
                    }
                    catch (SqlException e)
                    {
                        System.Windows.Forms.MessageBox.Show(e.ToString() + "\n\rStacktrace:\n\r" + e.StackTrace, "Could not SELECT Personal list");
                    }
                    return result;
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public static List<PersonWorkdays> getPersonWorkDays(DateTime fromDate, DateTime toDate, string departments)
        {
            string sql;
            if (departments == null)
            {
                sql =
                    "SELECT Count(DISTINCT Date) AS Count, Employees.FullName, Year(Date) As YearNo, Month(Date) As MonthNo " +
                    "FROM Main " +
                    "JOIN Employees " +
                    "ON Main.EmployeeID  = Employees.EmployeeID " +
                    "WHERE DATEPART(WEEKDAY, Date) > 1 AND DATEPART(WEEKDAY, Date) < 7 " +
                    "AND Date >= @FromDate AND Date <= @ToDate " +
                    "GROUP BY Year(Date), Month(Date),Fullname " +
                    "ORDER BY Year(Date), Month(Date),Fullname";
            }
            else
            {
                sql =
                    "SELECT Count(DISTINCT Date) AS Count, Employees.FullName, Year(Date) As YearNo, Month(Date) As MonthNo " +
                    "FROM Main " +
                    "JOIN Employees " +
                    "ON Main.EmployeeID  = Employees.EmployeeID " +
                    "WHERE Employees.Department IN (" + departments + ") " +
                    "AND DATEPART(WEEKDAY, Date) > 1 AND DATEPART(WEEKDAY, Date) < 7 " +
                    "AND Date >= @FromDate AND Date <= @ToDate " +
                    "GROUP BY Year(Date), Month(Date),Fullname " +
                    "ORDER BY Year(Date), Month(Date),Fullname";
            }

            List<PersonWorkdays> result = new List<PersonWorkdays>();

            using (conn = new SqlConnection(LocalConnectionStr))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Closed)
                        return result;
                }
                catch (SqlException)
                {
                    System.Windows.Forms.MessageBox.Show("Exception opening SQL-server using:\n\r" + LocalPublicConnectionStr);
                    return result;
                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    cmd.Parameters.AddWithValue("@FromDate", fromDate);
                    cmd.Parameters.AddWithValue("@ToDate", toDate);
                    try
                    {
                        SqlDataReader rdr = cmd.ExecuteReader();
                        int no = 0;
                        while (rdr.Read())
                        {
                            result.Add(new PersonWorkdays(rdr.GetString(1),     // Employee      
                                                      rdr.GetInt32(0),          // Count
                                                      rdr.GetInt32(2),          // Year
                                                      rdr.GetInt32(3)));          // Month
                            no++;
                        }
                        rdr.Close();
                    }
                    catch (SqlException e)
                    {
                        System.Windows.Forms.MessageBox.Show(e.ToString() + "\n\rStacktrace:\n\r" + e.StackTrace, "Could not SELECT Person Workdays list");
                    }

                    return result;
                }
            }
        }



        /// <summary>
        /// To have a dropdown listbox to select Department, we need relevant choices
        /// This is how we get them
        /// </summary>
        /// <returns></returns>
        public static List<int> getDepartments()
        {
            string sql = "SELECT DISTINCT Department FROM Employees";

            List<int> result = new List<int>();

            using (conn = new SqlConnection(LocalConnectionStr))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Closed)
                        return result;
                }
                catch (SqlException)
                {
                    System.Windows.Forms.MessageBox.Show("Exception opening SQL-server using:\n\r" + LocalPublicConnectionStr);
                    return result;
                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    try
                    {
                        SqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            result.Add(rdr.GetInt32(0));
                        }
                        rdr.Close();
                    }
                    catch (SqlException e)
                    {
                        System.Windows.Forms.MessageBox.Show(e.ToString() + "\n\rStacktrace:\n\r" + e.StackTrace, "Could not SELECT Departments");
                    }
                    return result;
                }
            }
        }
        #endregion "SQLReports"

        #region SQLExports
        /// <summary>
        /// Runs through all entries in the database and returns the ones eligible for export
        /// This is no longer 004 projects - only 003 projects
        /// 20170628: Now also Monica EU project 0060010511
        /// Already exported entries will have an "inOracle" entry and are not included in the list returned
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public static List<ExportItem> getExportable(DateTime fromDate, DateTime toDate)
        {
            string sql = "SELECT Main.Date, Projects.Name, Projects.ProjectNo, Employees.FullName, Main.Hours, " +
                         "Main.Comment, Employees.Number, Main.mainID " +
                         "FROM Main " +
                         "LEFT JOIN Employees  " +
                         "ON Employees.EmployeeID = main.EmployeeID " +
                         "LEFT JOIN Projects " +
                         "ON Projects.ProjectID = Main.ProjectID " +
                         "WHERE Main.InOracle IS NULL AND " +
                         "Main.Date >= @FromDate AND Main.Date <= @ToDate " +
                         "AND ((Projects.ProjectNo LIKE '003_______' ) OR Projects.ProjectNo = '0060010511' OR Projects.ProjectNo = '0060010512')" +
                         "ORDER BY Main.Date";

            List<ExportItem> result = new List<ExportItem>();

            using (conn = new SqlConnection(LocalConnectionStr))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Closed)
                        return result;
                }
                catch (SqlException)
                {
                    System.Windows.Forms.MessageBox.Show("Exception opening SQL-server using:\n\r" + LocalPublicConnectionStr);
                    return result;
                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@FromDate", fromDate);
                    cmd.Parameters.AddWithValue("@ToDate", toDate);
                    try
                    {
                        SqlDataReader rdr = cmd.ExecuteReader();
                        int no = 0;
                        while (rdr.Read())
                        {
                            //public ExportItem(DateTime date, string projectName, string projectNo, string employeeName, float hours, string comment, int employeeNo, int mainID)
                            result.Add(new ExportItem(rdr.GetDateTime(0),                             // Date of entry
                                                        rdr.GetString(1),                               // Project Name (child or parent makes no matter!)
                                                        rdr.GetString(2),                               // ProjectNo (not ProjectID)
                                                        rdr.GetString(3),                               // EmployeeName
                                                        rdr.GetFloat(4),                                // Hours
                                                        rdr.GetString(5),                               // Comment
                                                        rdr.GetInt32(6),                                // EmployeeNo
                                                        rdr.GetInt32(7)));                              // MainID                   
                            no++;
                        }
                        rdr.Close();
                    }
                    catch (SqlException e)
                    {
                        System.Windows.Forms.MessageBox.Show(e.ToString() + "\n\rStacktrace:\n\r" + e.StackTrace, "Could not SELECT data for Oracle");
                    }
                    return result;
                }
            }
        }

        #endregion "SQLExports
    }
}
