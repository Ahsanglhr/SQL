using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Win32;              // For Registry

namespace TimeReg
{
    internal class Model
    {
        List<DBUtil.Week> localList = null;

        public string Me { get { return me; } }
        public string FullName { get { return myFullName; } }
        public int MyID { get { return myID; } }
        public DateTime CurrentMonday { get { return m_monday; } set { m_monday = value; } }
        public bool SuperUser { get { return superUser; } }
        public bool StartupSuper { get { return startupSuper; } }
        public bool DebugDB { get; set; }
        public bool Dirty { get; set; }
        public Dictionary<string, string> Categories { get { return m_categories; } }

        private static Model theOnlyOne = null;
        private string  me = "";
        private string myFullName = "";
        private int     myID = 0;
        private bool superUser = false;
        private bool startupSuper = false;
        //private DataSet mainset;
        private DateTime m_monday;

        private string regKeyName = @"Software\Bruel and Kjaer\TimeReg\Settings\";
        private string regCatalog = @"Catalog";    
        private string regHolidays = "Holidays";
        private Dictionary<string, string> m_categories;

        private Dictionary<string, string> m_rolesDict = null;

        public  bool iMayDoThis(string role)
        {
            string myRoles;
            if (m_rolesDict != null && m_rolesDict.TryGetValue(me, out myRoles))
            {
                myRoles = myRoles.ToLower();
                if (myRoles.Contains(role.ToLower()))
                    return true;
            }

            return false;
        }

        private void setAccessRights(Dictionary<string, string> dict)
        {
            m_rolesDict = dict;
        }

        public string Catalog
        {
            get
            {
                RegistryKey regKey = Registry.CurrentUser.CreateSubKey(regKeyName);
                string catalog = regKey.GetValue(regCatalog, "DK").ToString();
                if (catalog.ToLower().Equals("timereg"))  // Used TimeReg at first
                    catalog = "DK";
                return catalog;
            }

            set 
            { 
                RegistryKey regKey = Registry.CurrentUser.CreateSubKey(regKeyName);
                regKey.SetValue(regCatalog, value);
            }
        }

        public string Holidays
        {
            get
            {
                RegistryKey regKey = Registry.CurrentUser.CreateSubKey(regKeyName);
                string holidays = regKey.GetValue(regHolidays, "DK").ToString();
                return holidays;
            }

            set
            {
                RegistryKey regKey = Registry.CurrentUser.CreateSubKey(regKeyName);
                regKey.SetValue(regHolidays, value);
            }
        }

        /// <summary>
        /// We use three letter codes for the various categories
        /// </summary>
        private void setupCategories()
        {
            m_categories = new Dictionary<string, string>();
            m_categories.Add("DPT", "DPT - Department");
            m_categories.Add("MAN", "MAN - R&D Management");
            m_categories.Add("NON", "NON - NOT Defined");
            m_categories.Add("NPI", "NPI - New Product");
            m_categories.Add("OPS", "OPS - Operations");
            m_categories.Add("PAT", "PAT - Patents");
            m_categories.Add("PLC", "PLC - Maintenance");
            m_categories.Add("PRE", "PRE - Presales");
            m_categories.Add("PSO", "PSO - aka PSE");
            m_categories.Add("PST", "PST - Post Sales");
            m_categories.Add("OUT", "OUT - Non Work");
            m_categories.Add("SPI", "SPI - Small Projects");
            m_categories.Add("STA", "STA - Staff Function");
            m_categories.Add("STD", "STD - Standardization");
            m_categories.Add("TEC", "TEC - Technology");
        }

