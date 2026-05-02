using Bookkeeping.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bookkeeping.Utility
{
    public static class Extension
    {
        private static System.Threading.Timer timer;
        private static Form mainForm;
        private static object action;
        private static object number;
        public static void DebounceTime<T>(this Form form, Action<T> callback, T t, int delay)
        {
            number = t;
            action = callback;
            mainForm = form;
            TimerCallback doSomething = new TimerCallback(DoSomething);
            if (timer == null)
            {
                timer = new System.Threading.Timer(doSomething, t, delay, -1);
            }
            timer.Change(delay, -1);
        }
        public static void DebounceTime(this Form form, Action callback, int delay)
        {
            action = callback;
            mainForm = form;
            TimerCallback doSomething = new TimerCallback(DoSomething);
            if (timer == null)
            {
                timer = new System.Threading.Timer(doSomething, null, delay, -1);
            }
            timer.Change(delay, -1);
        }
        private static void DoSomething(object t)
        {
            mainForm.Invoke(new Action(() =>
            {
                action.GetType().GetMethod("Invoke").Invoke(action, null);
            }));
        }

    }
}
