﻿using HealthCare_System.Core.Notifications.Model;
using HealthCare_System.Core.Notifications.Repository;
using HealthCare_System.Core.Users.Model;
using System.Collections.Generic;

namespace HealthCare_System.Core.Notifications
{
    public interface IDrugNotificationService
    {
        DrugNotificationRepo DrugNotificationRepo { get; }

        void CheckNotifications(List<DrugNotification> notifications, int minutesBeforeShowing);
        List<DrugNotification> CreateNotifications(Patient patient);
        List<DrugNotification> DrugNotifications();
    }
}