        // It's the easiest to let the view read directly from this - but don't allow writes
        public readonly Dictionary<DateTime, string> holidayDict = new Dictionary<DateTime,string>();
        private void setupDKHolidays()
        {
            holidayDict.Clear();
            holidayDict.Add(new DateTime(2014, 1, 1), "Nytårsdag");
            holidayDict.Add(new DateTime(2014, 4,17), "Skærtorsdag");
            holidayDict.Add(new DateTime(2014, 4,18), "Langfredag");
            holidayDict.Add(new DateTime(2014, 4,20), "1.Påskedag");
            holidayDict.Add(new DateTime(2014, 4,21), "2.Påskedag");
            holidayDict.Add(new DateTime(2014, 5, 1), "1.Maj");
            holidayDict.Add(new DateTime(2014, 5,16), "St.Bededag");
            holidayDict.Add(new DateTime(2014, 5,29), "Kr.Himmelfart");
            holidayDict.Add(new DateTime(2014, 5,30), "Feriefridag");
            holidayDict.Add(new DateTime(2014, 6, 5), "Grundlovsdag");
            holidayDict.Add(new DateTime(2014, 6, 8), "1.Pinsedag");
            holidayDict.Add(new DateTime(2014, 6, 9), "2.Pinsedag");
            holidayDict.Add(new DateTime(2014,12,24), "Juleaften");
            holidayDict.Add(new DateTime(2014,12,25), "1.Juledag");
            holidayDict.Add(new DateTime(2014,12,26), "2.Juledag");
            holidayDict.Add(new DateTime(2014,12,31), "Nytårsaften");

            holidayDict.Add(new DateTime(2015, 1, 1), "Nytårsdag");
            holidayDict.Add(new DateTime(2015, 4, 2), "Skærtorsdag");
            holidayDict.Add(new DateTime(2015, 4, 3), "Langfredag");
            holidayDict.Add(new DateTime(2015, 4, 5), "1.Påskedag");
            holidayDict.Add(new DateTime(2015, 4, 6), "2.Påskedag");
            holidayDict.Add(new DateTime(2015, 5, 1), "St.Bededag");
            holidayDict.Add(new DateTime(2015, 5,14), "Kr.Himmelfart");
            holidayDict.Add(new DateTime(2015, 5,24), "1.Pinsedag");
            holidayDict.Add(new DateTime(2015, 5,25), "2.Pinsedag");
            holidayDict.Add(new DateTime(2015, 6, 5), "Grundlovsdag");
            holidayDict.Add(new DateTime(2015,12,24), "Juleaften");
            holidayDict.Add(new DateTime(2015,12,25), "1.Juledag");
            holidayDict.Add(new DateTime(2015,12,26), "2.Juledag");
            holidayDict.Add(new DateTime(2015,12,31), "Nytårsaften");

            holidayDict.Add(new DateTime(2016, 1, 1), "Nytårsdag");
            holidayDict.Add(new DateTime(2016, 3,24), "Skærtorsdag");
            holidayDict.Add(new DateTime(2016, 3,25), "Langfredag");
            holidayDict.Add(new DateTime(2016, 3,27), "1.Påskedag");
            holidayDict.Add(new DateTime(2016, 3,28), "2.Påskedag");
            holidayDict.Add(new DateTime(2016, 4,22), "St.Bededag");
            holidayDict.Add(new DateTime(2016, 5, 1), "1.Maj");
            holidayDict.Add(new DateTime(2016, 5, 5), "Kr.Himmelfart");
            holidayDict.Add(new DateTime(2016, 5,15), "1.Pinsedag");
            holidayDict.Add(new DateTime(2016, 5,16), "2.Pinsedag");
            holidayDict.Add(new DateTime(2016,12,24), "Juleaften");
            holidayDict.Add(new DateTime(2016,12,25), "1.Juledag");
            holidayDict.Add(new DateTime(2016,12,26), "2.Juledag");
            holidayDict.Add(new DateTime(2016,12,31), "Nytårsaften");

            holidayDict.Add(new DateTime(2017, 1, 1), "Nytårsdag");
            holidayDict.Add(new DateTime(2017, 4, 9), "Palmesøndag");
            holidayDict.Add(new DateTime(2017, 4, 13), "Skærtorsdag");
            holidayDict.Add(new DateTime(2017, 4, 14), "Langfredag");
            holidayDict.Add(new DateTime(2017, 4, 16), "Påskedag");
            holidayDict.Add(new DateTime(2017, 4, 17), "2. Påskedag");
            holidayDict.Add(new DateTime(2017, 5, 12), "St. Bededag");
            holidayDict.Add(new DateTime(2017, 5, 25), "Kr. Himmelfart");
            holidayDict.Add(new DateTime(2017, 6, 4), "Pinsedag");
            holidayDict.Add(new DateTime(2017, 6, 5), "2. Pinsedag");
            holidayDict.Add(new DateTime(2017, 12, 24), "Juleaften");
            holidayDict.Add(new DateTime(2017, 12, 25), "1.Juledag");
            holidayDict.Add(new DateTime(2017, 12, 26), "2.Juledag");
            holidayDict.Add(new DateTime(2017, 12, 31), "Nytårsaften");

            holidayDict.Add(new DateTime(2018, 1, 1), "Nytårsdag");
            holidayDict.Add(new DateTime(2018, 3, 25), "Palmesøndag");
            holidayDict.Add(new DateTime(2018, 3, 29), "Skærtorsdag");
            holidayDict.Add(new DateTime(2018, 3, 30), "Langfredag");
            holidayDict.Add(new DateTime(2018, 4, 1), "Påskedag");
            holidayDict.Add(new DateTime(2018, 4, 2), "2. Påskedag");
            holidayDict.Add(new DateTime(2018, 4, 27), "St. Bededag");
            holidayDict.Add(new DateTime(2018, 5, 10), "Kr. Himmelfart");
            holidayDict.Add(new DateTime(2018, 5, 20), "Pinsedag");
            holidayDict.Add(new DateTime(2018, 5, 21), "2. Pinsedag");
            holidayDict.Add(new DateTime(2018, 12, 24), "Juleaften");
            holidayDict.Add(new DateTime(2018, 12, 25), "1.Juledag");
            holidayDict.Add(new DateTime(2018, 12, 26), "2.Juledag");
            holidayDict.Add(new DateTime(2018, 12, 31), "Nytårsaften");

            holidayDict.Add(new DateTime(2019, 1, 1), "Nytårsdag");
            holidayDict.Add(new DateTime(2019, 4, 14), "Palmesøndag");
            holidayDict.Add(new DateTime(2019, 4, 18), "Skærtorsdag");
            holidayDict.Add(new DateTime(2019, 4, 19), "Langfredag");
            holidayDict.Add(new DateTime(2019, 4, 21), "Påskedag");
            holidayDict.Add(new DateTime(2019, 4, 22), "2. Påskedag");
            holidayDict.Add(new DateTime(2019, 5, 17), "St. Bededag");
            holidayDict.Add(new DateTime(2019, 5, 30), "Kr. Himmelfart");
            holidayDict.Add(new DateTime(2019, 6, 9), "Pinsedag");
            holidayDict.Add(new DateTime(2019, 6, 10), "2. Pinsedag");
            holidayDict.Add(new DateTime(2019, 12, 24), "Juleaften");
            holidayDict.Add(new DateTime(2019, 12, 25), "1.Juledag");
            holidayDict.Add(new DateTime(2019, 12, 26), "2.Juledag");
            holidayDict.Add(new DateTime(2019, 12, 31), "Nytårsaften");
        }

