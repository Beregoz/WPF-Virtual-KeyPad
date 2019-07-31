using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;

namespace VirtualKeyPad
{
	/*
	 * Riistetty ReferenceSourcesta
	 */
	internal class VirtualKeyPadRepeatButton : ButtonBase
	{
		#region Data

		private DispatcherTimer _timer;

		#endregion

		#region Constructors

		static VirtualKeyPadRepeatButton()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(VirtualKeyPadRepeatButton), new FrameworkPropertyMetadata(typeof(VirtualKeyPadRepeatButton)));
			ClickModeProperty.OverrideMetadata(typeof(VirtualKeyPadRepeatButton), new FrameworkPropertyMetadata(ClickMode.Press));
		}

		#endregion

		#region Dependencies and Events

		/// <summary>
		///     The Property for the Delay property.
		///     Flags:              Can be used in style rules
		///     Default Value:      Depend on SPI_GETKEYBOARDDELAY from SystemMetrics
		/// </summary>
		public static readonly DependencyProperty DelayProperty
			= DependencyProperty.Register("Delay", typeof(int), typeof(VirtualKeyPadRepeatButton),
				new FrameworkPropertyMetadata(GetKeyboardDelay()), IsDelayValid);

		/// <summary>
		///     Specifies the amount of time, in milliseconds, to wait before repeating begins.
		///     Must be non-negative
		/// </summary>
		[Bindable(true)]
		[Category("Behavior")]
		public int Delay
		{
			get { return (int) this.GetValue(DelayProperty); }
			set { this.SetValue(DelayProperty, value); }
		}

		/// <summary>
		///     The Property for the Interval property.
		///     Flags:              Can be used in style rules
		///     Default Value:      Depend on SPI_GETKEYBOARDSPEED from SystemMetrics
		/// </summary>
		public static readonly DependencyProperty IntervalProperty
			= DependencyProperty.Register("Interval", typeof(int), typeof(VirtualKeyPadRepeatButton),
				new FrameworkPropertyMetadata(GetKeyboardSpeed()),
				IsIntervalValid);

		/// <summary>
		///     Specifies the amount of time, in milliseconds, between repeats once repeating starts.
		///     Must be non-negative
		/// </summary>
		[Bindable(true)]
		[Category("Behavior")]
		public int Interval
		{
			get { return (int) this.GetValue(IntervalProperty); }
			set { this.SetValue(IntervalProperty, value); }
		}

		#endregion Dependencies and Events

		#region Private helpers

		private static bool IsDelayValid(object value)
		{
			return (int) value >= 0;
		}

		private static bool IsIntervalValid(object value)
		{
			return (int) value > 0;
		}

		/// <summary>
		///     Starts a _timer ticking
		/// </summary>
		private void StartTimer()
		{
			if (this._timer == null)
			{
				this._timer = new DispatcherTimer();
				this._timer.Tick += this.OnTimeout;
			}
			else if (this._timer.IsEnabled)
			{
				return;
			}

			this._timer.Interval = TimeSpan.FromMilliseconds(this.Delay);
			this._timer.Start();
		}

		/// <summary>
		///     Stops a _timer that has already started
		/// </summary>
		private void StopTimer()
		{
			if (this._timer != null && this._timer.IsEnabled)
			{
				this._timer.Stop();
			}
		}

		/// <summary>
		///     This is the handler for when the repeat _timer expires. All we do
		///     is invoke a click.
		/// </summary>
		/// <param name="sender">Sender of the event</param>
		/// <param name="e">Event arguments</param>
		private void OnTimeout(object sender, EventArgs e)
		{
			var interval = TimeSpan.FromMilliseconds(this.Interval);
			if (this._timer.Interval != interval)
			{
				this._timer.Interval = interval;
			}
			this.OnClick();
		}

		/// <summary>
		///     Retrieves the keyboard repeat-delay setting, which is a value in the range from 0
		///     (approximately 250 ms delay) through 3 (approximately 1 second delay).
		///     The actual delay associated with each value may vary depending on the hardware.
		/// </summary>
		/// <returns></returns>
		internal static int GetKeyboardDelay()
		{
			var delay = SystemParameters.KeyboardDelay;
			// SPI_GETKEYBOARDDELAY 0,1,2,3 correspond to 250,500,750,1000ms
			if (delay < 0 || delay > 3)
			{
				delay = 0;
			}
			return (delay + 1) * 250;
		}

		/// <summary>
		///     Retrieves the keyboard repeat-speed setting, which is a value in the range from 0
		///     (approximately 2.5 repetitions per second) through 31 (approximately 30 repetitions per second).
		///     The actual repeat rates are hardware-dependent and may vary from a linear scale by as much as 20%
		/// </summary>
		/// <returns></returns>
		internal static int GetKeyboardSpeed()
		{
			var speed = SystemParameters.KeyboardSpeed;
			// SPI_GETKEYBOARDSPEED 0,...,31 correspond to 1000/2.5=400,...,1000/30 ms
			if (speed < 0 || speed > 31)
			{
				speed = 31;
			}
			return (31 - speed) * (400 - 1000 / 30) / 31 + 1000 / 30;
		}

		#endregion Private helpers

		#region Override methods

		/// <summary>
		///     This is the method that responds to the MouseButtonEvent event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonDown(e);

			if (this.IsPressed && this.ClickMode != ClickMode.Hover)
			{
				this.StartTimer();
			}
		}

		/// <summary>
		///     This is the method that responds to the MouseButtonEvent event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonUp(e);

			this.StopTimer();
		}


		/// <summary>
		///     An event reporting the mouse left this element.
		/// </summary>
		/// <param name="e">Event arguments</param>
		protected override void OnMouseLeave(MouseEventArgs e)
		{
			base.OnMouseLeave(e);
			this.StopTimer();
		}
		#endregion
	}
}