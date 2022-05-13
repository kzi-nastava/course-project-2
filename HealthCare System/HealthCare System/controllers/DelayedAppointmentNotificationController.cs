﻿using HealthCare_System.entities;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text.Json;

namespace HealthCare_System.controllers
{
    class DelayedAppointmentNotificationController
    {
        List<DelayedAppointmentNotification> delayedAppointmentNotifications;
        string path;
        string appointmentLinkerPath = "../../../data/links/DelayedAppointmentNotificationLinker.csv";

        public DelayedAppointmentNotificationController()
        {
            path = "../../../data/entities/Drugs.json";
            Load();
        }

        public DelayedAppointmentNotificationController(string path)
        {
            this.path = path;
            Load();
        }

        internal List<DelayedAppointmentNotification> DelayedAppointmentNotifications 
        { get => delayedAppointmentNotifications; set => delayedAppointmentNotifications = value; }

        public string Path { get => path; set => path = value; }

        void Load()
        {
            delayedAppointmentNotifications = JsonSerializer.
                Deserialize<List<DelayedAppointmentNotification>>(File.ReadAllText(path));
        }

        public DelayedAppointmentNotification FindById(int id)
        {
            foreach (DelayedAppointmentNotification delayedAppointmentNotification in delayedAppointmentNotifications)
                if (delayedAppointmentNotification.Id == id)
                    return delayedAppointmentNotification;
            return null;
        }

        public DelayedAppointmentNotification add(Appointment appointment, string text)
        {
            DelayedAppointmentNotification newNotification = new DelayedAppointmentNotification(GenerateId(), text, appointment);
            delayedAppointmentNotifications.Add(newNotification);
            return newNotification;
        }

        public int GenerateId()
        {
            return delayedAppointmentNotifications[^1].Id + 1;
        }

        private void RewriteAppointmentLinker()
        {
            string csv = "";
            foreach (DelayedAppointmentNotification notification in delayedAppointmentNotifications)
            {
                if (notification.Appointment is not null)
                {
                    csv += notification.Id + ";" + notification.Appointment.Id + "\n";
                }
            }
            File.WriteAllText(appointmentLinkerPath, csv);
        }

        public void Serialize()
        {
            string delayedAppointmentNotificationsJson = JsonSerializer.Serialize(delayedAppointmentNotifications,
                new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, delayedAppointmentNotificationsJson);

            RewriteAppointmentLinker();
        }
    }
}