        private void setupUKHolidays()
        {
            holidayDict.Clear();
            holidayDict.Add(new DateTime(2015, 1, 1), "New Years Day");
            holidayDict.Add(new DateTime(2015, 4, 3), "Good Friday");
            holidayDict.Add(new DateTime(2015, 4 ,6), "Easter Monday");
            holidayDict.Add(new DateTime(2015, 5, 4), "Early May BH"); 
            holidayDict.Add(new DateTime(2015, 5,25), "Spring BH");
            holidayDict.Add(new DateTime(2015, 8,31), "Summer BH");
            holidayDict.Add(new DateTime(2015,12,25), "Christmas Day");
            holidayDict.Add(new DateTime(2015,12,26), "Boxing Day");

            holidayDict.Add(new DateTime(2016, 1, 1), "New Years Day");
            holidayDict.Add(new DateTime(2016, 3,25), "Good Friday");
            holidayDict.Add(new DateTime(2016, 3,28), "Easter Monday");
            holidayDict.Add(new DateTime(2016, 5, 2), "Early May BH"); 
            holidayDict.Add(new DateTime(2016, 5,30), "Spring BH");
            holidayDict.Add(new DateTime(2016, 8,29), "Summer BH");
            holidayDict.Add(new DateTime(2016,12,25), "Christmas Day");
            holidayDict.Add(new DateTime(2016,12,26), "Boxing Day");

            holidayDict.Add(new DateTime(2017, 1, 1), "New Years Day");
            holidayDict.Add(new DateTime(2017, 1, 2), "N.Y. Observed");
            holidayDict.Add(new DateTime(2017, 4, 14), "Good Friday");
            holidayDict.Add(new DateTime(2017, 4, 17), "Easter Monday");
            holidayDict.Add(new DateTime(2017, 5, 1), "Early May BH");
            holidayDict.Add(new DateTime(2017, 5, 29), "Spring BH");
            holidayDict.Add(new DateTime(2017, 8, 28), "Summer BH");
            holidayDict.Add(new DateTime(2017, 12, 25), "Christmas Day");
            holidayDict.Add(new DateTime(2017, 12, 26), "Boxing Day");

            holidayDict.Add(new DateTime(2018, 1, 1),  "New Years Day");
            holidayDict.Add(new DateTime(2018, 3, 30), "Good Friday");
            holidayDict.Add(new DateTime(2018, 4, 2),  "Easter Monday");
            holidayDict.Add(new DateTime(2018, 5, 7),  "Early May BH");
            holidayDict.Add(new DateTime(2018, 5, 28), "Spring BH");
            holidayDict.Add(new DateTime(2018, 8, 27), "Summer BH");
            holidayDict.Add(new DateTime(2018, 12, 25),"Christmas Day");
            holidayDict.Add(new DateTime(2018, 12, 26),"Boxing Day");

            holidayDict.Add(new DateTime(2019, 1, 1), "New Years Day");
            holidayDict.Add(new DateTime(2019, 4, 19), "Good Friday");
            holidayDict.Add(new DateTime(2019, 4, 22), "Easter Monday");
            holidayDict.Add(new DateTime(2019, 5, 6), "Early May BH");
            holidayDict.Add(new DateTime(2019, 5, 27), "Spring BH");
            holidayDict.Add(new DateTime(2019, 8, 26), "Summer BH");
            holidayDict.Add(new DateTime(2019, 12, 25), "Christmas Day");
            holidayDict.Add(new DateTime(2019, 12, 26), "Boxing Day");
        }

