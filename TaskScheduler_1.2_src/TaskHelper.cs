using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;

namespace TaskScheduler
{
    public class TaskHelper
    {
        



        //private void CreateSchedulerItem()
        //{
        //    TaskScheduler.TriggerItem triggerItem = new TaskScheduler.TriggerItem();
        //    triggerItem.Tag = textBoxlabelOneTimeOnlyTag.Text;
        //    triggerItem.StartDate = dateTimePickerStartDate.Value;
        //    triggerItem.EndDate = dateTimePickerEndDate.Value;
        //    triggerItem.TriggerTime = dateTimePickerTriggerTime.Value;
        //    triggerItem.OnTrigger += new TaskScheduler.TriggerItem.OnTriggerEventHandler(triggerItem_OnTrigger); // And the trigger-Event :)

        //    // Set OneTimeOnly - Active and Date
        //    triggerItem.TriggerSettings.OneTimeOnly.Active = checkBoxOneTimeOnlyActive.Checked;
        //    triggerItem.TriggerSettings.OneTimeOnly.Date = dateTimePickerOneTimeOnlyDay.Value.Date;

        //    // Set the interval for daily trigger
        //    triggerItem.TriggerSettings.Daily.Interval = (ushort)numericUpDownDaily.Value;

        //    // Set the active days for weekly trigger
        //    for (byte day = 0; day < 7; day++) // Set the active Days
        //        triggerItem.TriggerSettings.Weekly.DaysOfWeek[day] = checkedListBoxWeeklyDays.GetItemChecked(day);

        //    // Set the active months for monthly trigger
        //    for (byte month = 0; month < 12; month++)
        //        triggerItem.TriggerSettings.Monthly.Month[month] = checkedListBoxMonthlyMonths.GetItemChecked(month);

        //    // Set active Days (0..30 = Days, 31=last Day) for monthly trigger
        //    for (byte day = 0; day < 32; day++)
        //        triggerItem.TriggerSettings.Monthly.DaysOfMonth[day] = checkedListBoxMonthlyDays.GetItemChecked(day);

        //    // Set the active weekNumber and DayOfWeek for monthly trigger
        //    // f.e. the first monday, or the last friday...
        //    for (byte weekNumber = 0; weekNumber < 5; weekNumber++) // 0..4: first, second, third, fourth or last week
        //        triggerItem.TriggerSettings.Monthly.WeekDay.WeekNumber[weekNumber] = checkedListBoxMonthlyWeekNumber.GetItemChecked(weekNumber);
        //    for (byte day = 0; day < 7; day++)
        //        triggerItem.TriggerSettings.Monthly.WeekDay.DayOfWeek[day] = checkedListBoxMonthlyWeekDay.GetItemChecked(day);

        //    triggerItem.Enabled = true; // Set the Item-Active - State
        //    _taskScheduler.AddTrigger(triggerItem); // Add the trigger to List
        //    _taskScheduler.Enabled = checkBoxEnabled.Checked; // Start the Scheduler


        //}

        //private void ShowAllTriggerDates()
        //{
        //    if (listViewItems.SelectedItems.Count > 0)
        //    {
        //        TaskScheduler.TriggerItem item = (TaskScheduler.TriggerItem)listViewItems.SelectedItems[0].Tag;
        //        Form form = new Form();
        //        ListView listView = new ListView();
        //        listView.FullRowSelect = true;

        //        form.Text = "Full list for Task: " + item.Tag.ToString();
        //        form.Width = 400;
        //        form.Height = 450;

        //        listView.Parent = form;
        //        listView.Dock = DockStyle.Fill;
        //        listView.View = View.Details;
        //        listView.Columns.Add("Date", 200);

        //        DateTime date = dateTimePickerStartDate.Value.Date;
        //        while (date <= dateTimePickerEndDate.Value.Date)
        //        {
        //            if (item.CheckDate(date)) // probe this date
        //                listView.Items.Add(date.ToLongDateString());
        //            date = date.AddDays(1);
        //        }
        //        form.Show();
        //    }
        //    else
        //        MessageBox.Show("Please select a trigger!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //}

        //private void ExportItemToXML()
        //{
        //    if (listViewItems.SelectedItems.Count > 0)
        //    {
        //        TaskScheduler.TriggerItem item = (TaskScheduler.TriggerItem)listViewItems.SelectedItems[0].Tag;
        //        textBoxEvents.Clear();
        //        textBoxEvents.AppendText(item.ToXML()); // Save the configuration to XML
        //    }
        //    else
        //        MessageBox.Show("Please select a trigger!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //}

        //private void ImportItemFromXML()
        //{
        //    try
        //    {
        //        TaskScheduler.TriggerItem newItem = TaskScheduler.TriggerItem.FromXML(textBoxEvents.Text);
        //        newItem.Enabled = true; // Enable Item here if you like 
        //        newItem.OnTrigger += new TaskScheduler.TriggerItem.OnTriggerEventHandler(triggerItem_OnTrigger); // And the trigger-Event :)
        //        _taskScheduler.AddTrigger(newItem); // Trigger hinzufьgen
        //        _taskScheduler.Enabled = checkBoxEnabled.Checked; // Start the Scheduler

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error: parse XML: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        private string ExportCollectionToXML(TaskScheduler taskSch)
        {
            String xmlString = String.Empty;
            try
            {
                xmlString = taskSch.TriggerItems.ToXML();
            }
            catch (Exception ex)
            {
              
            }
            return xmlString;
        }
        private void triggerItem_OnTrigger(Object sender, TaskScheduler.OnTriggerEventArgs e)
        {
        }

        public  TaskScheduler ImportCollectionFromXML(String xmlString)
        {
            TaskScheduler _taskSchedulerL = new TaskScheduler();
           
          
                TaskScheduler.TriggerItemCollection items = TaskScheduler.TriggerItemCollection.FromXML(xmlString);
                _taskSchedulerL.TriggerItems.AddRange(items, new TaskScheduler.TriggerItem.OnTriggerEventHandler(triggerItem_OnTrigger));
                //_taskSchedulerL.Enabled = true; // Start the Scheduler
          
           
            return _taskSchedulerL;
        }

    //    private void ImportCollectionFromXML()
    //    {
    //        ImportCollectionFromXML(textBoxEvents.Text);
    //    }

    //    private String GetServiceConfigFileName()
    //    {
    //        String commonAppData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
    //        String configDirectory = commonAppData + Path.DirectorySeparatorChar + "TaskScheduler";
    //        return configDirectory + Path.DirectorySeparatorChar + "SchedulerItems.xml";
    //    }

        public TaskScheduler ReadServiceConfig(String path)
        {
             String xmlString = String.Empty;
             xmlString = System.IO.File.ReadAllText(path);
             return ImportCollectionFromXML(xmlString);
         
        }

        public int SaveAsServiceConfig(TaskScheduler TaskSch, String Path)
        {
            if (TaskSch.TriggerItems.Count == 0)
            {
              
                return 1;
            }

            String xmlString = ExportCollectionToXML(TaskSch);
                  
           
            using (StreamWriter outfile = new StreamWriter(Path))
            {
                try
                {

                    outfile.Write(xmlString);
                    outfile.Flush();
                    outfile.Close();

                    return 0;
                }
                catch (Exception ex)
                {
                    return 2;
                }
            }
        }
    }
}
