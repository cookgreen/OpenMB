using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Game
{
	public enum Time
	{
		[Description("Early Morning")]
		Early_Morning,
		[Description("Morning")]
		Morning,
		[Description("Noon")]
		Noon,
		[Description("Afternoon")]
		Afternoon,
		[Description("Night")]
		Night,
	}
	public enum TimerState
	{
		Stop,
		Running,
		Paused
	}

	public class TimerManager
	{
		private int year;
		private int month;
		private int day;
		private int hour;
		private int minute;
		private int second;
		private Time currentTime;
		private Time lastTime;
		private TimerState state;
		public event Action TimeChanged;

		private static TimerManager instance;
		public static TimerManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new TimerManager();
				}
				return instance;
			}
		}
		public Time CurrentTime
		{
			get
			{
				if (hour >= 0 && hour < 7)
				{
					currentTime = Time.Early_Morning;
				}
				else if (hour >= 7 && hour < 12)
				{
					currentTime = Time.Morning;
				}
				else if (hour >= 12 && hour < 14)
				{
					currentTime = Time.Noon;
				}
				else if (hour >= 14 && hour < 18)
				{
					currentTime = Time.Afternoon;
				}
				else if (hour >= 18 && hour <= 24)
				{
					currentTime = Time.Night;
				}

				return currentTime;
			}
		}

		public void Init(int year, int month, int day, int hour, int minute, int second)
		{
			this.year = year;
			this.month = month;
			this.day = day;
			this.hour = hour;
			this.minute = minute;
			this.second = second;
			state = TimerState.Running;
			lastTime = CurrentTime;
		}

		public string GetDate()
		{
			return string.Format("{0} {1}, {2}", getMonthStr(month), getDayStr(day), year);
		}

		public string GetTime()
		{
			return currentTime.ToString();
		}

		public void Update()
		{
			if (state != TimerState.Running)
			{
				return;
			}
			second++;
			if (second == 59)
			{
				second = 0;
				minute++;
			}
			if (minute == 59)
			{
				minute = 0;
				hour++;
			}
			if (hour == 24)
			{
				hour = 1;
				day++;
			}
			if (day == 31)
			{
				day = 1;
				month++;
			}
			if (month == 12)
			{
				month = 1;
				year++;
			}

			if (lastTime != CurrentTime)
			{
				TimeChanged?.Invoke();
			}

			lastTime = currentTime;
		}

		public void Resume()
		{
			if(state== TimerState.Paused)
			{
				state = TimerState.Running;
			}
		}

		public void Pause()
		{
			state = TimerState.Paused;
		}

		public void Stop()
		{
			state = TimerState.Stop;
		}

		private string getMonthStr(int month)
		{
			switch(month)
			{
				case 1:
					return "January";
				case 2:
					return "February";
				case 3:
					return "March";
				case 4:
					return "April";
				case 5:
					return "May";
				case 6:
					return "June";
				case 7:
					return "July";
				case 8:
					return "August";
				case 9:
					return "Sepetember";
				case 10:
					return "October";
				case 11:
					return "November";
				case 12:
					return "December";
				default:
					return null;
			}
		}

		private string getDayStr(int day)
		{
			if (day == 1)
			{
				return "1st";
			}
			else if (day == 2)
			{
				return "2nd";
			}
			else if (day == 3)
			{
				return "3rd";
			}
			else
			{
				return day.ToString() + "th";
			}
		}
	}
}