        private void setupUSHolidays()
        {
            holidayDict.Clear();

            holidayDict.Add(new DateTime(2016, 1, 1),   "New Years Day");
            holidayDict.Add(new DateTime(2016, 1, 18),  "Martin LK Day");
            holidayDict.Add(new DateTime(2016, 2, 15),  "President's Day");
            holidayDict.Add(new DateTime(2016, 5, 30),  "Memorial Day");
            holidayDict.Add(new DateTime(2016, 7, 4),   "Indep. Day");
            holidayDict.Add(new DateTime(2016, 9, 5),   "Labor Day");
            holidayDict.Add(new DateTime(2016, 11, 24), "Thanksgiving");
            holidayDict.Add(new DateTime(2016, 11, 25), "Black Friday");
            holidayDict.Add(new DateTime(2016, 12, 23), "Christmas Eve");
            holidayDict.Add(new DateTime(2016, 12, 26), "Christmas Day");

            holidayDict.Add(new DateTime(2017, 1, 1),   "New Years Day");
            holidayDict.Add(new DateTime(2017, 1, 2),   "N.Y. Observed");
            holidayDict.Add(new DateTime(2017, 1, 16),  "Martin LK Day");
            holidayDict.Add(new DateTime(2017, 2, 20),  "President's Day");
            holidayDict.Add(new DateTime(2017, 5, 29),  "Memorial Day");
            holidayDict.Add(new DateTime(2017, 7, 4),   "Indep. Day");
            holidayDict.Add(new DateTime(2017, 9, 4),   "Labor Day");
            holidayDict.Add(new DateTime(2017, 11, 10), "Veterans Day ob");
            holidayDict.Add(new DateTime(2017, 11, 23), "Thanksgiving");
            holidayDict.Add(new DateTime(2017, 11, 24), "Black Friday");
            holidayDict.Add(new DateTime(2017, 12, 24), "Christmas Eve");
            holidayDict.Add(new DateTime(2017, 12, 25), "Christmas Day");

            holidayDict.Add(new DateTime(2018, 1, 1), "New Years Day");
            holidayDict.Add(new DateTime(2018, 1, 15), "Martin LK Day");
            holidayDict.Add(new DateTime(2018, 2, 19), "President's Day");
            holidayDict.Add(new DateTime(2018, 5, 28), "Memorial Day");
            holidayDict.Add(new DateTime(2018, 7, 4), "Indep. Day");
            holidayDict.Add(new DateTime(2018, 9, 3), "Labor Day");
            holidayDict.Add(new DateTime(2018, 11, 12), "Veterans Day ob");
            holidayDict.Add(new DateTime(2018, 11, 22), "Thanksgiving");
            holidayDict.Add(new DateTime(2018, 11, 23), "Black Friday");
            holidayDict.Add(new DateTime(2018, 12, 24), "Christmas Eve");
            holidayDict.Add(new DateTime(2018, 12, 25), "Christmas Day");

            holidayDict.Add(new DateTime(2019, 1, 1), "New Years Day");
            holidayDict.Add(new DateTime(2019, 1, 21), "Martin LK Day");
            holidayDict.Add(new DateTime(2019, 2, 18), "President's Day");
            holidayDict.Add(new DateTime(2019, 5, 27), "Memorial Day");
            holidayDict.Add(new DateTime(2019, 7, 4), "Indep. Day");
            holidayDict.Add(new DateTime(2019, 9, 2), "Labor Day");
            holidayDict.Add(new DateTime(2019, 11, 11), "Veterans Day");
            holidayDict.Add(new DateTime(2019, 11, 28), "Thanksgiving");
            holidayDict.Add(new DateTime(2019, 11, 29), "Black Friday");
            holidayDict.Add(new DateTime(2019, 12, 24), "Christmas Eve");
            holidayDict.Add(new DateTime(2019, 12, 25), "Christmas Day");
        }

        private void setupDEHolidays()
        {
            holidayDict.Clear();

            holidayDict.Add(new DateTime(2016, 10, 3), "German Unity");            
            holidayDict.Add(new DateTime(2016, 12, 25), "Christmas Day");
            holidayDict.Add(new DateTime(2016, 12, 26), "Boxing Day");


            holidayDict.Add(new DateTime(2017, 1, 1), "New Years Day");
            holidayDict.Add(new DateTime(2017, 4, 14), "Good Friday");
            holidayDict.Add(new DateTime(2017, 4, 17), "Easter Monday");
            holidayDict.Add(new DateTime(2017, 5, 1), "May Day");
            holidayDict.Add(new DateTime(2017, 5, 25), "Ascension Day");
            holidayDict.Add(new DateTime(2017, 6, 5), "Whit Monday");
            holidayDict.Add(new DateTime(2017, 10, 3), "German Unity");
            holidayDict.Add(new DateTime(2017, 12, 25), "Christmas Day");
            holidayDict.Add(new DateTime(2017, 12, 26), "Boxing Day");

            holidayDict.Add(new DateTime(2018, 1, 1), "New Years Day");
            holidayDict.Add(new DateTime(2018, 3, 30), "Good Friday");
            holidayDict.Add(new DateTime(2018, 4, 2), "Easter Monday");
            holidayDict.Add(new DateTime(2018, 5, 1), "May Day");
            holidayDict.Add(new DateTime(2018, 5, 10), "Ascension Day");
            holidayDict.Add(new DateTime(2018, 5, 21), "Whit Monday");
            holidayDict.Add(new DateTime(2018, 10, 3), "German Unity");
            holidayDict.Add(new DateTime(2018, 12, 25), "Christmas Day");
            holidayDict.Add(new DateTime(2018, 12, 26), "Boxing Day");

            holidayDict.Add(new DateTime(2019, 1, 1), "New Years Day");
            holidayDict.Add(new DateTime(2019, 4, 19), "Good Friday");
            holidayDict.Add(new DateTime(2019, 4, 22), "Easter Monday");
            holidayDict.Add(new DateTime(2019, 5, 1), "May Day");
            holidayDict.Add(new DateTime(2019, 5, 30), "Ascension Day");
            holidayDict.Add(new DateTime(2019, 6, 20), "Whit Monday");
            holidayDict.Add(new DateTime(2019, 10, 3), "German Unity");
            holidayDict.Add(new DateTime(2019, 12, 25), "Christmas Day");
            holidayDict.Add(new DateTime(2019, 12, 26), "Boxing Day");
        }

        /// <summary>
        /// Find the monday in this week (not really a DB operation)
        /// </summary>
        /// <returns></returns>
        public static DateTime mondayThisWeek()
        {
            return mondayThisWeek(DateTime.Today);
        }

        /// <summary>
        /// Find monday in the week that contains this day
        /// Here we start weeks on mondays (C# does NOT)
        /// </summary>
        /// <param name="today"></param>
        /// <returns></returns>
        public static DateTime mondayThisWeek(DateTime today)
        {
            int weekday = (int)today.DayOfWeek;     // Sunday is 0 in this system!!

            // Get Monday in this week
            DateTime monday = (weekday == 0) ? today.AddDays(-6) : today.AddDays(1 - weekday);
            return monday;
        }

/*
        /// <summary>
        /// Set up in good time
        /// </summary>
        private void setupDKRegistry()
        {
            // Let this method run in Nærum for a week before introducing more
            RegistryKey regKey = Registry.CurrentUser.CreateSubKey(regKeyName);
            regKey.SetValue(regCatalog, "DK");
            regKey.SetValue(regHollidays, "DK");
        }
*/
        /// <summary>
        /// Private to assure that no-one calls it by mistake
        /// </summary>
        private Model()
        {
        }

        /// <summary>
        /// This is were it all starts. MUST be called before model is used.
        /// This drives whether we are on the debug DB from start or not
        /// </summary>
        private void init(bool debug)
        {
            me = System.Environment.UserName.ToLower();

            DebugDB = debug;

            setupCategories();

            connect();
            startupSuper = superUser;
        }

        /// <summary>
        /// (Re)Connect to the relevant database/Catalog
        /// The Database Connect also refreshes the cached "All Projects"
        /// All other information from the database must be re-fetched
        /// (My Projects, Employees etc)
        /// </summary>
        public void connect()
        {
            string db = (DebugDB) ? "Debug" : Catalog;

            if (!DBUtil.connect(db))
                myID = 0;
            else
                myID = DBUtil.getEmployeeInfo(me, out superUser, out myFullName);

            if (Holidays.Equals("DK"))
                setupDKHolidays();
            else if (Holidays.Equals("UK"))
                setupUKHolidays();
            else if (Holidays.Equals("US"))
                setupUSHolidays();
            else if (Holidays.Equals("DE"))
                setupDEHolidays();

            setAccessRights(DBUtil.getAccessRights());

            Dirty = false;
        }

        /// <summary>
        /// Changes Me and MyID - but not SuperUser
        /// If name is "" - go back to being yourself
        /// </summary>
        /// <param name="name"></param>
        public bool alias(string name)
        {
            if (name == "")
            {
                // Going back
                me = System.Environment.UserName.ToLower();
                myID = DBUtil.getEmployeeInfo(me, out superUser, out myFullName);
                return true;
            }
            else
            {
                int ID = DBUtil.getEmployeeInfo(name, out superUser, out myFullName);
                if (ID == 0)
                    return false;
                else
                {
                    me = name;
                    myID = ID;
                    return true;
                }
            }
        }

        public List<DBUtil.Week> getTableRef()
        {
            return localList;
        }

        /// <summary>
        /// This method takes a monday and retrieves the week with this from the DB
        /// The localTable is filled and the m_monday is set. To be used by Next/Prev.
        /// </summary>
        /// <param name="monday"></param>
        /// <returns></returns>
        public List<DBUtil.Week> selectWeekOf(DateTime monday, out int weekno)
        {
            m_monday = monday;
            weekno = curWeekno();
            localList = DBUtil.createListFromDB(m_monday, myID);
            addMyProjectsList();
            Dirty = false;
            return localList;
        }


        /// <summary>
        /// Based on the m_monday - wich weeknumber is this?
        /// </summary>
        /// <returns></returns>
        private int curWeekno()
        {
            // Interestingly the following does NOT work (plenty of articles on that):
            //System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("da-DK");
            //return ci.Calendar.GetWeekOfYear(m_monday, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            System.Globalization.Calendar cal = System.Globalization.CultureInfo.InvariantCulture.Calendar;
            // Add 3 days !! - to get the right answer...
            return cal.GetWeekOfYear(m_monday.AddDays(3), System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        /// <summary>
        /// The localTable has been changed in the View
        /// Now all cells that had data before are known by their mainID being not -1 --> UPDATE
        /// All new cells have mainID=-1 --> INSERT
        /// </summary>
        public void updateDatabase()
        {
            // Loop through the rows - each is a project
            foreach (DBUtil.Week projectWeek in localList)
            {
                // Loop through the weekdays of the given project
                for (int weekday = 0; weekday < 7; weekday++)
                {
                    if (projectWeek[weekday].Changed)
                    {
                        if (projectWeek[weekday].MainID == -1)
                        {   // We will not make a new entry with 0.0!
                            if (projectWeek[weekday].Hours != 0.0F)
                                // We need to update the localList mainID; if the user makes a new change in the same cell it must be an update next time
                                projectWeek[weekday].MainID = DBUtil.insertNewMainRow(projectWeek[weekday].EmployeeID, projectWeek[weekday].Date, projectWeek[weekday].ProjectID, projectWeek[weekday].Hours, projectWeek[weekday].Comment);
                        }
                        else
                        {
                            if (projectWeek[weekday].Hours != 0.0F)
                                DBUtil.updateExistingMainRow(projectWeek[weekday].MainID, projectWeek[weekday].Hours, projectWeek[weekday].Comment);
                            else
                                // Inserting a "0" hours now leads to a deleted entry
                                DBUtil.deleteMainRow(projectWeek[weekday].MainID);
                        }
                    }
                }
            }
            Dirty = false;
        }

        /// <summary>
        /// The localList contains cells that each represent an existing row in the mainTable in the DB - or a possible one
        /// This makes it easier to write them back later (compared to a JOINed view)
        /// The table from the database is then merged with the "favorite" projects in MyProjects
        /// </summary>
        public void addMyProjectsList()
        {
            DateTime day;
            DBUtil.Week projectWeek = null;
            // We now have a localList with data corresponding to existing projects for this user
            // Now we need to merge in a lot of blanks for the favorites
            // Get the ordered list of project that this user wants to see - also if no data yet
            List<DBUtil.ProjTupple> myprojects = DBUtil.myProjects(myID);

            foreach (DBUtil.ProjTupple myproject in myprojects)
            {
                day = m_monday;
                // Create a weekfull of readonly and "empty" data 
                // If there is no entry for this project we are not using it - but it will help us search anyway
                projectWeek = new DBUtil.Week(myproject.Name); // 7 new days of references to CellData
                for (int weekday = 0; weekday < 7; weekday++)
                {
                    projectWeek[weekday] = new DBUtil.CellData(day.AddDays(weekday), myID, myproject.ProjectID, myproject.ParentID);
                }


                if (localList.IndexOf(projectWeek) == -1)
                    localList.Add(projectWeek);
            }

            // Now we have got all the stuff in the list and we only need the final step
            localList.Sort();
        }

        /// <summary>
        /// The model is instantiated exactly once
        /// </summary>
        /// <returns></returns>
        internal static Model getInstance(bool debug)
        {
            if (theOnlyOne == null)
            {
                theOnlyOne = new Model();
                theOnlyOne.init(debug);
            }
            return theOnlyOne;
        }

    }
}
